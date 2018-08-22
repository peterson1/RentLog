using System;
using System.Linq;
using OfficeOpenXml.Style;
using RentLog.DomainLib11.Reporters;
using RentLog.DomainLib11.ReportRows;

namespace RentLog.DomainLib45.Reporters
{
    public partial class GLRecapExcelWriter
    {
        private void WriteCategoryDetailsSheet(GLRecapCategory cat)
        {
            var subTitle = cat.AccountType.ToString() + " Details";
            _xl.AddWorksheet(subTitle);
            WriteSheetHeader(subTitle);

            var startCol = 2;
            var acctSpan = 4;
            var dateSpan = 1;
            var txnSpan  = 8;
            var rowCount = cat.Sum(_ => _.Count);
            var nextCol  = startCol;

            WriteGLAccountsColumn(cat, ref nextCol, acctSpan);

            WriteDatesColumn(cat, ref nextCol, dateSpan);

            WriteTransctionDescCol(cat, txnSpan, ref nextCol);
            WriteTotalsRowLabel(nextCol - txnSpan, txnSpan);

            WriteDetailAmounts(cat, ref nextCol, "Debit" , _ => _.AsDebit);
            SummarizeCurrentColumn(rowCount);

            WriteDetailAmounts(cat, ref nextCol, "Credit", _ => _.AsCredit);
            SummarizeCurrentColumn(rowCount);

            SetRowHeights(rowCount);
        }


        private void WriteDatesColumn(GLRecapCategory cat, ref int colNum, int colSpan)
        {
            WriteColHeader("date", colNum, 15);

            foreach (var acctGrp in cat)
            {
                foreach (var alloc in acctGrp)
                    _xl.WriteDate(alloc.Request.RequestDate);
            }
            colNum += colSpan;
        }


        private void WriteTransctionDescCol(GLRecapCategory cat, int colSpan, ref int colNum)
        {
            WriteMergedColHeader("description", colNum, colSpan);

            foreach (var acctGrp in cat)
            {
                foreach (var alloc in acctGrp)
                {
                    var rng = _xl.WriteMergedText(alloc.Request.Purpose, 1,
                                colSpan, ExcelHorizontalAlignment.Left);
                    rng.Style.Indent = 1;
                }
            }
            colNum += colSpan;
        }


        private void WriteGLAccountsColumn(GLRecapCategory cat, ref int colNum, int colSpan)
        {
            WriteMergedColHeader("GL Account", colNum, colSpan);

            foreach (var acctGrp in cat)
            {
                foreach (var alloc in acctGrp)
                {
                    var rng = _xl.WriteMergedText(acctGrp.Account.Name, 1,
                                colSpan, ExcelHorizontalAlignment.Left);
                    rng.Style.Indent = 1;
                }
            }
            colNum += colSpan;
        }


        private void WriteDetailAmounts(GLRecapCategory cat, ref int colNum, string colLabel, Func<GLRecapAllocation, decimal?> getter)
        {
            WriteColHeader(colLabel, colNum, 15);

            foreach (var acctGrp in cat)
            {
                foreach (var alloc in acctGrp)
                    _xl.WriteNumber(getter(alloc));
            }
            colNum += 1;

        }
    }
}
