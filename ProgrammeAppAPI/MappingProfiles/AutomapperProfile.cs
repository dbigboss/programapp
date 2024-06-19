using AutoMapper;

namespace ProgrammeAppAPI;

public class AutomapperProfile : Profile
{
    public AutomapperProfile()
    {
        CreateMap<ProgramDTO, ProgramForm>();
        CreateMap<QuestionDTO, ProgramQuestion>()
            .ReverseMap();
        CreateMap<QuestionDTO, ProgramQuestion>()
            .ReverseMap();
        CreateMap<QuestionResponseDTO, ProgramQuestion>()
            .ReverseMap();
        CreateMap<List<ProgramQuestion>, List<QuestionResponseDTO>>()
            .ReverseMap();
    }
}
