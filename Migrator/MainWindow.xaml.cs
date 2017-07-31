using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Xml.Linq;

namespace Migrator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            FillDataGrid();
        }


        private void FillDataGrid()
        {
            string ConString = @"Data Source=TALHAMALIK\TALHAMALIK;Initial Catalog=FASSQL;Integrated Security=True";
            string CmdString = string.Empty;
            using (SqlConnection con = new SqlConnection(ConString))
            {
                ////where Date LIKE %"+ DateTime.Now.ToString()+"%"
                CmdString = string.Format(@"SELECT * from dbo.RALog WHERE CAST(RALog.Date as DATE) = '" + DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd")+"'");
                SqlCommand cmd = new SqlCommand(CmdString, con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable customers = new DataTable("Raw_Log");
                sda.Fill(customers);


                DataSet dt = new DataSet("Futronic");
                dt.Tables.Add(customers);
               // sda.Fill(dt);
                string x = dt.GetXml();
                // XDocument myxml = XDocument.Load(@"Assets\Database.xml");
                using (StreamWriter fs = new StreamWriter(@"C:\My Backups\"+ DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") + ".xml")) // XML File Path
                {
                    try
                    {
                        dt.WriteXml(fs);
                    }
                    catch (Exception es)
                    {

                        Console.WriteLine(es.Message);
                    }

                }
                string result = dt.ToString();
            }
            break;
        }
    }
}
