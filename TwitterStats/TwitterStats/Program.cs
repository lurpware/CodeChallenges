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
			ITwitterStream twitterStream = new TwitterStream();
			var producer = new Thread(new ParameterizedThreadStart(twitterStream.Start));
			producer.Start(TwitterStream.GetTwitterStream());

			char key = 's';
			do
			{
				Console.WriteLine("Press 'q' to quit. Any other key to current statics.");
				key = Console.ReadKey().KeyChar;
				if (key == 'q')
					break;
				var tagUsage = await twitterStream.GetHashTagUsageAsync();
				Console.Clear();

				Console.WriteLine("Top 10 'English' Hash Tags:");
				foreach (var item in tagUsage.Take(10))
				{
					Console.WriteLine($"\tCount: {item.Key}; Hashtag: {item.Value}");
				}
				Console.WriteLine(@$"
Errors:               {twitterStream.GlobalStats.ErrorCount};
English:              {twitterStream.GlobalStats.English}; 
NonEnglish:           {twitterStream.GlobalStats.NonEnglish};
English /w Hashtags:  {twitterStream.GlobalStats.WithHashTags}; 
English /wo Hashtags: {twitterStream.GlobalStats.WithNoHashTags};
English Hashtags:     {tagUsage.Count}

");
			} while (true);

			twitterStream.Stop(); 

			while (producer.ThreadState != ThreadState.Stopped)
				Thread.Sleep(100);
		}
	}
}
