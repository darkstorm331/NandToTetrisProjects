
using System;
using System.IO;
using VMTranslator;

class Program
{
    static void Main(string[] args)
    {
        //Validity checks
        if(args.Length == 1) {
            string fileName = args[0];

            if(Path.GetExtension(fileName) == ".vm") {
                //Valid file
                Parser parser = new Parser(fileName);

                do {
                    parser.advance();
                } while(parser.hasMoreCommands);
            } else {
                Console.WriteLine("Please provide a '.vm' file");
                return;
            }
        } else {
            Console.WriteLine($"Incorrect number of arguments provided. Expected 1 and you provided {args.Length}");
            return;
        }
    }
}