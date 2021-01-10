using System;
using System.Collections.Generic;
using System.Text;

namespace MTCG
{
    public class Monstercard : BaseCard
    {
        public Monstercard(double damage, string name, string cardId ,Element element, Race race) : base(damage, name, cardId, element)
        {
            this.race = race;
            this.element = element;
            this.category = Category.Monster;
        }

         public Monstercard(double damage, Element element) : base(damage, element)
        {
            this.element = element;
            this.damage = damage;
        }
    }
}