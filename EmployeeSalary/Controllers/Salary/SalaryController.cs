using Application.Dto;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Servicehost.Controllers 
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalaryController : ControllerBase
    {
        private readonly ISalaryService _salaryService;
        private readonly ISalaryCalculationService _overtimeCalculationService; 
        private readonly ILogger<SalaryController> _logger;

        public SalaryController(ISalaryService salaryService)
        {
            _salaryService = salaryService ?? throw new ArgumentNullException(nameof(salaryService));
        }

        [HttpPost("ProcessRaw/{datatype}")]
        [ProducesResponseType(typeof(List<Salary>), StatusCodes.Status200OK)] 
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ProcessRawData([FromRoute] string datatype, [FromBody] string rawData)
        {
            if (string.IsNullOrWhiteSpace(rawData))
            {
                return BadRequest("Raw data cannot be empty.");
            }

            var results = await _salaryService.ProcessRawDataAsync(datatype, rawData);
            return Ok(results);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Salary), StatusCodes.Status201Created)] 
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddSalary([FromBody] SalaryCalculationRequest request)
        {
            if (request == null)
            {
                return BadRequest("Request body cannot be null.");
            }

            var createdSalary = await _salaryService.AddSalaryAsync(request);

            if (createdSalary != null && createdSalary.Id > 0)
            {
                return CreatedAtAction(nameof(GetSalaryById), new { id = createdSalary.Id }, createdSalary);
            }
            return StatusCode(StatusCodes.Status201Created, createdSalary);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Salary), StatusCodes.Status200OK)] 
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSalaryById(int id)
        {
            var salary = await _salaryService.GetSalaryByIdAsync(id);
            if (salary == null)
            {
                return NotFound($"Salary with ID {id} not found.");
            }
            return Ok(salary);
        }

        [HttpGet("employee/{employeeId}")]
        [ProducesResponseType(typeof(List<Salary>), StatusCodes.Status200OK)] 
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSalariesForEmployee([FromRoute] int employeeId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {

            var salaries = await _salaryService.GetSalariesForEmployeeAsync(employeeId, startDate, endDate);
            return Ok(salaries ?? new List<Salary>()); 
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Salary), StatusCodes.Status200OK)] 
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateSalary(int id, [FromBody] SalaryUpdateData request)
        {
            if (request == null)
            {
                return BadRequest("Request body cannot be null for update.");
            }

            var updatedResult = await _salaryService.UpdateSalaryAsync(id, request);
            if (updatedResult == null)
            {
                return NotFound($"Salary with ID {id} not found for update.");
            }
            return Ok(updatedResult);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteSalary(int id)
        {
            var existingSalary = await _salaryService.GetSalaryByIdAsync(id);
            if (existingSalary == null)
            {
                return NotFound($"Salary with ID {id} not found for deletion.");
            }

            await _salaryService.DeleteSalaryAsync(id);
            return NoContent();
        }

        [HttpPost("CalculateWithOvertime")]
        public async Task<IActionResult> CalculateSalaryWithOvertime([FromBody] SalaryCalculationRequest request)
        {
            if (request == null) return BadRequest("Request is null.");

            try
            {
                var result = await _overtimeCalculationService.CalculateAsync(request); 
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating overtime salary.");
                return StatusCode(500, "An internal server error occurred.");
            }
        }

    }
}
