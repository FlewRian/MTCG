using System;
using System.Collections.Generic;
using System.Text;

namespace MTCG
{
    public abstract class BaseCard
    {
        public Element element { get; set; }
        public Category category { get; set; }
        public int damage { get; set; }
        public string name { get; set; }
        public Race race { get; set; }
        
        public BaseCard(int damage, string name)
        {
            this.damage = damage;
            this.name = name;
        }

        public BaseCard(int damage, string name, Element element) :this(damage, name)
        {
            this.element = element;
        }
    }
}