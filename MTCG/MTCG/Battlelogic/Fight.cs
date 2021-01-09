using System;
using System.Collections.Generic;
using System.Text;

namespace MTCG
{
    public class Fight
    {
        public BaseCard cardPlayerOne { get; set; }
        public BaseCard cardPlayerTwo { get; set; }
        public double currentDamageCardOne { get; set; }
        public double currentDamageCardTwo { get; set; }
        public WinnerCard fightResult { get; set; }

        public Fight()
        {

        }

        public Fight(BaseCard card1, BaseCard card2)
        {
            this.cardPlayerOne = card1;
            this.cardPlayerTwo = card2;
            this.currentDamageCardOne = this.cardPlayerOne.damage;
            this.currentDamageCardTwo = this.cardPlayerTwo.damage;
        }

        public WinnerCard FightRound(BaseCard cardPlayerOne, BaseCard cardPlayerTwo, Arena arena)
        {
            this.cardPlayerOne = cardPlayerOne;
            this.cardPlayerTwo = cardPlayerTwo;

            switch (this.cardPlayerOne.category)
            {

                case Category.Monster when this.cardPlayerTwo.category == Category.Monster:
                    {
                        this.currentDamageCardOne = this.cardPlayerOne.damage;
                        this.currentDamageCardTwo = this.cardPlayerTwo.damage;
                        arena.ArenaLog.AddTextToLog("Fight between two Monsters!");
                        switch (this.cardPlayerOne.race)
                        {
                            case Race.Dragon when this.cardPlayerTwo.race == Race.Goblin:
                                {
                                    arena.ArenaLog.AddTextToLog("Goblin from " + arena.playerTwo.Username.ToUpper() + " is too afraid to attack the Dragon from " + arena.playerOne.Username.ToUpper());
                                    this.fightResult = WinnerCard.CardPlayerOne;
                                    break;
                                }
                            case Race.Goblin when this.cardPlayerTwo.race == Race.Dragon:
                                {
                                    arena.ArenaLog.AddTextToLog("Goblin from " + arena.playerOne.Username.ToUpper() + " is too afraid to attack the Dragon from " + arena.playerTwo.Username.ToUpper());
                                    this.fightResult = WinnerCard.CardPlayerTwo;
                                    break;
                                }
                            case Race.Wizzard when this.cardPlayerTwo.race == Race.Orc:
                                {
                                    arena.ArenaLog.AddTextToLog("Wizzard from " + arena.playerOne.Username.ToUpper() + " controls the Orc from " + arena.playerTwo.Username.ToUpper());
                                    this.fightResult = WinnerCard.CardPlayerOne;
                                    break;
                                }
                            case Race.Orc when this.cardPlayerTwo.race == Race.Wizzard:
                                {
                                    arena.ArenaLog.AddTextToLog("Wizzard from " + arena.playerTwo.Username.ToUpper() + " controls the Orc from " + arena.playerOne.Username.ToUpper());
                                    this.fightResult = WinnerCard.CardPlayerTwo;
                                    break;
                                }
                            case Race.Elve when this.cardPlayerTwo.race == Race.Dragon:
                                {
                                    if (this.cardPlayerOne.element == Element.Fire)
                                    {
                                        arena.ArenaLog.AddTextToLog("FireElve from " + arena.playerOne.Username.ToUpper() + " evede the attack from " + arena.playerTwo.Username.ToUpper() + "'s Dragon");
                                        this.fightResult = WinnerCard.CardPlayerOne;
                                        break;
                                    }
                                    else
                                    {
                                        this.fightResult = CheckWinner(arena);
                                        break;
                                    }
                                }
                            case Race.Dragon when this.cardPlayerTwo.race == Race.Elve:
                                {
                                    if (this.cardPlayerTwo.element == Element.Fire)
                                    {
                                        arena.ArenaLog.AddTextToLog("FireElve from " + arena.playerTwo.Username.ToUpper() + " evede the attack from " + arena.playerOne.Username.ToUpper() + "'s Dragon");
                                        this.fightResult = WinnerCard.CardPlayerTwo;
                                        break;
                                    }
                                    else
                                    {
                                        this.fightResult = CheckWinner(arena);
                                        break;
                                    }
                                }
                            default:
                                {
                                    this.fightResult = CheckWinner(arena);
                                    break;
                                }
                        }
                        return this.fightResult;
                    }

                case Category.Monster when this.cardPlayerTwo.category == Category.Spell:
                    {
                        arena.ArenaLog.AddTextToLog("Fight between one Monster and one Spell!");
                        fightResult = MonsterVsSpell(arena);
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
                                    this.fightResult = CheckWinner(arena);
                                    break;
                                }
                        }
                        return this.fightResult;
                    }

