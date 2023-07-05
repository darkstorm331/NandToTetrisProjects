using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VMTranslator
{
    public class CodeWriter
    {
        public StreamWriter OutFileWriter { get; set; }

        public CodeWriter(string outFile) {
            OutFileWriter = new StreamWriter(File.Create(outFile));
        }

        public void WriteArithmetic(string command) {
            OutFileWriter.WriteLine("Arithmetic");
        }

        public void WritePushPop(CommandType command, string segment, int index) {
            if(command == CommandType.C_PUSH) {
                switch(segment.ToLower()) {
                    case "constant":
                        PushConstant(index);
                        break;

                    case "local":
                    case "argument":
                    case "this":
                    case "that":
                        PushLclArgThisThat(segment, index);
                        break;                                        
                }
            } else {
                switch(segment.ToLower()) {
                    case "local":
                    case "argument":
                    case "this":
                    case "that":
                        PopLclArgThisThat(segment, index);
                        break;                                        
                }
            }                 
        }

        public void Close() {
            OutFileWriter.Close();
            OutFileWriter.Dispose();
        }

        private void PushConstant(int number) {
            OutFileWriter.WriteLine($"// push constant {number}");
            OutFileWriter.WriteLine($"@{number}");
            OutFileWriter.WriteLine("D=A");
            OutFileWriter.WriteLine("@SP");
            OutFileWriter.WriteLine("A=M");
            OutFileWriter.WriteLine("M=D");
            OutFileWriter.WriteLine("@SP");
            OutFileWriter.WriteLine("M=M+1");
        }

        private void PushLclArgThisThat(string section, int number) {
            string register = string.Empty;
            switch(section.ToLower()) {
                case "local":
                    register = "LCL";
                    break;

                case "argument":
                    register = "ARG";
                    break;

                case "this":
                case "that":
                    register = section.ToUpper();
                    break;
            }
            
            OutFileWriter.WriteLine($"// push {section} {number}");
            OutFileWriter.WriteLine($"@{number}");
            OutFileWriter.WriteLine("D=A");            
            OutFileWriter.WriteLine($"@{register}");            
            OutFileWriter.WriteLine("A=M+D");
            OutFileWriter.WriteLine("D=M");
            OutFileWriter.WriteLine("@SP");
            OutFileWriter.WriteLine("A=M");
            OutFileWriter.WriteLine("M=D");
            OutFileWriter.WriteLine("@SP");
            OutFileWriter.WriteLine("M=M+1");
        }

        private void PopLclArgThisThat(string section, int number) {
            string register = string.Empty;
            switch(section.ToLower()) {
                case "local":
                    register = "LCL";
                    break;

                case "argument":
                    register = "ARG";
                    break;

                case "this":
                case "that":
                    register = section.ToUpper();
                    break;
            }
            
            OutFileWriter.WriteLine($"// pop {section} {number}");           
            OutFileWriter.WriteLine($"@{register}");            
            OutFileWriter.WriteLine("D=M");
            OutFileWriter.WriteLine($"@{number}"); 
            OutFileWriter.WriteLine("D=D+A");
            OutFileWriter.WriteLine("@R13");
            OutFileWriter.WriteLine("M=D");
            OutFileWriter.WriteLine("@SP");
            OutFileWriter.WriteLine("M=M-1");
            OutFileWriter.WriteLine("A=M");
            OutFileWriter.WriteLine("D=M");
            OutFileWriter.WriteLine("@R13");
            OutFileWriter.WriteLine("A=M");
            OutFileWriter.WriteLine("M=D");
        }

        
    }
}