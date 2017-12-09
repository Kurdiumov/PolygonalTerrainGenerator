using System;

namespace Engine
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (var game = new App())
                try
                {
                    game.Run();
                    
                }
                catch(Exception e)
                {
                    Logger.Log.Error(e.ToString());
                }
        }
    }
#endif
}
