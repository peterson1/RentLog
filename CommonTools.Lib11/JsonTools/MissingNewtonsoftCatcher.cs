using Newtonsoft.Json;
using System;

namespace CommonTools.Lib11.JsonTools
{
    internal static class MissingNewtonsoftCatcher
    {
        internal static T ReadJson_Unsafe<T>(this string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception ex)
            {
                throw new InvalidCastException(ex.Message);
            }
        }


        internal static string ToJson_Unsafe(this object obj, bool indented = false)
        {
            return JsonConvert.SerializeObject(obj,
                indented ? Formatting.Indented : Formatting.None);
        }
    }
}
