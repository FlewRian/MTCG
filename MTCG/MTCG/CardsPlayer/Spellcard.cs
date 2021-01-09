using System;
using System.Collections.Generic;
using System.Text;

namespace MTCG
{
    public class Spellcard : BaseCard
    {
        public Spellcard(double damage, string name, string cardId ,Element element) : base(damage, name, cardId ,element)
        {
            this.element = element;
            this.category = Category.Spell;
            //Console.WriteLine(this.category + this.name + this.damage + this.element);
        }
    }
}