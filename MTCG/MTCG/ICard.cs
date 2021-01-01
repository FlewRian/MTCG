using System;
using System.Collections.Generic;
using System.Text;

namespace MTCG
{
    public interface ICard
    {
        int category { get; set; }
        int damage { get; set; }
        int element { get; set; }
    }
}