using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace CSharpLab8_LINQ2Xml
{
    public partial class AddEmp : Form
    {
        //public delegate void EmployeeDataChangedDelegate(object sender, XElement data);
        public event EventHandler<XElement> EmployeeDataEntered;
        public AddEmp()
        {
            InitializeComponent();
        }

        private void butConfirm_Click(object sender, EventArgs e)
        {
            if((tbTabNo.Text != "")&& (tbLName.Text != "") &&
                (tbFName.Text != "") && (dtPickerHire.Text != ""))
            {
                // добавляем нового сотрудника
                // в
                // 1) temp list
                //      DataSource += (или add row)
                //      после Save - в Xml
                //ИЛИ
                // 2) сразу в Xml
                // удобней для поиска
                //  но тогда Delete нужно делать с Confirm
                XElement newEmp = new XElement("employee", new XAttribute("tabno", tbTabNo.Text),
                        new XElement("deptno", tbDept.Text),
                        new XElement("firstname", tbFName.Text),
                        new XElement("lastname", tbLName.Text),
                        new XElement("gender", tbGen.Text),
                        new XElement("salary", tbSal.Text),
                        new XElement("hiredate", dtPickerHire.Text),
                        new XElement("commision_pct", tbComm.Text),
                        new XElement("income_tax", tbTax.Text),
                        new XElement("days_worked", numUpDWorked.Value.ToString()),
                        new XElement("workdays", numUpDworkdays.Value.ToString()),
                        new XElement("gross_pay", tbGross.Text),
                        new XElement("withheld", tbWithheld.Text));
                //var eventSubscribers = EmployeeDataEntered;
                //if (eventSubscribers != null)
                //{
                //    eventSubscribers(this, newEmp);
                //}
                EmployeeDataEntered(this, newEmp);
                
            }
            this.Close();
        }
    }
}
