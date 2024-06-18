using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ProgrammeAppAPI;

[ApiController]
[Route("[controller]")]
public class ProgramController : ControllerBase
{
    public ProgramController()
    {
        
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return BadRequest();
    }
}
