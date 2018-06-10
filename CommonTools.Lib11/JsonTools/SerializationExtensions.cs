using CommonTools.Lib11.ExceptionTools;
using CommonTools.Lib11.StringTools;
using System.IO;

namespace CommonTools.Lib11.JsonTools
{
    public static class SerializationExtensions
    {
        public static T ReadJson<T>(this string json)
        {
            try
            {
                return json.ReadJson_Unsafe<T>();
            }
            catch (FileNotFoundException ex)
            {
                throw Missing.File(ex.FileName, "Json Serializer library");
            }
        }


        public static string ToJson(this object obj, bool indented = false)
        {
            try
            {
                return obj.ToJson_Unsafe(indented);
            }
            catch (FileNotFoundException ex)
            {
                throw Missing.File(ex.FileName, "Json Serializer library");
            }
        }

        public static string SHA1OfJson(this object obj)
            => obj.ToJson().SHA1ForUTF8();
    }
}
