using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace StarducksManagementSystem
{
    public partial class frmNewStaff : Form
    {
        private int? staffID;
        SqlConnection connect = new SqlConnection(@"Data Source=LYCHYLEANG\MSSQLSERVER2022;
                                Initial Catalog=StarDucks;User ID=sa;Password=123;TrustServerCertificate=True");

        public frmNewStaff(int? staffID = null)
        {
            InitializeComponent();
            this.staffID = staffID;

            if (staffID.HasValue)
            {
                LoadStaffDetail();
                lblTitle.Text = "Modify Staff's Information";
                btnSave.Text = "Update Staff";

            }
            else
            {
                lblTitle.Text = "Add New Staff";
                btnSave.Text = "Add Staff";
                SetPlaceHolder(txtStaffName, "Enter Staff Name");
                SetPlaceHolder(txtBD, "DD/MM/YYYY");
                SetPlaceHolder(txtPhone, "Enter Phone Number");
            }
        }

        private void LoadStaffDetail()
        {
            string query = "SELECT * FROM tbStaff WHERE StaffID = @StaffID";

            using (SqlCommand cmd = new SqlCommand(query, connect))
            {
                cmd.Parameters.AddWithValue("@StaffID", staffID.Value); // Use Value since staffId is nullable

                try
                {
                    connect.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    String staffGender = "";

                    if (reader.Read())
                    {
                        // Populate the form fields with employee data
                        txtStaffName.Text = reader["StaffName"].ToString();
                        cboPosition.Text = reader["Position"].ToString();
                        if (reader["DateofBirth"] != DBNull.Value)
                        {
                            DateTime birthDate = Convert.ToDateTime(reader["DateofBirth"]);
                            txtBD.Text = birthDate.ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            txtBD.Text = "";  // Handle case where BirthDate is null
                        }
                        txtPhone.Text = reader["PhoneNumber"].ToString();
                        staffGender = reader["Gender"].ToString();
                        if (staffGender == "Male")
                        {
                            rdoMale.Checked = true;
                        }
                        else if (staffGender == "Female")
                        {
                            rdoFemale.Checked = true;
                        }
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while loading employee data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connect.Close();
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (staffID.HasValue)
            {
                UpdateEmployee(); // Update the existing employee
            }
            else
            {
                AddNewEmployee(); // Add a new employee
            }

            this.DialogResult = DialogResult.OK; // Set dialog result to OK
            this.Close(); // Close the form
        }

        private void AddNewEmployee()
        {
            try
            {
                DateTime birthDate;
                // Validate if the birthdate is in the correct format (dd/MM/yyyy)
                if (!DateTime.TryParseExact(txtBD.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out birthDate))
                {
                    MessageBox.Show("Please enter a valid birth date in the format DD/MM/YYYY.", "Invalid Date", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Proceed with adding the new employee if the date is valid
                connect.Open();
                using (SqlCommand cmd = new SqlCommand("INSERT INTO tbStaff (StaffName, Gender, DateofBirth, Position, PhoneNumber) VALUES (@name, @sex, @bd, @pos, @ph)", connect))
                {
                    cmd.Parameters.AddWithValue("@Name", txtStaffName.Text);
                    string sex = rdoMale.Checked ? rdoMale.Text : rdoFemale.Text;
                    cmd.Parameters.AddWithValue("@sex", sex);
                    cmd.Parameters.AddWithValue("@bd", birthDate);
                    cmd.Parameters.AddWithValue("@pos", cboPosition.Text);
                    cmd.Parameters.AddWithValue("@ph", txtPhone.Text);

                    int rowAffected = cmd.ExecuteNonQuery();
                    if (rowAffected > 0)
                    {
                        MessageBox.Show("New employee added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("No rows were affected. Please check the data.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connect.Close();
            }
        }

        private void UpdateEmployee()
        {
            try
            {
                // Step 1: Parse the birthdate from the TextBox if needed
                DateTime birthDate;
                if (DateTime.TryParseExact(txtBD.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out birthDate))
                {
                    // Step 2: Open the connection
                    connect.Open();

                    // Step 3: Create the SQL update query
                    string query = "UPDATE tbStaff SET StaffName = @name, Position = @pos, DateofBirth = @bd, PhoneNumber = @ph, Gender = @sex WHERE StaffID = @StaffID";

                    // Step 4: Use SqlCommand to execute the update
                    using (SqlCommand cmd = new SqlCommand(query, connect))
                    {
                        // Add parameters
                        cmd.Parameters.AddWithValue("@name", txtStaffName.Text);
                        cmd.Parameters.AddWithValue("@pos", cboPosition.Text);
                        cmd.Parameters.AddWithValue("@bd", birthDate);
                        cmd.Parameters.AddWithValue("@ph", txtPhone.Text);
                        cmd.Parameters.AddWithValue("@sex", rdoMale.Checked ? "Male" : "Female");
                        cmd.Parameters.AddWithValue("@StaffID", staffID.Value);  // Assuming staffID is nullable

                        // Execute the query
                        int rowsAffected = cmd.ExecuteNonQuery();

                        // Step 5: Check if any row was updated
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Employee record updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Cannot update the record, StaffID does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please enter a valid date in the format dd/MM/yyyy.", "Invalid Date Format", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while updating the employee record: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Ensure the connection is closed
                if (connect != null && connect.State == ConnectionState.Open)
                {
                    connect.Close();
                }
            }
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmNewStaff_Load(object sender, EventArgs e)
        {
            LoadPosition();
        }

        private void LoadPosition()
        {
            string query = "SELECT PosName FROM tbPosition"; // Adjust to your table and column names

            try
            {
                connect.Open();
                using (SqlCommand cmd = new SqlCommand(query, connect))
                {
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        // Add each position from the database to the ComboBox
                        cboPosition.Items.Add(reader["PosName"].ToString());
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading positions: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connect.Close();
            }
        }

        private bool isUpdatingText = false;
        private bool isPlaceHolderActive = false;

        private void SetPlaceHolder(TextBox textBox, string placeholder)
        {
            // Unsubscribe from the TextChanged event to prevent unintended changes during initialization
            textBox.TextChanged -= txtBD_TextChanged;

            // Set the placeholder text and mark it as active
            textBox.Text = placeholder;
            textBox.ForeColor = Color.Gray;
            bool isPlaceholderActive = true; // Local flag to track placeholder for each specific TextBox

            textBox.Enter += (s, ev) =>
            {
                if (isPlaceholderActive)
                {
                    textBox.Text = "";
                    textBox.ForeColor = Color.FromArgb(88, 88, 88);
                    isPlaceholderActive = false; // Placeholder is now inactive
                }
            };

            textBox.Leave += (s, ev) =>
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    textBox.Text = placeholder;
                    textBox.ForeColor = Color.Gray;
                    isPlaceholderActive = true; // Placeholder is now active again
                }
            };

            // Resubscribe to the TextChanged event
            textBox.TextChanged += txtBD_TextChanged;
        }

        private void txtBD_TextChanged(object sender, EventArgs e)
        {
            // Check if the user is entering values, and we are not processing placeholder text
            if (isUpdatingText || txtBD.ForeColor == Color.Gray) return;

            // Prevent recursive execution
            isUpdatingText = true;

            try
            {
                // Automatically insert "/" after 2 and 5 characters (e.g., "DD/MM/YYYY")
                if (txtBD.Text.Length == 2 || txtBD.Text.Length == 5)
                {
                    if (!txtBD.Text.EndsWith("/"))
                    {
                        txtBD.Text += "/";
                        txtBD.SelectionStart = txtBD.Text.Length; // Move cursor to the end
                    }
                }

                // Prevent entering anything other than numbers and "/"
                if (!System.Text.RegularExpressions.Regex.IsMatch(txtBD.Text, "^[0-9/]*$"))
                {
                    txtBD.Text = txtBD.Text.Remove(txtBD.Text.Length - 1); // Remove invalid character
                    txtBD.SelectionStart = txtBD.Text.Length; // Move cursor to the end
                }
            }
            finally
            {
                // Reset the flag after text modifications are done
                isUpdatingText = false;
            }
        }
    }
}
