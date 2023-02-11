using System.Text.Json.Serialization;

namespace TwitterStats.Dto
{
	/// <summary>
	/// Common entities found in Tweets, including hashtags, links, and user mentions.
	/// </summary>
	public class EntityItem
	{
		/// <summary>
		/// Array of Hashtag Objects
		/// </summary>
		[JsonPropertyName("hashtags")]
		public Hashtag[] Hashtags { get; set; }
	}
}