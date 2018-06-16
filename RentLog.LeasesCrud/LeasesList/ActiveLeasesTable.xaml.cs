using CommonTools.Lib45.ThreadTools;
using CommonTools.Lib45.UIExtensions;
using RentLog.DomainLib11.ReportRows;
using System.Windows.Controls;

namespace RentLog.LeasesCrud.LeasesList
{
    public partial class ActiveLeasesTable : UserControl
    {
        public ActiveLeasesTable()
        {
            InitializeComponent();
            //Loaded += (s, e) =>
            //    dg.EnableOpenCurrent<LeaseBalanceRow>();
        }
    }
}
