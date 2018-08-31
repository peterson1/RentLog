using CommonTools.Lib45.UIExtensions;
using RentLog.DomainLib11.DTOs;
using System.Windows;

namespace RentLog.ChequeVouchers.AllChequeVouchers
{
    /// <summary>
    /// Interaction logic for AllChequeVouchersWindow.xaml
    /// </summary>
    public partial class AllChequeVouchersWindow : Window
    {
        public AllChequeVouchersWindow()
        {
            InitializeComponent();
            Loaded += (a, b) =>
            {
                tbl.dg.EnableOpenCurrent<FundRequestDTO>();
            };
        }
    }
}
