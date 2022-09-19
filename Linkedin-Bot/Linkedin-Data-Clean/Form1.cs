using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Linkedin_Data_Clean.Cleaners;

namespace Linkedin_Data_Clean
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void BtnCleanClientes_Click(object sender, EventArgs e)
        {
            if (Cleaner_Clientes.CleanClients(TxtDirectory.Text))
            {
                MessageBox.Show("Limpieza con exito !!!");
            }
            else
            {
                MessageBox.Show("Error en la limpieza !!!");
            }
        }

        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            var fileopen = new OpenFileDialog();
            fileopen.Title = "Buscar datos";
            fileopen.Filter = "csv files|*.csv";
            fileopen.CheckFileExists = true;
            if (fileopen.ShowDialog() == DialogResult.OK)
            {
                TxtDirectory.Text = fileopen.FileName;
            }
        }
    }
}
