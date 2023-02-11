namespace TwitterStats.Service
{
	public class GlobalStats
	{
		public int English { get; set; }
		public int WithNoHashTags { get; set; }
		public int WithHashTags { get; set; }
		public int NonEnglish { get; set; }
		public int ErrorCount { get; set; }

	}
}