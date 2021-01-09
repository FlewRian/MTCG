using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace MTCG
{
    public class JsonCard
    {
        [JsonProperty("Id")]
        public string Id { get; set; }
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("Damage")]
        public double Damage { get; set; }
        public Race race { get; set; }
        public Category category { get; set; }

        public BaseCard ConvertToCard()
        {
            Element elementType = Element.Normal;
            if (Regex.IsMatch(Name, "Fire.*"))
                elementType = Element.Fire;
            if (Regex.IsMatch(Name, "Water.*"))
                elementType = Element.Water;

            if (Regex.IsMatch(Name, ".*Goblin"))
            {
                race = Race.Goblin;
                return new Monstercard(Damage, Name, Id, elementType, race);
            }
            if (Regex.IsMatch(Name, ".*Ork"))
            {
                race = Race.Orc;
                return new Monstercard(Damage, Name, Id, elementType, race);
            }
            if (Regex.IsMatch(Name, ".*Dragon"))
            {
                race = Race.Dragon;
                return new Monstercard(Damage, Name, Id, elementType, race);
            }
            if (Regex.IsMatch(Name, ".*Knight"))
            {
                race = Race.Knight;
                return new Monstercard(Damage, Name, Id, elementType, race);
            }
            if (Regex.IsMatch(Name, ".*Wizzard"))
            {
                race = Race.Wizzard;
                return new Monstercard(Damage, Name, Id, elementType, race);
            }
            if (Regex.IsMatch(Name, ".*Elf"))
            {
                race = Race.Elve;
                return new Monstercard(Damage, Name, Id, elementType, race);
            }
            if (Regex.IsMatch(Name, ".*Kraken"))
            {
                race = Race.Kraken;
                return new Monstercard(Damage, Name, Id, elementType, race);
            }
            if (Regex.IsMatch(Name, ".*Spell"))
            {
                category = Category.Spell;
                return new Spellcard(Damage, Name, Id, elementType);
            }
            return null;
        }
    }
}