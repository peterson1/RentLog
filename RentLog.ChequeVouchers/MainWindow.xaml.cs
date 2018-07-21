using CommonTools.Lib45.UIExtensions;
using RentLog.DomainLib11.DTOs;
using System.Windows;

namespace RentLog.ChequeVouchers
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += (a, b) =>
            {
                dcdrTab.rows.tbl.dg.EnableOpenCurrent<PassbookRowDTO>();
                dcdrTab.rows.tbl.dg.ConfirmToDelete<PassbookRowDTO>(_ => _.Subject);
                dcdrTab.rows.tbl.dg.ScrollToEndOnReplaced<PassbookRowDTO>();

                jourTab.rows.tbl.dg.EnableOpenCurrent<JournalVoucherDTO>();
                jourTab.rows.tbl.dg.ConfirmToDelete<JournalVoucherDTO>(_ => _.Description);
                jourTab.rows.tbl.dg.ScrollToEndOnReplaced<JournalVoucherDTO>();

                VM.DcdrReport.PrintTrigger.TargetDatagrid = dcdrTab.rows.tbl.dg;
            };
        }


        private MainWindowVM VM => DataContext as MainWindowVM;
    }
}
