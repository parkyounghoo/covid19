using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace covid19
{
    class covid19Incheon
    {
        protected ChromeDriverService _driverService = null;
        protected ChromeOptions _options = null;
        protected ChromeDriver _driver = null;

        public void covid19()
        {
            try
            {
                _driverService = ChromeDriverService.CreateDefaultService();
                _driverService.HideCommandPromptWindow = true;

                _options = new ChromeOptions();
                _options.AddArgument("disable-gpu");

                _driver = new ChromeDriver(_driverService, _options);

                _driver.Navigate().GoToUrl("https://www.incheon.go.kr/health/HE020409");

                _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                Console.WriteLine("시작");
                var xPath = _driver.FindElementByXPath("//*[@id='corona-tab-1']");
                IList<IWebElement> e1 = xPath.FindElements(By.ClassName("patinet-profile"));

                int cnt = e1.Count;

                for (int i = 0; i < cnt; i++)
                {
                    covid19_Incheon model = new covid19_Incheon();
                    model.patient_no = e1[i + 1].FindElements(By.TagName("strong"))[0].Text.Replace("#","");
                    model.residence = e1[i + 1].FindElements(By.TagName("strong"))[1].Text;
                    model.seq_no = Regex.Replace(e1[i].Text.Split('(')[0].Replace(model.patient_no, "").Replace(model.residence, ""), @"\D", "");
                    model.confirmation_date = e1[i].Text.Split('(')[1].Split('/')[0].Trim();
                    model.occurrence_history = e1[i].Text.Split('(')[1].Split('/')[1].Replace(")", "").Trim();

                    StringBuilder sb = new StringBuilder();
                    sb.Append(" insert into covid19_Incheon values(");
                    sb.Append(" '" + model.seq_no.Replace("'", "") + "',");
                    sb.Append(" '" + model.patient_no.Replace("'", "") + "',");
                    sb.Append(" '" + model.confirmation_date.Replace("'", "") + "',");
                    sb.Append(" '" + model.residence.Replace("'", "") + "',");
                    sb.Append(" '" + model.occurrence_history.Replace("'", "") + "')");

                    Program.insert(sb.ToString());
                }
                
                Console.WriteLine("종료");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            finally
            {
                _driverService.Dispose();
            }
        }
    }

    class covid19_Incheon
    {
        public string seq_no { get; set; }
        public string patient_no { get; set; }
        public string confirmation_date { get; set; }
        public string residence { get; set; }
        public string occurrence_history { get; set; }
    }
}
