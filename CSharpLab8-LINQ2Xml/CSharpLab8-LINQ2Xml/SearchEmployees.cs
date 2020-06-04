using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace CSharpLab8_LINQ2Xml
{
    public partial class SearchEmployees : Form
    {
        string workDir = Environment.CurrentDirectory;
        string path = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;

        string file; //= "EmpsCatalog.xml";
        XDocument doc  = null;
        public SearchEmployees()
        {
            InitializeComponent();
            file = path + "/EmpsCatalog.xml";
        }

        private void Load_Xml()
        {
            try { doc = XDocument.Load(file); }
            catch (FileNotFoundException) { MessageBox.Show("File not found\n" +
                "path: " + file); }
            if (doc != null) { 
            var employees = from e in doc.Descendants("employee")
                            select new
                            {
                                tabno = Convert.ToInt32((string)e.Attribute("tabno").Value),
                                deptno = (string)e.Element("deptno").Value,
                                name = (string)e.Element("lastname").Value + " " + e.Element("firstname").Value,
                                gender = (string)e.Element("gender").Value,
                                salary = (string)e.Element("salary").Value,
                                hiredate = e.Element("hiredate").Value,
                                commision_pct = e.Element("commision_pct").Value,
                                income_tax = e.Element("income_tax").Value,
                                days_worked = e.Element("days_worked").Value,
                                workdays = e.Element("workdays").Value,
                                gross_pay = e.Element("gross_pay").Value,
                                withheld = e.Element("withheld").Value
                            };
            dataGridEmps.DataSource = employees.ToList();
            }

        }
        private void butSearch_Click(object sender, EventArgs e)
        {

        }

        private void SearchEmployees_Load(object sender, EventArgs e)
        {
            Load_Xml();
        }
    }
}
