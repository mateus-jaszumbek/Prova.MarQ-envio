using Prova.MarQ.Domain.Entities;

namespace Prova.MarQ.Infra.Interfaces
{
    public interface ICompanyServices
    {
        Task<Company> AddCompany(Company company);
        Task<Company?> UpdateCompany(int companyId, Company company);
        Task<bool> DeleteCompany(int companyId);

        Task<List<Company>> GetAllCompanysAsync(int pageNumber, int pageSize);
        Task<Company?> GetCompanyByIdAsync(int companyId);
        Task<Company?> GetCompanyByNameAsync(string companyName);
    }
}
