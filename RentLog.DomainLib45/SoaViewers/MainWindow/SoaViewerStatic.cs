using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace RentLog.DomainLib45.SoaViewers.MainWindow
{
    public static class SoaViewer
    {
        public static void Show(LeaseDTO lse, ITenantDBsDir args)
        {
            if (lse == null || args == null) return;
            new SoaViewerVM(lse, args).Show<SoaViewerWindow>();
        }


        public static void F4ToViewSoA<T>(this DataGrid dg,
            Func<T, LeaseDTO> leaseGetter, 
            ITenantDBsDir tenantDBsDir,
            Key expectedKey = Key.F4)
            where T : class
        {
            dg.PreviewKeyDown += (s, e) =>
            {
                if (e.Key == expectedKey)
                    TryLaunchViewer(dg, leaseGetter, tenantDBsDir);
            };
        }


        private static void TryLaunchViewer<T>(DataGrid dg, Func<T, LeaseDTO> leaseGetter, ITenantDBsDir tenantDBsDir)
            where T : class
        {
            if (dg.SelectedIndex == -1) return;
            var item = dg.SelectedItem as T;
            if (item == null) return;
            Show(leaseGetter(item), tenantDBsDir);
        }
    }
}
