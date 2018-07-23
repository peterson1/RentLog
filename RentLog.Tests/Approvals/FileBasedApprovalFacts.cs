using CommonTools.Lib11.Approvals;
using CommonTools.Lib45.Approvals;
using FluentAssertions;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace RentLog.Tests.Approvals
{
    [Trait("File-based Approval", "temp files")]
    public class FileBasedApprovalFacts
    {
        [Fact(DisplayName = "Posting and Finding requests")]
        public void TestMethod00001()
        {
            var user1   = "Mr. Requester";
            var user2   = "Mr. Responder";
            var reqKey  = "sample-request-key";
            var dir     = Path.GetTempPath();
            var sut1    = new FileBasedApprovalRequester<SampleClass1>(dir, user1);
            var sut2    = new FileBasedApprovalResponder<SampleClass1>(dir, user2);
            var origObj = new SampleClass1 { Message = "abc" };
            sut1.SendRequest(origObj, reqKey);
            sut1.IsRequestPosted(reqKey).Should().BeTrue();

            sut2.TryGetEnvelope(reqKey, out ApprovalEnvelope<SampleClass1> env).Should().BeTrue();

            env.RequestKey.Should().Be(reqKey);
            env.RequesterName.Should().Be(user1);
            env.Content.Message.Should().Be(origObj.Message);
        }


        [Fact(DisplayName = "Respond to Request")]
        public void TestMethod00002()
        {
            var user1   = "Mr. Requester";
            var user2   = "Mr. Responder";
            var reqKey  = "sample-request-key";
            var dir     = Path.GetTempPath();
            var sut1    = new FileBasedApprovalRequester<SampleClass1>(dir, user1);
            var sut2    = new FileBasedApprovalResponder<SampleClass1>(dir, user2);
            var origObj = new SampleClass1 { Message = "abc" };
            sut1.SendRequest(origObj, reqKey);

            sut2.TryGetEnvelope(reqKey, out ApprovalEnvelope<SampleClass1> req).Should().BeTrue();
            sut2.SendResponse (req, true, "sample remarks");

            sut1.TryGetEnvelope(reqKey, out ApprovalEnvelope<SampleClass1> resp).Should().BeTrue();
            resp.IsApproved.Should().BeTrue();
            resp.ResponseRemarks.Should().Be("sample remarks");
        }


        [Fact(DisplayName = "Watch for Response")]
        public async Task TestMethod00003()
        {
            var user1   = "Mr. Requester";
            var user2   = "Mr. Responder";
            var reqKey  = "sample-request-key";
            var dir     = Path.GetTempPath();
            var sut1    = new FileBasedApprovalRequester<SampleClass1>(dir, user1);
            var sut2    = new FileBasedApprovalResponder<SampleClass1>(dir, user2);
            var origObj = new SampleClass1 { Message = "abc" };
            var raised  = 0;
            sut1.ResponseReceived += (s, e) => raised++;
            sut1.SendRequest(origObj, reqKey);
            raised.Should().Be(0);

            sut2.ApproveRequest(reqKey);
            await Task.Delay(2000);
            raised.Should().Be(1);
        }


        public class SampleClass1
        {
            public string Message { get; set; }
        }
    }
}
