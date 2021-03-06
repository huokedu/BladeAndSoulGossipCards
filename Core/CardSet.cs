﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BladeAndSoulGossipCards
{
    public class CardSet : Regulus.Utility.Singleton<CardSet>
    {
        SetEffect[] _Effects;
        Card[] _Cards;

        public Card[] Cards { get {return _Cards;}}

        public  IEnumerable<Card> Filter(Card[] cards , Property[] propertys)
        {
            Card[] suitCards = _FindPropertyInSuit(cards, propertys);
            var resultCards = _FindProperty(cards, propertys);
            return suitCards.Union(resultCards);
        }
        public Card[] Find(Property[] propertys, string[] filterSuits)
        {

            var cards = _FilterCardWithSuit(_Cards , filterSuits);
            var suitCards = _FindPropertyInSuit(cards , propertys);
            cards = _FindProperty(cards,propertys);
            return suitCards.Union(cards).ToArray();
        }

        private static IEnumerable< Card> _FilterCardWithSuit(Card[] cards, string[] filterSuits)
        {
            return (from c in cards where filterSuits.Contains(c.Group) select c);
        }

        private static List<Card> _FindProperty(  IEnumerable<Card> cardset , Property[] propertys)
        {
            List<Card> cards = new List<Card>();
            foreach (var property in propertys)
            {
                for(int no = 1 ; no <= 8 ; ++no)
                {
                    var c = (from card in cardset
                             let val = card.GetValue(property)                             
                             where card.No == no 
                             orderby val descending
                             select card).FirstOrDefault();
                    if (c != null)
                        cards.Add(c);
                }
                
            }
            return cards;
        }

        private Card[] _FindPropertyInSuit( IEnumerable<Card> cardset,Property[] propertys)
        {
            Card[] totalCards = new Card[0];
            foreach (var property in propertys)
            {
                List<Card> cards = new List<Card>();
                var suits = from e in _Effects
                           where e.GetValue(property) > 0
                           select e.Id;
                foreach(var suit in suits)
                {
                    for(int no = 1 ; no <= 8 ; ++ no)
                    {
                        var card = (from c in cardset
                                    let val = c.GetValue(property)
                                    orderby val descending
                                    where c.Group == suit && c.No == no
                                    select c).FirstOrDefault();

                        if (card != null)
                            cards.Add(card);
                    }
                }
                totalCards = totalCards.Union(cards).ToArray();
            }
            return totalCards;
        }

        private bool _HasProperty(Card card, Property[] propertys)
        {
            foreach(var property in propertys)
            {                
                if (card.HasProperty(property))
                    return true;
            }
            return false;
        }

        internal Card[] Find(int no)
        {
            return (from card in _Cards where card.No == no select card).ToArray();
        }



        internal int GetValue(Property property, string group_id, int count)
        {
            return (from effect in _Effects where effect.Id == group_id && count >= effect.Count select effect.GetValue(property)).Sum();
        }

        internal bool HasProperty(string id, Property property)
        {
            return (from effect in _Effects where effect.Id == id select effect.GetValue(property)).Sum() > 0;
        }

        public void Load(string text)
        {
            
            var cardsuits = Newtonsoft.Json.JsonConvert.DeserializeObject<CardSuit[]>(text);

            _Build(cardsuits);
        }

        public void Build(string stream)
        {
            var cardsuits = Newtonsoft.Json.JsonConvert.DeserializeObject<CardSuit[]>(stream);

            _Build(cardsuits);
        }

        private void _Build(CardSuit[] cardsuits)
        {
            List<Card> cards = new List<Card>();
            List<SetEffect> setEffects = new List<SetEffect>();
            foreach(var suit in cardsuits)
            {
                foreach(var e in suit.Effects)
                {
                    var setEffect = new SetEffect(e.Propertys) { Id = suit.Name, Count = e.Count };
                    setEffects.Add(setEffect);
                }
                
                foreach(var c in  suit.Cards)
                {
                    var card = new Card(c.Propertys, c.MaxAppreciation) { No = c.No, Group = suit.Name };
                    cards.Add(card);
                }

                _Cards = cards.ToArray();
                _Effects = setEffects.ToArray();
            }
        }

        public  IEnumerable<string> GetSuitNames()
        {
            foreach(var effect in from e in  _Effects group e by e.Id into g select g)
            {
                yield return effect.Key;
            }            
        }



        public Card GetEmpty(int num)
        {
            var empty = new Card( new PropertyValue[0]);
            empty.No = num;
            return empty;
        }

        
    }
}

