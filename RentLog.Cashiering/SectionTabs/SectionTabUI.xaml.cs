using System.Threading.Tasks;
using System.Windows.Controls;

namespace RentLog.Cashiering.SectionTabs
{
    public partial class SectionTabUI : UserControl
    {
        public SectionTabUI()
        {
            InitializeComponent();
            //Loaded += async (a, b) =>
            //{
            //    if (!this.IsVisible) return;
            //    await Task.Delay(500);
            //    uncol.Focus();
            //    uncol.filtr.Focus();
            //    uncol.filtr.txt1.Focus();
            //};
        }
    }
}
