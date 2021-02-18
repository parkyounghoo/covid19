using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Xml.Linq;

namespace covid19
{
    internal class dataGoKr
    {
        public void covid19(string date)
        {
            List<Covid19SidoInfState> list = new List<Covid19SidoInfState>();
            string url = "http://openapi.data.go.kr/openapi/service/rest/Covid19/getCovid19SidoInfStateJson"; // URL
            string param = "?serviceKey=" + "MCTvxxNKV%2BNJqOjy2XunYF3eQHIYbxSj7MgGZ%2F2gT8AJd2LA3mFeaeGijL5t8Ru%2BJ5%2FGS%2BU7fTINb7MuZ3sA%2Fw%3D%3D"; // Service Key
            param += "&pageNo=1";
            param += "&numOfRows=100";
            param += "&startCreateDt=" + date;
            param += "&endCreateDt=" + date;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + param);
            request.Method = "GET";

            string results2 = "";
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                results2 = reader.ReadToEnd();

                XDocument doc = XDocument.Parse(results2);

                var itemList = from r in doc.Descendants("item")
                               select new Covid19SidoInfState
                               {
                                   seq = r.Element("seq") == null ? "" : r.Element("seq").Value,
                                   gubun = r.Element("gubun") == null ? "" : r.Element("gubun").Value,
                                   incDec = r.Element("incDec") == null ? "" : r.Element("incDec").Value,
                                   deathCnt = r.Element("deathCnt") == null ? "" : r.Element("deathCnt").Value,
                                   isolClearCnt = r.Element("isolClearCnt") == null ? "" : r.Element("isolClearCnt").Value,
                                   qurRate = r.Element("qurRate") == null ? "" : r.Element("qurRate").Value,
                                   stdDay = r.Element("stdDay") == null ? "" : r.Element("stdDay").Value,
                                   createDt = r.Element("createDt") == null ? "" : r.Element("createDt").Value
                               };
                list = itemList.ToList();
            }

            Console.WriteLine(list.Count);

