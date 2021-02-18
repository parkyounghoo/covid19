using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace covid19
{
    class Program
    {
        //static string connectionString = "server = localhost; uid = sa; pwd = 1111; database = PrivateData;";
        static string connectionString = "server = 10.200.5.73,1477; uid = savewind; pwd = savewind11!; database = CENTER_RAW;";
        static void Main(string[] args)
        {
            string date = "";
            if (args.Length == 0)
            {
                date = DateTime.Now.AddDays(-1).ToString("yyyyMMdd");
            }
            else
            {
                date = args[0];
            }
            
            Console.WriteLine("공공데이터");
            //공공데이터 포털 보건복지부_코로나19 시·도발생_현황
            dataGoKr gokr = new dataGoKr();
            gokr.covid19(date);
            //gokr.covid19_2();

            Console.WriteLine("서울");
            //코로나19-서울특별시
            covid19Seoul seoul = new covid19Seoul();
            seoul.covid19(date);

            Console.WriteLine("경기");
            //코로나19-경기도
            covid19Gyeonggi gyeonggi = new covid19Gyeonggi();
            gyeonggi.covid19();

            //Console.WriteLine("부산");
            //코로나19-부산광역시
            //covid19Busan busan = new covid19Busan();
            //busan.covid19();
            //코로나19-대구광역시

            //Console.WriteLine("인천");
            //코로나19-인천광역시
            //covid19Incheon incheon = new covid19Incheon();
            //incheon.covid19();

            //Console.WriteLine("광주");
            //코로나19-광주광역시
            //covid19Gwangju gwangju = new covid19Gwangju();
            //gwangju.covid19();

            //Console.WriteLine("대전");
            //코로나19-대전광역시
            //covid19Daejeon daejeon = new covid19Daejeon();
            //daejeon.covid19();

            //Console.WriteLine("울산");
            //코로나19-울산광역시
            //covid19Ulsan ulsan = new covid19Ulsan();
            //ulsan.covid19();

            //코로나19-세종특별자치시

            //코로나19-강원도

            //Console.WriteLine("충북");
            //코로나19-충청북도
            //covid19Chungbuk chungbuk = new covid19Chungbuk();
            //chungbuk.covid19();

            //Console.WriteLine("충남");
            //코로나19-충청남도
            //covid19Chungnam chungnam = new covid19Chungnam();
            //chungnam.covid19();

            //코로나19-전라북도

            //Console.WriteLine("전남");
            //코로나19-전라남도
            //covid19Jeonnam jeonnam = new covid19Jeonnam();
            //jeonnam.covid19();

            //코로나19-경상북도

            //Console.WriteLine("경남");
            //코로나19-경상남도
            //covid19Gyeongnam gyeongnam = new covid19Gyeongnam();
            //gyeongnam.covid19();

            //코로나19-제주특별자치도
            //covid19Jeju jeju = new covid19Jeju();
            //jeju.covid19();
        }

        public static void insert(string query)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.CommandTimeout = 0; // timeout
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static DataSet selectDS(string query)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                DataSet ds = new DataSet();
                SqlDataAdapter _SqlDataAdapter = new SqlDataAdapter();
                _SqlDataAdapter.SelectCommand = new SqlCommand(query, conn);
                _SqlDataAdapter.Fill(ds);

                return ds;
            }
        }
    }
}
