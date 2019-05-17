using System;
using System.Collections.Generic;
using System.IO;
using jpeg2000_decoder.IO;

namespace jpeg2000_decoder
{
    public class Logger 
        {
            public enum Severity
            {
                Error,
                Warning,
                Debug
            }
            public static Severity MaxSeverity = Severity.Debug;
            public static void Log(Severity severity, string message)
            {
                if(severity > MaxSeverity)
                    return;
                
                switch (severity)
                {
                    case Severity.Error: Console.ForegroundColor = ConsoleColor.Red; break;
                    case Severity.Warning: Console.ForegroundColor = ConsoleColor.Yellow; break;
                    default: break;
                }
                System.Console.WriteLine(message);
                Console.ResetColor();
            }

            public static void Error(string message) 
            {
                Log(Severity.Error, message);
            }
            public static void Warning(string message) 
            {
                Log(Severity.Warning, message);
            }
            public static void Debug(string message) 
            {
                Log(Severity.Debug, message);
            }
        }
    class Program
    {
        

        static void Main(string[] args)
        {
            Console.Clear();
            if(args.Length == 0)
            {
                Logger.Error("No input file provided.");
                return;
            }
            var inputFileName = args[0];
            if(!File.Exists(inputFileName)) 
            {
                Logger.Error($@"Input file doesn't exist.
Filename provided: {inputFileName}");
                return;
            }
            Logger.Debug($"Processing file: {inputFileName}");
            try
            {
                var inputFile = File.OpenRead(inputFileName);
                
                var decoder = new Decoder();
                var randomAccessFile = new RandomAccessFile(inputFile);
                //var decoded = decoder.Decode(randomAccessFile);
                decoder.Decode(randomAccessFile);
                // //todo different codecs
                // Codec codec = null;
                // // switch(false) {
                //    codec = new CodecJP2(); 
                // // }
                // if(codec == null) {
                //     Logger.Error("Couldn't find suitable codec to decode file.");
                //     return;
                // }

                // var image = codec.Read(inputFile);
            } 
            catch(Exception e) {
                Logger.Error($"Problem reading input file.\nException: {e.Message}");
            }
        }
    }
}
