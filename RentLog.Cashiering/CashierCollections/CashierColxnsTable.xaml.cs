using CommonTools.Lib45.UIExtensions;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib45.SoaViewers.MainWindow;
using System.Windows.Controls;

namespace RentLog.Cashiering.CashierCollections
{
    public partial class CashierColxnsTable : UserControl
    {
        public CashierColxnsTable()
        {
            InitializeComponent();
            Loaded += (a, b) =>
            {
                dg.ConfirmToDelete<CashierColxnDTO>(_ => _.ToString());
                dg.EnableOpenCurrent<CashierColxnDTO>();
                dg.ScrollToEndOnChange();
                dg.F4ToViewSoA<CashierColxnDTO>(_ => _.Lease, VM.AppArgs);
            };
        }


        private CashierColxnsVM VM => DataContext as CashierColxnsVM;
    }
}
