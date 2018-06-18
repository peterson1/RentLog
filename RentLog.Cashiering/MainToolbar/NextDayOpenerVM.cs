using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.StateTransitions;
using System.Threading.Tasks;

namespace RentLog.Cashiering.MainToolbar
{
    public class NextDayOpenerVM
    {
        private MainWindowVM   _main;
        private ICollectionsDB _db;
        private bool           _wasRun;

        public NextDayOpenerVM(MainWindowVM mainWindowVM)
        {
            _main = mainWindowVM;
            _db   = _main.ColxnsDB;
        }


        internal async Task RunIfNeeded()
        {
            if (!ShouldRun()) return;
            _main.StartBeingBusy("Opening next market day ...");
            await Task.Run(() =>
            {
                var jobs = MarketDayOpener.GetActions(_db, _main.AppArgs);
                Parallel.Invoke(jobs.ToArray());
            });
            _main.StopBeingBusy();
            _wasRun = true;

        }


        private bool ShouldRun()
        {
            if (_wasRun) return false;
            if (!_main.CanEncode) return false;
            if (_db.IsPosted()) return false;
            if (_db.IsOpened()) return false;
            return true;
        }
    }
}
