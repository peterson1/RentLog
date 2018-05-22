using CommonTools.Lib45.UIExtensions;
using RentLog.DomainLib11.ReportRows;
using System.Windows.Controls;

namespace RentLog.LeasesCrud.LeasesList
{
    public partial class InactiveLeasesTable : UserControl
    {
        public InactiveLeasesTable()
        {
            InitializeComponent();
            Loaded += (s, e) =>
            {
                dg.EnableOpenCurrent<LeaseBalanceRow>();
            };
        }
    }
}
