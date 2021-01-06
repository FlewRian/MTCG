using System;
using System.Collections.Generic;
using System.Text;

namespace MTCG
{
    class Programm
    {
        static public void Main()
        {
            Fight fight = new Fight();
            Arena arena = new Arena();
            List<BaseCard> DeckPlayerOne = new List<BaseCard>();
            List<BaseCard> DeckPlayerTwo = new List<BaseCard>();
            Console.WriteLine("Hallo ich kann was testen");
            Monstercard WaterDragon = new Monstercard(20, "WaterDragon", Element.Water, Race.Dragon);
            Monstercard FireElve = new Monstercard(15, "FireElve", Element.Fire, Race.Elve);
            Monstercard NormalKraken = new Monstercard(25, "NormalKraken", Element.Normal, Race.Kraken);
            Monstercard FireKnight = new Monstercard(15, "FireKnight", Element.Fire, Race.Knight);
            Monstercard NormalGoblin = new Monstercard(22, "NormalGoblin", Element.Normal, Race.Goblin);

            Spellcard WaterSpell = new Spellcard(20, "WaterSpell", Element.Water);
            Spellcard FireSpell = new Spellcard(17, "FireSpell", Element.Fire);
            Spellcard NormalSpell = new Spellcard(15, "NoramlSpell", Element.Normal);

            DeckPlayerOne.Add(WaterDragon);
            DeckPlayerOne.Add(WaterSpell);
            DeckPlayerOne.Add(NormalKraken);
            DeckPlayerOne.Add(NormalSpell);

            DeckPlayerTwo.Add(FireElve);
            DeckPlayerTwo.Add(NormalGoblin);
            DeckPlayerTwo.Add(FireKnight);
            DeckPlayerTwo.Add(FireSpell);

            //Console.WriteLine("Dragon Element: " + waterDragon.element);
            
            //WinnerCard winner =  fight.FightRound(WaterDragon, FireElve);
            
            PlayerEnum winner = arena.Fight(DeckPlayerOne, DeckPlayerTwo);
            Console.WriteLine("Winner:" + winner);
        }
    }
}
