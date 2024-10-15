using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StarducksManagementSystem
{
    public partial class frmStaffManagement : Form
    {
        private Panel blurPanel;

        private DataGridViewImageColumn editImageColumn;
        private DataGridViewImageColumn deleteImageColumn;
        SqlConnection connect = new SqlConnection(@"Data Source=LYCHYLEANG\MSSQLSERVER2022;
                                Initial Catalog=StarDucks;User ID=sa;Password=123;TrustServerCertificate=True");
        public frmStaffManagement()
        {
            InitializeComponent();
            InitializeBlurPanel();
        }

        private void InitializeBlurPanel()
        {
            blurPanel = new Panel();
            blurPanel.Size = this.ClientSize;
            blurPanel.BackColor = Color.LightGray; // Adjust the transparency and color
            blurPanel.Visible = false; // Hidden initially
            blurPanel.Dock = DockStyle.Fill;
            this.Controls.Add(blurPanel);
            blurPanel.BringToFront(); // Ensure it stays on top of other controls
        }

        private void frmStaff_Load(object sender, EventArgs e)
        {
            this.ControlBox = false;
            FillStaffetail();
        }

        private void FillStaffetail()
        {
            string query = "SELECT * FROM tbStaff";
            using (SqlCommand cmd = new SqlCommand(query, connect))
            {
                try
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    tbStaffDataGridView.DataSource = dataTable;
                    tbStaffDataGridView.AllowUserToAddRows = true;

                    SetColumnVisivility();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while retrieving rental detail records: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            blurPanel.Visible = true; // set background to blur
            frmNewStaff addNewStaff = new frmNewStaff();
            if (addNewStaff.ShowDialog() == DialogResult.OK)
            {
                FillStaffetail();
            }
            blurPanel.Visible = false; // set background to normal
        }

        private void SetColumnVisivility()
        {
            tbStaffDataGridView.Columns["StaffID"].Visible = true;
            tbStaffDataGridView.Columns["StaffID"].Width = 80;
            tbStaffDataGridView.Columns["StaffID"].HeaderText = "No.";
            tbStaffDataGridView.Columns["StaffName"].Visible = true;
            tbStaffDataGridView.Columns["StaffName"].Width = 275;
            tbStaffDataGridView.Columns["StaffName"].HeaderText = "Staff Name";
            tbStaffDataGridView.Columns["Gender"].Visible = true;
            tbStaffDataGridView.Columns["Gender"].Width = 130;
            tbStaffDataGridView.Columns["Gender"].HeaderText = "Gender";
            tbStaffDataGridView.Columns["Position"].Visible = true;
            tbStaffDataGridView.Columns["Position"].Width = 175;
            tbStaffDataGridView.Columns["Position"].HeaderText = "Role";
            tbStaffDataGridView.Columns["PhoneNumber"].Visible = true;
            tbStaffDataGridView.Columns["PhoneNumber"].Width = 200;
            tbStaffDataGridView.Columns["PhoneNumber"].HeaderText = "Phone Number";
            tbStaffDataGridView.Columns["DateofBirth"].Visible = false;
            // Add Edit image column
            editImageColumn = new DataGridViewImageColumn();
            editImageColumn.HeaderText = "";
            editImageColumn.Name = "Edit";
            editImageColumn.Image = Image.FromFile(@"C:\Computer Science\Computer Science Year4\Semester I\OOAD and Prog\Assignment\StarducksManagementSystem\Assets\Pencil_Edit.png");
            tbStaffDataGridView.Columns.Add(editImageColumn);

            // Add Delete image column
            deleteImageColumn = new DataGridViewImageColumn();
            deleteImageColumn.HeaderText = "";
            deleteImageColumn.Name = "Delete";
            deleteImageColumn.Image = Image.FromFile(@"C:\Computer Science\Computer Science Year4\Semester I\OOAD and Prog\Assignment\StarducksManagementSystem\Assets\Trush_Icon_UIA.png");
            tbStaffDataGridView.Columns.Add(deleteImageColumn);
        }

        private void tbStaffDataGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            // Ensure the row index is valid
            if (e.RowIndex >= 0 && (tbStaffDataGridView.Columns[e.ColumnIndex].Name == "Edit" || tbStaffDataGridView.Columns[e.ColumnIndex].Name == "Delete"))
            {
                // Ensure the StaffID cell is not null or empty before painting images
                if (tbStaffDataGridView.Rows[e.RowIndex].Cells["StaffID"].Value != null)
                {
                    Image img = null;
                    if (tbStaffDataGridView.Columns[e.ColumnIndex].Name == "Edit")
                    {
                        img = editImageColumn.Image; // Get the Edit image
                    }
                    else if (tbStaffDataGridView.Columns[e.ColumnIndex].Name == "Delete")
                    {
                        img = deleteImageColumn.Image; // Get the Delete image
                    }

                    if (img != null)
                    {
                        // Fill the cell background with white or any color you want
                        e.Graphics.FillRectangle(new SolidBrush(Color.White), e.CellBounds);

                        // Calculate the position to center the image in the cell
                        int x = e.CellBounds.X + (e.CellBounds.Width - img.Width) / 2;
                        int y = e.CellBounds.Y + (e.CellBounds.Height - img.Height) / 2;

                        // Draw the image centered in the cell
                        e.Graphics.DrawImage(img, x, y, img.Width, img.Height);
                        e.Handled = true; // Indicate that you have handled the painting
                    }
                }
                else
                {
                    // Optionally set the background of the empty cell
                    e.Graphics.FillRectangle(new SolidBrush(Color.White), e.CellBounds);
                    e.Handled = true; // Indicate that you have handled the painting
                }
            }
        }

        private void tbStaffDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Ensure the row index is valid and the column is either Edit or Delete
            if (e.RowIndex >= 0 && (tbStaffDataGridView.Columns[e.ColumnIndex].Name == "Edit" || tbStaffDataGridView.Columns[e.ColumnIndex].Name == "Delete"))
            {
                // Ensure the StaffID cell is not null or empty before executing actions
                if (tbStaffDataGridView.Rows[e.RowIndex].Cells["StaffID"].Value != null)
                {
                    int staffId = Convert.ToInt32(tbStaffDataGridView.Rows[e.RowIndex].Cells["StaffID"].Value); // Get the StaffID
                    blurPanel.Visible = true; // set background to blur
                    if (tbStaffDataGridView.Columns[e.ColumnIndex].Name == "Edit")
                    {
                        // Handle the Edit button click
                        frmNewStaff editForm = new frmNewStaff(staffId); // Open the form for editing
                        if (editForm.ShowDialog() == DialogResult.OK)
                        {
                            FillStaffetail(); // Refresh the DataGridView after editing
                        }
                        blurPanel.Visible = false; // set the panel back to normal
                    }
                    else if (tbStaffDataGridView.Columns[e.ColumnIndex].Name == "Delete")
                    {
                        // Handle the Delete button click
                        var result = MessageBox.Show("Are you sure you want to delete this record?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (result == DialogResult.Yes)
                        {
                            // Delete the record from the database
                            DeleteRecord(staffId); // Implement this function to delete the record from the database
                            FillStaffetail(); // Refresh the DataGridView after deletion
                        }
                    }
                }
            }
        }

        private void DeleteRecord(int staffID)
        {
            
        }
    }
}
