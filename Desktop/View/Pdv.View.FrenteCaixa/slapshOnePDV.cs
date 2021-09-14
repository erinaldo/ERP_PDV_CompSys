using DevExpress.XtraSplashScreen;
using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace One_PDV
{
    public partial class slapshOnePDV : SplashScreen
    {
        public slapshOnePDV()
        {
            InitializeComponent();
            //dblocal = new Modelo.Modelo();
        }

   

        public enum SplashScreenCommand
        {
        }

        
       

      

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.youtube.com/watch?v=-2TV3NDWRNc&t=12s");
        }

        private void slapshOnePDV_Load(object sender, EventArgs e)
        {
            //SetInfoSistema();
            //CheckAtualizacao();
            //CheckVersaoBanco();
        }

     

       

        private void LogImageLogoSistema_Click(object sender, EventArgs e)
        {

        }
    }
}