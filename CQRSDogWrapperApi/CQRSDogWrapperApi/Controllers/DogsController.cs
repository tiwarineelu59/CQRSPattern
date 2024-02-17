using CQRSDogWrapperApi.Application.Commands;
using CQRSDogWrapperApi.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using CQRSDogWrapperApi.Domain.Entities;
using CQRSDogWrapperApi.Application.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CQRSDogWrapperApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DogsController : ControllerBase
    {
        private readonly DogService _dogService;

        public DogsController(DogService dogService)
        {
            _dogService = dogService;
        }

        //[Authorize]
        [HttpGet("{breed}")]
        public async Task<IActionResult> GetRandomDogImageUrl(string breed)
        {
            var userId = User.FindFirst(ClaimTypes.Name)?.Value;

            if (!String.IsNullOrEmpty(userId))
            {
                var imageUrl = await _dogService.GetRandomDogImageUrlAsync(breed);
                if (imageUrl == null)
                {
                    return NotFound();
                }

                return Ok(new { Breed = breed, ImageUrl = imageUrl });
            }
            else
            {
                return Unauthorized();

            }
        }
    }
   
}