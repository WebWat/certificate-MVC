using IntegrationTests.Repositories;
using Microsoft.Coyote;
using Microsoft.Coyote.SystematicTesting;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace CoyoteTests
{
    public class SystematicTests
    {
        private readonly ITestOutputHelper _output;

        public SystematicTests(ITestOutputHelper output)
        {
            _output = output;
        }


        [Fact]
        public void TestCertificateRepository()
        {
            var tests = new CertificateRepositoryTests();
            RunSystematicTest(tests.GetAndUpdateAtTheSameTime, nameof(tests.GetAndUpdateAtTheSameTime));
        }

        private void RunSystematicTest(Func<Task> test, string testName)
        {
            _output.WriteLine("Start testing " + testName);

            var configuration = Configuration.Create().
                WithTestingIterations(10).
                WithVerbosityEnabled();

            var testingEngine = TestingEngine.Create(configuration, test);
            testingEngine.Run();

            Console.WriteLine($"Done testing. Found {testingEngine.TestReport.NumOfFoundBugs} bugs.");

            if (testingEngine.TestReport.NumOfFoundBugs > 0)
            {
                var error = testingEngine.TestReport.BugReports.First();

                Assert.True(false, $"Found bug: {error}");
            }
        }
    }
}
