using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTCG
{
    public class Arena
    {
        private List<BaseCard> deckPlayerOne;
        private List<BaseCard> deckPlayerTwo;
        public Player playerOne { get; set; }
        public Player playerTwo { get ; set; }
        private BaseCard cardPlayerOne;
        private BaseCard cardPlayerTwo;
        private int cardsInDeckPlayerOne = 0;
        private int cardsInDeckPlayerTwo = 0;
        private WinnerCard winnerCard;
        //private PlayerEnum winner;
        public int Roundcount { get; set; }
        public int RoundsMax { get; set; }
        public ArenaLog ArenaLog { get; set; }

        public Arena()
        {
            Roundcount = 1;
            RoundsMax = 101;
            ArenaLog = new ArenaLog();
        }

        public Arena(Player playerOne, Player playerTwo)
        {
            Roundcount = 1;
            RoundsMax = 101;
            ArenaLog = new ArenaLog();
            this.playerOne = playerOne;
            this.playerTwo = playerTwo;
        }

        public ArenaLog Fight(Player playerOne, Player playerTwo, Arena arena)
        {
            this.playerOne = playerOne;
            this.playerTwo = playerTwo;
            this.deckPlayerOne = playerOne.CurrentDeck;
            this.deckPlayerTwo = playerTwo.CurrentDeck;
            Fight fight = new MTCG.Fight();
            Random rnd = new Random();
            int choosenCardPlayerOne = 0;
            int choosenCardPlayerTwo = 0;
            while (this.Roundcount < RoundsMax)
            {
                ArenaLog.AddTextToLog("Round: " + Roundcount);
                //Console.WriteLine("\nRoundcount: " + this.Roundcount + "\n");
                if (!this.deckPlayerOne.Any())
                {
                    ArenaLog.AddTextToLog("--------------------------------------------------\n" + playerTwo.Username.ToUpper() + " leaves the Arena as Winner!\n--------------------------------------------------");
                    return ArenaLog;
                }

                if (!this.deckPlayerTwo.Any())
                {
                    ArenaLog.AddTextToLog("--------------------------------------------------\n" + playerOne.Username.ToUpper() + " leaves the Arena as Winner!\n--------------------------------------------------");
                    return ArenaLog;
                }
                this.cardsInDeckPlayerOne = this.deckPlayerOne.Count;
                this.cardsInDeckPlayerTwo = this.deckPlayerTwo.Count;

                ArenaLog.AddTextToLog("Cards in Deck Player One: " + cardsInDeckPlayerOne);
                ArenaLog.AddTextToLog("Cards in Deck Player Two: " + cardsInDeckPlayerTwo);
                //Console.WriteLine("Cards in Deck Player One: " + cardsInDeckPlayerOne);
                //Console.WriteLine("Cards in Deck Player Two: " + cardsInDeckPlayerTwo + "\n");

                choosenCardPlayerOne = rnd.Next(0, cardsInDeckPlayerOne);
                this.cardPlayerOne = deckPlayerOne[choosenCardPlayerOne];

                choosenCardPlayerTwo = rnd.Next(0, cardsInDeckPlayerTwo);
                this.cardPlayerTwo = deckPlayerTwo[choosenCardPlayerTwo];

                winnerCard = fight.FightRound(cardPlayerOne, cardPlayerTwo, arena);

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
                this.Roundcount++;
            }
            ArenaLog.AddTextToLog("--------------------------------------------------\nDraw! No Winner after 100 Rounds\n--------------------------------------------------");
            return ArenaLog;
        }
    }
}