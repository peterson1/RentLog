using CommonTools.Lib45.UIExtensions;
using RentLog.DomainLib11.DTOs;
using System.Windows.Controls;

namespace RentLog.Cashiering.SectionTabs.Uncollecteds
{
    public partial class UncollectedsTable : UserControl
    {
        public UncollectedsTable()
        {
            InitializeComponent();
            Loaded += (s, e) =>
            {
                dg.EnableOpenCurrent<UncollectedLeaseDTO>();
            };
        }
    }
}
