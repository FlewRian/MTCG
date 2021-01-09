using System;
using System.Collections.Generic;
using System.Text;

namespace MTCG
{
    public class ArenaLog
    {
        public string ArenaText { get; set; } 
        public Player Winner { get; set; }
        public ArenaLog()
        {
            ArenaText = "";
        }

        public void AddTextToLog(string text)
        {
            ArenaText += text+"\n";
        }
    }
}