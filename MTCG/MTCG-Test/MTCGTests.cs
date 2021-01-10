using MTCG;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTCG_Test
{
    class MTCGTests
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void MonsterCheckElement()
        {
            Monstercard normalKraken = new Monstercard(25, "NormalKraken", "3", Element.Normal, Race.Kraken);
            Monstercard waterDragon = new Monstercard(20, "WaterDragon", "1", Element.Water, Race.Dragon);
            Monstercard fireElve = new Monstercard(15, "FireElve", "2", Element.Fire, Race.Elve);

            var elementNormal = normalKraken.element;
            var elementWater = waterDragon.element;
            var elementFire = fireElve.element;

            Assert.AreEqual(Element.Normal, elementNormal);
            Assert.AreEqual(Element.Fire, elementFire);
            Assert.AreEqual(Element.Water, elementWater);
        }

        [Test]
        public void SpellCheckElement()
        {
            Spellcard waterSpell = new Spellcard(20, "WaterSpell", "6", Element.Water);
            Spellcard fireSpell = new Spellcard(17, "FireSpell", "7", Element.Fire);
            Spellcard normalSpell = new Spellcard(15, "NoramlSpell", "8", Element.Normal);

            var elementNormal = normalSpell.element;
            var elementWater = waterSpell.element;
            var elementFire = fireSpell.element;

            Assert.AreEqual(Element.Normal, elementNormal);
            Assert.AreEqual(Element.Fire, elementFire);
            Assert.AreEqual(Element.Water, elementWater);
        }

        [Test]
        public void FightMonsterVsMonster()
        {
            Monstercard normalKraken = new Monstercard(25, "NormalKraken", "3", Element.Normal, Race.Kraken);
            Monstercard fireKnight = new Monstercard(15, "FireKnight", "4", Element.Fire, Race.Knight);
            Player player1 = new Player("Herbert");
            Player player2 = new Player("Günther");
            Fight fight = new Fight(normalKraken, fireKnight);
            Arena arena = new Arena(player1, player2);

            var winner = fight.CheckWinner(arena);

            Assert.AreEqual(WinnerCard.CardPlayerOne, winner);
        }

        [Test]
        public void FightMonsterVsSpellSameElement()
        {
            Spellcard normalSpell = new Spellcard(15, "NormalSpell", "1", Element.Normal);
            Monstercard normalKnight = new Monstercard(15, "NormalKnight", "2", Element.Normal, Race.Knight);
            Spellcard fireSpell = new Spellcard(20, "FireSpell", "3", Element.Fire);
            Monstercard fireKnight = new Monstercard(15, "FireKnight", "4", Element.Fire, Race.Knight);
            Spellcard waterSpell = new Spellcard(15, "WaterSpell", "5", Element.Water);
            Monstercard waterDragon = new Monstercard(16, "WaterDragon", "6", Element.Water, Race.Dragon);
            Player player1 = new Player("Herbert");
            Player player2 = new Player("Günther");
            Fight fight = new Fight();
            Arena arena = new Arena(player1, player2);

            var winner1 = fight.FightRound(normalSpell, normalKnight, arena);
            var winner2 = fight.FightRound(fireSpell, fireKnight, arena);
            var winner3 = fight.FightRound(waterSpell, waterDragon, arena);
            var winner4 = fight.FightRound(normalKnight, normalSpell, arena);
            var winner5 = fight.FightRound(fireKnight, fireSpell, arena);
            var winner6 = fight.FightRound(waterDragon, waterSpell, arena);

            Assert.AreEqual(WinnerCard.Draw, winner1);
            Assert.AreEqual(WinnerCard.CardPlayerOne, winner2);
            Assert.AreEqual(WinnerCard.CardPlayerTwo, winner3);
            Assert.AreEqual(WinnerCard.Draw, winner4);
            Assert.AreEqual(WinnerCard.CardPlayerTwo, winner5);
            Assert.AreEqual(WinnerCard.CardPlayerOne, winner6);
        }

        [Test]
        public void FightMonsterVsSpellDifferentElement()
        {
            Spellcard normalSpell = new Spellcard(15, "NormalSpell", "1", Element.Normal);
            Monstercard normalWizzard = new Monstercard(15, "NormalWizzard", "2", Element.Normal, Race.Wizzard);
            Spellcard fireSpell = new Spellcard(15, "FireSpell", "3", Element.Fire);
            Monstercard fireWizzard = new Monstercard(15, "FireWizzard", "4", Element.Fire, Race.Wizzard);
            Spellcard waterSpell = new Spellcard(15, "WaterSpell", "5", Element.Water);
            Monstercard waterWizzard = new Monstercard(15, "WaterWizzard", "6", Element.Water, Race.Wizzard);
            Player player1 = new Player("Herbert");
            Player player2 = new Player("Günther");
            Fight fight = new Fight();
            Arena arena = new Arena(player1, player2);

            var winner1 = fight.FightRound(normalSpell, waterWizzard, arena);
            var winner2 = fight.FightRound(fireSpell, normalWizzard, arena);
            var winner3 = fight.FightRound(waterSpell, fireWizzard, arena);
            var winner4 = fight.FightRound(waterWizzard, normalSpell, arena);
            var winner5 = fight.FightRound(normalWizzard, fireSpell, arena);
            var winner6 = fight.FightRound(fireWizzard, waterSpell, arena);

            Assert.AreEqual(WinnerCard.CardPlayerOne, winner1);
            Assert.AreEqual(WinnerCard.CardPlayerOne, winner2);
            Assert.AreEqual(WinnerCard.CardPlayerOne, winner3);
            Assert.AreEqual(WinnerCard.CardPlayerTwo, winner4);
            Assert.AreEqual(WinnerCard.CardPlayerTwo, winner5);
            Assert.AreEqual(WinnerCard.CardPlayerTwo, winner6);
        }

        [Test]
        public void FightMonsterVsMonsterSpezial()
        {
            Monstercard normalWizzard = new Monstercard(15, "NormalWizzard", "1", Element.Normal, Race.Wizzard);
            Monstercard normalOrc = new Monstercard(15, "NormalWizzard", "2", Element.Normal, Race.Orc);
            Monstercard fireElve = new Monstercard(15, "FireElve", "3", Element.Fire, Race.Elve);
            Monstercard fireDargon = new Monstercard(15, "FireDragon", "4", Element.Fire, Race.Dragon);
            Player player1 = new Player("Herbert");
            Player player2 = new Player("Günther");
            Fight fight = new Fight();
            Arena arena = new Arena(player1, player2);

            var winner1 = fight.FightRound(fireElve, fireDargon, arena);
            var winner2 = fight.FightRound(normalWizzard, normalOrc, arena);

            Assert.AreEqual(WinnerCard.CardPlayerOne, winner1);
            Assert.AreEqual(WinnerCard.CardPlayerOne, winner2);
        }

        [Test]
        public void FightMonsterVsSpellSpezial()
        {
            Monstercard normalKraken = new Monstercard(25, "NormalKraken", "3", Element.Normal, Race.Kraken);
            Monstercard fireKnight = new Monstercard(15, "FireKnight", "4", Element.Fire, Race.Knight);
            Spellcard waterSpell = new Spellcard(20, "WaterSpell", "6", Element.Water);
            Spellcard fireSpell = new Spellcard(17, "FireSpell", "7", Element.Fire);
            Spellcard normalSpell = new Spellcard(15, "NoramlSpell", "8", Element.Normal);
            Player player1 = new Player("Herbert");
            Player player2 = new Player("Günther");
            Fight fight = new Fight();
            Arena arena = new Arena(player1, player2);

            var winner1 = fight.FightRound(normalKraken, normalSpell, arena);
            var winner2 = fight.FightRound(normalKraken, fireSpell, arena);
            var winner3 = fight.FightRound(normalKraken, waterSpell, arena);
            var winner4 = fight.FightRound(fireKnight, waterSpell, arena);

            Assert.AreEqual(WinnerCard.CardPlayerOne, winner1);
            Assert.AreEqual(WinnerCard.CardPlayerOne, winner2);
            Assert.AreEqual(WinnerCard.CardPlayerOne, winner3);
            Assert.AreEqual(WinnerCard.CardPlayerTwo, winner4);
        }

        [Test]
        public void JsonCardTestMonsterFireDragon()
        {
            var card = new JsonCard();
            card.Id = "1";
            card.Name = "FireDragon";
            card.Damage = 20;
            card.ConvertToCard();

            Assert.AreEqual(card.category, Category.Monster);
            Assert.AreEqual(card.elementType, Element.Fire);
            Assert.AreEqual(card.race, Race.Dragon);
        }

        [Test]
        public void JsonCardTestMonsterKraken()
        {
            var card = new JsonCard();
            card.Id = "3";
            card.Name = "Kraken";
            card.Damage = 25;
            card.ConvertToCard();

            Assert.AreEqual(card.category, Category.Monster);
            Assert.AreEqual(card.elementType, Element.Normal);
            Assert.AreEqual(card.race, Race.Kraken);
        }

        [Test]
        public void JsonCardTestMonsterWaterWizzard()
        {
            var card = new JsonCard();
            card.Id = "3";
            card.Name = "WaterWizzard";
            card.Damage = 30;
            card.ConvertToCard();

            Assert.AreEqual(card.category, Category.Monster);
            Assert.AreEqual(card.elementType, Element.Water);
            Assert.AreEqual(card.race, Race.Wizzard);
        }

        [Test]
         public void JsonCardTestMonsterRegularSpell()
        {
            var card = new JsonCard();
            card.Id = "4";
            card.Name = "RegularSpell";
            card.Damage = 30;
            card.ConvertToCard();

            Assert.AreEqual(card.category, Category.Spell);
            Assert.AreEqual(card.elementType, Element.Normal);
        }

        [Test]
         public void JsonCardTestMonsterWaterSpell()
        {
            var card = new JsonCard();
            card.Id = "5";
            card.Name = "WaterSpell";
            card.Damage = 35;
            card.ConvertToCard();

            Assert.AreEqual(card.category, Category.Spell);
            Assert.AreEqual(card.elementType, Element.Water);
        }

        [Test]
         public void JsonCardTestMonsterFireSpell()
        {
            var card = new JsonCard();
            card.Id = "6";
            card.Name = "FireSpell";
            card.Damage = 12;
            card.ConvertToCard();

            Assert.AreEqual(card.category, Category.Spell);
            Assert.AreEqual(card.elementType, Element.Fire);
        }

        [Test]
        public void TestCheckElementCurrentDamage()
        {
            var card1 = new Spellcard(20, Element.Normal);
            var card2 = new Spellcard(14, Element.Fire);
            var card3 = new Spellcard(30, Element.Water);
            var fight1 = new Fight(card1, card2);
            var fight2 = new Fight(card2, card3);
            var fight3 = new Fight(card3, card1);
            
            fight1.CheckElement();
            fight2.CheckElement();
            fight3.CheckElement();

            Assert.AreEqual(fight1.currentDamageCardOne, 10);
            Assert.AreEqual(fight1.currentDamageCardTwo, 28);
            Assert.AreEqual(fight2.currentDamageCardOne, 7);
            Assert.AreEqual(fight2.currentDamageCardTwo, 60);
            Assert.AreEqual(fight3.currentDamageCardOne, 15);
            Assert.AreEqual(fight3.currentDamageCardTwo, 40);
        }
    }
}
