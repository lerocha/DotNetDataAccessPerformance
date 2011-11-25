using Iesi.Collections;

namespace DotNetDataAccessPerformance.Domain
{
	public class Artist
	{
		public virtual int ArtistId { get; set; }
		public virtual string Name { get; set; }
		public virtual ISet Albums { get; set; } 
 
		public Artist()
		{
			Albums = new HashedSet();
		}
	}
}