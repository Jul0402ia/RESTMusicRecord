using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Xunit;

namespace TestProject1
{
    public class MusicRecordSeleniumTests : IDisposable
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;
        private readonly string _baseUrl;

        public MusicRecordSeleniumTests()
        {
            // Base url til hjemmesiden
            _baseUrl = "https://musicrecorddr-aybub4ede4hmdgcf.polandcentral-01.azurewebsites.net";

            ChromeOptions options = new ChromeOptions();

            // Kører uden at åbne browser vindue
            options.AddArgument("--headless=new");

            // Chrome indstillinger
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");
            options.AddArgument("--disable-gpu");

            // Godkender https certifikat
            options.AddArgument("--ignore-certificate-errors");
            options.AcceptInsecureCertificates = true;

            // Gemmer chromedriver vindue
            ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            service.SuppressInitialDiagnosticInformation = true;
            service.HideCommandPromptWindow = true;

            // Starter browser
            _driver = new ChromeDriver(service, options, TimeSpan.FromSeconds(30));

            // Ventetid
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(15));
        }

        [Fact]
        public void SwaggerIndex_ShouldLoad()
        {
            // Url til swagger
            string url = _baseUrl + "/swagger/index.html";

            // Åbner side
            _driver.Navigate().GoToUrl(url);

            // Venter til siden er loaded
            _wait.Until(driver =>
                ((IJavaScriptExecutor)driver)
                .ExecuteScript("return document.readyState")
                .ToString() == "complete");

            // Tjekker at siden har indhold
            Assert.False(string.IsNullOrWhiteSpace(_driver.PageSource));
        }

        [Fact]
        public void SwaggerUi_ShouldExist()
        {
            // Url til swagger
            string url = _baseUrl + "/swagger/index.html";

            // Åbner side
            _driver.Navigate().GoToUrl(url);

            // Finder swagger container
            IWebElement element = _wait.Until(driver =>
            {
                try
                {
                    return driver.FindElement(By.CssSelector(".swagger-ui"));
                }
                catch
                {
                    return null;
                }
            });

            // Tjekker om den findes
            Assert.NotNull(element);
        }

        public void Dispose()
        {
            try
            {
                // Lukker browser
                _driver.Quit();
            }
            catch
            {
            }

            _driver.Dispose();
        }
    }
}