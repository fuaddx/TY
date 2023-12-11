namespace Pustok2.Models
{
    public class BlogTag
    {
        public int Id { get; set; }
        public Guid TagId { get; set; }
        public int BlogId { get; set; }
        public Blog? Blog { get; set; }
        public Tag? Tag { get; set; }
    }
}
