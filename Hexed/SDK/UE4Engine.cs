using Hexed.Core;
using Hexed.SDK.Engine;
using Hexed.SDK.Offsets;
using System.Text;

namespace Hexed.SDK
{
    internal class UE4Engine
    {
        public static string GetName(int i)
        {
            ulong GNames = GameManager.Memory.Read<ulong>(SigManager.GNamesAddress);
            ulong fNamePtr = GameManager.Memory.Read<ulong>(GNames + (ulong)i / 0x4000 * 8);
            ulong fName2 = GameManager.Memory.Read<ulong>(fNamePtr + (8 * ((ulong)i % 0x4000)));
            string fName3 = GameManager.Memory.Read<string>(fName2 + 0x10);
            if (fName3.Contains('/')) return fName3.Substring(fName3.LastIndexOf("/") + 1);

            return fName3;
        }

        public static string GetFullName(ulong entityAddr)
        {
            ulong classPtr = GameManager.Memory.Read<ulong>(entityAddr + 0x10);
            int classNameIndex = GameManager.Memory.Read<int>(classPtr + 0x18);
            string name = GetName(classNameIndex);
            ulong outerEntityAddr = entityAddr;
            string parentName = "";

            while (true)
            {
                ulong tempOuterEntityAddr = GameManager.Memory.Read<ulong>(outerEntityAddr + 0x20);
                if (tempOuterEntityAddr == outerEntityAddr || tempOuterEntityAddr == 0) break;
                outerEntityAddr = tempOuterEntityAddr;
                var outerNameIndex = GameManager.Memory.Read<int>(outerEntityAddr + 0x18);
                string tempName = GetName(outerNameIndex);

                if (tempName == "" || tempName == "None") break;

                parentName = tempName + "." + parentName;
            }

            name += " " + parentName;
            var nameIndex = GameManager.Memory.Read<int>(entityAddr + 0x18);
            name += GetName(nameIndex);

            return name;
        }

        public static bool IsA(ulong entityClassAddr, ulong targetClassAddr)
        {
            if (entityClassAddr == targetClassAddr) return true;

            while (true)
            {
                ulong tempEntityClassAddr = GameManager.Memory.Read<ulong>(entityClassAddr + 0x40);
                if (entityClassAddr == tempEntityClassAddr || tempEntityClassAddr == 0) break;

                if (tempEntityClassAddr == targetClassAddr) return true;
            }
 
            return false;
        }

        public static ulong FindClass(string className)
        {
            ulong GObjects = GameManager.Memory.Read<ulong>(SigManager.GObjectsAddress);
            ulong masterEntityList = GameManager.Memory.Read<ulong>((ulong)GameManager.Memory.Process.MainModule.BaseAddress + GObjects);
            ulong num = GameManager.Memory.Read<ulong>((ulong)GameManager.Memory.Process.MainModule.BaseAddress + GObjects + 0x14);
            ulong entityChunk = 0u;
            ulong entityList = GameManager.Memory.Read<ulong>(masterEntityList);
            for (ulong i = 0u; i < num; i++)
            {
                if ((i / 0x10000) != entityChunk)
                {
                    entityChunk = (uint)(i / 0x10000);
                    entityList = GameManager.Memory.Read<ulong>(masterEntityList + 8 * entityChunk);
                }
                ulong entityAddr = GameManager.Memory.Read<ulong>(entityList + 24 * (i % 0x10000));
                if (entityAddr == 0) continue;
                string name = GetFullName(entityAddr);
                if (name == className) return entityAddr;
            }

            return 0;
        }

        public static int FieldIsClass(string className, string fieldName)
        {
            ulong classAddr = FindClass(className);
            ulong fieldAddr = GetFieldAddr(classAddr, classAddr, fieldName);
            var offset = GetFieldOffset(fieldAddr);
            return offset;
        }

        public static ulong GetFieldAddr(ulong origClassAddr, ulong classAddr, string fieldName)
        {
            ulong field = classAddr + 0x10; // 0x10 on some versions?
            while ((field = GameManager.Memory.Read<ulong>(field + 0x28)) > 0)
            {
                string fName = GetName(GameManager.Memory.Read<int>(field + 0x18));
                if (fName == fieldName)
                {
                    //var offset = GameManager.Memory.ReadProcessMemory<Int32>(field + 0x44);
                    return field;
                }
            }

            ulong parentClass = GameManager.Memory.Read<ulong>(classAddr + 0x30); // 0x30 on some versions

            if (parentClass == classAddr) throw new Exception("parent is classAddr");
            if (parentClass == 0) throw new Exception("parent is null");

            return GetFieldAddr(origClassAddr, parentClass, fieldName);
        }

        public static int GetFieldOffset(ulong fieldAddr)
        {
            int offset = GameManager.Memory.Read<int>(fieldAddr + 0x44);
            return offset;
        }

        public static string GetFieldType(ulong fieldAddr)
        {
            ulong fieldType = GameManager.Memory.Read<ulong>(fieldAddr + 0x10);
            string name = GetName(GameManager.Memory.Read<int>(fieldType + 0x18));
            return name;
        }

        public static string DumpClass(string className)
        {
            ulong classAddr = FindClass(className);
            return DumpClass(classAddr);
        }

        public static string DumpClass(ulong classAddr)
        {
            StringBuilder sb = new();
            string name = GetFullName(classAddr);
            sb.Append(classAddr.ToString("X") + " : " + name);

            ulong pcAddr = classAddr;
            ulong c = 0;
            while ((pcAddr = GameManager.Memory.Read<ulong>(pcAddr + 0x40)) > 0 && c++ < 20)
            {
                string super = GetFullName(pcAddr);
                sb.Append(" : " + super);
            }
            sb.AppendLine();

            pcAddr = classAddr;

            while (true)
            {
                ulong field = pcAddr + 0x40;
                while (true)
                {
                    ulong nextField = GameManager.Memory.Read<ulong>(field + 0x28);
                    if (nextField == field) break;
                    field = nextField;
                    if (field == 0) break;
                    string fieldName = GetFullName(field);
                    ulong f = GameManager.Memory.Read<ulong>(field + 0x70);
                    string fType = GetName(GameManager.Memory.Read<int>(f + 0x18));
                    string fName = GetName(GameManager.Memory.Read<int>(field + 0x18));
                    var offset = GameManager.Memory.Read<int>(field + 0x44);
                    if (fType == "None" && string.IsNullOrEmpty(fName)) break;
                    var fieldObj = new UObject(field);
                    if (fieldObj.ClassName == "Class CoreUObject.Function")
                    {
                        sb.AppendLine("  " + fType + " " + fName + "(" + fieldObj.ClassName + ") : 0x" + offset.ToString("X"));
                        ulong funcParams = field + 0x20u;
                        while ((funcParams = GameManager.Memory.Read<ulong>(funcParams + 0x28u)) > 0)
                        {
                            sb.AppendLine("      " + GetFullName(funcParams));
                        }
                    }
                    else sb.AppendLine("  " + fType + " " + fName + "(" + fieldObj.ClassName + "Field Name " + fieldName + ") : 0x" + offset.ToString("X"));
                }

                pcAddr = GameManager.Memory.Read<ulong>(pcAddr + 0x40);
                if (pcAddr == 0) break;
            }

            return sb.ToString();
        }
    }
}
