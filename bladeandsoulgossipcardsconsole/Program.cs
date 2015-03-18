

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BladeAndSoulGossipCards
{
    using Regulus.Extension;
    using Extend;
    
    class Program
    {
        static void Main(string[] args)
        {
            
            
            CardSet set = CardSet.Instance;

            System.Console.WriteLine("劍靈八卦牌計算器");
            System.Console.WriteLine("讀取資源檔data.txt...");
            
            try
            {
                set.Load("data.txt");    
            }
            catch
            {
                System.Console.WriteLine("找不到data.txt");
                return;
            }


            var filterSuits = _ShowSuitFilter();

            PropertyValue[] filterPropertys = _InputProperty();

            System.Console.Write("\n輸出格式1.csv 2.html(預設1):");
            var fileFormat = System.Console.ReadLine();

            System.Console.Write("\n輸出最大筆數:");
            var outCount = System.Console.ReadLine();

            int outAmount = 0;
            int.TryParse(outCount, out outAmount);


            Property[] propertys = filterPropertys.ToArray();
            Card[] cards = set.Find(propertys, filterSuits);            

            Card[] cards1 = cards.Assort(1).Fill( CardSet.Instance, 1);
            Card[] cards2 = cards.Assort(2).Fill(CardSet.Instance, 2);
            Card[] cards3 = cards.Assort(3).Fill(CardSet.Instance, 3);
            Card[] cards4 = cards.Assort(4).Fill(CardSet.Instance, 4);
            Card[] cards5 = cards.Assort(5).Fill(CardSet.Instance, 5);
            Card[] cards6 = cards.Assort(6).Fill(CardSet.Instance, 6);
            Card[] cards7 = cards.Assort(7).Fill(CardSet.Instance, 7);
            Card[] cards8 = cards.Assort(8).Fill(CardSet.Instance, 8);

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

                                            bool pass = (from filter in filterPropertys
                                                        where s.GetValue(filter) < filter.Value
                                                        select false).Count() == 0;

                                            if (pass)
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



            string path;
            if (fileFormat == "1")
                path = _WriteCSV(suits.ToArray(), outAmount);
            else if (fileFormat == "2")
                path = _WriteHTML(suits.ToArray(), outAmount);
            else
                path = _WriteCSV(suits.ToArray(), outAmount);

            
            System.Console.WriteLine("寫入完成.");
            _ShowThank();
            System.Console.ReadKey();

            System.Diagnostics.Process.Start(path);
        }
        
        

        

        private static PropertyValue[] _InputProperty()
        {
            List<PropertyValue> values = new List<PropertyValue>();
            while(true)
            {
                _ShowProperty();
                System.Console.Write("\nQuit = 結束");
                System.Console.Write("\n輸入篩選條件(範例:LIFE 5000):");
                var propertyTypeValue = System.Console.ReadLine();
                

                try
                {
                    var typevalue = propertyTypeValue.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (typevalue[0].ToLower() == "quit")
                        break;
                    PropertyValue val = PropertyValue.Build(typevalue[0], typevalue[1]);
                    values.Add(val);
                }
                catch (SystemException ex)
                {
                    System.Console.Write("\n錯誤:" + ex.Message );
                    System.Console.WriteLine("按下任一鍵繼續...");
                    System.Console.ReadKey();
                }
            }

            return values.ToArray();
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
                軒轅與其快樂的夥伴們XD 製作
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

        private static string _WriteCSV(Suit[] suits,int out_count)
        {            
            using (var stream = new System.IO.StreamWriter( "output.csv" ,false , Encoding.UTF8))
            {
                stream.WriteLine("攻擊,穿刺,命中,集中,爆擊,熟練,額外傷害,威脅,生命,防禦,閃避,格檔,爆擊防禦,韌性,傷害減免,回復,治療,最大合成,1,2,3,4,5,6,7,8,");
                foreach (var suit in suits)
                {
                    
                    string line = "";
                    foreach(var val in suit.Values)
                    {
                        line += val.Value + ",";
                    }

                    line += suit.MaxAppreciation + ",";

                    foreach(var card in suit.Cards)
                    {
                                            
                        line += card.ToDescription() + ",";
                    }
                    stream.WriteLine(line);

                    if (--out_count == 0)
                        break;
                }
            }

            return "output.csv" ;
            
        }

        static string _WriteHTML(Suit[] suits, int out_count)
        {
            using (var stream = new System.IO.StreamWriter("output.html", false, Encoding.UTF8))
            {
                stream.WriteLine("<html>");
                stream.WriteLine("<body>");
                stream.WriteLine("<table>");
                stream.WriteLine("<tr><td>攻擊</td><td>穿刺</td><td>命中</td><td>集中</td><td>爆擊</td><td>熟練</td><td>額外傷害</td><td>威脅</td><td>生命</td><td>防禦</td><td>閃避</td><td>格檔</td><td>爆擊防禦</td><td>韌性</td><td>傷害減免</td><td>回復</td><td>治療</td><td>最大合成</td><td>1</td><td>2</td><td>3</td><td>4</td><td>5</td><td>6</td><td>7</td><td>8</td></tr>");
                foreach (var suit in suits)
                {
                    

                    string line = "<tr>";
                    foreach (var val in suit.Values)
                    {
                        line += "<td>" + val.Value + "</td>";
                    }
                    line += "<td>" + suit.MaxAppreciation + "</td>";

                    foreach (var card in suit.Cards)
                    {
                        string proper = "[";
                        foreach(var val in card.Values)
                        {
                            proper += string.Format("{0}({1})/", val.Id.GetEnumDescription(), val.Value);
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

            return "output.html";
        }

        
    }
}

