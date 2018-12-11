using CommonTools.Lib11.DateTimeTools;
using CommonTools.Lib45.BusyIndicators;
using CommonTools.Lib45.ExcelTools;
using CommonTools.Lib45.InputDialogs;
using RentLog.DomainLib11.Models;
using RentLog.DomainLib11.Reporters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.DomainLib45.Reporters
{
    public class ColxnSummaryExcelWriter
    {
        const int T1_ROW = 2;
        const int T1_COL = 2;
        const int T2_ROW = 3;
        const int H1_ROW = 5;
        const int H2_ROW = 6;
        const int N1_ROW = 7;
        const double GAP1 = 3;

        private ColxnSummaryReport _src;
        private CellWriter1ByRow   _xl;


        public ColxnSummaryExcelWriter(ColxnSummaryReport colxnSummaryReport)
        {
            _xl  = new CellWriter1ByRow("Collections_Summary");
            _src = colxnSummaryReport;
            _src.RemoveZeroSections();

            WriteTitles();
            writeDatesColumn();
            WriteBill("Rent"     , _ => _.Rent    );
            WriteBill("Rights"   , _ => _.Rights  );
            WriteBill("Electric" , _ => _.Electric);
            WriteBill("Water"    , _ => _.Water   );
            WriteBill("Ambulants", _ => _.Ambulant);
            WriteColumnsForOthers();
            WriteTotalColumn("Total Collections", _ => _.CollectionsSum, _ => _.TotalCollections);
            WriteTotalColumn("Total Deposits"   , _ => _.DepositsSum   , _ => _.TotalDeposits   );

            SetRowHeights();
        }


        private void WriteTitles()
        {
            const int COL_SPAN = 8;

            _xl.MoveTo(T1_ROW, T1_COL);
            _xl.WriteMergedH1(_src.Title, 2, COL_SPAN);

            _xl.MoveTo(T1_ROW, T1_COL + COL_SPAN);
            _xl.WriteMergedH2(_src.BranchName, 1, COL_SPAN);

            _xl.MoveTo(T2_ROW, T1_COL + COL_SPAN);
            _xl.WriteMergedH2(_src.DateRangeText, 1, COL_SPAN);
        }


        private void writeDatesColumn()
        {
            _xl.MoveTo(H2_ROW, 1);
            _xl.WriteH2("date");

            _xl.MoveTo(N1_ROW, null);
            foreach (var row in _src)
                _xl.WriteDate(row.Date);

            _xl.WriteH2("Sections Total");

            _xl.CurrentCol.AutoFit(15);
            _xl.MoveToNextCol();

            _xl.CurrentCol.Width = GAP1;
            _xl.MoveToNextCol();
        }


        private void WriteBill(string header1, Func<CollectionAmounts, decimal?> getter)
        {
            _xl.MoveTo(H1_ROW, null);
            _xl.WriteMergedH1(header1, 1, _src.Sections.Count);

            foreach (var sec in _src.Sections)
                WriteSectionAmounts(getter, sec.Key, sec.Value);

            _xl.CurrentCol.Width = GAP1;
            _xl.MoveToNextCol();
        }


        private void WriteSectionAmounts(Func<CollectionAmounts, decimal?> getter, int secID, string secName)
        {
            _xl.MoveTo(H2_ROW, null);
            _xl.WriteH2(secName);

            _xl.MoveTo(N1_ROW, null);
            foreach (var row in _src)
            {
                var colxn = row.SingleOrDefault(_ => _.Section.Id == secID);
                if (colxn == null)
                {
                    _xl.WriteNumber(null);
                    continue;
                }
                var val = getter(colxn);
                _xl.WriteNumber(val);
            }

            var total = getter(_src.SectionTotals[secID]);
            _xl.WriteNumber(total).SetBold();

            _xl.CurrentCol.AutoFit();
            _xl.MoveToNextCol();
        }


        private void WriteColumnsForOthers()
        {
            _xl.MoveTo(H1_ROW, null);
            _xl.WriteMergedH1("Other Collections", 1, _src.OtherTotals.Count);

            foreach (var gl in _src.OtherTotals)
                WriteOtherAmounts(gl);

            _xl.CurrentCol.Width = GAP1;
            _xl.MoveToNextCol();
        }


        private void WriteOtherAmounts(KeyValuePair<int, decimal> gl)
        {
            _xl.MoveTo(H2_ROW, null);
            _xl.WriteH2(_src.GLAccounts[gl.Key]);

            _xl.MoveTo(N1_ROW, null);
            foreach (var row in _src)
            {
                if (row.Others.TryGetValue(gl.Key, out decimal value))
                    _xl.WriteNumber(value);
                else
                    _xl.MoveToNextRow();
            }

            _xl.WriteNumber(_src.OtherTotals[gl.Key]).SetBold();

            _xl.CurrentCol.AutoFit();
            _xl.MoveToNextCol();
        }


        private void WriteTotalColumn(string header, 
            Func<DailyColxnsReport, decimal> sumGetter,
            Func<ColxnSummaryReport, decimal> totalGetter)
        {
            _xl.MoveTo(H1_ROW, null);
            _xl.WriteMergedH1(header, 2, 1);

            _xl.MoveTo(N1_ROW, null);
            foreach (var row in _src)
                _xl.WriteNumber(sumGetter(row));

            _xl.WriteNumber(totalGetter(_src)).SetBold();

            _xl.CurrentCol.AutoFit(13);
            _xl.MoveToNextCol();

            _xl.CurrentCol.Width = GAP1;
            _xl.MoveToNextCol();
        }


        private void SetRowHeights()
        {
            _xl.DefaultRowHeight   = 20;
            var last = N1_ROW + _src.Count;

            _xl.Row(1)     .Height = 10;
            _xl.Row(T1_ROW).Height = 25;
            _xl.Row(T2_ROW).Height = 25;
            _xl.Row(T2_ROW + 1).Height = 10;

            _xl.Row(H1_ROW).Height = 30;
            _xl.Row(H2_ROW).Height = 27;
            _xl.Row(last).Height   = 27;
        }


        public void LaunchExcel() => _xl.LaunchTempSave();


        public static void Launch(AppArguments args)
        {
            var now = DateTime.Now.Date;
            var bgn = new DateTime(now.Year, now.Month, 1);
            var end = args.Collections.LastPostedDate();

            if (!PopUpInput.TryGetDateRange
                ("Dates covered by Collection Summary Report", 
                out (DateTime Start, DateTime End) rng, bgn, end)) return;

            var splash = new LoadingSplash();

            new ColxnSummaryReport(rng.Start, rng.End, args).ToExcel();

            splash.Close();
        }
    }


    public static class ColxnSummaryExcelExtensions
    {
        public static void ToExcel(this ColxnSummaryReport colxnSummaryReport)
        {
            var writr = new ColxnSummaryExcelWriter(colxnSummaryReport);
            writr.LaunchExcel();
        }
    }
}
