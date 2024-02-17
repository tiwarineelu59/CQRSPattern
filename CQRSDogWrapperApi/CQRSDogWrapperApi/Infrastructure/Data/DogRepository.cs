using CQRSDogWrapperApi.Domain.Entities;
using CQRSDogWrapperApi.Infrastructure.Contract;
using Microsoft.Data.SqlClient;
using System.Data;
using Dapper;

namespace CQRSDogWrapperApi.Infrastructure.Data
{
    public class DogRepository : IDogRepository
    {
        private readonly DapperContext _context;

        private readonly HttpClient _httpClient;
        private readonly string _connectionString;

        public DogRepository(HttpClient httpClient, DapperContext context)
        {
            _httpClient = httpClient;
            //_connectionString = configuration.GetConnectionString("SqlConnection");
            _context = context;

        }

        public async Task<string> GetCachedImageUrlAsync(string breed_name)
        {
            var splitBreedSubreed = breed_name.Split(' ');
            var dog_breed = (splitBreedSubreed.Length > 1) ? splitBreedSubreed[1] + "-" + splitBreedSubreed[0] : breed_name;

            // Retrieve cached image URL from the database
            using (var connection = _context.CreateConnection())
            {
                return await connection.ExecuteScalarAsync<string>(
                "SELECT TOP 1 image_path FROM Breeds WHERE breed_name = @breed_name  ORDER BY cached_date DESC ",
                new { breed_name = dog_breed });

                //var parameters = new { breed_name = dog_breed.ToLower(), image_path = "" };
                //var apiResponse = await connection.QueryAsync<string>("usp_DogsBreed", parameters, commandType: System.Data.CommandType.StoredProcedure);
                //return apiResponse;

            }


            //using var connection = new SqlConnection(_connectionString);
            //return await connection.ExecuteScalarAsync<string>(
            //    "SELECT TOP 1 image_path FROM Breeds WHERE breed_name = @breed_name  ORDER BY cached_date DESC ",
            //    new { breed_name = breed_name });
            //
        }

        public async Task CacheImageUrlAsync(string breed_name, string image_path)
        {
            var splitBreedSubreed = breed_name.Split(' ');
            var dog_breed = (splitBreedSubreed.Length > 1) ? splitBreedSubreed[1] + "-" + splitBreedSubreed[0] : breed_name;

            // Cache the image URL in the database
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(
                "INSERT INTO Breeds (breed_name, image_path) VALUES (@breed_name, @image_path)",
                new { breed_name = dog_breed, image_path = image_path });
            }
        }


    }

    //public class DogRepository
    //{
    //    private readonly string _connectionString;

    //    public DogRepository(string connectionString)
    //    {
    //        _connectionString = connectionString;
    //    }

    //    //public async Task InsertOrUpdateDogCacheEntryAsync(DogCache entry)
    //    //{
    //    //    //, @CachedAt
    //    //    // target.CachedAt = source.CachedAt

    //    //    //    using var connection = new SqlConnection(_connectionString);
    //    //    //    await connection.ExecuteAsync(@"
    //    //    //    MERGE INTO Breeds AS target
    //    //    //    USING (VALUES (@breed_name, @image_path)) AS source (breed_name, image_path)
    //    //    //    ON target.Breed = source.Breed
    //    //    //    WHEN MATCHED THEN
    //    //    //        UPDATE SET target.image_path = source.image_path
    //    //    //    WHEN NOT MATCHED THEN
    //    //    //        INSERT (breed_name, image_path) VALUES (source.breed_name, source.image_path);
    //    //    //", entry);
    //    //}

    //    //public async Task<DogCache> GetDogCacheEntryAsync(string breed)
    //    //{
    //    //    using var connection = new SqlConnection(_connectionString);
    //    //    return await connection.QueryFirstOrDefaultAsync<DogCache>(
    //    //        "SELECT * FROM Breeds WHERE breed_name = @breed_name", new { breed_name = breed });
    //    //}
    //}
}