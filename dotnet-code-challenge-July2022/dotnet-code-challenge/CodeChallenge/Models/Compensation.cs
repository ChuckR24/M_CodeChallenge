using System;
using System.ComponentModel.DataAnnotations;

namespace CodeChallenge.Models
{
    public class Compensation
    {
        [Key]
        public String EmployeeId { get; set; }
        public int Salary {  get; set; }
        public DateTime EffectiveDate { get; set; }
    }
}
