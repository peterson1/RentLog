﻿using CommonTools.Lib45.UIExtensions;
using RentLog.DomainLib11.ReportRows;
using System.Windows.Controls;

namespace RentLog.StallsCrud.StallsList
{
    public partial class StallsTable : UserControl
    {
        public StallsTable()
        {
            InitializeComponent();
            Loaded += (a, b) =>
            {
                dg.ConfirmToDelete<StallRow>(_ => _.DTO.Name);
                dg.EnableOpenCurrent<StallRow>();
                dg.ScrollToEnd();
            };
        }
    }
}
