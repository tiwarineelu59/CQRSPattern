using MediatR;

namespace CQRSDogWrapperApi.Application.Queries
{
    public class GetCachedDogImageUrlQuery : IRequest<string>
    {
        public string Breed { get; }

        public GetCachedDogImageUrlQuery(string breed)
        {
            Breed= breed;
        }
    }
}
