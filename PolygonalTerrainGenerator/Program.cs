using System;

namespace ProceduralTerrainGenerator
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
            using (var game = new ProceduralTerrainGenerator())
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
