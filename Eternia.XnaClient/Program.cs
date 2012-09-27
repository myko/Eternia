using System;

namespace Eternia.XnaClient
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            try
            {
                using (EterniaXna game = new EterniaXna())
                {
                    game.Run();
                }
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                if (ex.InnerException != null)
                    message += Environment.NewLine + Environment.NewLine + ex.InnerException.Message;

                message += Environment.NewLine + Environment.NewLine + ex.StackTrace;

                System.Windows.Forms.MessageBox.Show(message);
            }
        }
    }
}

