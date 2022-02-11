# BrawlEvents
A powerful tracker for Brawl Stars that provides insights on battles, such as brawler usage and win rate.

## Installation
Install using the Package Manager Console in Visual Studio.
```ps
Install-Package BrawlMatic.Tools.BrawlEvents
```

## Dependencies
- [BrawlSharp](https://github.com/BrawlMatic/BrawlSharp)

## Usage
```cs
var client = new BrawlEventsClient("<token>");
    
var battles = await client.GetBattlesAsync(4500); //Gather data from 4500 recent battles around the game
var stats = Utils.GetBrawlerStatsFromBattles(battles, 16000000); //Parse stats from those previously fetched battles

var stat = stats[0];

Console.WriteLine($"Brawler ID {stat.BrawlerId} has {stat.GetUseRate(battles.Where(b => b.Map.Id == stat.MapId).Count())}% use rate, and {stat.GetWinRate()}% win rate in the map ID {stat.MapId}.");
//Brawler ID 16000020 has 3.87% use rate, and 31.03% win rate in the map ID 15000051.

Console.WriteLine($"Brawler ID {stat.BrawlerId} has been used {stat.UseCount} times and have won {stat.WinCount} times in the map ID {stat.MapId}.");
//Brawler ID 16000020 has been used 73 times and have won 27 times in the map ID 15000051.
```

## Support
Join our [Discord](https://discord.gg/AcE7W8h59D) server if you need any assistance.

## License
MIT