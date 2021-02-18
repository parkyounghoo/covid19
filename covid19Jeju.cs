using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace covid19
{
    class covid19Jeju
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

                _driver.Navigate().GoToUrl("https://www.jeju.go.kr/corona19.jsp");

                _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

                Console.WriteLine("시작");
                //IList<IWebElement> e1 = _driver.FindElements(By.Id("copperList")).FindElements(By.TagName("tr"));
                IList<IWebElement> e1 = _driver.FindElements(By.Id("copperList"));

                int cnt = e1.Count;

                for (int i = 0; i < cnt; i++)
                {
                    covid19_Jeju model = new covid19_Jeju();
                    IList<IWebElement> e2 = e1[i].FindElements(By.TagName("td"));
                    model.patient_no = e2[0].Text;
                    model.confirmation_date = e2[2].Text;
                    model.residence = "제주시";
                    model.occurrence_history = e2[1].Text;
                    model.discharge = e2[3].Text;

                    StringBuilder sb = new StringBuilder();
                    sb.Append(" insert into covid19_Jeju values(");
                    sb.Append(" '" + model.patient_no.Replace("'", "") + "',");
                    sb.Append(" '" + model.confirmation_date.Replace("'", "") + "',");
                    sb.Append(" '" + model.residence.Replace("'", "") + "',");
                    sb.Append(" '" + model.occurrence_history.Replace("'", "") + "',");
                    sb.Append(" '" + model.discharge.Replace("'", "") + "')");

                    Program.insert(sb.ToString());
                }

                Console.WriteLine("종료");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
            finally
            {
                _driverService.Dispose();
            }
        }
    }

    class covid19_Jeju
    {
        public string patient_no { get; set; }
        public string confirmation_date { get; set; }
        public string residence { get; set; }
        public string occurrence_history { get; set; }
        public string discharge { get; set; }
    }
}