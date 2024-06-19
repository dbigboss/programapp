using System.Text.Json.Serialization;

namespace ProgrammeAppAPI;

public class ProgramResponseDTO
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public List<QuestionResponseDTO> Questions { get; set; }
}
