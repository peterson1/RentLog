using OfficeOpenXml.Style;
using RentLog.DomainLib11.Reporters;
using System;

namespace RentLog.DomainLib45.Reporters
{
    public partial class GLRecapExcelWriter
    {


        private void WriteCategorySummarySheet(GLRecapCategory cat)
        {
            var subTitle = cat.AccountType.ToString() + " Summary";
            _xl.AddWorksheet(subTitle);

            WriteSheetHeader(subTitle);

            var labelCol = 2;
            var colSpan  = 8;
            WriteCatSmryRowNames(cat, labelCol, colSpan);
            WriteTotalsRowLabel(labelCol, colSpan);

            var nextCol = labelCol + colSpan;
            var rowCount = cat.Count;

            WriteCatSmryAmounts(cat, nextCol, "Debit", _ => _.TotalDebits);
            SummarizeCurrentColumn(rowCount);

            WriteCatSmryAmounts(cat, nextCol + 1, "Credit", _ => _.TotalCredits);
            SummarizeCurrentColumn(rowCount);

            SetRowHeights(rowCount);
        }


        private void WriteCatSmryRowNames(GLRecapCategory cat, int colNum, int colSpan)
        {
            _xl.MoveTo(H2_ROW, colNum);
            _xl.WriteMergedH2("account", 1, colSpan);

            _xl.MoveTo(N1_ROW, null);

            foreach (var row in cat)
            {
                var rng = _xl.WriteMergedText(row.Account.Name, 1,
                            colSpan, ExcelHorizontalAlignment.Left);
                rng.Style.Indent = 1;
            }
        }


        private void WriteCatSmryAmounts(GLRecapCategory cat, int colNum, string colLabel, Func<GLRecapAccountGroup, decimal?> getter)
        {
            _xl.MoveTo(H2_ROW, colNum);
            _xl.CurrentCol.Width = 15;
            _xl.WriteH2(colLabel);

            _xl.MoveTo(N1_ROW, null);

            foreach (var row in cat)
                _xl.WriteNumber(getter(row));
        }
    }
}
