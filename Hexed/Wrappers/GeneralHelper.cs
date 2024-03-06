using Hexed.Core;
using System.Diagnostics;

namespace Hexed.Wrappers
{
    internal class GeneralHelper
    {
        public static Process GetProcessByName(string Name)
        {
            Process[] AllProcesses = Process.GetProcessesByName(Name);
            if (AllProcesses != null && AllProcesses.Length > 0) return AllProcesses[0];

            return null;
        }

        public static bool IsKeyDown(int key)
        {
            return (NativeMethods.GetAsyncKeyState(key) & 0x8000) != 0;
        }

        public static void SendKeyDown(int Key)
        {
             NativeMethods.SendMessage(GameManager.Memory.MainWindow, 0x100, Key, 0x390000);
        }

        public static void SendKeyUp(int Key)
        {
            NativeMethods.SendMessage(GameManager.Memory.MainWindow, 0x101, Key, 0x390000);
        }

        public static bool IsModuleLoaded(Process p, string moduleName)
        {
            var q = from m in p.Modules.OfType<ProcessModule>() select m;

            return q.Any(pm => pm.ModuleName == moduleName && (int)pm.BaseAddress != 0);
        }
    }
}
