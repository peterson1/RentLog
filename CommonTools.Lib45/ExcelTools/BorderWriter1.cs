using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTools.Lib45.ExcelTools
{
    public class BorderWriter1
    {
        private ExcelWorksheet _ws;

        public BorderWriter1(ExcelWorksheet worksheet)
        {
            _ws = worksheet;
        }


        public ExcelBorderStyle this[int startRow, int startCol, int endRow, int endCol]
        {
            set
            {
                _ws.Cells[startRow, startCol, endRow, endCol].Style.Border.BorderAround(value);
            }
        }
    }
}
