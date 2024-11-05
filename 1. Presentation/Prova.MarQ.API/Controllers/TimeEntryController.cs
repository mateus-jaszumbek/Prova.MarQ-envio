using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Prova.MarQ.Domain.Entities;
using Prova.MarQ.Infra;
using Prova.MarQ.Infra.Interfaces;

namespace Prova.MarQ.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TimeEntryController : Controller
    {
        private readonly ITimeEntryServices _timeEntryServices;
        private readonly ProvaMarqDbContext _dbContext;

        public TimeEntryController(ITimeEntryServices timeEntryServices, ProvaMarqDbContext dbContext)
        {
            _timeEntryServices = timeEntryServices;
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<TimeEntry>>> GetAllTimeEntrysAsync(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var timeEntries = await _timeEntryServices.GetAllTimeEntrysAsync(pageNumber, pageSize);
                if (timeEntries == null || !timeEntries.Any()) { return NotFound("Nenhum ponto encontrada."); }

                var totalTimeEntries = await _dbContext.TbtimeEntries.CountAsync(e => e.IsDeleted == false);

                var response = new
                {
                    TotalCount = totalTimeEntries,
                    PageSize = pageSize,
                    CurrentPage = pageNumber,
                    TimeEntries = timeEntries
                };

                return Ok(timeEntries);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar ponto. Erro: {ex.Message}");
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterTimeEntryByPinAsync(string pin)
        {
            try
            {
                var timeEntry = await _timeEntryServices.RegisterTimeEntryByPinAsync(pin);
                if (timeEntry == null) { return NotFound("Nenhum ponto encontrado."); }

                return Ok(timeEntry);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar funcionários. Erro: {ex.Message}");
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteTimeEntryByPinAsync(int timeEntryId)
        {
            try
            {
                var success = await _timeEntryServices.DeleteTimeEntry(timeEntryId);
                if (success)
                {
                    return Ok("Ponto deletado com sucesso.");
                }
                else
                {
                    return NotFound($"Não foi possível deletar o ponto com ID {timeEntryId}.");
                }
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar deletar ponto. Erro: {ex.Message}");
            }
        }


    }
}
