using System;
using System.Collections.Generic;
using System.Text;

namespace MTCG
{
    public class Fight
    {
        private BaseCard cardPlayerOne;
        private BaseCard cardPlayerTwo;
        private WinnerCard fightResult;

        public WinnerCard FightRound(BaseCard cardPlayerOne, BaseCard cardPlayerTwo)
        {
            this.cardPlayerOne = cardPlayerOne;
            this.cardPlayerOne = cardPlayerTwo;
            switch (this.cardPlayerOne.category)
            {

                case Category.Monster when this.cardPlayerTwo.category == Category.Monster:
                    {
                        fightResult = CheckWinner();
                        return fightResult;
                    }

                case Category.Monster when this.cardPlayerTwo.category == Category.Spell:
                    {
                        CheckElement();
                        fightResult = CheckWinner();
                        return fightResult;
                    }

                case Category.Spell when this.cardPlayerTwo.category == Category.Monster:
                    {
                        CheckElement();
                        fightResult = CheckWinner();
                        return fightResult;
                    }
                case Category.Spell when this.cardPlayerTwo.category == Category.Spell:
                    {
                        CheckElement();
                        fightResult = CheckWinner();
                        return fightResult;
                    }
                default:
                    {
                        return fightResult;
                    }
            }
        }

        public void CheckElement()
        {
            switch (this.cardPlayerOne.element)
            {
                case Element.Fire:
                    {
                        if (this.cardPlayerTwo.element == Element.Water)
                        {
                            //water effectiv gegen fire
                            this.cardPlayerTwo.damage = this.cardPlayerTwo.damage * 2;
                            this.cardPlayerOne.damage = this.cardPlayerOne.damage / 2;
                        }
                        else if (this.cardPlayerTwo.element == Element.Normal)
                        {
                            //fire effectiv gegen normal
                            this.cardPlayerTwo.damage = this.cardPlayerTwo.damage / 2;
                            this.cardPlayerOne.damage = this.cardPlayerOne.damage * 2;
                        }
                        else
                        {
                            //Damage bleibt gleich
                        }
                        break;
                    }
                case Element.Water:
                    {
                        if (this.cardPlayerTwo.element == Element.Fire)
                        {
                            //water effectiv gegen fire
                            this.cardPlayerTwo.damage = this.cardPlayerTwo.damage / 2;
                            this.cardPlayerOne.damage = this.cardPlayerOne.damage * 2;
                        }
                        else if (this.cardPlayerTwo.element == Element.Normal)
                        {
                            //normal effectiv gegen water
                            this.cardPlayerTwo.damage = this.cardPlayerTwo.damage * 2;
                            this.cardPlayerOne.damage = this.cardPlayerOne.damage / 2;
                        }
                        else
                        {
                            //Damage bleibt gleich
                        }
                        break;
                    }
                case Element.Normal:
                    {
                        if (this.cardPlayerTwo.element == Element.Water)
                        {
                            //normal effectiv gegen water
                            this.cardPlayerTwo.damage = this.cardPlayerTwo.damage / 2;
                            this.cardPlayerOne.damage = this.cardPlayerOne.damage * 2;
                        }
                        else if (cardPlayerTwo.element == Element.Fire)
                        {
                            //fire effectiv gegen normal
                            this.cardPlayerTwo.damage = this.cardPlayerTwo.damage * 2;
                            this.cardPlayerOne.damage = this.cardPlayerOne.damage / 2;
                        }
                        else
                        {
                            //Damage bleibt gleich
                        }
                        break;
                    }
            }
        }

        public WinnerCard CheckWinner()
        {
            if (this.cardPlayerOne.damage > this.cardPlayerTwo.damage)
            {
                //cardPlayerOne wins
                return WinnerCard.CardPlayerOne;
            }
            else if (this.cardPlayerOne.damage < this.cardPlayerTwo.damage)
            {
                //cardPlayerTwo wins
                return WinnerCard.CardPlayerTwo;
            }
            else
            {
                //draw
                return WinnerCard.Draw;
            }
        }
    }
}