using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace JobFlowApi.Models;

public class Company
{
    public int Id {get; set;} 

    public string Name {get; set;} = string.Empty;

    public string Website {get; set;} = string.Empty;

    public string Industry {get; set;} = string.Empty;

    public string Location {get; set;} = string.Empty;

    public long EmployeeCount {get; set;}

    public List<Job> Jobs {get; set;} = new List<Job>();


}