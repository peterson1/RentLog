using CommonTools.Lib45.UIExtensions;
using System.Windows;

namespace RentLog.ChequeVouchers.JournalsTab.JournalsCrud
{
    public partial class JournalsCrudWindow : Window
    {
        public JournalsCrudWindow()
        {
            InitializeComponent();
            Loaded += (a, b) =>
            {
                VM.SetupForInsertCompleted += (c, d)
                    => txtSerial.ForceUpdateTarget();
            };
        }


        private JournalsCrudVM VM => DataContext as JournalsCrudVM;
    }
}
