using System;
using System.Collections.Generic;
using System.Text;

namespace MTCG
{
    public class Fight
    {
        private BaseCard cardPlayerOne;
        private BaseCard cardPlayerTwo;

        public FightRound(BaseCard cardPlayerOne, BaseCard cardPlayerTwo)
        {
            switch (cardPlayerOne.category)
            {
                case Category.Monster when cardPlayerTwo.category == Category.Monster:
                    {

                    }
            }
        }
    }
}