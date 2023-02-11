using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TwitterStats.Dto;
using TwitterStats.Service;

namespace TwitterStats.Interface
{
	/// <summary>
	/// Not really used yet but useful for Mocking
	/// </summary>
	public interface ITwitterStream
	{
		GlobalStats GlobalStats { get; }

		Task<bool> AddTweetForProcessingAsync(string json);
		Task<List<KeyValuePair<int, string>>> GetHashTagUsageAsync();
		void Start(object twitterStreamObject);
		void Stop();
	}
}