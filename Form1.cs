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

namespace OLC12k20P2
{
    public partial class Form1 : Form
    {

        Analizador a = new Analizador();

        public Form1()
        {
            InitializeComponent();
            
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void abrirToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "c:\\Users\\Pistacho\\Desktop";
            openFileDialog.Filter = "Archivos er (*.er)|*.er";
            openFileDialog.FilterIndex = 0;
            openFileDialog.RestoreDirectory = true;

            string pathArchivo = "";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                pathArchivo = openFileDialog.FileName;
                string textoArchivoEntrada = File.ReadAllText(pathArchivo);

                entrada.AppendText(textoArchivoEntrada);
            }
            
            
        }

        private void ejecutarToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            
            String limpio = a.limpiarArchivo(entrada.Text, this);
            a.analizadorLexico(limpio, this);
        }

        private void analizarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            coincidir();
        }

        public void coincidir()
        {
            for(int i = 0; i < Analizador.expRegulares.Count; i++)
            {
                for(int j = 0; j < Analizador.entradas.Count; j++)
                {
                    if(Analizador.expRegulares[i].getNombre().Equals(Analizador.entradas[j].getNombre()))
                    {
                        //this.consola.Text += "el lexema: " + Analizador.entradas[j].getNombre() + " coincidio con la expr: " + Analizador.expRegulares[i].getNombre() + " se enviara a analizar la cadena: \n\r\n\r";
                        Analizador.expRegulares[i].mt.leerEntrada(Analizador.entradas[j].getCadena().Substring(1, Analizador.entradas[j].getCadena().Length - 2), Analizador.entradas[j].getColumna(), this);
                        //consola.Text += Analizador.entradas[j].getCadena().Substring(1, Analizador.entradas[j].getCadena().Length - 2) + "\n\r\n\r";
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string path = @"C: \Users\Pistacho\Desktop\reportesP2\" + textBox1.Text;

            pictureBox1.ImageLocation = path;
            //pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
        }
    }
}
