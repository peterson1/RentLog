using CommonTools.Lib45.UIExtensions;
using System.Windows.Controls;

namespace RentLog.LeasesCrud.LeaseCRUD
{
    public partial class TenantInfoPane : UserControl
    {
        public TenantInfoPane()
        {
            InitializeComponent();

            Loaded += (s, e) =>
            {
                foreach (var ctrl in this.GetChildren())
                    if (ctrl is TextBox txt)
                        txt.ValidateNonBlank();
            };
        }
    }
}
