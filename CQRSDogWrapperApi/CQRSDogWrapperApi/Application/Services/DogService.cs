using CQRSDogWrapperApi.Application.Queries;
using CQRSDogWrapperApi.Domain.Entities;
using CQRSDogWrapperApi.Infrastructure.Contract;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Net.Http;

namespace CQRSDogWrapperApi.Application.Services
{
    public class DogService
    {
        private readonly IMediator _mediator;
        private readonly HttpClient _httpClient;
        private readonly IDogRepository _repository;
        public DogService(IMediator mediator, HttpClient httpClient, IDogRepository repository)
        {
            _mediator = mediator;
            _httpClient = httpClient;
            _repository = repository;
            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri("https://dog.ceo/") 
            };
        }

        public async Task<string> GetRandomDogImageUrlAsync(string breed)
        {
            // Check if the image is cached
            var cachedImageUrl = await _mediator.Send(new GetCachedDogImageUrlQuery(breed));
            if (cachedImageUrl != null)
            {
                return cachedImageUrl.ToString();
            }

            // Fetch image from Dog API
            var imageUrl = await FetchImageFromApiAsync(breed);

            // Cache the image
            await CacheImageUrlAsync(breed, imageUrl);

            return imageUrl;
        }

        private async Task<string> FetchImageFromApiAsync(string dog_breed_name)
        {

            // Call Dog API and fetch image
            string url = string.Empty;

            if (!string.IsNullOrEmpty(dog_breed_name))
            {
                var splitBreedSubreed = dog_breed_name.Split(' ');
                if (splitBreedSubreed.Length > 1)
                {
                    url = string.Format("/api/breed/{0}/{1}/images/random", splitBreedSubreed[1].ToLower(), splitBreedSubreed[0].ToLower());
                }
                else
                {
                    url = string.Format("/api/breed/{0}/images/random", dog_breed_name.ToLower());
                }
            }
          
            var response = await _httpClient.GetAsync(url);
            var result = string.Empty;
            if (response.IsSuccessStatusCode)
            {
                var stringResponse = await response.Content.ReadAsStringAsync();
                var dynamicObj = JsonConvert.DeserializeObject<dynamic>(stringResponse);
                result = dynamicObj?.message;
            }
            else
            {
                throw new HttpRequestException(response.ReasonPhrase);
            }
            return result;
        }

        private async Task CacheImageUrlAsync(string breed, string imageUrl)
        {
            // Cache the image URL
            await _repository.CacheImageUrlAsync(breed, imageUrl);

        }
    }
}
