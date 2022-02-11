using System;

namespace BrawlEvents
{
    public class Stat
    {
        public int Brawler;
        public int Map;
        public int UseCount;
        public int WinCount;

        public Stat(int brawler, int map, int useCount, int winCount)
        {
            Brawler = brawler;
            Map = map;
            UseCount = useCount;
            WinCount = winCount;
        }

        public double GetWinRate()
        {
            return Math.Round((double)WinCount / UseCount * 100, 2);
        }

        public double GetUseRate(int battleCount)
        {
            return Math.Round((double)UseCount / battleCount * 100, 2);
        }
    }
}
