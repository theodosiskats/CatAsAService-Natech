using Application.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CatsController(IMapper mapper, IUnitOfWork uow) : ControllerBase
{
    // [HttpPost("fetch")]
    // public async Task<List<CatDto>> Fetch()
    // {
    //     
    // }
}