using CommonTools.Lib45.UIExtensions;
using RentLog.DomainLib11.DTOs;
using System.Windows.Controls;

namespace RentLog.DomainLib45.SoaViewer.CellViewer
{
    public partial class AdjustmentsUI : UserControl
    {
        public AdjustmentsUI()
        {
            InitializeComponent();
            Loaded += (s, e) =>
            {
                dg.EnableOpenCurrent<BalanceAdjustmentDTO>();
            };
        }
    }
}
