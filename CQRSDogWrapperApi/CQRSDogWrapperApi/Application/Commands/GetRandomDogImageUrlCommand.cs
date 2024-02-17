using MediatR;

namespace CQRSDogWrapperApi.Application.Commands
{
    //public class CacheDogCommand
    //{

    //    public string Breed { get; set; }
    //    public string ImageUrl { get; set; }
    //}

    public class GetRandomDogImageUrlCommand : IRequest<string>
    {
        public string Breed { get; }

        public GetRandomDogImageUrlCommand(string breed)
        {
            Breed = breed;
        }
    }
}

