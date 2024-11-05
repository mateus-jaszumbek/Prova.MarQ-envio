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
    public class EmployeeController : Controller
    {
        private readonly IEmployeeServices _employeeServices;
        private readonly ProvaMarqDbContext _dbContext;
        public EmployeeController(IEmployeeServices employeeServices, ProvaMarqDbContext dbContext)
        {
            _employeeServices = employeeServices;
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<Employee>>> GetAllEmployeesAsync(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var employee = await _employeeServices.GetAllEmployeesAsync(pageNumber, pageSize);
                if (employee == null || !employee.Any()) { return BadRequest("Nenhum funcionário encontrado."); }

                var totalEmployees = await _dbContext.Tbemployees.CountAsync(e => e.IsDeleted == false);

                var response = new
                {
                    TotalCount = totalEmployees,
                    PageSize = pageSize,
                    CurrentPage = pageNumber,
                    Employees = employee
                };


                return Ok(employee);
            }
            catch (Exception ex)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar funcionários. Erro: {ex.Message}");

            }
        }

        [HttpGet("{employeeId:int}")]
        public async Task<ActionResult<Employee>> GetEmployeeById(int employeeId)
        {
            try
            {
                var employee = await _employeeServices.GetEmployeeByIdAsync(employeeId);
                if (employee == null) { return BadRequest("Funcionário não encontrado."); }

                return Ok(employee);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar funcionário. Erro: {ex.Message}");
            }
        }
        [HttpGet("employeeName/{employeeName}")]
        public async Task<ActionResult<Employee>> GetEmployeeByNameAsync(string employeeName)
        {
            try
            {
                var employee = await _employeeServices.GetEmployeeByNameAsync(employeeName);
                if (employee == null) { return BadRequest("Funcionário não encontrado."); }

                return Ok(employee);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar funcionário. Erro: {ex.Message}");
            }
        }
        [HttpPost]
        public async Task<ActionResult<Employee>> AddEmployee(Employee employee)
        {
            try
            {
                var newEmployee = await _employeeServices.AddEmployee(employee);
                if (newEmployee == null) { return BadRequest("Não foi possível Adicionar o funcionário."); }

                return CreatedAtAction(nameof(GetEmployeeById), new { employeeId = newEmployee.EmployeeId }, newEmployee);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar Adicionar funcionário. Erro: {ex.Message}");
            }
        }

        [HttpPut("{employeeId:int}")]
        public async Task<ActionResult<Employee>> UpdateEmployee(int employeeId, Employee employee)
        {
            try
            {
                var updatedEmployee = await _employeeServices.UpdateEmployee(employeeId, employee);
                if (updatedEmployee == null) { return BadRequest("Funcionário não encontrado."); }
                return Ok(updatedEmployee);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar atualizar funcionário. Erro: {ex.Message}");
            }
        }

        [HttpDelete("{employeeId:int}")]
        public async Task<IActionResult> DeleteEmployee(int employeeId)
        {
            try
            {
                var success = await _employeeServices.DeleteEmployee(employeeId);
                if (success)
                {
                    return Ok("Funcionário deletado com sucesso.");
                }
                else
                {
                    return BadRequest($"Funcionário com ID {employeeId} não encontrado.");
                }
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar deletar funcionário. Erro: {ex.Message}");
            }
        }

    }
}
