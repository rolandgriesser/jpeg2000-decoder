
using jpeg2000_decoder.Entropy;
using jpeg2000_decoder.Image;
using jpeg2000_decoder.Quantization;
using jpeg2000_decoder.ROI;
using jpeg2000_decoder.Wavelet.Synthesis;

public class DecoderSpecs {

    /** ICC Profiling specifications */
    public ModuleSpec iccs;

    /** ROI maxshift value specifications */
    public MaxShiftSpec rois;

    /** Quantization type specifications */
    public QuantTypeSpec qts;

    /** Quantization normalized base step size specifications */
    public QuantStepSizeSpec qsss;

    /** Number of guard bits specifications */
    public GuardBitsSpec gbs;

    /** Analysis wavelet filters specifications */
    public SynWTFilterSpec wfs;

    /** Number of decomposition levels specifications */
    public IntegerSpec dls;

    /** Number of layers specifications */
    public IntegerSpec nls;

    /** Progression order specifications */
    public IntegerSpec pos;
    
    /** The Entropy decoder options specifications */
    public ModuleSpec ecopts;

    /** The component transformation specifications */
    public CompTransfSpec cts;

    /** The progression changes specifications */
    public ModuleSpec pcs;

    /** The error resilience specifications concerning the entropy
     * decoder */
    public ModuleSpec ers;
    
    /** Precinct partition specifications */
    public PrecinctSizeSpec pss;

    /** The Start Of Packet (SOP) markers specifications */
    public ModuleSpec sops;

    /** The End of Packet Headers (EPH) markers specifications */
    public ModuleSpec ephs;

    /** Code-blocks sizes specification */
    public CBlkSizeSpec cblks;

    /** Packed packet header specifications */
    public ModuleSpec pphs;

    /** 
     * Returns a copy of the current object.
     * */
    // public DecoderSpecs getCopy() {
    //     DecoderSpecs decSpec2;
    //     try {
    //         decSpec2 = (DecoderSpecs)this.clone();
    //     } catch (CloneNotSupportedException e) {
    //         throw new Error("Cannot clone the DecoderSpecs instance");
    //     }
    //     // Quantization
    //     decSpec2.qts = (QuantTypeSpec)qts.getCopy();
    //     decSpec2.qsss = (QuantStepSizeSpec)qsss.getCopy();
    //     decSpec2.gbs = (GuardBitsSpec)gbs.getCopy();
    //     // Wavelet transform
    //     decSpec2.wfs = (SynWTFilterSpec)wfs.getCopy();
    //     decSpec2.dls = (IntegerSpec)dls.getCopy();
    //     // Component transformation
    //     decSpec2.cts = (CompTransfSpec)cts.getCopy();
    //     // ROI
    //     if(rois!=null) {
    //         decSpec2.rois = (MaxShiftSpec)rois.getCopy();
    //     }
    //     return decSpec2;
    // }

    /** 
     * Initialize all members with the given number of tiles and components.
     *
     * @param nt Number of tiles
     *
     * @param nc Number of components
     * */
    public DecoderSpecs(int nt,int nc) {
        // Quantization
        qts  = new QuantTypeSpec(nt,nc,ModuleSpec.SPEC_TYPE_TILE_COMP);
        qsss = new QuantStepSizeSpec(nt,nc,ModuleSpec.SPEC_TYPE_TILE_COMP);
        gbs  = new GuardBitsSpec(nt,nc,ModuleSpec.SPEC_TYPE_TILE_COMP);

        // Wavelet transform
        wfs = new SynWTFilterSpec(nt,nc,ModuleSpec.SPEC_TYPE_TILE_COMP);
        dls = new IntegerSpec(nt,nc,ModuleSpec.SPEC_TYPE_TILE_COMP);

        // Component transformation
        cts = new CompTransfSpec(nt,nc,ModuleSpec.SPEC_TYPE_TILE_COMP);

        // Entropy decoder
        ecopts = new ModuleSpec(nt,nc,ModuleSpec.SPEC_TYPE_TILE_COMP);
        ers = new ModuleSpec(nt,nc,ModuleSpec.SPEC_TYPE_TILE_COMP);
        cblks = new CBlkSizeSpec(nt,nc,ModuleSpec.SPEC_TYPE_TILE_COMP);

        // Precinct partition
        pss = new PrecinctSizeSpec(nt,nc,ModuleSpec.SPEC_TYPE_TILE_COMP,dls);

        // Codestream
        nls  = new IntegerSpec(nt,nc,ModuleSpec.SPEC_TYPE_TILE);
        pos  = new IntegerSpec(nt,nc,ModuleSpec.SPEC_TYPE_TILE);
        pcs  = new ModuleSpec(nt,nc,ModuleSpec.SPEC_TYPE_TILE);
        sops = new ModuleSpec(nt,nc,ModuleSpec.SPEC_TYPE_TILE);
        ephs = new ModuleSpec(nt,nc,ModuleSpec.SPEC_TYPE_TILE);
        pphs = new ModuleSpec(nt,nc,ModuleSpec.SPEC_TYPE_TILE);
	iccs = new ModuleSpec(nt,nc,ModuleSpec.SPEC_TYPE_TILE);
        pphs.setDefault(false);
    }
}
