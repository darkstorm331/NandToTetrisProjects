using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace VMTranslator
{
    public class CodeWriter
    {
        public string FileName { get; set; }
        public StreamWriter OutFileWriter { get; set; }

        private int LabelCounter { get; set; }

        public CodeWriter(string outFile) {
            OutFileWriter = new StreamWriter(File.Create(outFile));

            FileName = Path.GetFileNameWithoutExtension(outFile);

            LabelCounter = 0;
        }

        public void WriteArithmetic(string command) {
            switch(command.ToLower()) {
                case "add":
                    AddSubAndOr(command.ToLower(), "+");
                    break;

                case "sub":
                    AddSubAndOr(command.ToLower(), "-");
                    break;

                case "neg":
                    NegNot(command.ToLower(), "-");
                    break;

                case "eq":
                    EqGtLt(command.ToLower(), "JEQ");
                    break;

                case "gt":
                    EqGtLt(command.ToLower(), "JGT");
                    break;

                case "lt":
                    EqGtLt(command.ToLower(), "JLT");
                    break;

                case "and":
                    AddSubAndOr(command.ToLower(), "&");
                    break;

                case "or":
                    AddSubAndOr(command.ToLower(), "|");
                    break;

                case "not":
                    NegNot(command.ToLower(), "!");
                    break;
            }
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

                    case "static":
                        PushStatic(index);
                        break;

                    case "temp":
                        PushTemp(index);
                        break;

                    case "pointer":
                        PushPointer(index);
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

                    case "static":
                        PopStatic(index);
                        break;    

                    case "temp":
                        PopTemp(index);
                        break;   

                    case "pointer":
                        PopPointer(index);
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

        private void PushStatic(int number) {
            OutFileWriter.WriteLine($"// push static {number}");           
            OutFileWriter.WriteLine($"@{FileName}.{number}");           
            OutFileWriter.WriteLine("D=M");
            OutFileWriter.WriteLine("@SP");
            OutFileWriter.WriteLine("A=M");
            OutFileWriter.WriteLine("M=D");
            OutFileWriter.WriteLine("@SP");
            OutFileWriter.WriteLine("M=M+1");
        }

        private void PopStatic(int number) {
            OutFileWriter.WriteLine($"// pop static {number}");           
            OutFileWriter.WriteLine("@SP");
            OutFileWriter.WriteLine("M=M-1");
            OutFileWriter.WriteLine("A=M");
            OutFileWriter.WriteLine("D=M");
            OutFileWriter.WriteLine($"@{FileName}.{number}");           
            OutFileWriter.WriteLine("M=D");
        }

        private void PushTemp(int number) {
            OutFileWriter.WriteLine($"// push temp {number}");           
            OutFileWriter.WriteLine("@5");
            OutFileWriter.WriteLine("D=A");
            OutFileWriter.WriteLine($"@{number}");
            OutFileWriter.WriteLine("D=D+A");
            OutFileWriter.WriteLine("A=D");
            OutFileWriter.WriteLine("D=M");
            OutFileWriter.WriteLine("@SP");
            OutFileWriter.WriteLine("A=M");
            OutFileWriter.WriteLine("M=D");
            OutFileWriter.WriteLine("@SP");
            OutFileWriter.WriteLine("M=M+1");
        }

        private void PopTemp(int number) {
            OutFileWriter.WriteLine($"// pop temp {number}");           
            OutFileWriter.WriteLine("@5");
            OutFileWriter.WriteLine("D=A");
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

        private void PushPointer(int number) {
            string access = number == 0 ? "THIS" : "THAT";

            OutFileWriter.WriteLine($"// push pointer {number}");
            OutFileWriter.WriteLine($"@{access}");
            OutFileWriter.WriteLine("D=M");
            OutFileWriter.WriteLine("@SP");
            OutFileWriter.WriteLine("A=M");
            OutFileWriter.WriteLine("M=D");
            OutFileWriter.WriteLine("@SP");
            OutFileWriter.WriteLine("M=M+1");           
        }

        private void PopPointer(int number) {
            string access = number == 0 ? "THIS" : "THAT";

            OutFileWriter.WriteLine($"// pop pointer {number}");
            OutFileWriter.WriteLine("@SP");
            OutFileWriter.WriteLine("M=M-1");
            OutFileWriter.WriteLine("A=M");
            OutFileWriter.WriteLine("D=M");
            OutFileWriter.WriteLine($"@{access}");
            OutFileWriter.WriteLine("M=D");                       
        }

        private void AddSubAndOr(string command, string op) {
            OutFileWriter.WriteLine($"// {command}");
            OutFileWriter.WriteLine("@SP");
            OutFileWriter.WriteLine("M=M-1");
            OutFileWriter.WriteLine("A=M");
            OutFileWriter.WriteLine("D=M");
            OutFileWriter.WriteLine("@R13");
            OutFileWriter.WriteLine("M=D");
            OutFileWriter.WriteLine("@SP");
            OutFileWriter.WriteLine("M=M-1");
            OutFileWriter.WriteLine("A=M");
            OutFileWriter.WriteLine("D=M");
            OutFileWriter.WriteLine("@R13");
            OutFileWriter.WriteLine($"D=D{op}M");
            OutFileWriter.WriteLine("@SP");
            OutFileWriter.WriteLine("A=M");
            OutFileWriter.WriteLine("M=D");
            OutFileWriter.WriteLine("@SP");
            OutFileWriter.WriteLine("M=M+1");
        }

        private void NegNot(string command, string op) {
            OutFileWriter.WriteLine($"// {command}");
            OutFileWriter.WriteLine("@SP");
            OutFileWriter.WriteLine("M=M-1");
            OutFileWriter.WriteLine("A=M");
            OutFileWriter.WriteLine($"D={op}M");
            OutFileWriter.WriteLine("M=D");
            OutFileWriter.WriteLine("@SP");
            OutFileWriter.WriteLine("M=M+1");
        }

        private void EqGtLt(string command, string op) {
            string trueLabel = $"_TRUE_{LabelCounter}";
            string falseLabel = $"_FALSE_{LabelCounter}";
            string endLabel = $"_END_{LabelCounter}";
            
            OutFileWriter.WriteLine($"// {command}");
            OutFileWriter.WriteLine("@SP");
            OutFileWriter.WriteLine("M=M-1");
            OutFileWriter.WriteLine("A=M");
            OutFileWriter.WriteLine("D=M");
            OutFileWriter.WriteLine("@R13");
            OutFileWriter.WriteLine("M=D");
            OutFileWriter.WriteLine("@SP");
            OutFileWriter.WriteLine("M=M-1");
            OutFileWriter.WriteLine("A=M");
            OutFileWriter.WriteLine("D=M");
            OutFileWriter.WriteLine("@R13");
            OutFileWriter.WriteLine($"D=D-M");
            OutFileWriter.WriteLine($"@{trueLabel}");
            OutFileWriter.WriteLine($"D ; {op}");
            OutFileWriter.WriteLine($"@{falseLabel}");
            OutFileWriter.WriteLine($"0 ; JMP");
            OutFileWriter.WriteLine($"({trueLabel})");
            OutFileWriter.WriteLine("D=-1");
            OutFileWriter.WriteLine($"@{endLabel}");
            OutFileWriter.WriteLine("0 ; JMP");
            OutFileWriter.WriteLine($"({falseLabel})");
            OutFileWriter.WriteLine("D=0");
            OutFileWriter.WriteLine($"@{endLabel}");
            OutFileWriter.WriteLine("0 ; JMP");
            OutFileWriter.WriteLine($"({endLabel})");
            OutFileWriter.WriteLine("@SP");
            OutFileWriter.WriteLine("A=M");
            OutFileWriter.WriteLine("M=D");
            OutFileWriter.WriteLine("@SP");
            OutFileWriter.WriteLine("M=M+1");

            LabelCounter = LabelCounter + 1;
        }
    }
}