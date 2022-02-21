using System;
using System.Collections.Generic;
using System.Configuration; 
using System.Data.SqlClient;
using System.Windows.Forms;

namespace connexionWinForms.FormPerson
{
    public partial class FrmPersonList : Form
    {
        private List<Person> people;
        private readonly string connectionString;
       
        public FrmPersonList()
        {
            InitializeComponent();
            people = new List<Person>();
            connectionString = ConfigurationManager.ConnectionStrings["sqlserver"].ConnectionString;
        }

        private void searchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form form = new FrmPersonSearch();
            if(form.ShowDialog()== DialogResult.OK)
            {
                MessageBox.Show("ok");
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void FrmPersonList_Load(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("Sp_Person_Select", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        people.Add
                            (
                                new Person
                                (
                                   int.Parse(reader["id"].ToString()),
                                   reader["Nom"].ToString(), 
                                   long.Parse(reader["Tel"].ToString()),
                                   DateTime.Parse(reader["Date_nais"].ToString()),
                                   reader["Photo"] != DBNull.Value ? (byte[])reader["Photo"] : null


                                )
                            );
                    }
                }
            }
            
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = people;
            dataGridView1.ClearSelection();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                FrmPersonEdit form = new FrmPersonEdit();
                if (form.ShowDialog() == DialogResult.OK)
                {
                    var person = form.Person;
                    using (SqlConnection connnection = new SqlConnection(connectionString))
                    {
                        connnection.Open();
                        using (SqlCommand command = new SqlCommand("Sp_Person_Insert", connnection))
                        {
                            command.CommandType = System.Data.CommandType.StoredProcedure;
                            command.Parameters.Add("@Nom", System.Data.SqlDbType.VarChar).Value = person.Name;
                            command.Parameters.Add("@Tel", System.Data.SqlDbType.BigInt).Value = person.Phone;
                            command.Parameters.Add("@Birthday", System.Data.SqlDbType.Date).Value = person.Birthday;
                            command.Parameters.Add("@photo", System.Data.SqlDbType.VarBinary).Value = person.Photo;

                            command.ExecuteNonQuery();
                        }
                    }
                    people.Add(person);
                    dataGridView1.DataSource = null;
                    dataGridView1.DataSource = people;
                    MessageBox.Show
                        (
                            "Save done !",
                            "Confirmation",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show
                  (
                      ex.Message,
                      "Error",
                      MessageBoxButtons.OK,
                      MessageBoxIcon.Error
                  );
            }
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmPersonList_Load(sender, e);
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    var OldPerson = dataGridView1.SelectedRows[0].DataBoundItem as Person;
                    FrmPersonEdit form = new FrmPersonEdit(OldPerson);
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        var person = form.Person;
                        using (SqlConnection connnection = new SqlConnection(connectionString))
                        {
                            connnection.Open();
                            using (SqlCommand command = new SqlCommand("Sp_Person_Update", connnection))
                            {
                                command.CommandType = System.Data.CommandType.StoredProcedure;
                                command.Parameters.Add("@Id", System.Data.SqlDbType.VarChar).Value = person.Id;
                                command.Parameters.Add("@Nom", System.Data.SqlDbType.VarChar).Value = person.Name;
                                command.Parameters.Add("@Tel", System.Data.SqlDbType.BigInt).Value = person.Phone;
                                command.Parameters.Add("@Birthday", System.Data.SqlDbType.Date).Value = person.Birthday;
                                command.Parameters.Add("@photo", System.Data.SqlDbType.VarBinary).Value = person.Photo;

                                command.ExecuteNonQuery();
                            }
                        }
                        var index = people.IndexOf(OldPerson);
                        people[index] = person;
                        dataGridView1.DataSource = null;
                        dataGridView1.DataSource = people;
                        MessageBox.Show
                            (
                                "Save done !",
                                "Confirmation",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information
                            );
                    }
                
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show
                  (
                      ex.Message,
                      "Error",
                      MessageBoxButtons.OK,
                      MessageBoxIcon.Error
                  );
            }
            
            
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            editToolStripMenuItem_Click(sender, e);
        }
        private void dataGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            var dataGrid = (DataGridView)sender;
            var hti = dataGridView1.HitTest(e.X, e.Y);
            dataGridView1.ClearSelection();

            int rowIndex = hti.RowIndex;
            if (e.Button == MouseButtons.Right && rowIndex >= 0)
            {
                editToolStripMenuItem.Enabled = true;
                deleteToolStripMenuItem.Enabled = true;
                var row = dataGrid.Rows[rowIndex];
                row.Selected = true;
                dataGrid.Focus();
            }
            else
            {
                editToolStripMenuItem.Enabled = false;
                deleteToolStripMenuItem.Enabled = false;
            }
        }
    }
}
