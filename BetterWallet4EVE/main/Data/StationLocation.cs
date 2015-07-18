using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cn.zuoanqh.open.zut;
using cn.zuoanqh.open.zut.Data;

namespace cn.zuoanqh.open.bw4eve.Data
{
    public class StationLocation
    {
        public readonly String SolarSystem;
        public readonly String Planet;
        public readonly int PlanetNumber;
        public readonly bool IsAMoon;
        /// <summary>
        /// -1 means no moon. Happens only when IsAMoon is false.
        /// </summary>
        public readonly int MoonNumber;
        public readonly String StationType;

        public StationLocation(string GameRepresentationLocation)
        {   //TODO: make it work for player station names!
            //伦斯 VI - 卫星 8 - 布鲁特部族 财政部
            //planet - moon - station
            string[] s = zusp.Split(GameRepresentationLocation, " - ");
            IsAMoon = s.Length == 3;//3 segments means it's a moon station. 
            Twin<String> t = zusp.CutFirst(s[0], " ");
            SolarSystem = t.First;
            //TODO: process planet's romanic numeral
            Planet = t.Second;

            if (IsAMoon)
            {
                t = zusp.CutFirst(s[1], " ");
                MoonNumber = Convert.ToInt32(t.Second);
                StationType = s[2];
            }
            else
            {
                MoonNumber = -1;
                StationType = s[1];
            }
        }
    }
}