            for (int i = 0; i < list.Count; i++)
            {
                Console.WriteLine(list.Count + ": " + i + "번째 진행중....");

                StringBuilder sb = new StringBuilder();
                sb.Append(" EXEC PROC_dataGoKr_covid19Sido ");
                sb.Append(" '" + list[i].seq + "',");
                sb.Append(" '" + list[i].gubun + "',");
                sb.Append(" '" + list[i].incDec + "',");
                sb.Append(" '" + list[i].deathCnt + "',");
                sb.Append(" '" + list[i].isolClearCnt + "',");
                sb.Append(" '" + list[i].qurRate + "',");
                sb.Append(" '" + list[i].stdDay + "',");
                sb.Append(" '" + list[i].createDt + "'");

                Program.insert(sb.ToString());
            }
        }

        public void covid19_2()
        {
            List<Covid19SidoInfState> list = new List<Covid19SidoInfState>();
            string url = "http://openapi.data.go.kr/openapi/service/rest/Covid19/getCovid19InfStateJson"; // URL
            string param = "?serviceKey=" + "SfRfqLWT2LlZAqs5Ug3g9ro6HYeA3Xznw8tH%2Bs%2FGzE3exHM46aR%2BFlJgYMcov6dYn3csiT5rG16%2BLVi8IQbYtw%3D%3D"; // Service Key
            param += "&pageNo=1";
            param += "&numOfRows=100";
            param += "&startCreateDt=20190101";
            param += "&endCreateDt=20201231";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + param);
            request.Method = "GET";

            int totalCount = 0;
            string results = "";
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default, true);
                results = reader.ReadToEnd();
                XDocument doc = XDocument.Parse(results);
                Dictionary<string, string> dataDictionary = new Dictionary<string, string>();

                foreach (XElement element in doc.Descendants().Where(p => p.HasElements == false))
                {
                    int keyInt = 0;
                    string keyName = element.Name.LocalName;

                    if (keyName == "totalCount")
                    {
                        totalCount = int.Parse(element.Value);

                        break;
                    }
                }
            }

            int forCount = (totalCount / 100) + 1;
            int time = 0;

            for (int i = 1; i < forCount; i++)
            {
                HttpWebRequest request2 = (HttpWebRequest)WebRequest.Create(url + param);
                request2.Method = "GET";
                string results2 = "";

                using (HttpWebResponse response = request2.GetResponse() as HttpWebResponse)
                {
                    time++;
                    if (time == 50)
                    {
                        time = 0;
                        Console.WriteLine("대기중 ....");
                        Thread.Sleep(5000);
                    }

                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default, true);
                    results2 = reader.ReadToEnd();

                    XDocument doc = XDocument.Parse(results2);

                    var itemList = from r in doc.Descendants("item")
                                   select new Covid19Info
                                   {
                                       accDefRate = r.Element("accDefRate") == null ? "" : r.Element("accDefRate").Value,
                                       accExamCnt = r.Element("accExamCnt") == null ? "" : r.Element("accExamCnt").Value,
                                       accExamCompCnt = r.Element("accExamCompCnt") == null ? "" : r.Element("accExamCompCnt").Value,
                                       careCnt = r.Element("careCnt") == null ? "" : r.Element("careCnt").Value,
                                       clearCnt = r.Element("clearCnt") == null ? "" : r.Element("clearCnt").Value,
                                       createDt = r.Element("createDt") == null ? "" : r.Element("createDt").Value,
                                       deathCnt = r.Element("deathCnt") == null ? "" : r.Element("deathCnt").Value,
                                       decideCnt = r.Element("decideCnt") == null ? "" : r.Element("decideCnt").Value,
                                       examCnt = r.Element("examCnt") == null ? "" : r.Element("examCnt").Value,
                                       resutlNegCnt = r.Element("resutlNegCnt") == null ? "" : r.Element("resutlNegCnt").Value,
                                       seq = r.Element("seq") == null ? "" : r.Element("seq").Value,
                                       stateDt = r.Element("stateDt") == null ? "" : r.Element("stateDt").Value,
                                       stateTime = r.Element("stateTime") == null ? "" : r.Element("stateTime").Value,
                                       updateDt = r.Element("updateDt") == null ? "" : r.Element("updateDt").Value
                                   };

                    for (int j = 0; j < itemList.ToList().Count; j++)
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append(" insert into Covid19Info values(");
                        sb.Append(" '" + itemList.ToList()[j].accDefRate + "',");
                        sb.Append(" '" + itemList.ToList()[j].accExamCnt + "',");
                        sb.Append(" '" + itemList.ToList()[j].accExamCompCnt + "',");
                        sb.Append(" '" + itemList.ToList()[j].careCnt + "',");
                        sb.Append(" '" + itemList.ToList()[j].clearCnt + "',");
                        sb.Append(" '" + itemList.ToList()[j].createDt + "',");
                        sb.Append(" '" + itemList.ToList()[j].deathCnt + "',");
                        sb.Append(" '" + itemList.ToList()[j].decideCnt + "',");
                        sb.Append(" '" + itemList.ToList()[j].examCnt + "',");
                        sb.Append(" '" + itemList.ToList()[j].resutlNegCnt + "',");
                        sb.Append(" '" + itemList.ToList()[j].seq + "',");
                        sb.Append(" '" + itemList.ToList()[j].stateDt + "',");
                        sb.Append(" '" + itemList.ToList()[j].stateTime + "',");
                        sb.Append(" '" + itemList.ToList()[j].updateDt + "')");
                        Program.insert(sb.ToString());
                    }
                }
            }
                

                //for (int i = 0; i < list.Count; i++)
                //{
                //    StringBuilder sb = new StringBuilder();
                //    sb.Append(" insert into Covid19SidoInfState values(");
                //    sb.Append(" '" + list[i].seq + "',");
                //    sb.Append(" '" + list[i].gubun + "',");
                //    sb.Append(" '" + list[i].incDec + "',");
                //    sb.Append(" '" + list[i].deathCnt + "',");
                //    sb.Append(" '" + list[i].isolClearCnt + "',");
                //    sb.Append(" '" + list[i].qurRate + "',");
                //    sb.Append(" '" + list[i].stdDay + "',");
                //    sb.Append(" '" + list[i].createDt + "')");

                //    Program.insert(sb.ToString());
                //}
            }
    }

    internal class Covid19SidoInfState
    {
        //게시번호
        public string seq { get; set; }

        //시도명
        public string gubun { get; set; }

        //전일대비 증감 수
        public string incDec { get; set; }

        //사망자수
        public string deathCnt { get; set; }

        //격리 해제 수
        public string isolClearCnt { get; set; }

        //10만명당 발생률
        public string qurRate { get; set; }

        //기준일시
        public string stdDay { get; set; }

        //등록일시
        public string createDt { get; set; }
    }

    internal class Covid19Info
    {
        public string accDefRate { get; set; }
        public string accExamCnt { get; set; }
        public string accExamCompCnt { get; set; }
        public string careCnt { get; set; }
        public string clearCnt { get; set; }
        public string createDt { get; set; }
        public string deathCnt { get; set; }
        public string decideCnt { get; set; }
        public string examCnt { get; set; }
        public string resutlNegCnt { get; set; }
        public string seq { get; set; }
        public string stateDt { get; set; }
        public string stateTime { get; set; }
        public string updateDt { get; set; }
    }
}