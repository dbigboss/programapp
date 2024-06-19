namespace ProgrammeAppAPI;

public interface IAppLogic
{
    Task<ProgramForm> CreateProgramAsync(ProgramDTO program);
    Task<ProgramResponseDTO> GetProgramAsync(string programId);
    Task<List<ProgramResponseDTO>> GetAllProgramsAsync();
    Task<ProgramResponseDTO> UpdateProgramAsync(ProgramResponseDTO program);
    Task DeleteProgramAsync(string programId);
}
