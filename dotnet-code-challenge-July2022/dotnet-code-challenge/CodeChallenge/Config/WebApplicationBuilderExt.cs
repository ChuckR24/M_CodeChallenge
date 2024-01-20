using CodeChallenge.Data;

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CodeChallenge.Config
{
    public static class WebApplicationBuilderExt
    {
        private static readonly string EMPLOYEE_DB_NAME = "EmployeeDB";
        private static readonly string COMPENSATION_DB_NAME = "CompensationDB";
        public static void UseEmployeeDB(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<EmployeeContext>(options =>
            {
                options.UseInMemoryDatabase(EMPLOYEE_DB_NAME);
            });
        }

        public static void UseCompensationDB(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<CompensationContext>(options =>
            {
                options.UseInMemoryDatabase(COMPENSATION_DB_NAME);
            });

        }
    }
}
