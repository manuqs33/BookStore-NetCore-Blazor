using AutoMapper;
using BookAPI.Data;
using BookAPI.Models.Author;

namespace BookAPI.Properties.Config
{
    public class MapperConfig: Profile
    {
        public MapperConfig()
        {
            CreateMap<CreateAuthorDto, Author>().ReverseMap();
        }
    }
}
