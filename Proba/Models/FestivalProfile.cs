using AutoMapper;

namespace Mladen_Kuridza.Models
{
    public class FestivalProfile : Profile
    {
        public FestivalProfile()
        {
            CreateMap<Festival, FestivalDTO>();

        }
    }
}
