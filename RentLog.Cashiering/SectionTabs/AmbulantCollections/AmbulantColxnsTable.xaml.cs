using CommonTools.Lib45.UIExtensions;
using RentLog.DomainLib11.DTOs;
using System.Windows.Controls;

namespace RentLog.Cashiering.SectionTabs.AmbulantCollections
{
    public partial class AmbulantColxnsTable : UserControl
    {
        public AmbulantColxnsTable()
        {
            InitializeComponent();
            Loaded += (a, b) =>
            {
                dg.ConfirmToDelete<AmbulantColxnDTO>(_ => _.ToString());
                dg.EnableOpenCurrent<AmbulantColxnDTO>();
                dg.ScrollToEnd();
            };
        }
    }
}
