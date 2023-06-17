using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Translumo.Infrastructure.Constants
{
    public static class Global
    {
        public static string AppPath;

        public static string PythonPath;

        public static string PipPath;

        public static string ModelsPath;

        static Global()
        {
#if DEBUG
            AppPath = System.AppDomain.CurrentDomain.BaseDirectory;
#else
            AppPath = Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
#endif
            PythonPath = Path.Combine(AppPath, "Python");
            PipPath = Path.Combine(PythonPath, "Scripts/pip.exe");
            ModelsPath = Path.Combine(AppPath, "models");
        }


        public static string GetVersion()
        {
            return FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location).ProductVersion;
        }
    }
}
