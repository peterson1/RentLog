using CommonTools.Lib11.Approvals;
using CommonTools.Lib11.InputCommands;
using CommonTools.Lib11.StringTools;
using CommonTools.Lib45.Approvals;
using CommonTools.Lib45.FileSystemTools;
using CommonTools.Lib45.InputCommands;
using CommonTools.Lib45.ThreadTools;
using PropertyChanged;
using RentLog.DomainLib11.StateTransitions;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace RentLog.Cashiering.MainToolbar
{
    [AddINotifyPropertyChangedInterface]
    public class ApprovalRequesterVM
    {
        private MainWindowVM _main;
        private FileBasedApprovalRequester<string> _requestr;


        public ApprovalRequesterVM(MainWindowVM mainWindowVM)
        {
            _main      = mainWindowVM;
            FolderPath = FindFolderPath(_main);
            RequestKey = _main.Date.ToString("yyyy-MM-dd");
            CancelCmd  = R2Command.Relay(DoCancel);
        }

        public async Task WaitForApproval()
        {
            SubmitForApproval();

            _main.StartBeingBusy("Submitting collections for review ...");
            await Task.Delay(1000 * 4);//artificial delay for web sync to upload changes

            UIThread.Run(() => MessageBox.Show(
                $"Successfully submitted collections for review.",
            "   Operation Successful", MessageBoxButton.OK, MessageBoxImage.Information));

            StartBeingBusy("Waiting for Supervisor to Post & Close the market day..."
                   + L.F + "Please keep this window open.");
        }


        public string      RequestKey { get; }
        public string      FolderPath { get; }
        public IR2Command  CancelCmd  { get; }
        public bool        IsBusy     { get; set; }
        public string      BusyText   { get; set; }


        public void SubmitForApproval()
        {
            var dbHash = _main.ColxnsDB.DatabasePath.SHA1ForFile();
            _requestr  = CreateRequester();
            _requestr.ResponseReceived += _requestr_ResponseReceived;
            _requestr.SendRequest(dbHash, RequestKey);
        }


        private void _requestr_ResponseReceived(object sender, ApprovalEnvelope<string> envelope)
        {
            DoCancel();
            if (envelope.IsApproved != true) return;

            var jobs = MarketDayCloser.GetActions(_main.ColxnsDB, _main.AppArgs, 
                                                    envelope.ResponderName);
            Parallel.Invoke(jobs.ToArray());

            CurrentExe.RelaunchApp();
        }


        private FileBasedApprovalRequester<string> CreateRequester()
        {
            var user = _main.AppArgs.MarketState.CurrentUser;
            return new FileBasedApprovalRequester<string>(FolderPath, user);
        }


        private void StartBeingBusy(string message)
        {
            _main.StopBeingBusy();
            IsBusy = true;
            BusyText = message;
        }


        private void DoCancel()
        {
            _requestr.RevokeRequest(RequestKey);
            _requestr.Dispose();
            _requestr = null;
            IsBusy = false;
        }


        private string FindFolderPath(MainWindowVM main)
            => Path.GetDirectoryName(main.AppArgs.MarketState.DatabasePath);
    }
}
