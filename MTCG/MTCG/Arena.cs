using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTCG
{
    public class Arena
    {
        private List<BaseCard> deckPlayerOne = new List<BaseCard>();
        private List<BaseCard> deckPlayerTwo = new List<BaseCard>();
        private int playerOne;
        private int playerTwo;
        private BaseCard cardPlayerOne;
        private BaseCard cardPlayerTwo;
        private int cardsInDeckPlayerOne = 0;
        private int cardsInDeckPlayerTwo = 0;
        private WinnerCard winnerCard;

        public void Fight()
        {
            Fight fight = new MTCG.Fight();
            int roundcount = 0;
            Random rnd = new Random();
            int choosenCardPlayerOne = 0;
            int choosenCardPlayerTwo = 0;
            while (roundcount < 100)
            {
                if (!this.deckPlayerOne.Any())
                {
                    return;
                }

                if (!this.deckPlayerTwo.Any())
                {
                    return;
                }
                this.cardsInDeckPlayerOne = this.deckPlayerOne.Count;
                this.cardsInDeckPlayerTwo = this.deckPlayerTwo.Count;

                choosenCardPlayerOne = rnd.Next(0, cardsInDeckPlayerOne);
                this.cardPlayerOne = deckPlayerOne[choosenCardPlayerOne];

                choosenCardPlayerTwo = rnd.Next(0, cardsInDeckPlayerTwo);
                this.cardPlayerTwo = deckPlayerTwo[choosenCardPlayerTwo];

                winnerCard = fight.FightRound(cardPlayerOne, cardPlayerTwo);

                switch (winnerCard)
                {
                    case WinnerCard.CardPlayerOne:
                        {
                            deckPlayerOne.Add(deckPlayerTwo[choosenCardPlayerTwo]);
                            deckPlayerTwo.RemoveAt(choosenCardPlayerTwo);
                            break;
                        }
                    case WinnerCard.CardPlayerTwo:
                        {
                            deckPlayerTwo.Add(deckPlayerOne[choosenCardPlayerOne]);
                            deckPlayerOne.RemoveAt(choosenCardPlayerOne);
                            break;
                        }
                    case WinnerCard.Draw:
                        {
                            break;
                        }
                }
                roundcount++;
            }
        }
    }
}