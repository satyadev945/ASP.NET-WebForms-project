using System.IO;
using System.Reflection;
using Xunit;

namespace Films.Tests
{
    public class AllTests
    {
        [Fact]
        public void CheckTestFilesExist()
        {
            // Check if test directories exist
            string testRootPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..", "..", "Tests");

            // Normalize path
            testRootPath = Path.GetFullPath(testRootPath);

            Assert.True(Directory.Exists(testRootPath), $"Test directory should exist at {testRootPath}");

            // Check some specific test files to ensure they were created
            string[] testDirectories = {
                "Films.Domain.Entities",
                "Films.Application.Services",
                "Films.Web.Controllers",
                "Films.Web.Models",
                "Films.Infrastructure.Data"
            };

            foreach (var dir in testDirectories)
            {
                string dirPath = Path.Combine(testRootPath, dir);
                Assert.True(Directory.Exists(dirPath), $"Directory {dir} should exist");

                // Check that the directory contains test files
                string[] testFiles = Directory.GetFiles(dirPath, "*Tests.cs");
                Assert.True(testFiles.Length > 0, $"Directory {dir} should contain test files");
            }
        }
    }
}