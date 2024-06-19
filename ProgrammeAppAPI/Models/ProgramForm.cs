using System.Text.Json.Serialization;

namespace ProgrammeAppAPI;

public class ProgramForm
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
}
