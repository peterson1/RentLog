namespace RentLog.Tests.SampleDBs
{
    internal class SampleDir
    {
        internal const string JUN17_BALANCED = "2018-06-17 F_GRY Balanced";
        internal const string JUN29_F_MEY    = "2018-06-29 F_MEY";
        internal const string JUL03_F_GRY    = "2018-07-03 F_GRY";
        internal const string JUL09_F_GRY    = "2018-07-09 F_GRY";
        internal const string JUL09_GRY_OPEN = "2018-07-09 F_GRY_unclosed";
        internal const string JUL13_F_MEY    = "2018-07-13 F_MEY";
        internal const string JUL15_F_MEY    = "2018-07-15 F_MEY";

        internal static SampleArgs Lease197      () => GetArgsFor("2018-05-12 Lease 197 - DRY 008", "Supervisor");
        internal static SampleArgs May19_GRY     () => GetArgsFor("2018-05-19 F_GRY", "Supervisor");
        internal static SampleArgs Jun16_GRY     () => GetArgsFor("2018-06-16 F_GRY", "Supervisor");
        internal static SampleArgs Jun17_Balanced() => GetArgsFor(JUN17_BALANCED, "Supervisor");
        internal static SampleArgs Jul3_GRY      () => GetArgsFor(JUL03_F_GRY, "Supervisor");
        internal static SampleArgs Jul9_GRY      () => GetArgsFor(JUL09_F_GRY, "Supervisor");
        internal static SampleArgs Jul13_MEY     () => GetArgsFor(JUL13_F_MEY, "Supervisor");
        internal static SampleArgs Jul15_MEY     () => GetArgsFor(JUL15_F_MEY, "Supervisor");
        internal static SampleArgs Jul9_GRY_Open () => GetArgsFor(JUL09_GRY_OPEN, "Supervisor");


        private static SampleArgs GetArgsFor(string dirName, string roles)
        {
            SampleArgs.DirName = dirName;
            return new SampleArgs(roles);
        }
    }
}
