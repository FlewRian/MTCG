using System;
using System.Collections.Generic;
using System.Text;

namespace MTCG
{
    public class Monstercard : BaseCard
    {
        public Monstercard(int damage, string name, Element element, Race race) : base(damage, name, element)
        {
            this.race = race;
            this.element = element;
            this.category = Category.Monster;
            Console.WriteLine(this.category + this.name + this.damage + this.element + this.race);
        }
    }
}