﻿using CommonTools.Lib45.UIExtensions;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using static RentLog.TrayLauncher.Properties.Settings;

namespace RentLog.TrayLauncher
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Loaded += (a, b) =>
            {
                trayIcon.Icon = new Icon(Default.TrayIconFile);

                ctxMenu.Items.Add(new Separator());
                ctxMenu.Add("Cashiering");
                ctxMenu.AddLauncher(VM.EncoderCmd );
                ctxMenu.AddLauncher(VM.ReviewerCmd);

                ctxMenu.Items.Add(new Separator());
                ctxMenu.Add("Contracts");
                ctxMenu.AddLauncher(VM.LeaseFiltersCmd);
                ctxMenu.AddLauncher(VM.StallsCmd  );

                ctxMenu.Items.Add(new Separator());
                ctxMenu.Add("Finance");
                ctxMenu.AddLauncher(VM.ChequesCmd );

                ctxMenu.Items.Add(new Separator());
                ctxMenu.Add("Reports");
                ctxMenu.AddLauncher(VM.OverduesCmd);
                ctxMenu.AddLauncher(VM.ColxnSummaryCmd);
                ctxMenu.AddLauncher(VM.DailyStatusCmd);
                ctxMenu.AddLauncher(VM.GLRecapCmd);

                ctxMenu.Items.Add(new Separator());
                //ctxMenu.Add("Deprecated (do NOT use)");
                //ctxMenu.AddLauncher(VM.LeasesCmd  );
                ctxMenu.Items.Add(CreateBackOfficeMenu());

                ctxMenu.Items.Add(new Separator());
                ctxMenu.Add(VM.CloseWindowCmd);
            };
        }


        private MenuItem CreateBackOfficeMenu()
        {
            var mnu = new MenuItem();
            mnu.Header = "Back Office Maintenance";
            mnu.AddLauncher(VM.CollectorsCmd);
            mnu.Add("Deprecated (do NOT use)");
            mnu.AddLauncher(VM.LeasesCmd);
            return mnu;
        }


        private MainWindowVM VM => DataContext as MainWindowVM;
    }
}
