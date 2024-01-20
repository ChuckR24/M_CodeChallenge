using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeChallenge.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using CodeChallenge.Data;

namespace CodeChallenge.Repositories
{
    public class CompensationRepository : ICompensationRepository
    {
        private readonly CompensationContext _compContext;
        private readonly ILogger<ICompensationRepository> _logger;

        public CompensationRepository(ILogger<ICompensationRepository> logger, CompensationContext compContext)
        {
            _compContext = compContext;
            _logger = logger;
        }

        public Compensation Add(Compensation compensation)
        {
            _compContext.Compensations.Add(compensation);
            return compensation;
        }

        public Compensation GetById(string id)
        {
            var compensations = _compContext.Compensations;
            return compensations.SingleOrDefault(c => c.EmployeeId == id);
        }

        public Task SaveAsync()
        {
            return _compContext.SaveChangesAsync();
        }
    }
}
