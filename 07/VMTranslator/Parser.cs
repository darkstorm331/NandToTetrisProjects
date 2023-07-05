using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VMTranslator
{
    public class Parser
    {
        public bool hasMoreCommands { get; set; }
        public string arg1 { get; set; }
        public int arg2 { get; set; }
        public CommandType commandType { get; set; }

        private string[] InputFile { get; set; }
        private int lineNumber { get; set; }

        private CodeWriter codeWriter { get; set; }

        public Parser(string inFile) {
            try {
                InputFile = File.ReadAllLines(inFile);

                lineNumber = 0;

                string outputFile = inFile.Replace(".vm", ".asm");
                codeWriter = new CodeWriter(outputFile);

                if(InputFile.Length > 0) {
                    hasMoreCommands = true;
                } else {
                    hasMoreCommands = false;
                }
            } catch {
                Console.WriteLine("File could not be read.");
                throw;
            }            
        }

        public void advance() {
            string vmCode = InputFile[lineNumber];
            Console.WriteLine(vmCode);

            string[] segments = vmCode.Split(' ');

            switch(segments[0].ToLower()) {
                case "pop":
                    codeWriter.WritePushPop(CommandType.C_POP, segments[1], int.Parse(segments[2]));
                    break;

                case "push":
                    codeWriter.WritePushPop(CommandType.C_PUSH, segments[1], int.Parse(segments[2]));
                    break;

                case "add": 
                case "sub": 
                case "neg": 
                case "eq": 
                case "gt":
                case "lt":
                case "and":
                case "or":
                case "not":
                    codeWriter.WriteArithmetic(segments[0]);
                    break;

                default:
                    break;
            }

            lineNumber++;
            if(lineNumber == InputFile.Length) {
                hasMoreCommands = false;
                codeWriter.Close();
            }
        }
    }
}