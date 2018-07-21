using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Models;
using RentLog.DomainLib11.ReportRows;
using System;
using System.Collections.Generic;

namespace RentLog.DomainLib11.BalanceRepos
{
    public interface IBalanceDB
    {
        IDailyBillsRepo        GetRepo          (int leaseID);
        IDailyBillsRepo        GetRepo          (LeaseDTO lse);
        DailyBillDTO           GetBill          (LeaseDTO lse, DateTime date);
        BillAmounts            TotalOverdues    (DateTime? date = null);
        List<LeaseBalanceRow>  GetOverdueLeases (out BillAmounts totals, DateTime? asOfDate = null);
        List<LeaseBalanceRow>  GetOverdueLeases (out BillAmounts totals, SectionDTO sectionFilter, DateTime? asOfDate = null);
    }
}
