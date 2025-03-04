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
    public async Task<ActionResult<List<CatDto>>> Fetch()
    {
        try
        {
            var catsData = await catService.FetchCats();
            if (catsData == null) return BadRequest("Something went wrong while fetching data");
            return Ok(mapper.Map<List<CatDto>>(catsData));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}