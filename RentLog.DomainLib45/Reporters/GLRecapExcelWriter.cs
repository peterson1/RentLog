﻿using System;
using CommonTools.Lib45.ExcelTools;
using CommonTools.Lib45.InputDialogs;
using OfficeOpenXml.Style;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.Reporters;

namespace RentLog.DomainLib45.Reporters
{
    public partial class GLRecapExcelWriter
    {
        const int T1_ROW  = 2;
        const int T1_COL  = 2;
        const int T2_ROW  = 3;
        const int H1_ROW  = 4;
        const int H2_ROW  = 5;
        const int N1_ROW  = 7;
        const double GAP1 = 3;

        private GLRecapReport    _src;
        private CellWriter1ByRow _xl;


        public GLRecapExcelWriter(GLRecapReport glRecapReport)
        {
            WriteReportSummarySheet(glRecapReport);

            foreach (var category in _src)
            {
                WriteCategorySummarySheet(category);
                WriteCategoryDetailsSheet(category);
            }
        }


        private void WriteSheetHeader(string subTitle)
        {
            const int COL_SPAN = 8;

            _xl.MoveTo(T1_ROW, T1_COL);
            _xl.WriteMergedH1($"{_src.Title} :  {subTitle}", 2, COL_SPAN);

            _xl.MoveTo(T1_ROW, T1_COL + COL_SPAN);
            _xl.WriteMergedH2(_src.BranchName, 1, COL_SPAN);

            _xl.MoveTo(T2_ROW, T1_COL + COL_SPAN);
            _xl.WriteMergedH2(_src.DateRangeText, 1, COL_SPAN);

            _xl.MoveTo(null, 1);
            _xl.CurrentCol.Width = GAP1;
        }


        private void SummarizeCurrentColumn(int rowCount)
        {
            var rng = _xl.WriteSumFormulaForColumn(N1_ROW, N1_ROW + rowCount - 1);
            rng.SetBold();
        }

        private void WriteTotalsRowLabel(int colNum, int colSpan)
        {
            var rng = _xl.WriteMergedText("total  ", 1, colSpan, ExcelHorizontalAlignment.Right);
            rng.Style.Indent = 1;
            rng.SetBold();
            rng.SetItalic();
        }


        private void SetRowHeights(int rowCount)
        {
            _xl.DefaultRowHeight = 20;
            var last = N1_ROW + rowCount;

            _xl.Row(1).Height          = 10;
            _xl.Row(T1_ROW).Height     = 25;
            _xl.Row(T2_ROW).Height     = 25;
            _xl.Row(T2_ROW + 1).Height = 10;

            _xl.Row(H1_ROW).Height     = 30;
            _xl.Row(H2_ROW).Height     = 27;
            _xl.Row(last).Height       = 27;
        }


        private void WriteColHeader(string label, int colNum, double colWidth)
        {
            _xl.MoveTo(H2_ROW, colNum);
            _xl.CurrentCol.Width = colWidth;
            _xl.WriteH2(label);

            _xl.MoveTo(N1_ROW, null);
        }


        private void WriteMergedColHeader(string label, int colNum, int colSpan)
        {
            _xl.MoveTo(H2_ROW, colNum);
            _xl.WriteMergedH2(label, 1, colSpan);
            _xl.MoveTo(N1_ROW, null);
        }


        public void LaunchExcel() => _xl.LaunchTempSave();


        public static void Launch(ITenantDBsDir dir)
        {
            var now = DateTime.Now.Date;
            var bgn = new DateTime(now.Year, now.Month, 1);
            var end = dir.Collections.LastPostedDate();

            if (!PopUpInput.TryGetDateRange("Dates covered by GL Recap Report", out (DateTime Start, DateTime End) rng, bgn, end)) return;

            new GLRecapReport(rng.Start, rng.End, dir).ToExcel();
        }
    }


    public static class GLRecapExcelWriterExtensions
    {
        public static void ToExcel(this GLRecapReport glRecapReport) 
            => new GLRecapExcelWriter(glRecapReport).LaunchExcel();
    }
}
