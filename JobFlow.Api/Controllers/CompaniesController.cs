using JobFlowApi.DTO;
using JobFlowApi.Models;
using JobFlowApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobFlowApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CompaniesController : ControllerBase
{
    private readonly CompanyService _companyService;

    public CompaniesController(CompanyService companyService)
    {
        _companyService = companyService;
    }

    [HttpGet]
    public IActionResult GetAllCompanies()
    {
        List<Company> companies = _companyService.GetAllCompanies();
        return Ok(companies);
    }

    [HttpGet("{id}")]
    public IActionResult GetCompanyById(int id)
    {
        var company = _companyService.GetCompanyById(id);
        if (company == null)
        {
            return NotFound();
        }

        return Ok(company);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public IActionResult CreateCompany(CompanyRequest companyRequest)
    {
        Company company = _companyService.CreateCompany(companyRequest);
        return Ok(company);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteCompany(int id)
    {
        bool result = _companyService.DeleteCompany(id);
        return Ok("Company deleted successfully");
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public IActionResult UpdateCompany(int id, CompanyRequest companyRequest)
    {
        var company = _companyService.UpdateCompany(id, companyRequest);

        return Ok(company);
    }
}
