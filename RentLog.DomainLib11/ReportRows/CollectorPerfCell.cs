using CommonTools.Lib11.MathTools;
using RentLog.DomainLib11.Models;
using System;

namespace RentLog.DomainLib11.ReportRows
{
    public class CollectorPerfCell
    {
        public CollectorPerfCell(decimal actual, decimal target, BillCode billCode)
        {
            Actual   = actual;
            Target   = target.ZeroIfNegative();
            NoExcess = GetNoExcess(billCode);
            Overage  = GetOverage (billCode);
            Shortage = GetShortage(billCode);
        }


        public decimal   Target    { get; }
        public decimal   Actual    { get; }
        public decimal   NoExcess  { get; }
        public decimal   Overage   { get; }
        public decimal   Shortage  { get; }
        public bool  IsOver    => Overage  != 0;
        public bool  IsShort   => Shortage != 0;


        private decimal GetNoExcess(BillCode billCode)
        {
            switch (billCode)
            {
                case BillCode.Rent:
                    return Math.Max(0, Math.Min(Target, Actual));
                default:
                    return Actual;
            }
        }


        private decimal GetOverage(BillCode billCode)
        {
            switch (billCode)
            {
                case BillCode.Rent:
                    return Actual > Target ? Actual - Target : 0;
                default:
                    return 0;
            }
        }


        private decimal GetShortage(BillCode billCode)
        {
            switch (billCode)
            {
                case BillCode.Rent:
                case BillCode.Rights:
                    return Target > Actual ? Target - Actual : 0;
                default:
                    return 0;
            }
        }
    }
}
