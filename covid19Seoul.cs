using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace covid19
{
    class covid19Seoul
    {
        protected ChromeDriverService _driverService = null;
        protected ChromeOptions _options = null;
        protected ChromeDriver _driver = null;

        public void covid19(string date)
        {
            try
            {
                int seqNo = int.Parse(Program.selectDS("select max(seq_no) from seoul_sigungu_covid19").Tables[0].Rows[0][0].ToString());

                _driverService = ChromeDriverService.CreateDefaultService();
                _driverService.HideCommandPromptWindow = true;

                _options = new ChromeOptions();
                _options.AddArgument("headless");
                _options.AddArgument("disable-gpu");

                _driver = new ChromeDriver(_driverService, _options);

                _driver.Navigate().GoToUrl("https://www.seoul.go.kr/coronaV/coronaStatus.do#status_page_top");

                _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                Console.WriteLine("시작");

                bool check = true;
                for (int i = 0; i < 100; i++)
                {
                    if (!check)
                    {
                        break;
                    }

                    var xPath = _driver.FindElementByXPath("//*[@id='DataTables_Table_0']/tbody");
                    IList<IWebElement> e1 = xPath.FindElements(By.ClassName("odd"));
                    IList<IWebElement> e2 = xPath.FindElements(By.ClassName("even"));

                    int cnt = e1.Count + e2.Count;
                    List<covid19_Seoul> list = new List<covid19_Seoul>();

                    for (int j = 0; j < cnt; j++)
                    {
                        covid19_Seoul model = new covid19_Seoul();

                        model.seq_no = _driver.FindElementByXPath("//*[@id='DataTables_Table_0']/tbody/tr[" + (j + 1) + "]/th").Text;
                        model.patient_no = _driver.FindElementByXPath("//*[@id='DataTables_Table_0']/tbody/tr[" + (j + 1) + "]/td[1]").Text;
                        model.confirmation_date = _driver.FindElementByXPath("//*[@id='DataTables_Table_0']/tbody/tr[" + (j + 1) + "]/td[2]").Text;
                        model.residence = _driver.FindElementByXPath("//*[@id='DataTables_Table_0']/tbody/tr[" + (j + 1) + "]/td[3]").Text;
                        model.trip = _driver.FindElementByXPath("//*[@id='DataTables_Table_0']/tbody/tr[" + (j + 1) + "]/td[4]").Text;
                        model.contact = _driver.FindElementByXPath("//*[@id='DataTables_Table_0']/tbody/tr[" + (j + 1) + "]/td[5]").Text;
                        model.discharge = _driver.FindElementByXPath("//*[@id='DataTables_Table_0']/tbody/tr[" + (j + 1) + "]/td[6]").Text;

                        if(int.Parse(model.seq_no) <= seqNo)
                        {
                            check = false;
                            break;
                        }

                        StringBuilder sb = new StringBuilder();
                        sb.Append(" insert into seoul_sigungu_covid19 values(");
                        sb.Append(" '" + model.seq_no.Replace("'","") + "',");
                        sb.Append(" '" + model.patient_no.Replace("'", "") + "',");
                        sb.Append(" '" + model.confirmation_date.Replace("'", "") + "',");
                        sb.Append(" '" + model.residence.Replace("'", "") + "',");
                        sb.Append(" '" + model.trip.Replace("'", "") + "',");
                        sb.Append(" '" + model.contact.Replace("'", "") + "',");
                        sb.Append(" '" + model.discharge.Replace("'", "") + "',");
                        sb.Append(" getdate() )");

                        Program.insert(sb.ToString());
                    }
                    
                    Console.WriteLine("next");
                    _driver.FindElementByXPath("//*[@id='DataTables_Table_0_next']").Click();
                    Thread.Sleep(5000);
                    Console.WriteLine("end");
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

    class covid19_Seoul
    {
        public string seq_no { get; set; }
        public string patient_no { get; set; }
        public string confirmation_date { get; set; }
        public string residence { get; set; }
        public string trip { get; set; }
        public string contact { get; set; }
        public string discharge { get; set; }
    }
}
