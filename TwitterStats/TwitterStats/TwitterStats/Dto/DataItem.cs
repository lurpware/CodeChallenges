using System.Text.Json.Serialization;

namespace TwitterStats.Dto
{
	/// <summary>
	/// Wrapper class returned by twitter
	/// </summary>
	public class DataItem
	{
		/// <summary>
		/// The acutal tweet and related meta data
		/// </summary>
		[JsonPropertyName("data")]
		public TweetItem Tweet { get; set; }
	}
}