using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace covid19
{
    class covid19Gyeonggi
    {
        protected ChromeDriverService _driverService = null;
        protected ChromeOptions _options = null;
        protected ChromeDriver _driver = null;

        public void covid19()
        {
            try
            {
                int seqNo = int.Parse(Program.selectDS("select max(seq_no) from gyeonggi_sigungu_covid19").Tables[0].Rows[0][0].ToString());

                _driverService = ChromeDriverService.CreateDefaultService();
                _driverService.HideCommandPromptWindow = true;

                _options = new ChromeOptions();
                _options.AddArgument("headless");
                _options.AddArgument("disable-gpu");

                _driver = new ChromeDriver(_driverService, _options);

                _driver.Navigate().GoToUrl("https://gnews.gg.go.kr/briefing/brief_covid19.do");

                _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                Console.WriteLine("시작");

                bool check = true;
                for (int i = 0; i < 100; i++)
                {
                    if (!check)
                    {
                        break;
                    }

                    var xPath = _driver.FindElementByXPath("//*[@id='chk-table']/tbody");
                    IList<IWebElement> e1 = xPath.FindElements(By.TagName("tr"));

                    int cnt = e1.Count;
                    List<covid19_Gyeonggi> list = new List<covid19_Gyeonggi>();

                    for (int j = 0; j < cnt; j++)
                    {
                        covid19_Gyeonggi model = new covid19_Gyeonggi();                        
                        model.seq_no = _driver.FindElementByXPath("//*[@id='chk-table']/tbody/tr[" + (j + 1) + "]/td[1]").Text;
                        model.patient_no = _driver.FindElementByXPath("//*[@id='chk-table']/tbody/tr[" + (j + 1) + "]/td[2]").Text;
                        model.confirmation_date = _driver.FindElementByXPath("//*[@id='chk-table']/tbody/tr[" + (j + 1) + "]/td[3]").Text;
                        model.residence = _driver.FindElementByXPath("//*[@id='chk-table']/tbody/tr[" + (j + 1) + "]/td[4]").Text;
                        model.occurrence_history = _driver.FindElementByXPath("//*[@id='chk-table']/tbody/tr[" + (j + 1) + "]/td[5]").Text;
                        model.relevance = _driver.FindElementByXPath("//*[@id='chk-table']/tbody/tr[" + (j + 1) + "]/td[6]").Text;
                        model.hospital = _driver.FindElementByXPath("//*[@id='chk-table']/tbody/tr[" + (j + 1) + "]/td[7]").Text;

                        if (int.Parse(model.seq_no) <= seqNo)
                        {
                            check = false;
                            break;
                        }

                        StringBuilder sb = new StringBuilder();
                        sb.Append(" insert into gyeonggi_sigungu_covid19 values(");
                        sb.Append(" '" + model.seq_no.Replace("'","") + "',");
                        sb.Append(" '" + model.patient_no.Replace("'", "") + "',");
                        sb.Append(" '" + model.confirmation_date.Replace("'", "") + "',");
                        sb.Append(" '" + model.residence.Replace("'", "") + "',");
                        sb.Append(" '" + model.occurrence_history.Replace("'", "") + "',");
                        sb.Append(" '" + model.relevance.Replace("'", "") + "',");
                        sb.Append(" '" + model.hospital.Replace("'", "") + "',");
                        sb.Append(" getdate() )");

                        Program.insert(sb.ToString());
                    }

                    Console.WriteLine("next");
                    _driver.Navigate().GoToUrl("https://gnews.gg.go.kr/briefing/brief_covid19.do?page=" + (i + 2));
                    Thread.Sleep(5000);
                    Console.WriteLine("end");
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

    class covid19_Gyeonggi
    {
        public string seq_no { get; set; }
        public string patient_no { get; set; }
        public string confirmation_date { get; set; }
        public string residence { get; set; }
        public string occurrence_history { get; set; }
        public string relevance { get; set; }
        public string hospital { get; set; }
    }
}
