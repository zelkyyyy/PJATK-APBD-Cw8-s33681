using Cw8.DTOs;
using Cw8.Services;
using Microsoft.AspNetCore.Mvc;


namespace Cw8.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientsController : ControllerBase
{
    private readonly IPatientService _service;
    public PatientsController(IPatientService service)
    {
        _service = service;
    }
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? search, CancellationToken cancellationToken)
    {
        return Ok(await _service.GetPatientsAsync(search, cancellationToken));
    }

    [HttpPost("{pesel}/bedassignments")]
    public async Task<IActionResult> Add([FromRoute] string pesel, [FromBody] PostBedAssignmentDTO request, CancellationToken cancellationToken)
    {
        if (request == null)
        {
            return BadRequest("Invalid request");
        }

        try
        {
            var body = await _service.AddPatientAsync(pesel, request, cancellationToken);

            return CreatedAtAction(nameof(GetAll), new { search = pesel }, body);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
       
    }
}