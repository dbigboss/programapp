using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ProgrammeAppAPI;

[ApiController]
[Route("api/[controller]")]
public class ProgramController : ControllerBase
{
    private readonly IAppLogic _appLogic;
    public ProgramController(IAppLogic appLogic)
    {
        _appLogic = appLogic;
    }

    /// <summary>
    /// Retrieves a list of all programs.
    /// </summary>
    /// <returns>A list of programs.</returns>
    [HttpGet]
    public async Task<IActionResult> GetPrograms()
    {
        var programs = await _appLogic.GetAllProgramsAsync();
        return Ok(programs);
    }

    /// <summary>
    /// Retrieves a program by id
    /// </summary>
    /// <param name="id">The ID of the program to get</param>
    /// <returns>A a single program.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProgram(string id)
    {
        var program = await _appLogic.GetProgramAsync(id);
        if (program == null)
        {
            return NotFound();
        }
        return Ok(program);
    }

     /// <summary>
    /// Creates a progrsm
    /// </summary>
    /// <returns>Return the created program.</returns>
    [HttpPost]
    public async Task<IActionResult> CreateProgram(ProgramDTO program)
    {
        var createdProgram = await _appLogic.CreateProgramAsync(program);
        return CreatedAtAction(nameof(GetProgram), new { id = createdProgram.Id }, createdProgram);
    }

     /// <summary>
    /// Updates an existing program
    /// </summary>
    /// <param name="id">The ID of the program to be updated</param>
    /// <returns>Returns 204 status code with no cotent</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProgram(string id, ProgramResponseDTO program)
    {
        if (id != program.Id)
        {
            return BadRequest();
        }

        await _appLogic.UpdateProgramAsync(program);
        return NoContent();
    }

     /// <summary>
    /// Deletes a program
    /// </summary>
    /// <param name="id">The ID of the program to be deleted</param>
    /// <returns>Returns 204 status code with no content.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProgram(string id)
    {
        await _appLogic.DeleteProgramAsync(id);
        return NoContent();
    }
}
