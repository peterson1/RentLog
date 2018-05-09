using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static RentLog.DomainLib11.DTOs.DailyBillDTO;

namespace RentLog.DomainLib45.SoaViewer.MainWindow
{
    /// <summary>
    /// Interaction logic for BillStateSoaCell.xaml
    /// </summary>
    public partial class BillStateSoaCell : UserControl
    {
        public BillStateSoaCell()
        {
            InitializeComponent();
            Loaded += (a, b) =>
            {
                duPenalties.Visibility = VM?.TotalPenalties == 0
                    ? Visibility.Collapsed : Visibility.Visible;

                duAdjustments.Visibility = VM?.TotalAdjustments == 0
                    ? Visibility.Collapsed : Visibility.Visible;
            };
        }


        private BillState VM => DataContext as BillState;
    }
}
