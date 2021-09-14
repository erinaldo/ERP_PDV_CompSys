using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Atualizador_WEB
{
    public partial class AtualizadorWEB : Form
    {
        const string user = "admin";
        const string pass = "Zhaddmuswest02!";
        const string ftpPath = "ftp://khaddmussistemas.com:21/atualizadordue";
        static string[] files = null;
        public AtualizadorWEB()
        {
            InitializeComponent();
         
        }
        
        public void Atuaizar()
        {
            
            Console.ForegroundColor = ConsoleColor.White;

            while (true)
            {
                try
                {
                    Console.WriteLine("Iniciando leitura dos arquivos na pasta.");
                    
                    files = GetFileList().Split('\n').Where(f => !string.IsNullOrEmpty(f)).ToArray();
                    IniciarProgressBar(files.Count());
                    
                    Console.WriteLine("Encontrou " + files.Length + " arquivos...\n");
                    break;
                }
                catch (IOException ioe)
                {
                    progressBarControl1.Properties.Maximum = 0;
                    progressBarControl1.EditValue = 0;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ioe.Message);
                    Console.ForegroundColor = ConsoleColor.White;
                }
                catch (WebException wex)
                {
                    progressBarControl1.Properties.Maximum = 0;
                    progressBarControl1.EditValue = 0;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(wex.Message);
                    Console.ForegroundColor = ConsoleColor.White;
                }
                catch (Exception ex)
                {
                    progressBarControl1.Properties.Maximum = 0;
                    progressBarControl1.EditValue = 0;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex.Message);
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }

            DownloadArquivos();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nFinalizado!! Pressione enter para fechar.");
            progressBarControl1.Properties.Maximum = 0;
            progressBarControl1.EditValue = 0;
            Console.Read();
        }

        public void DownloadArquivos()
        {
            foreach (var item in files)
            {
                var file = item.Replace("\r", "").Split(' ').Last();
                var arquivoPath = System.IO.Path.Combine("C:\\Khaddmus Sistemas\\", file);
                AtualizarProgressBar();
                while (!string.IsNullOrEmpty(file))
                {
                    Console.WriteLine("Iniciando download do arquivo: " + file);
                    try
                    {
                        DownloadFile(file, arquivoPath);
                        Console.WriteLine("Download completo em: " + arquivoPath);
                        break;
                    }
                    catch (IOException ioe)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(ioe.Message);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    catch (WebException wex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(wex.Message);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(ex.Message);
                        Console.ForegroundColor = ConsoleColor.White;
                        throw;
                    }
                }

            }
        }

        public void DownloadFile(string filename, string endFilePath)
        {
            try
            {
                using (WebClient ftpClient = new WebClient())
                {
                    ftpClient.Credentials = new System.Net.NetworkCredential(user, pass);
                    string path = System.IO.Path.Combine(ftpPath, filename).Replace("\\", "/");
                    if (System.IO.File.Exists(endFilePath))
                    {
                        System.IO.File.Delete(endFilePath);
                    }
                    ftpClient.DownloadFile(path, endFilePath);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetFileList()
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpPath);
                request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

                // This example assumes the FTP site uses anonymous logon.
                request.Credentials = new NetworkCredential(user, pass);

                using (var response = (FtpWebResponse)request.GetResponse())
                {

                    Stream responseStream = response.GetResponseStream();
                    using (var reader = new StreamReader(responseStream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
            }
        }

        public void AtualizarProgressBar()
        {
            progressBarControl1.PerformStep();
            progressBarControl1.Update();
        }
        private void AtualizarProgressBar(int pos)
        {
            progressBarControl1.Position = pos;
        }
        private void IniciarProgressBar(int Value)
        {
            progressBarControl1.Properties.Step = 1;
            progressBarControl1.Properties.PercentView = true;
            progressBarControl1.Properties.Maximum = Value;
            progressBarControl1.Properties.Minimum = 0;
            progressBarControl1.Properties.PercentView = true;
            progressBarControl1.Properties.TextOrientation = DevExpress.Utils.Drawing.TextOrientation.Horizontal;
            progressBarControl1.Properties.ProgressViewStyle = DevExpress.XtraEditors.Controls.ProgressViewStyle.Solid;
            progressBarControl1.Properties.EndColor = System.Drawing.Color.SteelBlue;
            progressBarControl1.Properties.StartColor = System.Drawing.Color.PowderBlue;
            progressBarControl1.Properties.ShowTitle = true;
        }
        private void AtualizadorWEB_Load(object sender, EventArgs e)
        {

            Process[] procs = Process.GetProcessesByName("One PDV.exe");

            foreach (Process proc in procs)
                proc.Kill();

          
        }

        private void progressBarControl1_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void AtualizadorWEB_Shown(object sender, EventArgs e)
        {
            Atuaizar();
        }
    }


}

