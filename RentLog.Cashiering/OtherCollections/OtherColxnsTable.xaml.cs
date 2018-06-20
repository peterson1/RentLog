using CommonTools.Lib45.UIExtensions;
using RentLog.DomainLib11.DTOs;
using System.Windows.Controls;

namespace RentLog.Cashiering.OtherCollections
{
    public partial class OtherColxnsTable : UserControl
    {
        public OtherColxnsTable()
        {
            InitializeComponent();
            Loaded += (a, b) =>
            {
                dg.ConfirmToDelete<OtherColxnDTO>(_ => _.ToString());
                dg.EnableOpenCurrent<OtherColxnDTO>();
                dg.ScrollToEnd();
            };
        }
    }
}
