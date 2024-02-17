namespace CQRSDogWrapperApi.Domain.Entities
{
    public class DogCache
    {
        public int Breed_Id { get; set; }
        public string Breed { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CachedAt { get; set; }
    }
}
