

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BladeAndSoulGossipCards
{
    class Program
    {
        static void Main(string[] args)
        {
            
            
            CardSet set = CardSet.Instance;

            System.Console.WriteLine("劍靈八卦計算器");
            System.Console.WriteLine("讀取資源檔data.txt...");
            set.Load("data.txt");


            var filterSuits = _ShowSuitFilter();

            


            _ShowProperty();


            

            System.Console.Write("\n輸入篩選條件(範例:LIFE GRID_FILE):");
            var data = System.Console.ReadLine();

            System.Console.Write("\n輸出格式1.csv 2.html(預設1):");
            var fileFormat = System.Console.ReadLine();

            System.Console.Write("\n輸出最大筆數:");
            var outCount = System.Console.ReadLine();

            int outAmount = 0;
            int.TryParse(outCount, out outAmount);




            var propertyStrings = data.Split( new char[] {' '}, StringSplitOptions.RemoveEmptyEntries);

            List<PROPERTY_TYPE> propertyTypes = new List<PROPERTY_TYPE>();
            foreach (var propertyString in propertyStrings)
            {
                PROPERTY_TYPE pt ;
                if(Enum.TryParse<PROPERTY_TYPE>(propertyString,true, out pt))
                {
                    propertyTypes.Add(pt);
                }
            }
            

            Property[] propertys = (from pt in propertyTypes select new Property{ Id = pt }).ToArray();
            Card[] cards = set.Find(propertys);
            cards = _FilterCardWithSuit(cards , filterSuits);

            Queue<Card> cards1 = _Find(cards, 1);
            Queue<Card> cards2 = _Find(cards, 2);
            Queue<Card> cards3 = _Find(cards, 3);
            Queue<Card> cards4 = _Find(cards, 4);
            Queue<Card> cards5 = _Find(cards, 5);
            Queue<Card> cards6 = _Find(cards, 6);
            Queue<Card> cards7 = _Find(cards, 7);
            Queue<Card> cards8 = _Find(cards, 8);

            System.Int64 total = cards1.Count() * cards2.Count() * cards3.Count() * cards4.Count() * cards5.Count() * cards6.Count() * cards7.Count() * cards8.Count();
            System.Console.WriteLine(string.Format("{0}筆資料比對中...請稍候", total));
            int count = 0;
            Regulus.Utility.TimeCounter timeCounter = new Regulus.Utility.TimeCounter();

            List<Suit> suits = new List<Suit>();

            
            foreach(var card1 in  cards1)
                foreach(var card2 in  cards2)
                    foreach(var card3 in  cards3)
                        foreach(var card4 in  cards4)
                            foreach(var card5 in  cards5)
                                foreach(var card6 in  cards6)
                                    foreach(var card7 in  cards7)
                                        foreach(var card8 in  cards8)
                                        {
                                            if(timeCounter.Second > 1)
                                            {
                                                System.Console.WriteLine(string.Format("{0}/{1}...", count ,total));

                                                timeCounter.Reset();

                                                var orders = suits.OrderBy((suit) => suit.GetValue(propertys[0]));
                                                foreach (var property in propertys.Skip(1))
                                                {
                                                    orders = orders.ThenBy((suit) => suit.GetValue(property));
                                                }

                                                suits = orders.ToList();

                                                if (suits.Count > outAmount)
                                                    suits.RemoveRange(0, suits.Count - outAmount);

                                            }

                                            var s = new Suit(card1,card2,card3,card4,card5,card6,card7,card8);
                                            suits.Add(s);
                                            count++;
                                            
                                        }




            var result = suits.OrderByDescending((suit) => suit.GetValue(propertys[0]));
            foreach (var property in propertys.Skip(1))
            {
                result = result.ThenByDescending((suit) => suit.GetValue(property));
            }

            suits = result.ToList();

            
            System.Console.WriteLine(string.Format("檔案寫入中..."));

            
            
            if (fileFormat == "1")
                _WriteCSV(suits.ToArray(), outAmount);
            else if (fileFormat == "2")
                _WriteHTML(suits.ToArray(), outAmount);
            else
                _WriteCSV(suits.ToArray(), outAmount);

            
            System.Console.WriteLine("寫入完成.");
            _ShowThank();
            System.Console.ReadKey();
        }

        private static Card[] _FilterCardWithSuit(Card[] cards, string[] filterSuits)
        {
            return (from c in cards where filterSuits.Contains(c.Group) select c).ToArray();
        }

        private static string[] _ShowSuitFilter()
        {
            List<string> suits = new List<string>();
            Dictionary<int , string> menu = new Dictionary<int,string>();
            int no = 1;
            foreach(var name in CardSet.Instance.GetSuitNames())
            {
                suits.Add(name);
                menu.Add(no,name);
                System.Console.Write(string.Format("{0}:{1}\t", no , name));
                no++;
            }

            System.Console.Write("\n選擇八卦牌(範例:1 2):");
            var suitFilters = System.Console.ReadLine();
            if (suitFilters.Length > 0)
                suits.Clear();

            foreach(var num in suitFilters.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
            {
                int n = 0;
                if(int.TryParse(num,out n))
                {
                    string suit = "";
                    if (menu.TryGetValue(n, out suit))
                        suits.Add(suit);
                }
            }

            return suits.ToArray();
        }

        private static void _ShowThank()
        {
            string message =
@"謝謝您的使用

    拔山蓋世 龍裔 
                軒轅與其快樂的夥伴們XD製作
";

            System.Console.WriteLine(message);
        }

        private static void _ShowProperty()
        {
            string message = 
@"ATTACK                  攻擊
PUNCTURE                穿刺
HIT                     命中
FOCUS                   集中
CRIT                    爆擊
SKILLED                 熟練
ADDITIONAL_DAMAGE       額外傷害
THREAT                  威脅
LIFE                    生命
DEFENSE                 防禦
DODGE                   閃避
GRID_FILE               格檔
CRIT_DEFENSE            爆擊防禦
TOUGHNESS               韌性
DAMAGE_REDUCTION        傷害減免
REPLY                   回復
TREATMENT               治療";

            System.Console.WriteLine(message);
        }

        private static void _WriteCSV(Suit[] suits,int out_count)
        {            
            using (var stream = new System.IO.StreamWriter( "output.csv" ,false , Encoding.UTF8))
            {
                stream.WriteLine("攻擊,穿刺,命中,集中,爆擊,熟練,額外傷害,威脅,生命,防禦,閃避,格檔,爆擊防禦,韌性,傷害減免,回復,治療,1,2,3,4,5,6,7,8,");
                foreach (var suit in suits)
                {
                    
                    string line = "";
                    foreach(var val in suit.Values)
                    {
                        line += val.Value + ",";
                    }

                    foreach(var card in suit.Cards)
                    {
                        string proper = "[";
                        foreach (var val in card.Values)
                        {
                            proper += val.Id + "/";
                        }
                        proper = proper.Remove(proper.Length - 1);
                        proper += "]";
                        line += card.Group + proper + ",";
                    }
                    stream.WriteLine(line);

                    if (--out_count == 0)
                        break;
                }
            }
            
        }

        static void _WriteHTML(Suit[] suits, int out_count)
        {
            using (var stream = new System.IO.StreamWriter("output.html", false, Encoding.UTF8))
            {
                stream.WriteLine("<html>");
                stream.WriteLine("<body>");
                stream.WriteLine("<table>");
                stream.WriteLine("<tr><td>攻擊</td><td>穿刺</td><td>命中</td><td>集中</td><td>爆擊</td><td>熟練</td><td>額外傷害</td><td>威脅</td><td>生命</td><td>防禦</td><td>閃避</td><td>格檔</td><td>爆擊防禦</td><td>韌性</td><td>傷害減免</td><td>回復</td><td>治療</td><td>1</td><td>2</td><td>3</td><td>4</td><td>5</td><td>6</td><td>7</td><td>8</td></tr>");
                foreach (var suit in suits)
                {
                    

                    string line = "<tr>";
                    foreach (var val in suit.Values)
                    {
                        line += "<td>" + val.Value + "</td>";
                    }                    

                    foreach (var card in suit.Cards)
                    {
                        string proper = "[";
                        foreach(var val in card.Values)
                        {
                            proper += val.Id + "/";
                        }
                        proper = proper.Remove(proper.Length - 1);
                        proper += "]";
                        line += "<td>" + card.Group + proper + "</td>";
                    }
                    line += "</tr>";
                    stream.WriteLine(line);

                    if (--out_count == 0)
                        break;
                }
                stream.WriteLine("</table>");
                stream.WriteLine("</body>");
                stream.WriteLine("</html>");
            }
        }

        private static Queue<Card> _Find(Card[] cards, int no)
        {
            var cs = (from c in cards where c.No == no select c).ToList();            
            if (cs.Count == 0)
                //return new Card[] { new Card (new PropertyValue[0]){ No = no }};
                return new Queue<Card>();
            return new Queue<Card>(cs);
        }
    }
}

