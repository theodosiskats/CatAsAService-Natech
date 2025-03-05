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
        var catsData = await catService.FetchCats();
        if (catsData == null) return BadRequest("Something went wrong while fetching data");
        return Ok(mapper.Map<List<CatDto>>(catsData));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CatDto>> Get(int id)
    {
        var foundCat = await catService.GetCatById(id);
        if (foundCat == null) return NotFound();
        return Ok(mapper.Map<CatDto>(foundCat));
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CatDto>>> GetCats(
        [FromQuery] string? tag,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        // Validate query params
        if (page < 1)
            return BadRequest("Page number must be greater than 0.");

        switch (pageSize)
        {
            case > 50:
                pageSize = 50;
                break;
            case < 1:
                return BadRequest("PageSize must be at least 1.");
        }
        
        // Fetch cats
        var cats = await catService.GetCatsAsync(tag, page, pageSize);
        return Ok(mapper.Map<IEnumerable<CatDto>>(cats));
    }

    
    
}