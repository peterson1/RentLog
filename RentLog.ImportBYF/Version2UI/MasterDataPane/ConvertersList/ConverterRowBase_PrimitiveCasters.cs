using CommonTools.Lib11.ExceptionTools;
using CommonTools.Lib11.StringTools;
using System;

namespace RentLog.ImportBYF.Version2UI.MasterDataPane.ConvertersList
{
    public abstract partial class ConverterRowBase<T>
    {
        protected string AsText(dynamic dynamic)
        {
            var txt = ((string)dynamic)?.NullIfBlank();
            return txt?.Trim();
        }


        protected int AsID(dynamic dynamic)
        {
            var str = (string)dynamic;
            if (int.TryParse(str, out int id))
                return id;
            else
                throw Bad.Cast(str, id);
        }


        protected bool AsBool(dynamic dynamic)
        {
            var str = (string)dynamic;
            if (str.IsBlank()) return false;
            str = str.Trim().ToLower();

            switch (str)
            {
                case "1":
                case "y":
                case "yes":
                case "t":
                case "true": return true;

                case "0":
                case "n":
                case "no":
                case "f":
                case "false": return false;

                default: throw Bad.Cast(str, true);
            }
        }


        protected DateTime AsDate(dynamic dynamic)
        {
            var str = (string)dynamic;
            if (DateTime.TryParse(str, out DateTime date))
                return date.Date.ToLocalTime();
            else
                throw Bad.Cast(str, date);
        }
    }
}
