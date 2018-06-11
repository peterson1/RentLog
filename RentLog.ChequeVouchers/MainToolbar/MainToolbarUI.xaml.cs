using System.Windows.Controls;
using System.Windows.Media;

namespace RentLog.ChequeVouchers.MainToolbar
{
    public partial class MainToolbarUI : UserControl
    {
        public MainToolbarUI()
        {
            InitializeComponent();
            Loaded += (a, b) =>
            {
                cmbBnkAccts.DropDownOpened += (c, d)
                    => cmbBnkAccts.Foreground = Brushes.Black;

                cmbBnkAccts.DropDownClosed += (c, d)
                    => cmbBnkAccts.Foreground = Brushes.White;
            };
        }
    }
}
