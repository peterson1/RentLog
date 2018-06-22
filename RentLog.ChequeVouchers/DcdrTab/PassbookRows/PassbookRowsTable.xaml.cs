using System.Windows.Controls;

namespace RentLog.ChequeVouchers.DcdrTab.PassbookRows
{
    public partial class PassbookRowsTable : UserControl
    {
        public PassbookRowsTable()
        {
            InitializeComponent();

            //  don't call these here 
            //    this will be invoked twice, because it's on a TabControl
            //
            //Loaded += (a, b) =>
            //{
            //    dg.EnableOpenCurrent<PassbookRowDTO>();
            //    dg.ScrollToEndOnReplaced<PassbookRowDTO>();
            //};
        }


        //private PassbookRowsVM VM => DataContext as PassbookRowsVM;
    }
}
