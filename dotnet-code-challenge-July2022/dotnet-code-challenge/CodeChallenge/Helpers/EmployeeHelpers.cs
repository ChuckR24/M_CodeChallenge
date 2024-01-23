using CodeChallenge.Models;
using System.Text.Json.Nodes;
using System.Linq;
using System.Text.Json;

namespace CodeChallenge.Helpers
{
    public class EmployeeHelpers
    {
        public static int totalReports(Employee employee)
        {
            //recursive function
            //assumes that employees cannot directly report to more than one other employee
            if (employee.DirectReports == null) return 0;

            var total = employee.DirectReports.Count;
            employee.DirectReports.ForEach(e => total += totalReports(e));

            return total;
        }
    } 
}
