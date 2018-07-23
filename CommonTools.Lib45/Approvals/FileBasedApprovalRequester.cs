using CommonTools.Lib11.Approvals;
using CommonTools.Lib11.FileSystemTools;
using CommonTools.Lib11.StringTools;
using CommonTools.Lib45.FileSystemTools;
using System;
using System.IO;

namespace CommonTools.Lib45.Approvals
{
    public class FileBasedApprovalRequester<T> : FileBasedApprovalBase, IDisposable
    {
        public event EventHandler<ApprovalEnvelope<T>> ResponseReceived = delegate { };
        private IThrottledFileWatcher _watchr;


        public FileBasedApprovalRequester(string folderPath, string requesterName) : base(folderPath, requesterName)
        {
        }


        public void SendRequest(T contentToApprove, string requestKey, string remarks = null)
        {
            WriteEnvelope(new ApprovalEnvelope<T>()
            {
                Content        = contentToApprove,
                RequestDate    = DateTime.Now,
                RequesterName  = Username,
                RequestKey     = requestKey,
                RequestRemarks = remarks
            });
            InitializeFileWatcher(requestKey);
        }


        private void InitializeFileWatcher(string requestKey)
        {
            _watchr?.StopWatching();
            _watchr = null;
            _watchr = new ThrottledFileWatcher1();
            _watchr.FileChanged += _watchr_FileChanged;
            _watchr.StartWatching(GetFilePath(requestKey));
        }


        private void _watchr_FileChanged(object sender, EventArgs e)
        {
            if (!TryGetEnvelope(out ApprovalEnvelope<T> envelope, 
                _watchr.TargetFile)) return;

            if (envelope.IsApproved.HasValue)
                ResponseReceived?.Invoke(this, envelope);
        }


        public bool IsRequestPosted(string requestKey)
            => TryGetEnvelope(requestKey, out ApprovalEnvelope<T> env);


        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _watchr.StopWatching();
                    _watchr = null;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~FileBasedApprovalRequester() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
