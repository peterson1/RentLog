using CommonTools.Lib45.UIExtensions;
using RentLog.DomainLib11.DTOs;
using System.Windows.Controls;

namespace RentLog.Cashiering.SectionTabs.IntendedCollections
{
    public partial class IntendedColxnsTable : UserControl
    {
        public IntendedColxnsTable()
        {
            InitializeComponent();
            Loaded += (s, e) =>
            {
                dg.EnableOpenCurrent<IntendedColxnDTO>();
                dg.ConfirmToDelete<IntendedColxnDTO>(_ => _.ToString());
            };
        }
    }
}
