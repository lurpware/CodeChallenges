using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace UnitTestTwitterStats
{
	[TestClass]
	public class UnitTwitterStream
	{
		[TestMethod]
		public async Task InvalidJson_AddTweetForProcessing_InvalidJson_Skipped()
		{
			// Arrange
			var service = new TwitterStats.Service.TwitterStream();
			var json = "";

			// Act
			var rtn = await service.AddTweetForProcessingAsync(json);

			// Assert
			Assert.IsFalse(rtn, "Invalid Function Return Value");
			Assert.AreEqual(0, service.GlobalStats.ErrorCount, "Tweet should not be counted as an error.");
			Assert.AreEqual(0, service.GlobalStats.English, "Tweet should not have queued.");
			Assert.AreEqual(0, service.GlobalStats.NonEnglish, "Tweet should not have counted.");
		}

		[TestMethod]
		public async Task InvalidJson_AddTweetForProcessing_InvalidJson_CountsError()
		{
			// Arrange
			var service = new TwitterStats.Service.TwitterStream();
			var json = "{sdasdasdsaddasda}";

			// Act
			var rtn = await service.AddTweetForProcessingAsync(json);

			// Assert
			Assert.IsFalse(rtn, "Invalid Function Return Value");
			Assert.AreEqual(1, service.GlobalStats.ErrorCount, "Tweet should be counted as an error.");
			Assert.AreEqual(0, service.GlobalStats.English, "Tweet should not have queued.");
			Assert.AreEqual(0, service.GlobalStats.NonEnglish, "Tweet should not have counted.");
		}

		[TestMethod]
		public async Task InvalidJson_AddTweetForProcessing_NonEnglish_CountsNonEnglish()
		{
			// Arrange
			var service = new TwitterStats.Service.TwitterStream();
			var json = "{\"data\":{\"created_at\":\"2023-02-11T02:02:52.000Z\",\"edit_history_tweet_ids\":[\"1624227438430687233\"],\"entities\":{\"mentions\":[{\"start\":3,\"end\":18,\"username\":\"ShivamSanghi12\",\"id\":\"1112295704448417793\"}]},\"id\":\"1624227438430687233\",\"lang\":\"hi\",\"text\":\"RT @ShivamSanghi12: भाजपा का हर एक फैसला जो देशहित में है विपक्ष उसका विरोध करता है और हर उस फैसले का समर्थन करता है जो देश हित में नहीं है…\"}}";

			// Act
			var rtn = await service.AddTweetForProcessingAsync(json);

			// Assert
			Assert.IsFalse(rtn, "Invalid Function Return Value");
			Assert.AreEqual(0, service.GlobalStats.ErrorCount, "Tweet should be counted as an error.");
			Assert.AreEqual(0, service.GlobalStats.English, "Tweet should not have queued.");
			Assert.AreEqual(1, service.GlobalStats.NonEnglish, "Tweet should have counted.");
		}

		[TestMethod]
		public async Task InvalidJson_AddTweetForProcessing_ValidJson_NoHashTags()
		{
			// Arrange
			var service = new TwitterStats.Service.TwitterStream();
			var json = "{\"data\":{\"created_at\":\"2023-02-11T02:09:35.000Z\",\"edit_history_tweet_ids\":[\"1624229128739373056\"],\"entities\":{\"mentions\":[{\"start\":3,\"end\":14,\"username\":\"gayvinpogi\",\"id\":\"1535891670796906497\"}],\"urls\":[{\"start\":90,\"end\":113,\"url\":\"https://t.co/DF6fo4ioNo\",\"expanded_url\":\"https://twitter.com/gayvinpogi/status/1624176968513232900/video/1\",\"display_url\":\"pic.twitter.com/DF6fo4ioNo\",\"media_key\":\"7_1624176819489611777\"}]},\"id\":\"1624229128739373056\",\"lang\":\"en\",\"text\":\"RT @gayvinpogi: NEW VIDEO ON MY PRIVATE CHANNEL!!\\n\\nkapagod mag jakol, can u do it for me? https://t.co/DF6fo4ioNo\"}}";

			// Act
			var rtn = await service.AddTweetForProcessingAsync(json);

			// Assert
			Assert.IsFalse(rtn, "Invalid Function Return Value");
			Assert.AreEqual(0, service.GlobalStats.ErrorCount, "Tweet should be counted as an error.");
			Assert.AreEqual(1, service.GlobalStats.English, "Tweet should not have queued.");
			Assert.AreEqual(0, service.GlobalStats.NonEnglish, "Tweet should not have counted.");
		}

		[TestMethod]
		public async Task InvalidJson_AddTweetForProcessing_ValidJson_AddedToQueue()
		{
			// Arrange
			var service = new TwitterStats.Service.TwitterStream();
			var json = "{\"data\":{\"created_at\":\"2023-02-11T02:07:48.000Z\",\"edit_history_tweet_ids\":[\"1624228679944400897\"],\"entities\":{\"hashtags\":[{\"start\":12,\"end\":21,\"tag\":\"BREAKING\"}],\"mentions\":[{\"start\":3,\"end\":10,\"username\":\"VABVOX\",\"id\":\"138168339\"},{\"start\":29,\"end\":37,\"username\":\"CBSNews\",\"id\":\"15012486\"}]},\"id\":\"1624228679944400897\",\"lang\":\"en\",\"text\":\"RT @VABVOX: #BREAKING \\nBig: \\n@CBSNews: A source close to the Trump legal team says Trump's attorneys intend to contest Mike Pence's subpoen…\"}}";

			// Act
			var rtn = await service.AddTweetForProcessingAsync(json);

			// Assert
			Assert.IsTrue(rtn, "Invalid Function Return Value");
			Assert.AreEqual(0, service.GlobalStats.ErrorCount, "Tweet should be counted as an error.");
			Assert.AreEqual(1, service.GlobalStats.English, "Tweet should have queued.");
			Assert.AreEqual(0, service.GlobalStats.NonEnglish, "Tweet should not have counted.");
		}

		// TODO: Add tests for removing old tweets from queue.
		// TODO: Need unit tests for GetHashTagUsage.
		// TODO: Test various stream error.
	}
}
