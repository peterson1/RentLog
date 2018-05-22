using CommonTools.Lib45.UIExtensions;
using System.Windows.Controls;

namespace RentLog.LeasesCrud.LeaseCRUD
{
    public partial class ContractInfoPane : UserControl
    {
        public ContractInfoPane()
        {
            InitializeComponent();

            Loaded += (a, b) =>
            {
                SetNonBlankValidations();
                cmbStart.SelectedDateChanged += (c, d) => VM.UpdateDerivatives();
                cmbEnd  .SelectedDateChanged += (c, d) => VM.UpdateDerivatives();
                txtGracePeriod  .TextChanged += (c, d) => VM.UpdateDerivatives();
                txtRightsSpan   .TextChanged += (c, d) => VM.UpdateDerivatives();
            };
        }


        private LeaseCrudVM VM => DataContext as LeaseCrudVM;


        private void SetNonBlankValidations()
        {
            foreach (var ctrl in this.GetChildren())
                if (ctrl is TextBox txt)
                    txt.ValidateNonBlank();
        }
    }
}
