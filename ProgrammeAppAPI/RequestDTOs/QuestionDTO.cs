namespace ProgrammeAppAPI;

public class QuestionDTO
{
    public string ProgramId { get; set; }
    public string Title { get; set; }
    public QuestionType Type { get; set; }
    public List<string> Options { get; set; } = new List<string>();
}
