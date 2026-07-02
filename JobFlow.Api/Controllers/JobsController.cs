using JobFlowApi.Models;
using JobFlowApi.DTO;
using JobFlowApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace JobFlowApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
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

    [HttpGet("{id:int}")]
    public IActionResult GetJobById(int id)
    {
        var job = _jobService.GetJobById(id);

        return Ok(job);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public IActionResult CreateJob(JobRequest jobRequest)
    {
        var job = _jobService.CreateJob(jobRequest);
        return Ok(job);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public IActionResult UpdateJob(int id, JobRequest jobRequest)
    {
        var job = _jobService.UpdateJob(id, jobRequest);
        return Ok(job);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
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

    [HttpGet("{me}")]
    public IActionResult GetLoggedInUser()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            throw new UnauthorizedAccessException("User is not logged in.");
        }
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        var role = User.FindFirst(ClaimTypes.Role)?.Value;

        return Ok(new
        {
            userId,
            email,
            role
        });
    }

}
