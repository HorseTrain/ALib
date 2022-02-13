using System;

namespace DCpu.Common
{
    static class EnumUtils
    {
        public static int GetCount(Type enumType)
        {
            return Enum.GetNames(enumType).Length;
        }
    }
}
