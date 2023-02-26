using System.IO;

namespace TwitterStats.Interface
{
	/// <summary>
	/// Not really used yet but useful for Mocking
	/// </summary>
	public interface ITwitterStreamService
	{
		StreamReader GetTwitterStream(string bearerToken);
	}
}