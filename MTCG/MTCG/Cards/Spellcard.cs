using System;
using System.Collections.Generic;
using System.Text;

namespace MTCG
{
    public class Spellcard : BaseCard
    {
        public Spellcard(int damage, string name, Element element) : base(damage, name, element)
        {
            this.element = element;
            this.category = Category.Spell;
            Console.WriteLine(this.category + this.name + this.damage + this.element);
        }
    }
}