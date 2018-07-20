using System.Windows.Controls;

namespace RentLog.DomainLib45.DailyStatusReporter.BankDeposits
{
    public partial class BankDepositsTable : UserControl
    {
        public BankDepositsTable()
        {
            InitializeComponent();
            Loaded += (a, b) =>
            {
                dg.CurrentCellChanged += (c, d)
                    => dg.SelectedIndex = -1;
            };
        }
    }
}
