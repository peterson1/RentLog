using CommonTools.Lib45.UIExtensions;
using RentLog.DomainLib11.DTOs;
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
            };
        }
    }
}
