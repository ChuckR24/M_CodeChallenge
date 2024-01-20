using System;

namespace CodeChallenge.Models
{
    public class Compensation
    {
        public Employee Employee {  get; set; }
        public int Salary {  get; set; }
        public DateOnly EffectiveDate { get; set; }
    }
}
