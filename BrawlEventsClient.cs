using BrawlSharp;
using BrawlSharp.Model.Player.BattleLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace BrawlEvents
{
    public class BrawlEventsClient
    {
        readonly BrawlSharpClient BrawlSharpClient;

        List<Battle> Battles;

        List<string> TagsToFetch;
        List<string> FetchedTags;

        readonly int Threads;
        readonly bool Log;
        readonly bool LogErrors;

        int BattlesLimit;

        Stopwatch stopwatch;

        public BrawlEventsClient(string token, int threads = 75, bool log = true, bool logErrors = false)
        {
            BrawlSharpClient = new BrawlSharpClient(token);

            Threads = threads;
            Log = log;
            LogErrors = logErrors;
        }

        bool ShouldFetch(string tag, int tries)
        {
            if (tries <= 0) return false;
            if (Battles.Count >= BattlesLimit) return false;
            if (FetchedTags.Contains(tag)) return false;

            return true;
        }

        async void FetchTag(string tag, int tries = 3)
        {
            retry:
            if (!ShouldFetch(tag, tries)) return;

            wait:
            if (TagsToFetch.Count >= Threads)
            {
                await Task.Delay(2000);
                if (!ShouldFetch(tag, tries)) return;
                goto wait;
            }

            TagsToFetch.Add(tag);

            try
            {
                BattleLog battleLog = await BrawlSharpClient.GetPlayerBattleLogAsync(tag.Replace("#", ""));
                FetchedTags.Add(tag);

                foreach (Battle battle in battleLog.Battles)
                {
                    if (Battles.Count >= BattlesLimit)
                    {
                        if (stopwatch.IsRunning) stopwatch.Stop();
                        return;
                    }

                    if (!Battles.Contains(battle)) Battles.Add(battle);

                    foreach (Player player in Utils.GetPlayersFromBattle(battle))
                    {
                        if (ShouldFetch(player.Tag, tries))
                            new Thread(() => FetchTag(player.Tag)).Start();
                    }
                }

                TagsToFetch.Remove(tag);
            }
            catch (Exception ex)
            {
                if (LogErrors)
                    Console.WriteLine(ex);

                TagsToFetch.Remove(tag);
                FetchedTags.Remove(tag);

                tries--;
                goto retry;
            }
        }

        public async Task<List<Battle>> GetBattlesAsync(int limit, string genesisTag = null)
        {
            #region Resets
            BattlesLimit = limit;

            Battles = new List<Battle>();

            TagsToFetch = new List<string>();
            FetchedTags = new List<string>();

            stopwatch = new Stopwatch();
            #endregion

            stopwatch.Start();

            if (genesisTag != null)
                new Thread(() => FetchTag(genesisTag)).Start();
            else
            {
                var lb = await BrawlSharpClient.GetPlayerLeaderboardAsync();
                new Thread(() => FetchTag(lb.Players[0].Tag)).Start();
            }

            log:
            if (Battles.Count < BattlesLimit)
            {
                if (Log)
                    Console.WriteLine($"[BrawlEvents] Collecting battles... {Battles.Count}/{BattlesLimit} ({((decimal)Battles.Count / BattlesLimit * 100).ToString("N2")}%)");

                await Task.Delay(500);
                goto log;
            }
            else
            {
                if (Log)
                    Console.WriteLine($"[BrawlEvents] Collecting finished in {stopwatch.Elapsed.TotalSeconds.ToString("N0")}s!");

                return Battles;
            }
        }
    }
}
