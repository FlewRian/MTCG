using System;
using System.Collections.Generic;
using System.Text;

namespace MTCG
{
    public interface IPlayer
    {
        int coins { get; set; }
        List<string> collection { get; set; }
        List<string> deck { get; set; }
        string name { get; set; }
    }
}