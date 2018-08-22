using OfficeOpenXml.Style;
using RentLog.DomainLib11.Reporters;
using RentLog.DomainLib11.ReportRows;
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
            var headr = $"{cat.AccountType.ToString()} accounts";
            WriteMergedColHeader(headr, colNum, colSpan);

            foreach (var row in cat)
            {
                var rng = _xl.WriteMergedText(row.Account.Name, 1,
                            colSpan, ExcelHorizontalAlignment.Left);
                rng.Style.Indent = 1;
            }
        }


        private void WriteCatSmryAmounts(GLRecapCategory cat, int colNum, string colLabel, Func<GLRecapAccountGroup, decimal?> getter)
        {
            WriteColHeader(colLabel, colNum, 15);

            foreach (var row in cat)
                _xl.WriteNumber(getter(row));
        }
    }
}
