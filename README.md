# BrawlEvents
A powerful tracker for Brawl Stars that provides insights on battles, such as brawler usage and win rate.

## Installation
Install using the Package Manager Console in Visual Studio.
```ps
Install-Package BrawlMatic.Tools.BrawlEvents
```

## Usage
```cs
var client = new BrawlEventsClient("<token>");
    
List<Battle> battles = await client.GetBattlesAsync(1500); //Gather data from 1500 recent battles around the game
List<EventBrawlerStat> stats = Utils.GetBrawlerStatsFromBattles(battles, 16000020); //Parse stats from those previously fetched battles

EventBrawlerStat stat = stats[0];

Console.WriteLine($"Brawler ID {stat.Brawler} has {stat.GetUseRate(battles.Count)}% use rate, and {stat.GetWinRate()}% win rate in the map ID {stat.Map}."); //Brawler ID 16000020 has 3.87% use rate, and 31.03% win rate in the map ID 15000051.
Console.WriteLine($"Brawler ID {stat.Brawler} has been used {stat.UseCount} times and have won {stat.WinCount} times in the map ID {stat.Map}.");//Brawler ID 16000020 has been used 73 times and have won 27 times in the map ID 15000051.
```

## Support
Join our [Discord](https://discord.gg/AcE7W8h59D) server if you need any assistance.

## License
MIT