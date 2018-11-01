using CommonTools.Lib45.UIExtensions;
using System.Windows.Controls;

namespace RentLog.ImportBYF.Version2UI.LeaseBalancesPane.LeasesList
{
    public partial class LeasesListUI : UserControl
    {
        public LeasesListUI()
        {
            InitializeComponent();
            Loaded += (s, e) =>
            {
                dg.EnableOpenCurrent<LeaseRowVM>();
            };
        }
    }
}
