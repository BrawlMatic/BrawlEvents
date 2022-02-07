using System;

namespace BrawlEvents
{
    public class EventBrawlerStat
    {
        public int Brawler;
        public int Map;
        public int UseCount;
        public int WinCount;

        public EventBrawlerStat(int brawler, int map, int useCount, int winCount)
        {
            Brawler = brawler;
            Map = map;
            UseCount = useCount;
            WinCount = winCount;
        }

        public decimal GetWinRate()
        {
            return Math.Round((decimal)WinCount / UseCount * 100, 2);
        }

        public decimal GetUseRate(int battleCount)
        {
            return Math.Round((decimal)UseCount / battleCount * 100, 2);
        }
    }
}
