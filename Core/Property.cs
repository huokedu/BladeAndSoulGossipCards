using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/*
 * /*
 
無盡八卦牌☵1	最大生命1120/1360/1600	爆擊54/68	命中54/68	防禦54/68	114
無盡八卦牌☳2	爆擊117/142/168	命中56/71	防禦56/71	格檔56/71	120
無盡八卦牌☶3	最大生命1220/1480/1750	防禦60/75	閃避60/75	格檔60/75	125
無盡八卦牌☱4	命中128/155/183	爆擊62/78	閃避62/78	格檔62/78	131
無盡八卦牌☲5	最大生命1390/1690/1990	爆擊67/84	閃避67/84	格檔67/84	142
無盡八卦牌☷6	防禦144/175/206	爆擊70/88	閃避70/88	格檔70/88	147
無盡八卦牌☴7	最大生命1490/1810/2140	防禦72/91	閃避72/91	格檔72/91	153
無盡八卦牌☰8	爆擊154/187/221	命中75/94	防禦75/94	閃避750/940	158
套裝效果	※3件套效果：生命650      ※5件套效果：防禦196      ※8件套效果：生命2740、格檔117

 */
 
namespace BladeAndSoulGossipCards
{
    /*
攻擊
穿刺
命中
集中
爆擊
熟練
額外傷害
威脅
生命
防禦
閃避
格檔
爆擊防禦
韌性
傷害減免
回復
治療

     */

    [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum PROPERTY_TYPE
    {        
        [Regulus.Utility.EnumDescription("攻擊")]
        ATTACK,
        [Regulus.Utility.EnumDescription("穿刺")]
        PUNCTURE,
        [Regulus.Utility.EnumDescription("命中")]
        HIT,
        [Regulus.Utility.EnumDescription("集中")]
        FOCUS,
        [Regulus.Utility.EnumDescription("爆擊")]
        CRIT,
        [Regulus.Utility.EnumDescription("熟練")]
        SKILLED,
        [Regulus.Utility.EnumDescription("額外傷害")]
        ADDITIONAL_DAMAGE,
        [Regulus.Utility.EnumDescription("威脅")]
        THREAT,
        [Regulus.Utility.EnumDescription("生命")]
        LIFE,
        [Regulus.Utility.EnumDescription("防禦")]
        DEFENSE,
        [Regulus.Utility.EnumDescription("閃避")]        
        DODGE,
        [Regulus.Utility.EnumDescription("格擋")]
        GRID_FILE,
        [Regulus.Utility.EnumDescription("爆擊防禦")]
        CRIT_DEFENSE,
        [Regulus.Utility.EnumDescription("韌性")]
        TOUGHNESS,
        [Regulus.Utility.EnumDescription("傷害減免")]
        DAMAGE_REDUCTION,
        [Regulus.Utility.EnumDescription("回復")]
        REPLY,
        [Regulus.Utility.EnumDescription("治療")]
        TREATMENT     
    };
    public class Property
    {
        [Newtonsoft.Json.JsonProperty("效果")]
        
        public PROPERTY_TYPE Id;
    }
}
