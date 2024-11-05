using Microsoft.EntityFrameworkCore;
using Prova.MarQ.Domain.Entities;
using Prova.MarQ.Infra.Interfaces;

namespace Prova.MarQ.Infra.Services
{
    public class CompanyServices : ICompanyServices
    {
        private readonly ProvaMarqDbContext _context;

        public CompanyServices(ProvaMarqDbContext context)
        {
            _context = context;
        }
        public async Task<Company> AddCompany(Company company)
        {
            try
            {
                var existinCompany = await _context.Tbcompanies
                    .FirstOrDefaultAsync(c => c.Document == company.Document);

                if (existinCompany != null)
                {
                    throw new Exception("Uma empresa com o mesmo documento já existe.");
                }


                _context.Tbcompanies.Add(company);
                await _context.SaveChangesAsync();

                return company;
            }
            catch (DbUpdateException dbEx)
            {

                Console.WriteLine($"Erro ao adicionar a empresa: {dbEx.Message}");
                throw new Exception("Ocorreu um erro inesperado ao tentar adicionar a empresa ao banco de dados.");
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Erro inesperado: {ex.Message}");
                throw new Exception("Ocorreu um erro inesperado ao tentar adicionar a empresa.");
            }
        }

        public async Task<Company?> UpdateCompany(int companyId, Company company)
        {
            try
            {
                var entry = await _context.Tbcompanies.FindAsync(companyId);
                if (entry == null) { return null; }

                entry.Name = company.Name;
                entry.Document = company.Document;
                entry.IsDeleted = false;

                await _context.SaveChangesAsync();
                return entry;
            }
            catch (DbUpdateException dbEx)
            {
                Console.WriteLine($"Erro ao atualizar a empresa: {dbEx.Message}");
                throw new Exception("Ocorreu um erro ao tentar atualizar a empresa no banco de dados.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro inesperado: {ex.Message}");
                throw new Exception("Ocorreu um erro inesperado ao tentar atualizar a empresa.");
            }
        }

        public async Task<bool> DeleteCompany(int companyId)
        {
            try
            {
                var entry = await _context.Tbcompanies.FindAsync(companyId);
                if (entry == null) { return false; }

                entry.IsDeleted = true;
                _context.Tbcompanies.Update(entry);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException dbEx)
            {
                Console.WriteLine($"Erro ao excluir a empresa: {dbEx.Message}");
                throw new Exception("Ocorreu um erro ao tentar excluir a empresa do banco de dados.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro inesperado: {ex.Message}");
                throw new Exception("Ocorreu um erro inesperado ao tentar excluir a empresa.");
            }
        }

        public async Task<List<Company>> GetAllCompanysAsync(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                return await _context.Tbcompanies
                    .Include(c => c.Employees)
                    .Where(c => c.IsDeleted == false)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao obter todas as empresas: {ex.Message}");
                throw new Exception("Ocorreu um erro ao tentar obter a lista de empresas.");
            }
        }

        public async Task<Company?> GetCompanyByIdAsync(int companyId)
        {
            try
            {
                return await _context.Tbcompanies
                    .Include(c => c.Employees)
                    .Where(c => c.IsDeleted == false)
                    .FirstOrDefaultAsync(c => c.CompanyId == companyId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao obter a empresa: {ex.Message}");
                throw new Exception("Ocorreu um erro ao tentar obter a empresa.");
            }
        }

        public async Task<Company?> GetCompanyByNameAsync(string companyName)
        {
            try
            {

                return await _context.Tbcompanies
                          .Include(c => c.Employees)
                          .Where(c => c.Name.ToLower().Contains(companyName.ToLower()) || c.Document.Contains(companyName))
                          .FirstOrDefaultAsync();
            }
            catch (Exception)
            {

                throw new Exception("Ocorreu um erro ao tentar obter a empresa.");

            }

        }
    }
}
