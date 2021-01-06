using System;
using System.Collections.Generic;
using System.Text;

namespace MTCG
{
    public class Fight
    {
        private BaseCard cardPlayerOne;
        private BaseCard cardPlayerTwo;
        private int currentDamageCardOne;
        private int currentDamageCardTwo;
        private WinnerCard fightResult = WinnerCard.Draw;

        public WinnerCard FightRound(BaseCard cardPlayerOne, BaseCard cardPlayerTwo)
        {
            this.cardPlayerOne = cardPlayerOne;
            this.cardPlayerTwo = cardPlayerTwo;

            Console.WriteLine("Card PlayerOne: ");
            WriteCardInfo(cardPlayerOne);
            Console.WriteLine("Card PlayerTwo: ");
            WriteCardInfo(cardPlayerTwo);

            switch (this.cardPlayerOne.category)
            {

                case Category.Monster when this.cardPlayerTwo.category == Category.Monster:
                    {
                        Console.WriteLine("Monster vs Monster\n");
                        switch (this.cardPlayerOne.race)
                        {
                            case Race.Dragon when this.cardPlayerTwo.race == Race.Goblin:
                                {
                                    Console.WriteLine("Dragon vs Goblin\n");
                                    this.fightResult = WinnerCard.CardPlayerOne;
                                    break;
                                }
                            case Race.Goblin when this.cardPlayerTwo.race == Race.Dragon:
                                {
                                    Console.WriteLine("Goblin vs Dragon\n");
                                    this.fightResult = WinnerCard.CardPlayerTwo;
                                    break;
                                }
                            case Race.Wizzard when this.cardPlayerTwo.race == Race.Orc:
                                {
                                    Console.WriteLine("Wizzard vs Orc\n");
                                    this.fightResult = WinnerCard.CardPlayerOne;
                                    break;
                                }
                            case Race.Orc when this.cardPlayerTwo.race == Race.Wizzard:
                                {
                                    Console.WriteLine("Orc vs Wizzard\n");
                                    this.fightResult = WinnerCard.CardPlayerTwo;
                                    break;
                                }
                            case Race.Elve when this.cardPlayerTwo.race == Race.Dragon:
                                {
                                    if (this.cardPlayerOne.element == Element.Fire)
                                    {
                                        Console.WriteLine("FireElve vs Dragon\n");
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
                                        Console.WriteLine("Dragon vs FireElve\n");
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
                        Console.WriteLine("Monster vs Spell");
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
                        Console.WriteLine("Spell vs Monster");
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
                        Console.WriteLine("Spell vs Spell");
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
                        this.currentDamageCardOne = this.cardPlayerOne.damage * 2;
                        this.currentDamageCardTwo = this.cardPlayerTwo.damage / 2;
                        break;
                    }
                case Element.Fire when this.cardPlayerTwo.element == Element.Water:
                    {
                        //water effectiv gegen fire
                        this.currentDamageCardOne = this.cardPlayerOne.damage / 2;
                        this.currentDamageCardTwo = this.cardPlayerTwo.damage * 2;
                        break;
                    }
                case Element.Water when this.cardPlayerTwo.element == Element.Fire:
                    {
                        //water effectiv gegen fire
                        this.currentDamageCardOne = this.cardPlayerOne.damage * 2;
                        this.currentDamageCardTwo = this.cardPlayerTwo.damage / 2;
                        break;
                    }
                case Element.Water when this.cardPlayerTwo.element == Element.Normal:
                    {
                        //normal effectiv gegen water
                        this.currentDamageCardOne = this.cardPlayerOne.damage / 2;
                        this.currentDamageCardTwo = this.cardPlayerTwo.damage * 2;
                        break;
                    }
                case Element.Normal when this.cardPlayerTwo.element == Element.Water:
                    {
                        //normal effectiv gegen water
                        this.currentDamageCardOne = this.cardPlayerOne.damage * 2;
                        this.currentDamageCardTwo = this.cardPlayerTwo.damage / 2;
                        break;
                    }
                case Element.Normal when this.cardPlayerTwo.element == Element.Fire:
                    {
                        //fire effectiv gegen normal
                        this.currentDamageCardOne = this.cardPlayerOne.damage / 2;
                        this.currentDamageCardTwo = this.cardPlayerTwo.damage * 2;
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
                Console.WriteLine("\nDamage Card One: " + this.cardPlayerOne.damage);
                Console.WriteLine("Damage Card Two: " + this.cardPlayerTwo.damage);
                Console.WriteLine("Player One wins");
                return WinnerCard.CardPlayerOne;
            }
            else if (this.cardPlayerOne.damage < this.cardPlayerTwo.damage)
            {
                //cardPlayerTwo wins
                Console.WriteLine("\nDamage Card One: " + this.cardPlayerOne.damage);
                Console.WriteLine("Damage Card Two: " + this.cardPlayerTwo.damage);
                Console.WriteLine("Player Two wins");
                return WinnerCard.CardPlayerTwo;
            }
            else
            {
                //draw
                Console.WriteLine("\nDamage Card One: " + this.cardPlayerOne.damage);
                Console.WriteLine("Damage Card Two: " + this.cardPlayerTwo.damage);
                Console.WriteLine("Draw");
                return WinnerCard.Draw;
            }
        }

        public WinnerCard MonsterVsSpell()
        {
            if (this.cardPlayerOne.race == Race.Kraken)
            {
                Console.WriteLine("Kraken vs Spell\n");
                return WinnerCard.CardPlayerOne;
            }
            else if (this.cardPlayerTwo.race == Race.Kraken)
            {
                Console.WriteLine("Spell vs Kraken\n");
                return WinnerCard.CardPlayerTwo;
            }
            else if (this.cardPlayerOne.race == Race.Knight && this.cardPlayerTwo.element == Element.Water)
            {
                Console.WriteLine("Knight vs WaterSpell\n");
                return WinnerCard.CardPlayerTwo;
            }
            else if (this.cardPlayerTwo.race == Race.Knight && this.cardPlayerOne.element == Element.Water)
            {
                Console.WriteLine("WaterSpell vs Knight\n");
                return WinnerCard.CardPlayerOne;
            }
            else
            {
                return WinnerCard.Draw;
            }
        }

        public void WriteCardInfo(BaseCard card)
        {
            switch (card.category)
            {
                case Category.Spell:
                    {
                        Console.WriteLine("Name: " + card.name + ", Category: " + card.category + ", Element: " + card.element + ", Damage: " + card.damage);
                        break;
                    }

                case Category.Monster:
                    {
                        Console.WriteLine("Name: " + card.name + ", Category: " + card.category + ", Element: " + card.element + ", Race: " + card.race + ", Damage: " + card.damage);
                        break;
                    }
            }
        }
    }
}