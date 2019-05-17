using System;
using System.IO;
using jpeg2000_decoder.Exceptions;
using jpeg2000_decoder.FileFormat.Reader;
using jpeg2000_decoder.IO;

public class Decoder {



    public Decoder()
    {
        
    }

    public Image Decode(IRandomAccessIO input) {
        var fileFormatReader = new FileFormatReader(input);
        if(fileFormatReader.JP2FFUsed) {
            input.Seek(fileFormatReader.FirstCodeStreamPosition);
        }

           // **** Header decoder ****
            // Instantiate header decoder and read main header 
        //     hi = new HeaderInfo();
	    // try {
		// hd = new HeaderDecoder(in,pl,hi);
	    // } catch (EOFException e) {
		// error("Codestream too short or bad header, "+
        //               "unable to decode.",2);
        //         if(pl.getParameter("debug").equals("on")) {
        //             e.printStackTrace();
        //         } else {
        //             error("Use '-debug' option for more details",2);
        //         }
		// return;
	    // }


        return null;
    }
}