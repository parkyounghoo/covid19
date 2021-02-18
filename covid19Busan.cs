using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace covid19
{
    internal class covid19Busan
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

                _driver.Navigate().GoToUrl("http://www.busan.go.kr/covid19/Course01.do");

                _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                Console.WriteLine("시작");

                bool check = true;
                while(check)
                {
                    var xPath = _driver.FindElementByXPath("//*[@id='contents']/div/div/button[1]");
                    
                    check = xPath.GetCssValue("display") == "block" ? true : false;

                    if (check)
                    {
                        _driver.FindElementByXPath("//*[@id='contents']/div/div/button[1]").Click();
                    }
                }

                IList<IWebElement> e1 = _driver.FindElementByClassName("list_body").FindElements(By.TagName("ul"));

                int cnt = e1.Count;

                for (int i = 0; i < cnt; i++)
                {
                    covid19_Busan model = new covid19_Busan();
                    model.patient_no = _driver.FindElementByXPath("//*[@id='contents']/div/div/div[2]/ul[" + (i + 1) + "]/li[1]").Text;
                    model.confirmation_date = _driver.FindElementByXPath("//*[@id='contents']/div/div/div[2]/ul[" + (i + 1) + "]/li[2]").Text;
                    model.residence = _driver.FindElementByXPath("//*[@id='contents']/div/div/div[2]/ul[" + (i + 1) + "]/li[3]").Text;
                    model.infection = _driver.FindElementByXPath("//*[@id='contents']/div/div/div[2]/ul[" + (i + 1) + "]/li[4]").Text;
                    model.cure = _driver.FindElementByXPath("//*[@id='contents']/div/div/div[2]/ul[" + (i + 1) + "]/li[5]").Text;
                    model.contactor_cnt = _driver.FindElementByXPath("//*[@id='contents']/div/div/div[2]/ul[" + (i + 1) + "]/li[6]").Text;

                    StringBuilder sb = new StringBuilder();
                    sb.Append(" insert into covid19_Busan values(");
                    sb.Append(" '" + model.patient_no.Replace("'", "") + "',");
                    sb.Append(" '" + model.confirmation_date.Replace("'", "") + "',");
                    sb.Append(" '" + model.residence.Replace("'", "") + "',");
                    sb.Append(" '" + model.infection.Replace("'", "") + "',");
                    sb.Append(" '" + model.cure.Replace("'", "") + "',");
                    sb.Append(" '" + model.contactor_cnt.Replace("'", "") + "')");

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

    internal class covid19_Busan
    {
        public string patient_no { get; set; }
        public string confirmation_date { get; set; }
        public string residence { get; set; }
        public string infection { get; set; }
        public string cure { get; set; }
        public string contactor_cnt { get; set; }
    }
}