using System;
using System.Collections.Generic;
using System.Text;

namespace MTCG
{
    public abstract class BaseCard
    {
        public Element element { get; set; }
        public Category category { get; set; }
        public double damage { get; set; }
        public string name { get; set; }
        public Race race { get; set; }
        public string cardId { get; set; }
        
        public BaseCard(int damage, string name, string cardId)
        {
            this.damage = damage;
            this.name = name;
            this.cardId = cardId;
        }

        public BaseCard(int damage, string name, string cardId ,Element element) :this(damage, name, cardId)
        {
            this.element = element;
        }
    }
}