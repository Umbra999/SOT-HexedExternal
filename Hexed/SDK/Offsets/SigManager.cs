using Hexed.Core;
using Hexed.Wrappers;

namespace Hexed.SDK.Offsets
{
    internal class SigManager
    {
        public static ulong UWorldAddress = 0;
        public static ulong GNamesAddress = 0;
        public static ulong GObjectsAddress = 0;

        public static void FindPatterns()
        {
            FindUWorld();
            FindGNames();
            FindGObjects();

            //FindProcessEvent();
        }

        private static void FindUWorld()
        {
            ulong UWorldPattern = (ulong)GameManager.Memory.FindPattern(new byte[] { 0x48, 0x8B, 0x05, 0x00, 0x00, 0x00, 0x00, 0x48, 0x8B, 0x88, 0x00, 0x00, 0x00, 0x00, 0x48, 0x85, 0xC9, 0x74, 0x06, 0x48, 0x8B, 0x49, 0x70 }, "xxx????xxx????xxxxxxxxx");
            uint offset = GameManager.Memory.Read<uint>(UWorldPattern + 3);

            UWorldAddress = UWorldPattern + 7 + offset;
        }

        private static void FindGNames()
        {
            ulong GNamesPattern = (ulong)GameManager.Memory.FindPattern(new byte[] { 0x48, 0x89, 0x3D, 0x00, 0x00, 0x00, 0x00, 0x41, 0x8B, 0x75, 0x00 }, "xxx????xxxx");
            uint offset = GameManager.Memory.Read<uint>(GNamesPattern + 3);

            GNamesAddress = GNamesPattern + offset + 7;
        }

        private static void FindGObjects()
        {
            ulong GObjectsPattern = (ulong)GameManager.Memory.FindPattern(new byte[] { 0x48, 0x8B, 0x0D, 0x00, 0x00, 0x00, 0x00, 0x81, 0x4C, 0xD1, 0x00, 0x00, 0x00, 0x00, 0x00, 0x48, 0x8D, 0x4D, 0xD8 }, "xxx????xxx?????xxxx");
            uint offset = GameManager.Memory.Read<uint>(GObjectsPattern + 2);

            GObjectsAddress = GObjectsPattern + 6 + offset;
        }

        //private static void FindProcessEvent()
        //{
        //    UWorld GWorld = new(GameManager.Memory.Read<ulong>(UWorldAddress));
        //    IntPtr vTable = GWorld.VTableAddress;
        //    for (int i = 50; i < 256; i++)
        //    {
        //        IntPtr currentOffset = GameManager.Memory.Read<IntPtr>((ulong)(vTable + i * 8));
        //        var sig = GameManager.Memory.FindPattern("40 55 56 57 41 54 41 55 41 56 41 57", currentOffset, 32);
        //        if (sig != 0)
        //        {
        //            ProcessEventIndex = i;
        //            break;
        //        }
        //    }
        //}
    }
}
