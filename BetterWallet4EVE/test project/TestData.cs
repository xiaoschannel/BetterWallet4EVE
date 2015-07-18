using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using cn.zuoanqh.open.bw4eve.Data;
using cn.zuoanqh.open.zut;
using cn.zuoanqh.open.zut.FileIO.Text;
using System.Collections.Generic;

namespace test_project
{
    [TestClass]
    public class TestData
    {
        [TestMethod]
        public void TestISKCreate()
        {
            ISK t = new ISK("12,345.67 ISK");
            Assert.AreEqual(12345, t.WholeISK);
            Assert.AreEqual(67, t.CentsOnly);
            Assert.AreEqual(12345.67, t.Value);

            t = new ISK("123,456,789,012.34 ISK");
            Assert.AreEqual(123456789012, t.WholeISK);
            Assert.AreEqual(34, t.CentsOnly);
            Assert.AreEqual(123456789012.34, t.Value);

            t = new ISK(12345678901234);
            Assert.AreEqual(123456789012, t.WholeISK);
            Assert.AreEqual(34, t.CentsOnly);
            Assert.AreEqual(123456789012.34, t.Value);
        }

        [TestMethod]
        public void TestISKToString()
        {
            ISK t = new ISK("123,456,789,012.34 ISK");
            Assert.AreEqual("123,456,789,012.34 ISK", t.ToString());
            Assert.AreEqual("1234,5678,9012.34 ISK", t.ToString4());
        }

        [TestMethod]
        public void TestStationLocationCreate()
        {
            StationLocation l = new StationLocation("伦斯 VI - 卫星 8 - 布鲁特部族 财政部");
            Assert.AreEqual("伦斯", l.SolarSystem);
            Assert.AreEqual("VI", l.Planet);
            //Assert.AreEqual(6, l.PlanetNumber);
            Assert.AreEqual(true, l.IsAMoon);
            Assert.AreEqual(8, l.MoonNumber);
            Assert.AreEqual("布鲁特部族 财政部", l.StationType);
        }

        [TestMethod]
        public void TestTransaction()
        {
            Transaction t = new Transaction("2015.06.30 00:08	扩充货柜舱 I	60,000.00 ISK	10	600,000.00 ISK	ISK	U-231	伦斯 VI - 卫星 8 - 布鲁特部族 财政部	左岸清河	主账户");

            DateTime dt = t.Time;
            Assert.AreEqual(2015, dt.Year);
            Assert.AreEqual(6, dt.Month);
            Assert.AreEqual(30, dt.Day);
            Assert.AreEqual(0, dt.Hour);
            Assert.AreEqual(8, dt.Minute);

            Assert.AreEqual("扩充货柜舱 I", t.Item);
        }

        [TestMethod]
        public void TestMain()
        {
            List<string> l = ByLineFileIO.readFileNoWhitespace("data.txt");

            List<Transaction> lt = new List<Transaction>();
            foreach (string s in l)
            {
                lt.Add(new Transaction(s));
            }
            /*
            for (int i = 0; i < lt.Count; i++)
            {
                for (int j = 0; j < lt.Count - i - 1; j++)
                {
                    if (lt[j].TotalPrice.WholeISK > lt[j + 1].TotalPrice.WholeISK)
                    {
                        var t = lt[j+1];
                        lt[j+1] = lt[j];
                        lt[j] = t;
                    }
                }
            }*/

            //Dictionary<string, long> itemTransCount = new Dictionary<string, long>();
            Dictionary<string, int> customerOrder = new Dictionary<string, int>(); 
            Dictionary<string, ISK> customerAmount = new Dictionary<string, ISK>();
            Dictionary<string, DateTime> customerLastTransaction = new Dictionary<string, DateTime>();
            
            foreach (var v in lt)
            {
                if (v.TotalPrice.WholeISK > 0)
                {
                    if (!customerAmount.ContainsKey(v.Customer)) 
                    {
                        customerAmount.Add(v.Customer, new ISK(0));
                        customerOrder.Add(v.Customer,0);
                        customerLastTransaction.Add(v.Customer,v.Time);
                    }
                    customerAmount[v.Customer] += v.TotalPrice;
                    customerOrder[v.Customer]++;
                    if (v.Time > customerLastTransaction[v.Customer]) 
                        customerLastTransaction[v.Customer] = v.Time;
                }
                //if ((v.Time.Month == 5 && v.Time.Day >= 25) || (v.Time.Month == 6 && v.Time.Day <= 10))
                //{
                //Logger.Log(v.OrigionalInput);
                //}
                /*
                if (v.Item.EndsWith("级") && v.TotalPrice.WholeISK > 0)
                    if (itemTransCount.ContainsKey(v.Item))
                        itemTransCount[v.Item]++;
                    else
                        itemTransCount.Add(v.Item, 1);*/
            }

            /*
            foreach (var v in lt)
            {
                if (v.Time.Hour == 6) Logger.Log(v.OrigionalInput);
            }*/

            foreach (var v in customerAmount.Keys)
            {
                if (customerOrder[v]>=10&&customerLastTransaction[v].Month>=6&&customerLastTransaction[v].Day>=24)
                    Logger.Log(v + "\t" + customerAmount[v].WholeISK+"\t"+customerOrder[v]+"\t"+customerLastTransaction[v]);
            }
            Logger.Save();
        }

    }
}
