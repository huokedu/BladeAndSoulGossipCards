using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Regulus.Extension;
namespace BladeAndSoulGossipCards
{
    public class Card
    {
        internal bool HasProperty(Property property)
        {        
            foreach(var val in _PropertyValues)
            {
                if (val.Id == property.Id)
                    return true;
            }
            return false;
        }

        PropertyValue[] _PropertyValues;
        public int No;
        public string Group;
        
        public int MaxAppreciation;

        public PropertyValue[] Values { get { return _PropertyValues; } }
        

        public Card(PropertyValue[] property_value)
        {            
            _PropertyValues = property_value;
        }

        public Card(PropertyValue[] propertyValue, int max_appreciation)
            : this(propertyValue)
        {            
            
            this.MaxAppreciation = max_appreciation;
        }

        internal int GetValue(Property property)
        {
            return _PropertyValues.GetValue(property);
        }

        public string ToDescription()
        {
            string proper = "[";
            foreach (var val in Values)
            {
                proper += string.Format("{0}({1})/", val.Id.GetEnumDescription(), val.Value);
            }
            proper = proper.Remove(proper.Length - 1);
            proper += "]";

            return Group + proper;
        }
    }

    namespace Extend
    {
        public static class CardExtend
        {
            public static Card[] Assort(this Card[] cards, int no)
            {
                var cs = (from c in cards where c.No == no select c);
                return cs.ToArray();
            }

            public static Card[] Fill(this Card[] cards, CardSet cardSet, int num)
            {
                if (cards.Length == 0)
                {
                    return new Card[] { CardSet.Instance.GetEmpty(num) };
                }
                return cards;
            }
        }

        
    }
}
