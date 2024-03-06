using Hexed.Wrappers;
using System.Runtime.InteropServices;

namespace Hexed.Memory
{
    internal static class TypeCache<T>
    {
        public static readonly int Size;
        public static readonly Type Type;
        public static readonly TypeCode TypeCode;

        static TypeCache()
        {
            Type = typeof(T);

            if (Type == typeof(IntPtr)) Size = IntPtr.Size;
            else if (Type.IsEnum)
            {
                Type = Type.GetEnumUnderlyingType();
                Size = Marshal.SizeOf(Type);
            }
            else if (Type == typeof(bool)) Size = 1;
            else Size = Marshal.SizeOf<T>();

            TypeCode = Type.GetTypeCode(Type);
        }
    }
}
