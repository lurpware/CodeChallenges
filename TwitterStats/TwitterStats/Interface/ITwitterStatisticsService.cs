using System.Collections.Generic;
using System.Threading.Tasks;
using TwitterStats.Service;

namespace TwitterStats.Interface
{
	public interface ITwitterStatisticsService
	{
		GlobalStats GlobalStats { get; }

		Task<bool> AddTweetForProcessingAsync(string json);
		Task<List<KeyValuePair<int, string>>> GetHashTagUsageAsync();
		void Start();
		void Stop();
	}
}
