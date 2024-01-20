using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CodeChallenge.Services;
using CodeChallenge.Models;
using System.Text.Json.Nodes;
using System.Text.Json;

namespace CodeChallenge.Controllers
{
    [ApiController]
    [Route("api/employee")]
    public class EmployeeController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IEmployeeService _employeeService;

        public EmployeeController(ILogger<EmployeeController> logger, IEmployeeService employeeService)
        {
            _logger = logger;
            _employeeService = employeeService;
        }

        [HttpPost]
        public IActionResult CreateEmployee([FromBody] Employee employee)
        {
            _logger.LogDebug($"Received employee create request for '{employee.FirstName} {employee.LastName}'");

            _employeeService.Create(employee);

            return CreatedAtRoute("getEmployeeById", new { id = employee.EmployeeId }, employee);
        }

        [HttpGet("{id}", Name = "getEmployeeById")]
        public IActionResult GetEmployeeById(String id)
        {
            _logger.LogDebug($"Received employee get request for '{id}'");

            var employee = _employeeService.GetById(id);
            //employee.DirectReports = employee.DirectReports.Select(r => new Employee { EmployeeId = r.EmployeeId }).ToList();
            //employee.DirectReports.ForEach(r => r = new Employee() { EmployeeId = r.EmployeeId });



            if (employee == null)
                return NotFound();
            var response = convertDirectReportsToIds(employee);
            return Ok(response);
        }

        private JsonObject convertDirectReportsToIds(Employee employee)
        {
            var employeeIds = employee.DirectReports.Select(r => r.EmployeeId).ToList();

            var obj = JsonObject.Create(JsonSerializer.SerializeToElement(employee));
            obj.Remove("DirectReports");
            obj.Add("DirectReports", JsonSerializer.SerializeToNode(employeeIds));
            return obj;
        }

        [HttpGet("reporting-structure/{id}", Name = "getReportingStructure")]
        public IActionResult GetReportingStructure(String id)
        {
            _logger.LogDebug($"Received employee reporting structure get request for '{id}'");

            var employee = _employeeService.GetById(id);
            var numberOfReports = totalReports(employee); 

            if (employee == null)
                return NotFound();

            return Ok(new ReportingStructure(employee, numberOfReports));
        }

        private int totalReports(Employee employee)
        {
            if (employee.DirectReports == null) return 0;

            var total = employee.DirectReports.Count;
            
            employee.DirectReports.ForEach(e => total += totalReports(e));

            return total;
        }

        [HttpPut("{id}")]
        public IActionResult ReplaceEmployee(String id, [FromBody]Employee newEmployee)
        {
            _logger.LogDebug($"Recieved employee update request for '{id}'");

            var existingEmployee = _employeeService.GetById(id);
            if (existingEmployee == null)
                return NotFound();

            _employeeService.Replace(existingEmployee, newEmployee);

            return Ok(newEmployee);
        }
    }
}
