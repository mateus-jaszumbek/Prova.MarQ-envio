using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Prova.MarQ.Domain.Entities;
using Prova.MarQ.Infra;
using Prova.MarQ.Infra.Interfaces;


namespace Prova.MarQ.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : Controller
    {
        private readonly ICompanyServices _companyServices;
        private readonly ProvaMarqDbContext _dbContext;

        public CompanyController(ICompanyServices companyServices, ProvaMarqDbContext dbContext)
        {
            _companyServices = companyServices;
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<Company>>> GetAllCompanysAsync(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var company = await _companyServices.GetAllCompanysAsync(pageNumber, pageSize);
                if (company == null || !company.Any()) { return NotFound("Nenhuma empresa encontrada."); }

                var totalCompanys = await _dbContext.Tbcompanies.CountAsync(e => e.IsDeleted == false);

                var response = new
                {
                    TotalCount = totalCompanys,
                    PageSize = pageSize,
                    CurrentPage = pageNumber,
                    Companys = company
                };


                return Ok(company);
            }
            catch (Exception ex)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar empresa. Erro: {ex.Message}");
            }
        }

        [HttpGet("{companyId:int}")]
        public async Task<ActionResult<Company>> GetCompanyById(int companyId)
        {
            try
            {
                var company = await _companyServices.GetCompanyByIdAsync(companyId);
                if (company == null) { return NotFound("Empresa não encontrada."); }

                return Ok(company);
            }
            catch (Exception ex)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar Empresa. Erro: {ex.Message}");
            }
        }
        [HttpGet("companyName/{companyName}")]
        public async Task<ActionResult<Company>> GetCompanyByName(string companyName)
        {
            try
            {
                var company = await _companyServices.GetCompanyByNameAsync(companyName);
                if (company == null) { return NotFound("Empresa não encontrada."); }

                return Ok(company);

            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar Procurar empresa. Erro: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Company>> AddCompany(Company company)
        {
            try
            {
                var newCompany = await _companyServices.AddCompany(company);
                if (newCompany == null) { return BadRequest("Não foi possível Adicionar a empresa."); }

                return CreatedAtAction(nameof(GetCompanyById), new { companyId = newCompany.CompanyId }, newCompany);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar Adicionar empresa. Erro: {ex.Message}");
            }
        }

        [HttpPut("{companyId:int}")]
        public async Task<ActionResult<Company>> UpdateCompany(int companyId, Company company)
        {
            try
            {
                var updatedCompany = await _companyServices.UpdateCompany(companyId, company);
                if (updatedCompany == null) { return NotFound("Não foi possível atualizar a empresa."); }

                return Ok(updatedCompany);
            }
            catch (Exception ex)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar atualizar o empresa. Erro: {ex.Message}");
            }


        }

        [HttpDelete("{companyId:int}")]
        public async Task<ActionResult> DeleteCompany(int companyId)
        {
            try
            {
                var success = await _companyServices.DeleteCompany(companyId);
                if (success)
                {
                    return Ok("Evento deletado com sucesso.");
                }
                else
                {
                    return NotFound($"Não foi possível deletar a empresa com ID {companyId}.");
                }
            }
            catch (Exception ex)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar deletar eventos. Erro: {ex.Message}");
            }



        }
    }
}
