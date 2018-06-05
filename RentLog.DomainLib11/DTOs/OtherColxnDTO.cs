using CommonTools.Lib11.DTOs;
using CommonTools.Lib11.ExceptionTools;
using CommonTools.Lib11.ReflectionTools;
using System;

namespace RentLog.DomainLib11.DTOs
{
    public enum OtherCode
    {
        CR_Fee         = 1,
        Parking        = 2,
        Processing_Fee = 3,
        Other          = 4
    }

    public class OtherColxnDTO : DocumentDTOBase
    {
        public string         DocumentRef  { get; set; }
        public CollectorDTO   Collector    { get; set; }
        public decimal        Amount       { get; set; }
                              
        public OtherCode      OtherCode    { get; set; }
        public GLAccountDTO   GLAccount    { get; set; }
    }


    public static class OtherColxnDTOExtensions
    {
        public static  int GetGLId(this OtherColxnDTO dto)
        {
            if (dto.GLAccount != null) return dto.GLAccount.Id;
            switch (dto.OtherCode)
            {
                case OtherCode.CR_Fee        : return 56;
                case OtherCode.Parking       : return 55;
                case OtherCode.Processing_Fee: return 57;
                case OtherCode.Other         : return 60;
                default:
                    throw Bad.Arg("OtherColxnDTO.OtherCode", dto.OtherCode);
            }
        }
    }
}
