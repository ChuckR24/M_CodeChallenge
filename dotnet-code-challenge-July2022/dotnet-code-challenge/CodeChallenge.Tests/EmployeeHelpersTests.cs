
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;

using CodeChallenge.Models;
using CodeChallenge.Helpers;
using CodeCodeChallenge.Tests.Integration.Helpers;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Text.Json.Nodes;

namespace CodeCodeChallenge.Tests.Integration
{
    [TestClass]
    public class EmployeeHelperTests
    {
        private static HttpClient _httpClient;
        private static TestServer _testServer;
        private static ReportingStructure _sampleReportingStructure;

        [ClassInitialize]
        // Attribute ClassInitialize requires this signature
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public static void InitializeClass(TestContext context)
        {
            _testServer = new TestServer();
            _httpClient = _testServer.NewClient();

            Employee employee = new Employee()
            {
                EmployeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f",
                FirstName = "John",
                LastName = "Lennon",
                Position = "Development Manager",
                Department = "Engineering",
                DirectReports = new List<Employee>() {
                        new Employee(){
                            EmployeeId = "b7839309-3348-463b-a7e3-5de1c168beb3",
                            FirstName = "Paul",
                            LastName = "McCartney",
                            Position = "Developer I",
                            Department = "Engineering",
                            DirectReports = null
                        },
                        new Employee(){
                            EmployeeId = "03aa1462-ffa9-4978-901b-7c001562cf6f",
                            FirstName = "Ringo",
                            LastName = "Starr",
                            Position = "Developer V",
                            Department = "Engineering",
                            DirectReports = new List<Employee>() {
                                new Employee(){
                                    EmployeeId = "62c1084e-6e34-4630-93fd-9153afb65309",
                                    FirstName = "Pete",
                                    LastName = "Best",
                                    Position = "Developer I",
                                    Department = "Engineering",
                                    DirectReports = null
                                },
                                new Employee(){
                                    EmployeeId = "c0c2293d-16bd-4603-8e08-638a9d18b22c",
                                    FirstName = "George",
                                    LastName = "Harrison",
                                    Position = "Developer V",
                                    Department = "Engineering",
                                    DirectReports = null
                                }
                            }
                        }
                    }
            };

            //sample ReportingStructure JSON response with 4 directReports
            _sampleReportingStructure = new ReportingStructure(employee, 4);
        }

        [ClassCleanup]
        public static void CleanUpTest()
        {
            _httpClient.Dispose();
            _testServer.Dispose();
        }

        [TestMethod]
        public void convertToJsonWithDirectReportIds_With_Direct_Reports_Returns_Correct_JSON()
        {
            //Arrange
            List<string> expectedDirectReportsIds = new List<string> { "b7839309-3348-463b-a7e3-5de1c168beb3", "03aa1462-ffa9-4978-901b-7c001562cf6f" };

            //Execute
            var jsonObject = EmployeeHelpers.convertToJsonWithDirectReportIds(_sampleReportingStructure.employee);
            JsonArray directReports = (JsonArray) jsonObject["DirectReports"];

            //Assert
            Assert.IsTrue(expectedDirectReportsIds.Contains(directReports[0].ToString()));
            Assert.IsTrue(expectedDirectReportsIds.Contains(directReports[1].ToString()));
        }

        [TestMethod]
        public void convertToJsonWithDirectReportIds_With_Null_Direct_Reports_Returns_Correct_JSON()
        {
            //Arrange
            List<string> expectedDirectReportsIds = new List<string> { "b7839309-3348-463b-a7e3-5de1c168beb3", "03aa1462-ffa9-4978-901b-7c001562cf6f" };
            List<Employee> initialDirectReports = _sampleReportingStructure.employee.DirectReports;
            _sampleReportingStructure.employee.DirectReports = null;
            
            //Execute
            var jsonObject = EmployeeHelpers.convertToJsonWithDirectReportIds(_sampleReportingStructure.employee);
            var directReports = jsonObject["DirectReports"];

            //Assert
            Assert.IsNull(directReports);

            //Cleanup
            _sampleReportingStructure.employee.DirectReports = initialDirectReports;
        }

        [TestMethod]
        public void totalReports_With_Null_Direct_Reports_Returns_Zero()
        {
            //Arrange
            List<Employee> initialDirectReports = _sampleReportingStructure.employee.DirectReports;
            _sampleReportingStructure.employee.DirectReports = null;

            //Execute
            var numDirectReports = EmployeeHelpers.totalReports(_sampleReportingStructure.employee);

            //Assert
            Assert.AreEqual(numDirectReports, 0);

            //Cleanup
            _sampleReportingStructure.employee.DirectReports = initialDirectReports;
        }

        [TestMethod]
        public void totalReports_With_Direct_Reports_Returns_Correct_Number()
        {
            //Execute
            var numDirectReports = EmployeeHelpers.totalReports(_sampleReportingStructure.employee);

            //Assert
            Assert.AreEqual(numDirectReports, 4);
        }

    }
}
