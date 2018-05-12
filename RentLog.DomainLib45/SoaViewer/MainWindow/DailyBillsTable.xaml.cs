﻿using CommonTools.Lib11.ExceptionTools;
using CommonTools.Lib45.UIExtensions;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.ReportRows;
using System;
using System.Windows.Controls;

namespace RentLog.DomainLib45.SoaViewer.MainWindow
{
    public partial class DailyBillsTable : UserControl
    {
        public DailyBillsTable()
        {
            InitializeComponent();
            Loaded += (s, e) =>
            {
                dg.EnableOpenCurrent<DailyBillsRow>();

                if (dg.Columns.Count != 7)
                    throw new NotImplementedException("update hard-coded column indices");
            };
        }


        private BillCode GetCurrentBillCode()
        {
            var colIndex = dg.CurrentCell.Column.DisplayIndex;
            switch (colIndex)
            {
                case 3: return BillCode.Rent;
                case 4: return BillCode.Rights;
                case 5: return BillCode.Electric;
                case 6: return BillCode.Water;
                default:
                    throw Fault.BadArg("columnIndex", colIndex);
            }
        }


        private void dg_CurrentCellChanged(object sender, EventArgs e)
            => VM.BillCode = GetCurrentBillCode();


        private SoaViewerVM VM => DataContext as SoaViewerVM;
    }
}
