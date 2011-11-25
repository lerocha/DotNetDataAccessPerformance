namespace DotNetDataAccessPerformance.Domain
{
	public class Track
	{
		public virtual int TrackId { get; set; }
		public virtual int AlbumId { get; set; }
		public virtual int Bytes { get; set; }
		public virtual string Composer { get; set; }
		public virtual int GenreId { get; set; }
		public virtual int MediaTypeId { get; set; }
		public virtual int Milliseconds { get; set; }
		public virtual string Name { get; set; }
		public virtual decimal UnitPrice { get; set; }
		public virtual Album Album { get; set; }
	}
}