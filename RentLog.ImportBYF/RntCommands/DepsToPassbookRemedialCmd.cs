using CommonTools.Lib11.InputCommands;
using CommonTools.Lib11.StringTools;
using CommonTools.Lib45.InputCommands;
using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.StateTransitions;
using RentLog.ImportBYF.Version2UI;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentLog.ImportBYF.RntCommands
{
    public static class DepsToPassbookRemedialCmd
    {
        private const string META_KEY = "DepositsAddedToPassbook_v1";


        private static async Task RemediateAll(MainWindowVM2 main)
        {
            var txns = main.TransactionData;
            var colxnsDir = main.AppArgs.Collections;
            var orderDesc = txns.PeriodsList.OrderBy(_ => _.Date);
            foreach (var row in orderDesc)
            {
                txns.Status = $"Remediating [{row.Date:d MMM yyyy}]...";
                row.StartBeingBusy("Adding Deposits to Passbook ...");
                await Task.Run(() =>
                {
                    row.FillRntCell();
                    if (!row.RntCell.HasValue) return;
                    var db = colxnsDir.For(row.Date);
                    var deps = db.BankDeposits.GetAll();
                    db.AddDepsToPassbookIfNeeded(deps, main.AppArgs);
                });
                row.StopBeingBusy();
            }
            txns.Status = "Remediations finished.";
        }


        public static void AddDepsToPassbookIfNeeded(this ICollectionsDB colxnsDB, IEnumerable<BankDepositDTO> deps, ITenantDBsDir dir)
        {
            if (colxnsDB.IsRemediated()) return;
            var tweakd = TweakDepositsForPassbook(deps);
            dir.UpdateAccountPassbooks(tweakd, colxnsDB.Date, false);
            colxnsDB.MarkAsRemediated();
        }


        private static IEnumerable<BankDepositDTO> TweakDepositsForPassbook(IEnumerable<BankDepositDTO> deps)
        {
            foreach (var dep in deps)
            {
                if (dep.DocumentRef.IsBlank())
                    dep.DocumentRef = "-";
            }
            return deps;
        }


        private static bool IsRemediated(this ICollectionsDB colxnsDB)
            => colxnsDB.Meta.Has(META_KEY);

        private static void MarkAsRemediated(this ICollectionsDB colxnsDB)
            => colxnsDB.Meta.SetTrue(META_KEY);


        public static IR2Command CreateADTPBCmd(this MainWindowVM2 main)
            => R2Command.Async(() => RemediateAll(main), null, "Add Deposits to Passbook");
    }
}
