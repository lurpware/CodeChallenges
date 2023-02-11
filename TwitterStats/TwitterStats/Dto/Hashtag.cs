using System.Text.Json.Serialization;

namespace TwitterStats.Dto
{
	/// <summary>
	/// A hashtag and related meta data
	/// </summary>
	public class Hashtag
	{
		/// <summary>
		/// The text of the Hashtag
		/// </summary>
		[JsonPropertyName("tag")]
		public string Tag { get; set; }
	}
}