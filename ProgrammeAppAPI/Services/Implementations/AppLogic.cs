
using System.Linq;

namespace ProgrammeAppAPI;

public class AppLogic : IAppLogic
{
    private readonly ICosmosDBService _db;
    public AppLogic(ICosmosDBService cosmosDBService)
    {
        _db = cosmosDBService;
    }

    #region PROGRAM
    public async Task<ProgramForm> CreateProgramAsync(ProgramDTO program)
    {
        var result = await _db.CreateProgramAsync(program);

        return result;
    }

    public async Task DeleteProgramAsync(string programId)
    {
        await _db.DeleteProgramAsync(programId);
    }

    public async Task<List<ProgramResponseDTO>> GetAllProgramsAsync()
    {
        var result = new List<ProgramResponseDTO>();
        var programs = await _db.GetAllProgramsAsync();

        if (programs != null && programs.Any())
        {
            foreach (var item in programs)
            {
                var questions = await _db.GetQuestionsForProgramAsync(item.Id);
                var program = new ProgramResponseDTO
                {
                    Description = item.Description,
                    Id = item.Id,
                    Title = item.Title,
                    Questions = questions?.Select(q => new QuestionResponseDTO { Id = q.Id, Options = q.Options, ProgramId = q.ProgramId, Title = q.Title, Type = q.Type }).ToList()
                };

                result.Add(program);
            }
        }

        return result;
    }

    public async Task<ProgramResponseDTO> GetProgramAsync(string programId)
    {
        var program = await _db.GetProgramAsync(programId);
        var questions = await _db.GetQuestionsForProgramAsync(programId);

        var result = new ProgramResponseDTO
        {
             Description = program.Description,
             Id = program.Id,
             Title = program.Title,
            Questions = questions?.Select(q => new QuestionResponseDTO { Id = q.Id, Options = q.Options, ProgramId = q.ProgramId, Title = q.Title, Type = q.Type }).ToList()
        };

        return result;
    }

    public Task<ProgramResponseDTO> UpdateProgramAsync(ProgramResponseDTO program)
    {
        var result = _db.UpdateProgramAsync(program);
        return result;
    }

    #endregion
}
