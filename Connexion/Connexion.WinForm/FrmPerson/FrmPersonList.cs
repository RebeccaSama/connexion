using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows.Forms;
using Connexion.BO;
using Connexion.DAL;
using System.Linq;
namespace Connexion.WinForm.FrmPerson
{
    public partial class FrmPersonList : Form
    {
        private List<Person> people;
        private readonly string connectionString;
        private readonly PersonDAO personDAO;
        public FrmPersonList()
        {
            InitializeComponent();
            people = new List<Person>();
            connectionString = ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString;
            personDAO = new PersonDAO("SqlServer");
        }

        private void FrmPersonList_Load(object sender, EventArgs e)
        {
            people = personDAO.GetAll().ToList();
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = people;
            dataGridView1.ClearSelection();
        }


        private void searchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form form = new FrmPersonSearch();
            if (form.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("ok");
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                FrmPersonEdit form = new FrmPersonEdit();
                if (form.ShowDialog() == DialogResult.OK)
                {
                    var person = form.Person;
                    personDAO.Add(person);
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
            catch (Exception ex)
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
                    for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)
                    {
                        var oldPerson = dataGridView1.SelectedRows[i].DataBoundItem as Person;
                        FrmPersonEdit form = new FrmPersonEdit(oldPerson);
                        if (form.ShowDialog() == DialogResult.OK)
                        {
                            var person = form.Person;
                            using (SqlConnection connection = new SqlConnection(connectionString))
                            {
                                connection.Open();
                                using (SqlCommand command = new SqlCommand("Sp_Person_Update", connection))
                                {
                                    command.CommandType = System.Data.CommandType.StoredProcedure;
                                    command.Parameters.Add("@id", System.Data.SqlDbType.VarChar).Value = person.Id;
                                    command.Parameters.Add("@name", System.Data.SqlDbType.VarChar).Value = person.Name;
                                    command.Parameters.Add("@phone", System.Data.SqlDbType.BigInt).Value = person.PhoneNumber;
                                    command.Parameters.Add("@birth_day", System.Data.SqlDbType.Date).Value = person.BirthDay;
                                    command.Parameters.Add("@picture", System.Data.SqlDbType.VarBinary).Value = person.Photo;
                                    command.ExecuteNonQuery();
                                }
                            }
                            var index = people.IndexOf(oldPerson);
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
