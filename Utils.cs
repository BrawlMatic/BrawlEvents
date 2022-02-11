using BrawlSharp.Model.Player.BattleLog;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace BrawlEvents
{
    public class Utils
    {
        public static List<Player> GetPlayersFromBattle(Battle battle)
        {
            List<Player> players = new List<Player>();

            if (battle.Match.Players != null)
            {
                foreach (var player in battle.Match.Players)
                {
                    if (!players.Contains(player))
                        players.Add(player);
                }
            }
            else
            {
                foreach (var team in battle.Match.Teams)
                {
                    foreach (var player in team)
                    {
                        if (!players.Contains(player))
                            players.Add(player);
                    }
                }
            }

            return players;
        }

        public static List<Stat> GetBrawlerStatsFromBattles(List<Battle> battles, int brawlerId)
        {
            ConcurrentDictionary<int, int> matches = new ConcurrentDictionary<int, int>();
            ConcurrentDictionary<int, int> wins = new ConcurrentDictionary<int, int>();

            foreach (Battle battle in battles)
            {
                if (battle.Match.Type != "friendly" && battle.Map.Id != 0 && battle.Match.Mode != "duels")
                {
                    bool hasBrawler = false;

                    if (battle.Match.Players != null)
                    {
                        foreach (Player player in battle.Match.Players)
                        {
                            if (player.Brawler.Id == brawlerId && !hasBrawler)
                            {
                                hasBrawler = true;
                                matches.AddOrUpdate(battle.Map.Id, 1, (id, value) => value + 1);
                            } 
                        }

                        if (battle.Match.Players[0].Brawler.Id == brawlerId)
                            wins.AddOrUpdate(battle.Map.Id, 1, (id, value) => value + 1);
                    }
                    else
                    {
                        foreach (var team in battle.Match.Teams)
                        {
                            if (team.Any(p => p.Brawler.Id == brawlerId) && !hasBrawler)
                            {
                                hasBrawler = true;
                                matches.AddOrUpdate(battle.Map.Id, 1, (id, value) => value + 1);
                            }
                        }

                        if (battle.Match.Mode == "duoShowdown")
                        {
                            bool hasBrawlerWin = false;

                            foreach (Player player in battle.Match.Teams[0])
                            {
                                if (player.Brawler.Id == brawlerId && !hasBrawlerWin)
                                {
                                    hasBrawlerWin = true;
                                    wins.AddOrUpdate(battle.Map.Id, 1, (id, value) => value + 1);
                                } 
                            }
                        }

                        if (battle.Match.StarPlayer?.Brawler.Id == brawlerId)
                            wins.AddOrUpdate(battle.Map.Id, 1, (id, value) => value + 1);
                    }
                }
            }

            List<Stat> stats = new List<Stat>();

            foreach (var map in matches)
            {
                if (wins.TryGetValue(map.Key, out int winCount))
                    stats.Add(new Stat(brawlerId, map.Key, map.Value, winCount));
                else
                    stats.Add(new Stat(brawlerId, map.Key, map.Value, 0));
            }

            return stats;
        }
    }
}
