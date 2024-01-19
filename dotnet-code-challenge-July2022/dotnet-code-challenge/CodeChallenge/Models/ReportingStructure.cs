using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeChallenge.Models
{
    public class ReportingStructure
    {
        public ReportingStructure(Employee employee, int numberOfReports) {
            this.employee = employee;
            this.numberOfReports = numberOfReports;
        } 
        public Employee employee { get; set; }
        public int numberOfReports { get; set; }
    }
}
