using CommonTools.Lib11.ExceptionTools;
using CommonTools.Lib45.InputDialogs;
using CommonTools.Lib45.ThreadTools;
using RentLog.DomainLib11.Authorization;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Models;
using RentLog.DomainLib45.SoaViewers.MainWindow;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace RentLog.DomainLib45.SoaViewers.StartBalanceEditor
{
    class StartBalanceEditorVM
    {
        internal static void ListenTo(Window win)
        {
            win.KeyUp += (s, e) =>
            {
                if (e.Key == Key.F2)
                    LaunchStartBalanceEditor(win);
            };
        }


        private static async void LaunchStartBalanceEditor(Window win)
        {
            var vm = win.DataContext as SoaViewerVM;
            if (!vm.AppArgs.CanEditLedgerStartBalance(true)) return;

            if (IsUpdateConfirmed(out DailyBillDTO dto, vm))
            {
                vm.StartBeingBusy("Recomputing balances...");
                await PersistThenRecompute(dto, vm);
                vm.StopBeingBusy();
                vm.ClickRefresh();
            }
        }


        private static bool IsUpdateConfirmed(out DailyBillDTO dtoForUpdate, SoaViewerVM vm)
        {
            var day1 = vm?.Rows?.LastOrDefault();
            if (day1 == null) throw Bad.Data("Day 1 row");

            dtoForUpdate = day1.DTO;
            var origVal  = day1.Rights.OpeningBalance;
            var suggVal  = GetSuggestedVal(day1.Rights, vm);
            var dte      = $"{day1.Date:d-MMM-yyyy}";
            var msg      = $"[{dte}] Starting Rights  (orig.val: {origVal:N2})";
            var ok       = PopUpInput.TryGetDecimal(msg, out decimal newVal, suggVal);
            if (!ok) return false;

            EditBillRow(dtoForUpdate, newVal, vm);
            return true;
        }


        private static decimal GetSuggestedVal(DailyBillDTO.BillState state, SoaViewerVM vm)
        {
            var orig = state.OpeningBalance;
            if (!orig.HasValue)
                Alert.ShowModal("Unsafe Operation", "Day 1 Opening Balance is NULL");

            var param = vm.Lease.Rights;
            return (param.TotalAmount - orig.Value) * -1;
        }


        private static void EditBillRow(DailyBillDTO dto, decimal newVal, SoaViewerVM vm)
        {
            var bCode = BillCode.Rights;
            var billr = vm.AppArgs.DailyBiller;
            var newState = billr.ComputeBill(bCode, vm.Lease, dto.GetBillDate(), newVal);
            dto.Bills.RemoveAll(_ => _.BillCode == bCode);
            dto.Bills.Add(newState);
        }


        private static async Task PersistThenRecompute(DailyBillDTO updatedDto, SoaViewerVM vm)
        {
            var repo = vm.AppArgs.Balances.GetRepo(vm.Lease);
            var date = updatedDto.GetBillDate();
            await Task.Delay(1);
            await Task.Run(() =>
            {
                repo.Update(updatedDto);
                repo.RecomputeFrom(date.AddDays(1));
            });
        }
    }
}
