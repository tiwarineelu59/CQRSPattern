using CQRSDogWrapperApi.Domain.Entities;
using CQRSDogWrapperApi.Infrastructure.Contract;
using MediatR;
using Newtonsoft.Json;

namespace CQRSDogWrapperApi.Application.Commands
{
    public class GetDogImageCommandHandler : IRequestHandler<GetRandomDogImageUrlCommand, string>
    {
        private readonly HttpClient _httpClient;
        private readonly IDogRepository _repository;

        public GetDogImageCommandHandler(HttpClient httpClient, IDogRepository repository)
        {
            _httpClient = httpClient;
            _repository = repository;
        }

        public async Task<string> Handle(GetRandomDogImageUrlCommand request, CancellationToken cancellationToken)
        {
            var cachedImageUrl = await _repository.GetCachedImageUrlAsync(request.Breed);
            if (!string.IsNullOrEmpty(cachedImageUrl))
                return cachedImageUrl;

            var response = await _httpClient.GetStringAsync($"https://dog.ceo/api/breed/{request.Breed}/images/random");
            var dogApiResponse = JsonConvert.DeserializeObject<DogApiResponse>(response);
            var imageUrl = dogApiResponse.Message.FirstOrDefault();

            if (!string.IsNullOrEmpty(imageUrl))
                await _repository.CacheImageUrlAsync(request.Breed, imageUrl);

            return imageUrl;
        }
    }
}
