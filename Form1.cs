using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StarducksManagementSystem
{
    public partial class Form1 : Form
    {
        frmMenu menu;
        frmPayment payment;
        frmOrderHistory history;
        frmStaffManagement employee;

        public Form1()
        {
            InitializeComponent();
            mdiProp();
        }
        private void mdiProp()
        {
            this.SetBevel(false);
            Controls.OfType<MdiClient>().FirstOrDefault().BackColor = Color.FromArgb(243, 244, 248);
        }
        private void InitializeMenuForm()
        {
            if (menu == null)
            {
                menu = new frmMenu();
                menu.FormClosed += Menu_FormClosed;
                menu.MdiParent = this;
                menu.Dock = DockStyle.Fill;
                menu.Show();
            }
            else
            {
                menu.Activate();
            }
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            InitializeMenuForm();
        }

        private void Menu_FormClosed(object sender, FormClosedEventArgs e)
        {
            menu = null;
        }

        private void btnPayment_Click(object sender, EventArgs e)
        {
            if (payment == null)
            {
                payment = new frmPayment();
                payment.FormClosed += Payment_FormClosed;
                payment.MdiParent = this;
                payment.Dock = DockStyle.Fill;
                payment.Show();
            }
            else
            {
                payment.Activate();
            }
        }

        private void Payment_FormClosed(object sender, FormClosedEventArgs e)
        {
            payment = null;
        }

        private void btnOrderHis_Click(object sender, EventArgs e)
        {
            if (history == null)
            {
                history = new frmOrderHistory();
                history.FormClosed += OrderHistory_FormClosed;
                history.MdiParent = this;
                history.Dock = DockStyle.Fill;
                history.Show(); 
            }
            else
            {
                history.Activate();
            }
        }

        private void OrderHistory_FormClosed(object sender, FormClosedEventArgs e)
        {
            history = null;
        }

        private void btnEmployee_Click(object sender, EventArgs e)
        {
            if (employee == null)
            {
                employee = new frmStaffManagement();
                employee.FormClosed += Employee_FormClosed;
                employee.MdiParent = this;
                employee.Dock = DockStyle.Fill;
                employee.Show();
            }
            else
            {
                employee.Activate();
            }
        }

        private void Employee_FormClosed(object sender, FormClosedEventArgs e)
        {
            employee = null;
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
