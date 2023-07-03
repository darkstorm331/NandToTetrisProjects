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

        public Parser(string inFile) {

        }

        public void advance() {

        }
    }
}