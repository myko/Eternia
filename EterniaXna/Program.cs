using System;

namespace EterniaXna
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (EterniaXna game = new EterniaXna())
            {
                game.Run();
            }
        }
    }
}

