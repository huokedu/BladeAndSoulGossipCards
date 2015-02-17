using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BladeAndSoulGossipCards
{
    class Suit
    {
        private Card[] _Cards;

        public Card[] Cards { get {return _Cards;}}

        PropertyValue[] _Values;

        public int MaxAppreciation { get; private set; }

        public PropertyValue[] Values { get { return _Values; } }
        public Suit(params Card[] cards)
        {
            _Cards = cards;
            List<PropertyValue> values = new List<PropertyValue>();
            foreach (PROPERTY_TYPE t in Enum.GetValues(typeof(PROPERTY_TYPE)))
            {
                var value = new PropertyValue { Id = t, Value = _GetValue(new Property { Id = t }) };
                values.Add(value);
            }
            MaxAppreciation = _GetAppreciation(cards);
            _Values = values.ToArray();

        }

        private int _GetAppreciation(Card[] cards)
        {
            return (from card in cards select card.MaxAppreciation).Sum();
        }
        internal int GetValue(Property property)
        {
            return (from v in _Values where v.Id == property.Id select v.Value).SingleOrDefault();
        }

        internal int _GetValue(Property property)
        {
            var sum = (from card in _Cards
                         let value = card.GetValue(property)
                         select value).Sum();

            var groups = (from card in _Cards
                          group card by card.Group into g
                          select new { Id = g.Key, Count = g.Count() }).ToArray();

            var gsum = (from g in groups select CardSet.Instance.GetValue(property, g.Id, g.Count)).Sum();

            return sum + gsum;
        }
    }
}
