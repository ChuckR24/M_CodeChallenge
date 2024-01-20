using CodeChallenge.Models;
using System.Text.Json.Nodes;
using System.Linq;
using System.Text.Json;

namespace CodeChallenge.Helpers
{
    public class EmployeeHelpers
    {
        public static JsonObject convertToJsonWithDirectReportIds(Employee employee)
        {
            //convert employee to JSON 
            var employeeJson = JsonObject.Create(JsonSerializer.SerializeToElement(employee));

            if(employee.DirectReports != null)
            {
                //get a list of direct report ids
                var directReportIds = employee.DirectReports.Select(r => r.EmployeeId).ToList();

                //replace DirectReports attribute with list of ids
                employeeJson.Remove("DirectReports");
                employeeJson.Add("DirectReports", JsonSerializer.SerializeToNode(directReportIds));
            }

            return employeeJson;
        }

        public static int totalReports(Employee employee)
        {
            if (employee.DirectReports == null) return 0;

            var total = employee.DirectReports.Count;
            employee.DirectReports.ForEach(e => total += totalReports(e));

            return total;
        }
    } 
}
