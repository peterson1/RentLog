using CommonTools.Lib11.Approvals;
using CommonTools.Lib11.FileSystemTools;
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
            if (_watchr == null) return;

            if (!TryGetEnvelope(out ApprovalEnvelope<T> envelope, 
                _watchr.TargetFile)) return;

            if (envelope.IsApproved.HasValue)
                ResponseReceived?.Invoke(this, envelope);
        }


        public bool IsRequestPosted(string requestKey)
            => TryGetEnvelope(requestKey, out ApprovalEnvelope<T> env);


        public void RevokeRequest(string requestKey)
        {
            _watchr?.StopWatching();
            _watchr = null;
            File.Delete(GetFilePath(requestKey));
        }


        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _watchr?.StopWatching();
                    _watchr = null;
                }
                disposedValue = true;
            }
        }
        public void Dispose() => Dispose(true);
        #endregion
    }
}
