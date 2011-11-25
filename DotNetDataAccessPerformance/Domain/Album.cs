using Iesi.Collections;

namespace DotNetDataAccessPerformance.Domain
{
	public class Album
	{
		public virtual int AlbumId { get; set; }
		public virtual string Title { get; set; }
		public virtual int ArtistId { get; set; }
		public virtual Artist Artist { get; set; }
		public virtual ISet Tracks { get; set; }

		public Album()
		{
			Tracks = new HashedSet();
		}
	}
}