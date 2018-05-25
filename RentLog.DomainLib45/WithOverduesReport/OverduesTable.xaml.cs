using CommonTools.Lib45.UIExtensions;
using RentLog.DomainLib11.ReportRows;
using System.Windows.Controls;

namespace RentLog.DomainLib45.WithOverduesReport
{
    public partial class OverduesTable : UserControl
    {
        public OverduesTable()
        {
            InitializeComponent();
            Loaded += (s, e)
                => dg.EnableOpenCurrent<LeaseBalanceRow>();
        }
    }
}
