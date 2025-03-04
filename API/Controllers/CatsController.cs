using Application.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CatsController(IMapper mapper, ICatService catService) : ControllerBase
{
    [HttpPost("fetch")]
    public async Task<List<CatDto>> Fetch()
    {
        var catsData = await 
    }
}