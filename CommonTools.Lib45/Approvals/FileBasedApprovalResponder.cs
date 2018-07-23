using CommonTools.Lib11.Approvals;
using CommonTools.Lib11.ExceptionTools;
using System;

namespace CommonTools.Lib45.Approvals
{
    public class FileBasedApprovalResponder<T> : FileBasedApprovalBase
    {
        public FileBasedApprovalResponder(string folderPath, string responderName) : base(folderPath, responderName)
        {
        }


        public void SendResponse(ApprovalEnvelope<T> reqEnvelope, bool approvalOfRequest, string responseRemarks = null)
        {
            reqEnvelope.ResponderName   = Username;
            reqEnvelope.ResponseDate    = DateTime.Now;
            reqEnvelope.IsApproved      = approvalOfRequest;
            reqEnvelope.ResponseRemarks = responseRemarks;

            WriteEnvelope(reqEnvelope);
        }


        public void ApproveRequest(string reqKey)
        {
            if (!TryGetEnvelope(reqKey, out ApprovalEnvelope<T> env))
                throw Bad.Arg("Request Key", reqKey);

            SendResponse(env, true);
        }
    }
}
