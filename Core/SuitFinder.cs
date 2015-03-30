using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BladeAndSoulGossipCards
{
    public class SuitFinder
    {
        public delegate void ProgressCallback(int count ,long total , int found );
        public event ProgressCallback ProgressEvent;
        Card[][] _CardSets;
        long _TotalCards;
        int[] _BaseUnits;
        public SuitFinder(params Card[][] cardSets)
        {
            _CardSets = cardSets;
            _TotalCards = _Multiply((from cardSet in _CardSets select cardSet.Length));

            _BuildBaseUint();
        }

        private void _BuildBaseUint()
        {
            _BaseUnits = new int[_CardSets.Length];
            for (int i = 0; i < _BaseUnits.Length; ++i)
            {
                _BaseUnits[i] = _CardSetCount(i + 1);
            }
        }

        public List<Suit> Find(PropertyValue[] filter_propertys,int out_amount )
        {

            
            object suitSetLock = new object();
            List<Suit> suitSet = new List<Suit>();
            

            var rangePartitioner = System.Collections.Concurrent.Partitioner.Create(0, _Total());

            object readLock = new object();
            int reads = 0;
            System.Threading.SpinWait sw = new System.Threading.SpinWait();
            Parallel.ForEach(rangePartitioner, (range, loopState) =>
            {
                
                Regulus.Utility.TimeCounter time = new Regulus.Utility.TimeCounter();
                List<Suit> suits = new List<Suit>();
                int count = 0;
                //var range = new {Item1 = 0 , Item2 = _Total()};
                for (long i = range.Item2 - 1; i >= range.Item1; --i, ++count)
                {
                    
                    int[] indexs = _GetIndexs(i);
                    var s = new Suit(
                        _GetCard(0, indexs[0]),
                        _GetCard(1, indexs[1]), 
                        _GetCard(2, indexs[2]), 
                        _GetCard(3, indexs[3]), 
                        _GetCard(4, indexs[4]), 
                        _GetCard(5, indexs[5]), 
                        _GetCard(6, indexs[6]), 
                        _GetCard(7, indexs[7]));

                    bool pass = (from filter in filter_propertys
                                 where s.GetValue(filter) < filter.Value
                                 select false).Count() == 0;
                    sw.SpinOnce();
                    if (pass)
                    {
                        suits.Add(s);
                        
                    }

                    if (time.Second > 1)
                    {
                        lock (readLock)
                        {
                            reads += count;
                            count = 0;
                        }

                        _UpdateSet(ref suitSet, filter_propertys, out_amount, suitSetLock, reads, suits);
                        suits.Clear();
                        time.Reset();

                        sw.SpinOnce();
                    }
                    
                }

                lock (readLock)
                {
                    reads += count;
                    count = 0;
                }
                _UpdateSet(ref suitSet ,filter_propertys, out_amount, suitSetLock, reads, suits);
            });




            var result = suitSet.OrderByDescending((suit) => suit.GetValue(filter_propertys[0]));
            foreach (var property in filter_propertys.Skip(1))
            {
                result = result.ThenByDescending((suit) => suit.GetValue(property));
            }

            return result.ToList();
        }

        private void _UpdateSet(ref List<Suit> suitSet ,PropertyValue[] filter_propertys, int out_amount, object suitSetLock, int count, List<Suit> suits)
        {            
            lock (suitSetLock)
            {
                
                suitSet.AddRange(suits);

                var orders = suitSet.OrderBy((suit) => suit.GetValue(filter_propertys[0]));
                foreach (var property in filter_propertys.Skip(1))
                {
                    orders = orders.ThenBy((suit) => suit.GetValue(property));
                }


                suitSet = orders.ToList();

                if (suitSet.Count > out_amount)
                    suitSet.RemoveRange(0, suitSet.Count - out_amount);                

                if (ProgressEvent != null)
                    ProgressEvent(count, _Total(), suitSet.Count);
            }            
        }

        private Card _GetCard(int set_index, int card_index)
        {
            return _CardSets[set_index][card_index];
        }

        private int[] _GetIndexs(long count)
        {
            var len = _CardSets.Length;
            var last = count;
            var indexs = new int[len];
            for (int i = 0; i < len; ++i)
            {
                var total = _GetBaseUnit(i);
                if (total > 0)
                {
                    indexs[i] = (int)last / total;
                    last = last % total;
                }
                else
                {
                    indexs[i] = (int)last;
                }
                
            }
            return indexs;
        }

        private int _GetBaseUnit(int i)
        {
            return _BaseUnits[i];
        }

        private int _CardSetCount(int skip)
        {
            if(skip < _CardSets.Length)
                return _Multiply((from cardSet in _CardSets.Skip(skip) select cardSet.Length));
            return 0;
        }

        private int _Multiply(IEnumerable<int> enumerable) 
        {
            int number = enumerable.First();
            foreach(var e in enumerable.Skip(1))
            {
                number *= e;
            }
            return number;
        }

        private long _Total()
        {            
            return _TotalCards;
        }
    }
}