                case Category.Spell when this.cardPlayerTwo.category == Category.Monster:
                    {
                        arena.ArenaLog.AddTextToLog("Fight between one Spell and one Monster!");
                        fightResult = MonsterVsSpell(arena);
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
                                    this.fightResult = CheckWinner(arena);
                                    break;
                                }
                        }
                        return this.fightResult;
                    }
                case Category.Spell when this.cardPlayerTwo.category == Category.Spell:
                    {
                        arena.ArenaLog.AddTextToLog("Fight between two Spells!");
                        CheckElement();
                        this.fightResult = CheckWinner(arena);
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
                        this.currentDamageCardTwo = this.cardPlayerTwo.damage * 0.5;
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
                        this.currentDamageCardOne = this.cardPlayerOne.damage;
                        this.currentDamageCardTwo = this.cardPlayerTwo.damage;
                        break;
                    }
            }
        }

        public WinnerCard CheckWinner(Arena arena)
        {
            if (this.currentDamageCardOne > this.currentDamageCardTwo)
            {
                //cardPlayerOne wins
                arena.ArenaLog.AddTextToLog(arena.playerOne.Username.ToUpper() + ": " + cardPlayerOne.name + " with " + this.currentDamageCardOne + " Damage fights against " + arena.playerTwo.Username.ToUpper() + ": " + cardPlayerTwo.name + " with " + this.currentDamageCardTwo);
                arena.ArenaLog.AddTextToLog(arena.playerOne.Username.ToUpper() + " wins this Round!");
                return WinnerCard.CardPlayerOne;
            }
            else if (this.currentDamageCardOne < this.currentDamageCardTwo)
            {
                //cardPlayerTwo wins
                arena.ArenaLog.AddTextToLog(arena.playerOne.Username.ToUpper() + ": " + cardPlayerOne.name + " with " + this.currentDamageCardOne + " Damage fights against " + arena.playerTwo.Username.ToUpper() + ": " + cardPlayerTwo.name + " with " + this.currentDamageCardTwo);
                arena.ArenaLog.AddTextToLog(arena.playerTwo.Username.ToUpper() + " wins this Round!");
                return WinnerCard.CardPlayerTwo;
            }
            else
            {
                //draw
                arena.ArenaLog.AddTextToLog(arena.playerOne.Username.ToUpper() + ": " + cardPlayerOne.name + " with " + this.currentDamageCardOne + " Damage fights against " + arena.playerTwo.Username.ToUpper() + ": " + cardPlayerTwo.name + " with " + this.currentDamageCardTwo);
                arena.ArenaLog.AddTextToLog("Draw!");
                return WinnerCard.Draw;
            }
        }

        public WinnerCard MonsterVsSpell(Arena arena)
        {
            if (this.cardPlayerOne.race == Race.Kraken)
            {
                arena.ArenaLog.AddTextToLog("Kraken from " + arena.playerOne.Username.ToUpper() + " is immune against spell from " + arena.playerTwo.Username.ToUpper());
                return WinnerCard.CardPlayerOne;
            }
            else if (this.cardPlayerTwo.race == Race.Kraken)
            {
                arena.ArenaLog.AddTextToLog("Kraken from " + arena.playerTwo.Username.ToUpper() + " is immune against spell from " + arena.playerOne.Username.ToUpper());
                return WinnerCard.CardPlayerTwo;
            }
            else if (this.cardPlayerOne.race == Race.Knight && this.cardPlayerTwo.element == Element.Water)
            {
                arena.ArenaLog.AddTextToLog("Knight from " + arena.playerOne.Username.ToUpper() + " gets drowned by WaterSpell from " + arena.playerTwo.Username.ToUpper());
                return WinnerCard.CardPlayerTwo;
            }
            else if (this.cardPlayerTwo.race == Race.Knight && this.cardPlayerOne.element == Element.Water)
            {
                arena.ArenaLog.AddTextToLog("Knight from " + arena.playerTwo.Username.ToUpper() + " gets drowned by WaterSpell from " + arena.playerOne.Username.ToUpper());
                return WinnerCard.CardPlayerOne;
            }
            else
            {
                return WinnerCard.Draw;
            }
        }

        /*public void WriteCardInfo(BaseCard card)
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
        }*/
    }
}