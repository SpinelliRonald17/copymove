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
using Microsoft.VisualBasic.Devices; 
using System.IO.Compression;
using ImageMagick;
using iTextSharp.text.pdf;
using PdfSharp.Pdf;
using PdfDocument = PdfSharp.Pdf.PdfDocument;
using PdfPage = PdfSharp.Pdf.PdfPage;
using PdfSharp.Drawing;
using Org.BouncyCastle.Asn1.X500;

//#############################################
// ## Autor: Ronald Jesus Contreras Spinelli ##
// ## Fecha: 13-07-2020                      ##
// ## inspiración mis 2 hijos                ##
//#############################################

namespace c_sahrp
{
    public partial class Form1 : Form
    {

        string stRutaOrigen;
        string stRutaDestino;

        Computer mycomputer = new Computer(); // Así accederemos al "FileSystem".
        string tipo = "carpeta"; // Para esta aplicación, para saber si copiar/mover un archivo a una carpeta.
        public Form1()
        {
            InitializeComponent();
        }

        // Habilitar ruta predeterminada
        public static
        string pathWithEnv = @"%USERPROFILE%\Downloads";
        string fullpath = Environment.ExpandEnvironmentVariables(pathWithEnv);


        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                tipo = "carpeta"; // El tipo se cambiará a archivo.
                button1.Enabled = true;
                button2.Enabled = false; //Para poder seleccionar un archivo y no una carpeta.
            }
            else // Si el radiobutton de la carpeta no esta seleccionado (significa lo contrario de la condición que antes hemos puesto).
            {
                tipo = "archivo"; // El tipo se cambiará a carpeta.
                button1.Enabled = false;
                button2.Enabled = true; //Para poder seleccionar una carpeta y no un archivo.
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //// El siguiente código serve para que si hacemos un click en Ok con el selector de carpeta, el texto del TextBox1 sea la ruta seleccionada.
            //var resultado = fbd1.ShowDialog();
            //if (resultado == DialogResult.OK)
            //{
            //    textBox1.Text = fbd1.SelectedPath;
            //}

            textBox1.Text = fullpath;
            FolderBrowserDialog folderDlg = new FolderBrowserDialog
            {
                ShowNewFolderButton = true
            };

            // Show the FolderBrowserDialog.

            DialogResult result = folderDlg.ShowDialog();

            if (result == DialogResult.OK)

            {

                textBox1.Text = folderDlg.SelectedPath;

                Environment.SpecialFolder root = folderDlg.RootFolder;

                fullpath = folderDlg.SelectedPath;

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {

            string error1;
            error1 = textBox1.Text;


            if (error1 == "")
            {
                MessageBox.Show("Debes llenar la Ruta Origen", "información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                // El siguiente código serve para que el texto del textbox2 sea igual a la ruta seleccionada + desde el último índice de "\", para copiar el nombre de la carpeta.
                var resultado = fbd1.ShowDialog();
                if (resultado == DialogResult.OK)
                {
                    textBox2.Text = fbd1.SelectedPath + textBox1.Text.Substring(textBox1.Text.LastIndexOf(@"\"));

                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // El siguiente código serve para que si hacemos un click en Ok con el selector de archivos, el texto del TextBox1 sea el archivo seleccionado.
            var resultado = ofd1.ShowDialog();
            if (resultado == DialogResult.OK)
            {

                textBox1.Text = ofd1.FileName;

            }

        }

        private void button5_Click(object sender, EventArgs e)
        {

            string error1;
            error1 = textBox1.Text;

            string error2;
            error2 = textBox2.Text;

            if (error1 == "")
            {
                MessageBox.Show("Debes llenar la Ruta Origen", "información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (error2 == "")
                {
                    MessageBox.Show("Debes llenar la Ruta Destino", "información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {

                    if (tipo == "carpeta")
                    {

                        mycomputer.FileSystem.CopyDirectory(textBox1.Text, textBox2.Text,true);


                        if (checkBox1.Checked)
                        {
                            // Count files
                            ShowFiles(fullpath, true);
                        }
                        else
                        {
                            // Count files
                            ShowFiles(fullpath, false);
                        }

                    } // Copiamos la carpeta.

                    if (tipo == "archivo")
                    {

                        mycomputer.FileSystem.CopyFile(textBox1.Text, textBox2.Text,true);

                        progressBar1.Value = 0;
                        progressBar1.Minimum = 0;
                        progressBar1.Maximum = 100000;
                        progressBar1.Step = 1;
                        progressBar1.ForeColor = Color.Red;

                        for (int i = 0; i < 100000; i++)
                        {
                            progressBar1.PerformStep();
                        }

                        lblTotalArchivos.Text = "Archivo encontrado y copiado = 1";

                        MessageBox.Show("Proceso Terminado", "LISTO", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    } // Copiamos el archivo


                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {


            string error1;
            error1 = textBox1.Text;

            string error2;
            error2 = textBox2.Text;

            if (error1 == "")
            {
                MessageBox.Show("Debes llenar la Ruta Origen", "información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (error2 == "")
                {
                    MessageBox.Show("Debes llenar la Ruta Destino", "información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    if (tipo == "carpeta")
                    {
                        mycomputer.FileSystem.MoveDirectory(textBox1.Text, textBox2.Text, true);

                        if (checkBox1.Checked)
                        {
                            // Count files
                            ShowFiles(textBox2.Text, true);
                        }
                        else
                        {
                            // Count files
                            ShowFiles(textBox2.Text, false);

                        }// Movemos la carpeta.
                    }


                    if (tipo == "archivo")
                    {
                        mycomputer.FileSystem.MoveFile(textBox1.Text, textBox2.Text, true);

                        progressBar1.Value = 0;
                        progressBar1.Minimum = 0;
                        progressBar1.Maximum = 100;
                        progressBar1.Step = 1;
                        progressBar1.ForeColor = Color.Red;

                        for (int i = 0; i < 100; i++)
                        {
                            progressBar1.PerformStep();
                        }

                        lblTotalArchivos.Text = "Archivo encontrado y movido = 1";

                        MessageBox.Show("Proceso Terminado", "LISTO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    // Movemos el archivo.
                   
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {

            if (checkBox2.Checked)
            {
                // Count files
                ShowFiles(textBox3.Text, true);
            }
            else
            {
                // Count files
                ShowFiles(textBox3.Text, false);
            }

            string error1;
            error1 = textBox3.Text;

            if (error1 == "")
            {
                MessageBox.Show("Debes llenar la Ruta Comprimir", "información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Title = "Guardar Archivo Comprimido";
                sfd.Filter = "Archivos comprimidos zip|*.zip";
                sfd.InitialDirectory = @"C:\comprimidos";
                if (sfd.ShowDialog().Equals(DialogResult.OK))

                {
                    ZipFile.CreateFromDirectory(textBox3.Text, sfd.FileName);
                    MessageBox.Show("Archivos Comprimidos", "LISTO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        //private void button7_Click(object sender, EventArgs e)
        //{
        //    string directorio = Path.GetDirectoryName(textBox2.Text) + "\\" + Path.GetFileNameWithoutExtension(textBox2.Text);
        //    if (Directory.Exists(directorio)) Directory.CreateDirectory(directorio);

        //    ZipFile.ExtractToDirectory(textBox2.Text, directorio);
        //    MessageBox.Show("Archivos Comprimidos", "LISTO", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //}

        private void button7_Click_1(object sender, EventArgs e)
        {
            //FolderBrowserDialog fbd = new FolderBrowserDialog();
            //fbd.SelectedPath = @"C:\";
            //if (fbd.ShowDialog().Equals(DialogResult.OK))
            //{
            //    textBox3.Text = fbd.SelectedPath;
            //    listBox1.Items.AddRange(Directory.GetFiles(textBox3.Text, "*.*"));
            //    lblTotalArchivos.Text = listBox1.Items.Count.ToString() + " Archivos encontrados";
            //}

            textBox1.Text = fullpath;
            FolderBrowserDialog folderDlg = new FolderBrowserDialog
            {
                ShowNewFolderButton = true
            };

            // Show the FolderBrowserDialog.

            DialogResult result = folderDlg.ShowDialog();

            if (result == DialogResult.OK)

            {

                textBox3.Text = folderDlg.SelectedPath;

                Environment.SpecialFolder root = folderDlg.RootFolder;

                fullpath = folderDlg.SelectedPath;

            }
        }

        void DrawImage(XGraphics gfx, string jpegSamplePath, int x, int y, int width, int height)
        {
            XImage image = XImage.FromFile(jpegSamplePath);
            gfx.DrawImage(image, x, y, width, height);
        }



        #region contar archivos
        public void ShowFiles(String pathFrom, Boolean incDirs)
        {
            // Si el subdirectorio está activo
            if (incDirs)
            {
                string[] allfiles = Directory.GetFiles(pathFrom, "*.*", SearchOption.AllDirectories);
                // Establecer el valor mínimo para la barra
                progressBar1.Minimum = 0;
                //Establecer el valor de la barra de longitud máxima relacionada con los archivos encontrados
                progressBar1.Maximum = allfiles.Length;
                // Acción para cada archivo
                foreach (var file in allfiles)
                {
                    // Aumenta la barra para cada archivo encontrado
                    progressBar1.Increment(1);
                    FileInfo info = new FileInfo(file);
                }
                // Acumular archivos
                lblTotalArchivos.Text = "Total de archivos leídos = " + progressBar1.Value.ToString();
                System.Windows.Forms.MessageBox.Show("Archivos encontrados: " + allfiles.Length.ToString(), "Listo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Barra de reinicio
                progressBar1.Value = 0;
            }
            else
            {
                // Obtener archivos
                string[] files = Directory.GetFiles(pathFrom);
                // Establecer el valor mínimo para la barra
                progressBar1.Minimum = 0;
                // Establecer el valor de la barra de longitud máxima relacionada con los archivos encontrados
                progressBar1.Maximum = files.Length;
                foreach (var file in files)
                {
                    //Aumenta la barra para cada archivo encontrado
                    progressBar1.Increment(1);
                    FileInfo info = new FileInfo(file);
                }
                // Acumular archivos
                lblTotalArchivos.Text = "Total de archivos leídos = " + progressBar1.Value.ToString();
                System.Windows.Forms.MessageBox.Show("Archivos encontrados: " + files.Length.ToString(), "Listo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Barra de reinicio
                progressBar1.Value = 0;
            }
        }

        #endregion

       


        }

}



        
    


       