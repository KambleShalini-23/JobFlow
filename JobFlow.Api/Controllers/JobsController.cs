using JobFlowApi.Models;
using JobFlowApi.DTO;
using JobFlowApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace JobFlowApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class JobsController : ControllerBase
{
    private readonly JobService _jobService;

    public JobsController(JobService jobService)
    {
        _jobService = jobService;
    }

    [HttpGet]
    public IActionResult GetAllJobs()
    {
        List<JobResponse> jobs = _jobService.GetAllJobs();
        return Ok(jobs);
    }

    [HttpGet("{id}")]
    public IActionResult GetJobById(int id)
    {
        var job = _jobService.GetJobById(id);

        return Ok(job);
    }

    [HttpPost]
    public IActionResult CreateJob(JobRequest jobRequest)
    {
        var job = _jobService.CreateJob(jobRequest);
        return Ok(job);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateJob(int id, JobRequest jobRequest)
    {
        var job = _jobService.UpdateJob(id, jobRequest);
        return Ok(job);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteJob(int id)
    {
        _jobService.DeleteJob(id);
        return NoContent();
    }

    [HttpGet("company/{companyId}")]
    public IActionResult GetJobsByCompanyId(int companyId)
    {
        var jobs = _jobService.GetJobsByCompanyId(companyId);
        return Ok(jobs);
    }

    [HttpGet("highest-salary/{salary}")]
    public IActionResult GetJobsByHighSalary(decimal salary)
    {
        var jobs = _jobService.GetJobsByHighSalary(salary);
        return Ok(jobs);
    }

    [HttpGet("ascending-order")]
    public IActionResult GetJobsByAscendingOrder()
    {
        var jobs = _jobService.GetJobsByAscendingOrder();
        return Ok(jobs);
    }

    [HttpGet("search")]
    public IActionResult GetJobsByTitle(string title)
    {
        var jobs = _jobService.GetJobsByTitle(title);
        return Ok(jobs);
    }

    [HttpGet("count")]
    public IActionResult GetJobsCount(string title)
    {
        var count = _jobService.GetJobsCount(title);
        return Ok(count);
    }
}
