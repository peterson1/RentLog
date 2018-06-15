using System.Windows;
using System.Windows.Controls;

namespace RentLog.ChequeVouchers.VoucherReqsTab.FundRequests.FundRequestCrud
{
    public partial class FundRequestCrudWindow : Window
    {
        public FundRequestCrudWindow()
        {
            InitializeComponent();
        }


        private void AmountTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            allocs.IsEnabled = VM.Draft.Amount.HasValue;
            VM.Allocations.OnAmountChanged(VM.Draft.Amount);
        }

        private FundRequestCrudVM VM => DataContext as FundRequestCrudVM;
    }
}
