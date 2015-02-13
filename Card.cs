using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BladeAndSoulGossipCards
{
    class Card
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

        public PropertyValue[] Values { get { return _PropertyValues; } }
        

        public Card(PropertyValue[] property_value)
        {            
            _PropertyValues = property_value;
        }

        internal int GetValue(Property property)
        {
            return _PropertyValues.GetValue(property);
        }
    }
}
