using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Escuela
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Contraseña_TextChanged(object sender, EventArgs e)
        {
            if (txtContraseña.Text == txtContraseña2.Text)
            {
                pictureBox4.Image = Properties.Resources.password;
            }
            else
            {
                pictureBox4.Image = Properties.Resources._lock;
            }
        }

        private void btnEntrar_Click(object sender, EventArgs e)
        {
            frmCargasAlumnos alumnos = new frmCargasAlumnos();
            alumnos.Show();
            this.Hide();
        }
    }
}
