using System;
using System.Collections.Generic;
using System.Text;

namespace MTCG
{
    public class Fight
    {
        private BaseCard cardPlayerOne;
        private BaseCard cardPlayerTwo;
        private WinnerCard fightResult = WinnerCard.Draw;

        public WinnerCard FightRound(BaseCard cardPlayerOne, BaseCard cardPlayerTwo)
        {
            this.cardPlayerOne = cardPlayerOne;
            this.cardPlayerOne = cardPlayerTwo;
            switch (this.cardPlayerOne.category)
            {

                case Category.Monster when this.cardPlayerTwo.category == Category.Monster:
                    {
                        switch (this.cardPlayerOne.race)
                        {
                            case Race.Dragon when this.cardPlayerTwo.race == Race.Goblin:
                                {
                                    this.fightResult = WinnerCard.CardPlayerOne;
                                    break;
                                }
                            case Race.Goblin when this.cardPlayerTwo.race == Race.Dragon:
                                {
                                    this.fightResult = WinnerCard.CardPlayerTwo;
                                    break;
                                }
                            case Race.Wizzard when this.cardPlayerTwo.race == Race.Orc:
                                {
                                    this.fightResult = WinnerCard.CardPlayerOne;
                                    break;
                                }
                            case Race.Orc when this.cardPlayerTwo.race == Race.Wizzard:
                                {
                                    this.fightResult = WinnerCard.CardPlayerTwo;
                                    break;
                                }
                            case Race.Elve when this.cardPlayerTwo.race == Race.Dragon:
                                {
                                    if (this.cardPlayerOne.element == Element.Fire)
                                    {
                                        this.fightResult = WinnerCard.CardPlayerOne;
                                        break;
                                    }
                                    else
                                    {
                                        this.fightResult = CheckWinner();
                                        break;
                                    }
                                }
                            case Race.Dragon when this.cardPlayerTwo.race == Race.Elve:
                                {
                                    if (this.cardPlayerTwo.element == Element.Fire)
                                    {
                                        this.fightResult = WinnerCard.CardPlayerTwo;
                                        break;
                                    }
                                    else
                                    {
                                        this.fightResult = CheckWinner();
                                        break;
                                    }
                                }
                            default:
                                {
                                    this.fightResult = CheckWinner();
                                    break;
                                }
                        }
                        return this.fightResult;
                    }

                case Category.Monster when this.cardPlayerTwo.category == Category.Spell:
                    {
                        fightResult = MonsterVsSpell();
                        switch (this.fightResult)
                        {
                            case WinnerCard.CardPlayerOne:
                                {
                                    break;
                                }
                            case WinnerCard.CardPlayerTwo:
                                {
                                    break;
                                }
                            case WinnerCard.Draw:
                                {
                                    CheckElement();
                                    this.fightResult = CheckWinner();
                                    break;
                                }
                        }
                        return this.fightResult;
                    }

                case Category.Spell when this.cardPlayerTwo.category == Category.Monster:
                    {
                        fightResult = MonsterVsSpell();
                        switch (this.fightResult)
                        {
                            case WinnerCard.CardPlayerOne:
                                {
                                    break;
                                }
                            case WinnerCard.CardPlayerTwo:
                                {
                                    break;
                                }
                            case WinnerCard.Draw:
                                {
                                    CheckElement();
                                    this.fightResult = CheckWinner();
                                    break;
                                }
                        }
                        return this.fightResult;
                    }
                case Category.Spell when this.cardPlayerTwo.category == Category.Spell:
                    {
                        CheckElement();
                        this.fightResult = CheckWinner();
                        return this.fightResult;
                    }
                default:
                    {
                        return this.fightResult;
                    }
            }
        }

        public void CheckElement()
        {
            switch (this.cardPlayerOne.element)
            {
                case Element.Fire when this.cardPlayerTwo.element == Element.Normal:
                    {
                        //fire effectiv gegen normal
                        this.cardPlayerTwo.damage = this.cardPlayerTwo.damage / 2;
                        this.cardPlayerOne.damage = this.cardPlayerOne.damage * 2;
                        break;
                    }
                case Element.Fire when this.cardPlayerTwo.element == Element.Water:
                    {
                        //water effectiv gegen fire
                        this.cardPlayerTwo.damage = this.cardPlayerTwo.damage * 2;
                        this.cardPlayerOne.damage = this.cardPlayerOne.damage / 2;
                        break;
                    }
                case Element.Water when this.cardPlayerTwo.element == Element.Fire:
                    {
                        //water effectiv gegen fire
                        this.cardPlayerTwo.damage = this.cardPlayerTwo.damage / 2;
                        this.cardPlayerOne.damage = this.cardPlayerOne.damage * 2;
                        break;
                    }
                case Element.Water when this.cardPlayerTwo.element == Element.Normal:
                    {
                        //normal effectiv gegen water
                        this.cardPlayerTwo.damage = this.cardPlayerTwo.damage * 2;
                        this.cardPlayerOne.damage = this.cardPlayerOne.damage / 2;
                        break;
                    }
                case Element.Normal when this.cardPlayerTwo.element == Element.Water:
                    {
                        //normal effectiv gegen water
                        this.cardPlayerTwo.damage = this.cardPlayerTwo.damage / 2;
                        this.cardPlayerOne.damage = this.cardPlayerOne.damage * 2;
                        break;
                    }
                case Element.Normal when this.cardPlayerTwo.element == Element.Fire:
                    {
                        //fire effectiv gegen normal
                        this.cardPlayerTwo.damage = this.cardPlayerTwo.damage * 2;
                        this.cardPlayerOne.damage = this.cardPlayerOne.damage / 2;
                        break;
                    }
                default:
                    {
                        //same Element, nothing changes
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

        public WinnerCard MonsterVsSpell()
        {
            if (this.cardPlayerOne.race == Race.Kraken)
            {
                return WinnerCard.CardPlayerOne;
            }
            else if (this.cardPlayerTwo.race == Race.Kraken)
            {
                return WinnerCard.CardPlayerTwo;
            }
            else if (this.cardPlayerOne.race == Race.Knight && this.cardPlayerTwo.element == Element.Water)
            {
                return WinnerCard.CardPlayerTwo;
            }
            else if (this.cardPlayerTwo.race == Race.Knight && this.cardPlayerOne.element == Element.Water)
            {
                return WinnerCard.CardPlayerOne;
            }
            else
            {
                return WinnerCard.Draw;
            }
        }
    }
}