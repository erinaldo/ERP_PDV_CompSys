using One.Loja.PDV_Manager;
using PDV.VIEW.FRENTECAIXA.App_Context;
using System;
using System.Windows.Forms;

namespace PDV.VIEW.FRENTECAIXA
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
       {
            try
            {
               

                Contexto.SetConfiguracaoPrimaria();
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Main());

               
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message, "Erro ao inicializar o sistema!",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            

        
        }
    }
}
