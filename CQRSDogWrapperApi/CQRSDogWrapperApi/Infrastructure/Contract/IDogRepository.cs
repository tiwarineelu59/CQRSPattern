namespace CQRSDogWrapperApi.Infrastructure.Contract
{
    public interface IDogRepository
    {
        Task<string> GetCachedImageUrlAsync(string breed);
        Task CacheImageUrlAsync(string breed, string imageUrl);
    }
}
