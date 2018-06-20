using CommonTools.Lib45.UIExtensions;
using RentLog.DomainLib11.DTOs;
using System.Windows.Controls;

namespace RentLog.ChequeVouchers.DcdrTab.PassbookRows
{
    public partial class PassbookRowsTable : UserControl
    {
        public PassbookRowsTable()
        {
            InitializeComponent();
            Loaded += (a, b) =>
            {
                dg.ScrollToEndOnReplaced<PassbookRowDTO>();
            };
        }


        private PassbookRowsVM VM => DataContext as PassbookRowsVM;
    }
}
