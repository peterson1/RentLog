using System;
using CommonTools.Lib11.Approvals;
using CommonTools.Lib11.ExceptionTools;
using CommonTools.Lib11.StringTools;
using CommonTools.Lib45.Approvals;
using CommonTools.Lib45.FileSystemTools;
using PropertyChanged;

namespace RentLog.Cashiering.MainToolbar
{
    [AddINotifyPropertyChangedInterface]
    public class ApprovalResponderVM
    {
        private MainWindowVM _main;
        private string       _reqKey;
        private FileBasedApprovalResponder<string> _respondr;


        public ApprovalResponderVM(MainWindowVM mainWindowVM)
        {
            _main     = mainWindowVM;
            _reqKey   = _main.ApprovalAwaiter.RequestKey;
            _respondr = CreateResponder();
        }


        public bool CanApproveRequest(out string whyNot)
        {
            if (!_respondr.TryGetEnvelope(_reqKey, out ApprovalEnvelope<string> env))
            {
                whyNot = "Cashier did not submit.";
                return false;
            }
            if (!HashIsValid(env))
            {
                whyNot = "Database is currently updating.";
                return false;
            }
            whyNot = "";
            return true;
        }


        private FileBasedApprovalResponder<string> CreateResponder()
            => new FileBasedApprovalResponder<string>(_main.ApprovalAwaiter.FolderPath, 
                                                      _main.AppArgs.MarketState.CurrentUser);


        public void ApproveTheRequest() 
            => _respondr.ApproveRequest(_reqKey);


        private bool HashIsValid(ApprovalEnvelope<string> env)
            => _main.ColxnsDB.DatabasePath.SHA1ForFile() == env.Content;
    }
}
