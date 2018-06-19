namespace RentLog.Tests.SampleDBs
{
    internal class SampleDir
    {
        internal const string JUN17_BALANCED = "2018-06-17 F_GRY Balanced";

        internal static SampleArgs Lease197       () => GetArgsFor("2018-05-12 Lease 197 - DRY 008");
        internal static SampleArgs May19_GRY      () => GetArgsFor("2018-05-19 F_GRY");
        internal static SampleArgs Jun16_GRY      () => GetArgsFor("2018-06-16 F_GRY");
        internal static SampleArgs Jun17_Balanced () => GetArgsFor(JUN17_BALANCED);


        private static SampleArgs GetArgsFor(string dirName)
        {
            SampleArgs.DirName = dirName;
            return new SampleArgs();
        }
    }
}
