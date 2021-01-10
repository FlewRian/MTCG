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
        }

        public Spellcard(double damage, Element element) : base(damage, element)
        {
            this.element = element;
            this.damage = damage;
        }
    }
}