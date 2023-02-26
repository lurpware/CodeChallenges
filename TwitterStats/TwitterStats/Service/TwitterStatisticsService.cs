using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using TwitterStats.Dto;
using TwitterStats.Interface;

namespace TwitterStats.Service
{
	public class TwitterStatisticsService : ITwitterStatisticsService
	{
		private static readonly string BearerToken = ConfigurationManager.AppSettings["BearerToken"];
		private readonly ITwitterStreamService twitterStreamService;

		public bool IsRunning { get; protected set; }
		private bool StopRequested;

		public GlobalStats GlobalStats { get; protected set; }

		// For controlling access to TweetsToProcess
		private System.Threading.SemaphoreSlim sync;
		private ILogger<TwitterStatisticsService> _logger;

		private Queue<TweetItem> TweetsToProcess { get; }

		public TwitterStatisticsService(ILoggerFactory loggerFactory, ITwitterStreamService twitterStreamService)
		{
			_logger = loggerFactory.CreateLogger<TwitterStatisticsService>();


			TweetsToProcess = new Queue<TweetItem>();
			IsRunning = false;
			StopRequested = false;
			sync = new System.Threading.SemaphoreSlim(1);
			GlobalStats = new GlobalStats();
			this.twitterStreamService = twitterStreamService;
		}

		public void Stop()
		{ StopRequested = true; }

		public void Start()
		{
			var twitterStream = twitterStreamService.GetTwitterStream(BearerToken);
			IsRunning = true;
			while (!StopRequested)
			{
				try
				{
					var jsonText = twitterStream.ReadLine();

					// Fire and forget the task
					_ = AddTweetForProcessingAsync(jsonText);
				}
				catch (Exception ex)
				{
					// Add code to re initilize the twitter stream on a web error? 
					GlobalStats.ErrorCount++;
					Console.WriteLine(ex);
				}
			}

			IsRunning = false;
		}

		/// <summary>
		/// builds global stats and does queue management
		/// </summary>
		/// <param name="json">The json tweet to process</param>
		/// <returns>true if added to the queue, else false</returns>
		public async Task<bool> AddTweetForProcessingAsync(string json)
		{
			if (string.IsNullOrWhiteSpace(json))
				return false;

			TweetItem tweet = null;
			try
			{
				tweet = JsonSerializer.Deserialize<DataItem>(json).Tweet;
			}
			catch (JsonException ex)
			{
				GlobalStats.ErrorCount++;
				Console.WriteLine(ex);
				return false;
			}

			// Filter tweets that are not in "English" becasue the console window struggles with non ascii chars
			if (tweet.Language != "en")
			{
				GlobalStats.NonEnglish++;
				return false;
			}
			GlobalStats.English++;
			if (tweet?.Entities?.Hashtags?.Any() ?? false)
			{
				await sync.WaitAsync();
				TweetsToProcess.Enqueue(tweet);
				GlobalStats.WithHashTags++;
				sync.Release();
			}
			else
			{
				GlobalStats.WithNoHashTags++;
				return false;
			}
			await sync.WaitAsync();
			while (TweetsToProcess.Any() && DateTimeOffset.UtcNow - TweetsToProcess.Peek().CreatedAt > new TimeSpan(1, 0, 0, 0))
				TweetsToProcess.Dequeue();
			sync.Release();

			return true;
		}

		public async Task<List<KeyValuePair<int, string>>> GetHashTagUsageAsync()
		{
			List<string> tags = null;

			// Get all the tags used
			await sync.WaitAsync();
			tags = TweetsToProcess.SelectMany(s => s.Entities?.Hashtags?.Select(x => x.Tag)).ToList();
			sync.Release();

			// Make a grouping of each tag
			var groupedTags = tags.GroupBy(x => x);

			// Count how many times the tag appears and save it with the tag
			return groupedTags.Select(x => new KeyValuePair<int, string>(x.Count(), x.Key)).OrderByDescending(x => x.Key).ToList();
		}
	}
}
