using RESTMusicRecord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace TestProject1
{

    // Selenium tests that exercise the running RESTMusicRecord application (Swagger UI).
    // Configure the tests using environment variables:
    // - BASE_URL: base address where the API is running (default: https://localhost:5001)
    // - HEADLESS: if "true", Chrome runs headless (default: false)
    public class MusicRecordSeleniumTests : IDisposable
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;
        private readonly string _baseUrl;

        public MusicRecordSeleniumTests()
        {
            _baseUrl = "https://localhost:7010";
            ChromeOptions options = new ChromeOptions();

            string headless = Environment.GetEnvironmentVariable("HEADLESS") ?? "false";
            if (string.Equals(headless, "true", StringComparison.OrdinalIgnoreCase))
            {
                // Use the new headless flag when available; fallback is okay for older Chrome
                options.AddArgument("--headless=new");
            }

            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");
            options.AddArgument("--disable-gpu");

            // Prevent noisy ChromeDriver window
            ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            service.SuppressInitialDiagnosticInformation = true;
            service.HideCommandPromptWindow = true;

            _driver = new ChromeDriver(service, options, TimeSpan.FromSeconds(30));
            _driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(30);

            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }

        [Fact]
        public void SwaggerIndex_ShouldLoadAndContainContent()
        {
            // Arrange
            string url = $"{_baseUrl.TrimEnd('/')}/swagger/index.html";

            // Act
            _driver.Navigate().GoToUrl(url);

            // Wait until document ready
            _wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));

            // Assert: page returned content
            Assert.True(!string.IsNullOrWhiteSpace(_driver.PageSource), "Page source should not be empty.");
        }

        [Fact]
        public void SwaggerUi_ContainerShouldBePresent()
        {
            // Arrange
            string url = $"{_baseUrl.TrimEnd('/')}/swagger/index.html";

            // Act
            _driver.Navigate().GoToUrl(url);

            // Wait for the main swagger-ui container (this selector is stable for Swagger UI)
            bool found = false;
            try
            {
                _wait.Until(d => d.FindElement(By.CssSelector(".swagger-ui")) != null);
                found = true;
            }
            catch (WebDriverTimeoutException)
            {
                found = false;
            }

            // Assert
            Assert.True(found, "Expected the Swagger UI container (.swagger-ui) to be present on the page.");
        }

        public void Dispose()
        {
            try
            {
                _driver.Quit();
            }
            catch
            {
                // best-effort cleanup
            }

            _driver.Dispose();
        }
        
    }
}