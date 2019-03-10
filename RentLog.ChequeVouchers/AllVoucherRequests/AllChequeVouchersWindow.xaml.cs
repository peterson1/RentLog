using CommonTools.Lib45.ExcelTools;
using CommonTools.Lib45.UIExtensions;
using RentLog.DomainLib11.DTOs;
using System.Windows;

namespace RentLog.ChequeVouchers.AllVoucherRequests
{
    public partial class AllChequeVouchersWindow : Window
    {
        public AllChequeVouchersWindow()
        {
            InitializeComponent();
            Loaded += (a, b) =>
            {
                tbl.dg.EnableOpenCurrent<FundRequestDTO>();
                tBar.btn.Click += (c, d) => tbl.dg.ExportToExcel();
            };
        }
    }
}
