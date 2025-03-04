using AutoMapper;
using Domain.Entities;
using Models.DTOs;

namespace Application.Helpers;

public class MapperProfiles: Profile
{
    public MapperProfiles()
    {
        CreateMap<Cat, CatDto>();
        CreateMap<Tag, TagDto>();
        CreateMap<CatImage, CatImageDto>();
    }
}