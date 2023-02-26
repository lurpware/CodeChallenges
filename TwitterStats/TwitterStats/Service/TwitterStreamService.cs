using Microsoft.Extensions.Logging;
using System.IO;
using System.Net;
using System.Text;
using TwitterStats.Interface;

namespace TwitterStats.Service
{
	public class TwitterStreamService: ITwitterStreamService
	{
		public TwitterStreamService(ILoggerFactory loggerFactory)
		{ }

		/// <summary>
		/// Gets a StreamReader where each line is a json tweet
		/// </summary>
		/// <returns></returns>
		public StreamReader GetTwitterStream(string bearerToken)
		{
			var webRequest = (HttpWebRequest)WebRequest.Create("https://api.twitter.com/2/tweets/sample/stream?tweet.fields=entities,lang,created_at");
			webRequest.Headers.Add(HttpRequestHeader.Authorization, $"Bearer {bearerToken}");
			var resp = (HttpWebResponse)webRequest.GetResponse();

			return new StreamReader(resp.GetResponseStream(), Encoding.UTF8);
		}
	}
}
