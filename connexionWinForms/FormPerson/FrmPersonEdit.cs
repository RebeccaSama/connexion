using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using Connexion.BO;

namespace connexionWinForms.FormPerson
{
    public partial class FrmPersonEdit : Form
    {
        public Person Person { get; set; }
        public FrmPersonEdit()
        {
            InitializeComponent();
        }

        public FrmPersonEdit(Person person) : this()
        {
            Person = person;
            txtname.Text = person.Name;
            txtphone.Text = person.Phone.ToString();
            dateTimePicker1.Value = person.Birthday;
            if(person.Photo !=  null)
                 pictureBox1.Image = Image.FromStream(new MemoryStream(person.Photo));
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Images(*.jpg, *.jpeg, *.png, *.gif, *.tiff)|*.jpg, *.jpeg, *.png, *.gif, *.tiff";
            if(openFileDialog.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.ImageLocation = openFileDialog.FileName;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            long phone;
            long.TryParse(txtphone.Text, out phone);
            byte[] photo = null;
            if(!string.IsNullOrEmpty(pictureBox1.ImageLocation))
            {
                photo = File.ReadAllBytes(pictureBox1.ImageLocation);
            }
            else if(pictureBox1.Image != null)
            {
                //MemoryStream ms = new MemoryStream();
                //pictureBox1.Image.Save(ms, ImageFormat.Jpeg);
                //ms.Position = 0;
                //photo = ms.ToArray();
                photo = Person.Photo;
            }
            Person = new Person(Person?.Id ?? 0, txtname.Text, phone, dateTimePicker1.Value, photo);
        }
    }
}
