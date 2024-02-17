namespace CQRSDogWrapperApi.Domain.Entities
{
    public class DogCacheEntry
    {
        public int Id { get; set; }
        public string Breed { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CachedAt { get; set; }
    }
}
