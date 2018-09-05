using CommonTools.Lib45.UIExtensions;
using RentLog.DomainLib11.ReportRows;
using System.Windows;

namespace RentLog.DomainLib45.DailyStatusReporter.CollectorsPerformance.MainRowInspector
{
    public partial class MainRowInspectorWindow : Window
    {
        public MainRowInspectorWindow()
        {
            InitializeComponent();
            Loaded += (a, b) =>
            {
                dg.EnableOpenCurrent<CollectorPerfSubRow>();
            };
        }
    }
}
