using CommonTools.Lib11.DataStructures;
using CommonTools.Lib45.PrintTools.PrintPreviewer2;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.MarketStateRepos;
using RentLog.DomainLib11.PassbookRepos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.ChequeVouchers.VoucherReqsTab.VouchersListPrint
{
    public class VouchersListPrintTrigger
    {
        public VouchersListPrintTrigger(MainWindowVM mainWindowVM)
        {
            Main = mainWindowVM;
            Main.PrintClicked += (a, b) =>
            {
                if (mainWindowVM.SelectedTab == MainTabs.CheckVouchers)
                {
                    LatestRow = GetLatestRow();
                    Outstandings.SetItems(GetOutstandingChecks());
                    Outstandings.SetSummary(GetOutstandingsSummary());
                    ForPreparations.SetSummary(GetForPreparationsSummary());
                    AskToPrintVouchersList(mainWindowVM);
                }
            };
        }


        public MainWindowVM             Main            { get; }
        public PassbookRowDTO           LatestRow       { get; private set; }
        public UIList<ChequeVoucherDTO> Outstandings    { get; } = new UIList<ChequeVoucherDTO>();
        public UIList<FundRequestDTO>   ForPreparations => Tab?.FundRequests.ItemsList;
        public ITenantDBsDir            AppArgs         => Main?.AppArgs;
        public BankAccountDTO           BankAcct        => AppArgs?.CurrentBankAcct;
        public MarketStateDbBase            MarketState     => AppArgs?.MarketState;
        public VoucherReqsTabVM         Tab             => Main?.VoucherReqs;
        public decimal?                 BankBalance     => LatestRow?.RunningBalance;
        public DateTime?                AsOfDate        => LatestRow?.TransactionDate;
        public decimal                  MaintainingBal  => BankAcct?.MaintainingBalance ?? 50000;
        public decimal? Unallocated => BankBalance - (MaintainingBal + (decimal)Outstandings.SummaryAmount + (decimal)ForPreparations.SummaryAmount);


        private PassbookRowDTO GetLatestRow()
        {
            var start = Main.DateRange.Start;
            var end   = Main.DateRange.End;
            var repo  = AppArgs.Passbooks.GetRepo(BankAcct.Id);
            var rows  = repo.RowsFor(start, end);
            return rows.LastOrDefault();
        }


        private IEnumerable<ChequeVoucherDTO> GetOutstandingChecks()
                    => Tab?.IssuedCheques?.ItemsList?
               .Concat(Tab?.PreparedCheques?.ItemsList);


        private ChequeVoucherDTO GetOutstandingsSummary()
        {
            var total = Outstandings.Sum(_ => _.Request.Amount);
            Outstandings.SummaryAmount = (double)total;
            return new ChequeVoucherDTO
            {
                Request = new FundRequestDTO { Amount = total }
            };
        }

        private FundRequestDTO GetForPreparationsSummary()
        {
            var total = ForPreparations.Sum(_ => _.Amount);
            ForPreparations.SummaryAmount = (double)total;
            return new FundRequestDTO
            {
                Amount = total
            };
        }

        private void AskToPrintVouchersList(MainWindowVM main)
        {
            PrintPreviewer2.Preview(this)
                .On<VouchersListPrintLayout1>();
        }
    }
}
