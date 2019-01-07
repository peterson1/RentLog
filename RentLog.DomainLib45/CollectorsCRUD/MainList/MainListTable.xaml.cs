using CommonTools.Lib45.UIExtensions;
using RentLog.DomainLib11.DTOs;
using System.Windows.Controls;

namespace RentLog.DomainLib45.CollectorsCRUD.MainList
{
    public partial class MainListTable : UserControl
    {
        public MainListTable()
        {
            InitializeComponent();
            Loaded += (a, b) =>
            {
                dg.EnableOpenCurrent<CollectorDTO>();
            };
        }
    }
}
