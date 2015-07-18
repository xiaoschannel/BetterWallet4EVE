using cn.zuoanqh.open.zut;
using cn.zuoanqh.open.zut.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cn.zuoanqh.open.bw4eve.Data
{
    public class Transaction
    {
        public readonly DateTime Time;
        public readonly string Item;
        public readonly ISK UnitPrice;
        public readonly ISK TotalPrice;
        public readonly ulong Amount;
        public readonly string Customer;
        public readonly string Merchant;
        public readonly StationLocation Location;
        public readonly string OrigionalInput;
        public Transaction(string input)
        {
            OrigionalInput = input;
            //2015.06.30 00:08	扩充货柜舱 I	60,000.00 ISK	10	600,000.00 ISK	ISK	U-231	伦斯 VI - 卫星 8 - 布鲁特部族 财政部	左岸清河	主账户
            string[] s = zusp.Split(input.Trim(), "\t");
            //fancy eh?
            int[] t = new Procedure<string, int>((str) => Convert.ToInt32(str)).BatchProcess(zusp.Pick(s[0], 4, 1, 2, 1, 2, 1, 2, 1, 2)).ToArray();
            Time = new DateTime(t[0], t[1], t[2], t[3], t[4], 0);
            Item = s[1];
            UnitPrice = new ISK(s[2]);
            TotalPrice = new ISK(s[4]);
            Amount = Convert.ToUInt64(s[3].Replace(",", ""));
            Customer = s[6];
            Merchant = s[8];
            Location = new StationLocation(s[7]);
        }
    }
}
