using System.Threading.Tasks;
using System.Windows;

namespace RentLog.DomainLib45.StallPicker
{
    public partial class StallPickerWindow : Window
    {
        public StallPickerWindow()
        {
            InitializeComponent();
            Loaded += async (a, b) =>
            {
                await Task.Delay(500);
                cmbSec.IsDropDownOpen = true;

                cmbSec.SelectionChanged += (c, d)
                    => cmbStall.IsDropDownOpen = true;
            };
        }
    }
}
