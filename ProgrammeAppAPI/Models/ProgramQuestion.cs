using System.Text.Json.Serialization;

namespace ProgrammeAppAPI;

public class ProgramQuestion
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    public string ProgramId { get; set; }
    public string Title { get; set; }
    public QuestionType Type { get; set; }
    public List<string> Options { get; set; } = new List<string>();
}

public enum QuestionType
{
    Text,
    Dropdown,
    YesNo,
    MultiChoice
}
