using JobFlowApi.Data;
using JobFlowApi.DTO;
using JobFlowApi.Models;
using Microsoft.EntityFrameworkCore;

namespace JobFlowApi.Services;

public class CompanyService
{

    private readonly AppDbContext _context;

    public CompanyService(AppDbContext context)
    {
        _context = context;
    }

    public Company CreateCompany(CompanyRequest request)
    {
        var company = new Company
        {
            Name = request.Name,
            Website = request.Website,
            Industry = request.Industry,
            Location = request.Location,
            EmployeeCount = request.EmployeeCount
        };

        _context.Companies.Add(company);
        _context.SaveChanges();

        return company;
    }

    public List<Company> GetAllCompanies()
    {
        return  _context.Companies
                        .Include(c => c.Jobs)
                        .ToList();
    }

    public Company GetCompanyById(int id)
    {
        var company = _context.Companies
                        .Include(c => c.Jobs)
                        .FirstOrDefault(c => c.Id == id);

        if (company == null) 
        {
            // Throwing an exception satisfies the compiler that null will never be returned
            throw new KeyNotFoundException($"Company with ID {id} does not exist."); 
        }

        return company;
    }

    public bool DeleteCompany(int id)
    {
        var company = _context.Companies.Find(id);
        if (company == null) 
        {
            throw new KeyNotFoundException($"Company with ID {id} does not exist."); 
        }

       _context.Companies.Remove(company);
       _context.SaveChanges();

       return true;
    }

    public Company UpdateCompany(int id, CompanyRequest request)
    {
        var company = _context.Companies
                        .Include(c => c.Jobs)
                        .FirstOrDefault(c => c.Id == id);

        if (company == null) 
        {
            throw new KeyNotFoundException($"Company with ID {id} does not exist."); 
        }

        company.Name = request.Name;
        company.Website = request.Website;
        company.Industry = request.Industry;
        company.Location = request.Location;

        _context.SaveChanges();

        return company;
    }
}