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
        XDocument doc = null;
        public SearchEmployees()
        {
            InitializeComponent();
            file = path + "/EmpsCatalog.xml";
        }

        private void Load_Xml(bool search = false)
        {
            try { doc = XDocument.Load(file); }
            catch (FileNotFoundException)
            {
                MessageBox.Show("File not found\n" + "path: " + file);
            }
            catch { MessageBox.Show("Error"); }
            if (doc != null)
            {
                try
                {
                    var employees = from e in doc.Descendants("employee")
                                    select new
                                    {
                                        tabno =(string)e.Attribute("tabno").Value,
                                        // Convert.ToInt32((string)e.Attribute("tabno").Value)
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
                    if (search)
                    {
                        dataGridEmps.DataSource = (from e in employees.ToList()
                                                   where (e.name.ToString().ToLower().Contains(tbName.Text.ToLower())
                                                   || tbName.Text.ToString() == "")
                                                   && (e.gender.ToString() == tbGender.Text.ToLower()
                                                   || tbGender.Text.ToString() == "")
                                                   && (e.hiredate.ToString().Contains(tbHireDate.Text.ToLower())
                                                   || tbHireDate.Text.ToString() == "")
                                                   && (e.deptno.ToString().ToLower().Contains(tbDept.Text.ToLower())
                                                   || tbDept.Text.ToString() == "")
                                                   select e).ToList();
                        //dataGridEmps.DataSource = employees.ToList()
                        //    //.Where(name => name.ToString().ToLower().Contains(tbName.Text.ToLower()))
                        //    .Where(gender => gender.ToString().ToLower().Contains(tbGender.Text.ToLower())).ToList();
                        //    //.Where(hiredate => hiredate.ToString().ToLower().Contains(tbHireDate.Text.ToLower()))
                        //    //.Where(deptno => deptno.ToString().ToLower().Contains(tbDept.Text.ToLower()));

                        //    //.Where(name => name.ToString().ToLower().Contains(tbName.Text.ToLower()))
                        //.Where(gender => gender.ToString().ToLower().Contains(tbGender.Text.ToLower()))
                        //.Where(hiredate => hiredate.ToString().ToLower().Contains(tbHireDate.Text.ToLower()))
                        //.Where(deptno => deptno.ToString().ToLower().Contains(tbDept.Text.ToLower()));
                    }
                    else
                        dataGridEmps.DataSource = employees.ToList();
                }
                catch(Exception ex) {
                    MessageBox.Show("Error acquired while reading XML file\n" + file
                        + "\n"+ ex.ToString());
                }
                }
            

        }
        private void butSearch_Click(object sender, EventArgs e)
        {
            Load_Xml(search: true);
        }

        private void SearchEmployees_Load(object sender, EventArgs e)
        {
            Load_Xml();
            //var employees = ((IEnumerable)dataGridEmps.DataSource).ToList();
        }

        private void butAdd_Click(object sender, EventArgs e)
        {
            AddEmp addEmployee = new AddEmp();
            //подписываем на событие,
            //которое передает данные о новом сотруднике
            addEmployee.EmployeeDataEntered += new EventHandler<XElement>(AddToXml);
            addEmployee.Show();
        }

        private void AddToXml(object sender, XElement data)
        {
            try { doc = XDocument.Load(file); }
            catch (FileNotFoundException)
            {
                MessageBox.Show("File not found\n" + "path: " + file);
            }
            catch { MessageBox.Show("Error"); }
            if (doc != null)
            {

                doc.Element("company").Add(data);
                //new XElement("employee", 
                //data.Element("employee").Attribute("tabno"),
                //    new XElement("deptno", data.Element("deptno").Value),
                //    new XElement("firstname", data.Element("firstname").Value),
                //    new XElement("lastname", data.Element("lastname").Value),
                //    new XElement("gender", data.Element("gender").Value),
                //    new XElement("salary", data.Element("salary").Value),
                //    new XElement("hiredate", data.Element("hiredate").Value),
                //    new XElement("commision_pct", data.Element("commision_pct").Value),
                //    new XElement("income_tax", data.Element("income_tax").Value),
                //    new XElement("days_worked", data.Element("days_worked").Value),
                //    new XElement("workdays", data.Element("workdays").Value),
                //    new XElement("gross_pay", data.Element("gross_pay").Value),
                //    new XElement("withheld", data.Element("withheld").Value)));
                doc.Save(file);
                Load_Xml();

            }
            
        }

        private void butDel_Click(object sender, EventArgs e)
        {
            //Dialog to ask if ...
            DialogResult dlg = MessageBox.Show("Are you sure you want to delete Employee(s)?",
                   "Delete Employee",
                   MessageBoxButtons.OKCancel,
                   MessageBoxIcon.Question);
            if (dlg == DialogResult.OK)
            {
                try { doc = XDocument.Load(file); }
                catch (FileNotFoundException)
                {
                    MessageBox.Show("File not found\n" + "path: " + file);
                }
                catch { MessageBox.Show("Error"); }

                if (doc != null)
                {
                    //Delete from xml
                    foreach (DataGridViewRow row in dataGridEmps.SelectedRows)
                    {
                        //dataGridEmps.Rows.RemoveAt(row.Index);
                        doc.Root.Elements()
                            .Where(el => el.Attribute("tabno").Value
                            == row.Cells[0].Value.ToString()).Remove();
                    }
                    doc.Save(file);

                    Load_Xml();
                }
            }
        }
    }
}
