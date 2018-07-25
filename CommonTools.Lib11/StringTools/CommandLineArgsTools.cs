using System.Linq;

namespace CommonTools.Lib11.StringTools
{
    public static class CommandLineArgsTools
    {
        public static string QuotifyCommandLineArgs(this string[] commandLineArgs)
        {
            //var ss = commandLineArgs.ToList();
            //ss[0]  = Enquote(ss[0]);
            //
            //for (int i = 1; i < ss.Count; i++)
            //    ss[i] = EnquoteCmdArg(ss[i]);
            //
            //return string.Join(" ", ss);
            return string.Join(" ",
                commandLineArgs.Skip(1)
                    .Select(_ => EnquoteCmdArg(_)));
        }


        private static string EnquoteCmdArg(string commandLineArg)
        {
            if (!commandLineArg.Contains("="))
                return Enquote(commandLineArg);

            //var ss = commandLineArg.Split('=');
            //return $"{ss[0]}={Enquote(ss[1])}";
            var key = commandLineArg.Before("=");
            var val = commandLineArg.After ("=");
            return $"{key}={Enquote(val)}";
        }


        private static string Enquote(string text)
            => $"\"{text}\"";
    }
}
