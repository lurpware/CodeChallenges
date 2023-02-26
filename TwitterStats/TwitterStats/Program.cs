using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TwitterStats.Interface;
using TwitterStats.Service;

namespace TwitterStats
{
	class Program
	{
		static async Task Main(string[] args)
		{
			//setup our DI
			var serviceProvider = new ServiceCollection()
				.AddLogging()
				.AddSingleton<ITwitterStatisticsService, TwitterStatisticsService>()
				.AddSingleton<ITwitterStreamService, TwitterStreamService>()
				.BuildServiceProvider();

			//configure logging
			var loggerFactory =
				LoggerFactory.Create(builder =>
					builder.AddFile("app_{0:yyyy}-{0:MM}-{0:dd}.log", fileLoggerOpts =>
					{
						fileLoggerOpts.FormatLogFileName = fName => 
						{
							return string.Format(fName, DateTime.UtcNow);
						};
					}));

			var logger = loggerFactory.CreateLogger<Program>();
			logger.LogInformation("Starting application");

			//do the actual work here
			var twitterStatistics = serviceProvider.GetService<ITwitterStatisticsService>();
			
			var producer = new Thread(new ThreadStart(twitterStatistics.Start));
			producer.Start();

			char key = 's';
			do
			{
				Console.WriteLine("Press 'q' to quit. Any other key to current statics.");
				key = Console.ReadKey().KeyChar;
				if (key == 'q')
					break;
				var tagUsage = await twitterStatistics.GetHashTagUsageAsync();
				Console.Clear();

				Console.WriteLine("Top 10 'English' Hash Tags:");
				foreach (var item in tagUsage.Take(10))
				{
					Console.WriteLine($"\tCount: {item.Key}; Hashtag: {item.Value}");
				}
				Console.WriteLine(@$"
Errors:               {twitterStatistics.GlobalStats.ErrorCount};
English:              {twitterStatistics.GlobalStats.English}; 
NonEnglish:           {twitterStatistics.GlobalStats.NonEnglish};
English /w Hashtags:  {twitterStatistics.GlobalStats.WithHashTags}; 
English /wo Hashtags: {twitterStatistics.GlobalStats.WithNoHashTags};
English Hashtags:     {tagUsage.Count}

");
			} while (true);

			twitterStatistics.Stop(); 

			while (producer.ThreadState != ThreadState.Stopped)
				Thread.Sleep(100);
		}
	}
}
