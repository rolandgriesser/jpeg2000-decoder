/* 
 * CVS identifier:
 * 
 * $Id: QuantStepSizeSpec.java,v 1.12 2001/10/24 12:05:04 grosbois Exp $
 * 
 * Class:                   QuantStepSizeSpec
 * 
 * Description:    Quantization base normalized step size specifications
 * 
 * 
 * 
 * COPYRIGHT:
 * 
 * This software module was originally developed by Rapha�l Grosbois and
 * Diego Santa Cruz (Swiss Federal Institute of Technology-EPFL); Joel
 * Askel�f (Ericsson Radio Systems AB); and Bertrand Berthelot, David
 * Bouchard, F�lix Henry, Gerard Mozelle and Patrice Onno (Canon Research
 * Centre France S.A) in the course of development of the JPEG2000
 * standard as specified by ISO/IEC 15444 (JPEG 2000 Standard). This
 * software module is an implementation of a part of the JPEG 2000
 * Standard. Swiss Federal Institute of Technology-EPFL, Ericsson Radio
 * Systems AB and Canon Research Centre France S.A (collectively JJ2000
 * Partners) agree not to assert against ISO/IEC and users of the JPEG
 * 2000 Standard (Users) any of their rights under the copyright, not
 * including other intellectual property rights, for this software module
 * with respect to the usage by ISO/IEC and Users of this software module
 * or modifications thereof for use in hardware or software products
 * claiming conformance to the JPEG 2000 Standard. Those intending to use
 * this software module in hardware or software products are advised that
 * their use may infringe existing patents. The original developers of
 * this software module, JJ2000 Partners and ISO/IEC assume no liability
 * for use of this software module or modifications thereof. No license
 * or right to this software module is granted for non JPEG 2000 Standard
 * conforming products. JJ2000 Partners have full right to use this
 * software module for his/her own purpose, assign or donate this
 * software module to any third party and to inhibit third parties from
 * using this software module for non JPEG 2000 Standard conforming
 * products. This copyright notice must be included in all copies or
 * derivative works of this software module.
 * 
 * Copyright (c) 1999/2000 JJ2000 Partners.
 * */
using jpeg2000_decoder.Util;
using System;

namespace jpeg2000_decoder.Quantization {


/**
 * This class extends ModuleSpec class in order to hold specifications about
 * the quantization base normalized step size to use in each tile-component.
 *
 * @see ModuleSpec
 * */
public class QuantStepSizeSpec : ModuleSpec {

    /** 
     * Constructs an empty 'QuantStepSizeSpec' with specified number of
     * tile and components. This constructor is called by the decoder.
     *
     * @param nt Number of tiles
     *
     * @param nc Number of components
     *
     * @param type the type of the specification module i.e. tile specific,
     * component specific or both.
     * */
    public QuantStepSizeSpec(int nt, int nc, byte type): base(nt, nc, type) {
	
    }

    /**
     * Constructs a new 'QuantStepSizeSpec' for the specified number of
     * components and tiles and the arguments of "-Qstep" option.
     *
     * @param nt The number of tiles
     *
     * @param nc The number of components
     *
     * @param type the type of the specification module i.e. tile specific,
     * component specific or both.
     *
     * @param pl The ParameterList
     * */
    public QuantStepSizeSpec(int nt,int nc,byte type,ParameterList pl): base(nt, nc, type) {


	String param = pl.getParameter("Qstep");
	if(param==null) {
	    throw new ArgumentException("Qstep option not specified");
	}

	// Parse argument
	StringTokenizer stk = new StringTokenizer(param);
	String word; // current word
	byte curSpecType = SPEC_DEF; // Specification type of the
	// current parameter
	bool[] tileSpec = null; // Tiles concerned by the specification
	bool[] compSpec = null; // Components concerned by the specification
	float value; // value of the current step size
	
	while(stk.HasMoreTokens()) {
	    word = stk.NextToken().ToLower();
	  
	    switch(word[0]) {
	    case 't': // Tiles specification
 		tileSpec = parseIdx(word,nTiles);
		if(curSpecType==SPEC_COMP_DEF)
		    curSpecType = SPEC_TILE_COMP;
		else
		    curSpecType = SPEC_TILE_DEF;
 		break;
	    case 'c': // Components specification
		compSpec = parseIdx(word,nComp);
		if(curSpecType==SPEC_TILE_DEF)
		    curSpecType = SPEC_TILE_COMP;
		else
		    curSpecType = SPEC_COMP_DEF;
		break;
	    default: // Step size value
		try{
		    value = float.Parse(word);
		}
		catch(FormatException e) {
		    throw new ArgumentException("Bad parameter for "+
						       "-Qstep option : "+
						       word);
		}

		if (value <= 0.0f) {
		    throw new ArgumentException("Normalized base step "+
						       "must be positive : "+
						       value);
		}
		

		if(curSpecType==SPEC_DEF) {
		    setDefault(value);
		} else if(curSpecType==SPEC_TILE_DEF) {
		    for(int i=tileSpec.Length-1; i>=0; i--)
			if(tileSpec[i]) {
			    setTileDef(i,value);
                        }
		}
		else if(curSpecType==SPEC_COMP_DEF){
		    for(int i=compSpec.Length-1; i>=0; i--)
			if(compSpec[i]){
			    setCompDef(i,value);
                        }
		}
		else{
		    for(int i=tileSpec.Length-1; i>=0; i--){
			for(int j=compSpec.Length-1; j>=0 ; j--){
			    if(tileSpec[i] && compSpec[j]){
				setTileCompVal(i,j,value);
                            }
			}
		    }
		}

		// Re-initialize
		curSpecType = SPEC_DEF;
		tileSpec = null;
		compSpec = null;
		break;
	    }
	}

        // Check that default value has been specified
        if(getDefault()==null){
            int ndefspec = 0;
            for(int t=nt-1; t>=0; t--){
                for(int c=nc-1; c>=0 ; c--){
                    if(specValType[t][c] == SPEC_DEF){
                        ndefspec++;
                    }
                }
            }
            
            // If some tile-component have received no specification, it takes
            // the default value defined in ParameterList
            if(ndefspec!=0){
                setDefault(float.Parse(pl.getDefaultParameterList().
                                     getParameter("Qstep")));
            }
            else{
                // All tile-component have been specified, takes the first
                // tile-component value as default.
                setDefault(getTileCompVal(0,0));
                switch(specValType[0][0]){
                case SPEC_TILE_DEF:
                    for(int c=nc-1; c>=0; c--){
                        if(specValType[0][c]==SPEC_TILE_DEF)
                            specValType[0][c] = SPEC_DEF;
                    }
                    tileDef[0] = null;
                    break;
                case SPEC_COMP_DEF:
                    for(int t=nt-1; t>=0; t--){
                        if(specValType[t][0]==SPEC_COMP_DEF)
                            specValType[t][0] = SPEC_DEF;
                    }
                    compDef[0] = null;
                    break;
                case SPEC_TILE_COMP:
                    specValType[0][0] = SPEC_DEF;
                    tileCompVal.Add("t0c0",null);
                    break;
                }
            }
	}
   }

}
}