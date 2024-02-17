using CQRSDogWrapperApi.Infrastructure.Contract;
using MediatR;

namespace CQRSDogWrapperApi.Application.Queries
{
    public class GetCachedDogImageQueryHandler: IRequestHandler<GetCachedDogImageUrlQuery, string>
    {
        private readonly IDogRepository _repository;

        public GetCachedDogImageQueryHandler(IDogRepository repository)
        {
            _repository = repository;
        }

        public async Task<string> Handle(GetCachedDogImageUrlQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetCachedImageUrlAsync(request.Breed);
        }
    }
}
