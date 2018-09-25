using CommonTools.Lib11.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentLog.ImportBYF.Converters
{
    public abstract class ConverterBase <TBYF, TDTO>
        where TDTO : DocumentDTOBase
    {
        public abstract TDTO Convert(TBYF byfRecord);
    }
}
