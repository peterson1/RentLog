using CommonTools.Lib45.UIExtensions;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace RentLog.FilteredLeases.FilteredLists.AllActiveLeases
{
    public partial class AllActiveLeasesTable : UserControl
    {
        public AllActiveLeasesTable()
        {
            InitializeComponent();
            Loaded += (a, b) =>
            {
                dg.EnableToggledColumns(FindColumnHeaderStyle());
            };
        }


        private Style FindColumnHeaderStyle() 
            => dg?.Style?.Resources?.Values?
                .OfType<Style>()?.FirstOrDefault();
    }
}
