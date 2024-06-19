namespace ProgrammeAppAPI;

public interface ICosmosDBService
{
    Task<ProgramForm> CreateProgramAsync(ProgramDTO program);
    Task<ProgramForm> GetProgramAsync(string programId);
    Task<List<ProgramForm>> GetAllProgramsAsync();
    Task<ProgramResponseDTO> UpdateProgramAsync(ProgramResponseDTO program);
    Task DeleteProgramAsync(string programId);
    Task<ProgramQuestion> CreateQuestionAsync(ProgramQuestion question);
    Task<List<ProgramQuestion>> GetQuestionsForProgramAsync(string programId);
    Task<ProgramQuestion> GetQuestionAsync(string questionId);
    Task<ProgramQuestion> UpdateQuestionAsync(ProgramQuestion question);
    Task DeleteQuestionAsync(string questionId);
}
