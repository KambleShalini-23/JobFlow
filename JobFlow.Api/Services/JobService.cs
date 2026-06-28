using JobFlowApi.Models;
using JobFlowApi.Data;
using JobFlowApi.DTO;
using Microsoft.EntityFrameworkCore;

namespace JobFlowApi.Services;

public class JobService
{
    private readonly AppDbContext _context;

    public JobService(AppDbContext context){
        _context = context;
    }

    private static JobResponse MapToJobResponse(Job job)
    {
        return new JobResponse
        {
            Id = job.Id,
            Title = job.Title,
            Description = job.Description,
            Location = job.Location,
            Salary = job.Salary,
            CompanyId = job.CompanyId,
            CompanyName = job.Company?.Name ?? string.Empty
        };
    }

    public List<JobResponse> GetAllJobs()
    {
        
        return _context.Jobs
            .Include(j => j.Company)
            .Select(j => MapToJobResponse(j))
            .ToList();
    }

    public JobResponse GetJobById(int id)
    {
        var job = _context.Jobs
                .Include(j => j.Company)
                .FirstOrDefault(j => j.Id == id);

        if (job == null)
        {
            throw new KeyNotFoundException($"Job with Id {id} does not exist.");
        }

        return MapToJobResponse(job);
    }
    
    public JobResponse CreateJob(JobRequest request){
        if(!_context.Companies.Any(c => c.Id == request.CompanyId))
        {
            throw new KeyNotFoundException($"Company with Id {request.CompanyId} does not exist.");
        }
        var job = new Job
        {
            Title = request.Title,
            Description = request.Description,
            Location = request.Location,
            Salary = request.Salary,
            CompanyId = request.CompanyId
        };
        _context.Jobs.Add(job);
        _context.SaveChanges();

        job.Company = _context.Companies.Find(job.CompanyId);
        return MapToJobResponse(job);
    }

    public JobResponse UpdateJob(int id, JobRequest request)
    {
        if (!_context.Companies.Any(c => c.Id == request.CompanyId))
        {
            throw new KeyNotFoundException($"Company with Id {request.CompanyId} does not exist.");
        }
        var job = _context.Jobs.Find(id);                      
        if(job == null)
        {
            throw new KeyNotFoundException($"Job with Id {id} does not exist.");
        }
        job.Title = request.Title;
        job.Description = request.Description;
        job.Location = request.Location;
        job.Salary = request.Salary;
        job.CompanyId = request.CompanyId;

        _context.SaveChanges();

        job.Company = _context.Companies.Find(job.CompanyId);

        return MapToJobResponse(job);
    }

    public void DeleteJob(int id)
    {
        var job = _context.Jobs.Find(id);
        if(job == null)
        {
            throw new KeyNotFoundException($"Job with Id {id} does not exist.");
        }
        _context.Jobs.Remove(job);
        _context.SaveChanges();
    }

    public List<JobResponse> GetJobsByCompanyId(int companyId)
    {
        if(!_context.Companies.Any(c => c.Id == companyId))
        {
            throw new KeyNotFoundException($"Company with Id {companyId} does not exist.");
        }
        return _context.Jobs.Where( c => c.CompanyId == companyId)
                            .Include(j => j.Company)
                            .AsEnumerable()
                            .Select(j => MapToJobResponse(j))
                            .ToList();
    }

    public List<JobResponse> GetJobsByHighSalary(decimal salary)
    {
        return _context.Jobs.Where(j => j.Salary > salary)
                            .Include(j => j.Company)
                            .AsEnumerable()
                            .Select(j => MapToJobResponse(j)).ToList();
    }

    public List<JobResponse> GetJobsByAscendingOrder()
    {
        return _context.Jobs.OrderBy(j => j.Salary)
                            .Include(j => j.Company)
                            .AsEnumerable()
                            .Select(j => MapToJobResponse(j)).ToList();
    }

    public List<JobResponse> GetJobsByTitle(string title)
    {
        return _context.Jobs.Where(j => j.Title.Contains(title))
                            .Include(j => j.Company)
                            .AsEnumerable()
                            .Select(j => MapToJobResponse(j)).ToList();
    }

    public int GetJobsCount(string title)
    {
        return _context.Jobs.Count(j => j.Title.Contains(title));
    }
}

