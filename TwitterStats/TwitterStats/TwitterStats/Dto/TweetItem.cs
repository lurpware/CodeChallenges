using System;
using System.Text.Json.Serialization;

namespace TwitterStats.Dto
{
	/// <summary>
	/// An actual tweet item retuened by twitter
	/// </summary>
	public class TweetItem
	{
		/// <summary>
		/// A collection of common entities found in Tweets, including hashtags, links, and user mentions.
		/// </summary>
		[JsonPropertyName("entities")]
		public EntityItem Entities { get; set; }

		/// <summary>
		/// When the tweet was created
		/// </summary>
		[JsonPropertyName("created_at")]
		public DateTimeOffset CreatedAt { get; set; }

		/// <summary>
		/// The language of the tweet
		/// </summary>
		[JsonPropertyName("lang")]
		public string Language { get; set; }
	}
}