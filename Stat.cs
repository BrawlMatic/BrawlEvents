using System;

namespace BrawlEvents
{
    public class Stat
    {
        public int BrawlerId;
        public int MapId;
        public int UseCount;
        public int WinCount;

        public Stat(int brawlerId, int mapId, int useCount, int winCount)
        {
            BrawlerId = brawlerId;
            MapId = mapId;
            UseCount = useCount;
            WinCount = winCount;
        }

        public double GetWinRate()
        {
            return Math.Round((double)WinCount / UseCount * 100, 5);
        }

        public double GetUseRate(int battleCount)
        {
            return Math.Round((double)UseCount / battleCount * 100, 5);
        }
    }
}
