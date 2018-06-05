using FluentAssertions;
using FluentAssertions.Extensions;
using Moq;
using RentLog.Cashiering;
using RentLog.DomainLib11.CollectionRepos;
using RentLog.Tests.TestTools;
using System;
using Xunit;

namespace RentLog.Tests.CashieringVMTests
{
    [Trait("Cashiering VM", "Solitary")]
    public class CashieringVMFacts
    {
        [Fact(DisplayName = "Reviewing non-closed won't exit")]
        public void Reviewingnonclosedwontexit()
        {
            var args = new MockDBsDir();
            var date = 2.May(2018);

            args.Credentials.Roles = "Supervisor";

            args.MoqCollectionsDir.Setup(_ 
                => _.For(date))
                .Returns(Mock.Of<ICollectionsDB>());

            var sut  = new MainWindowVM(date, args, false);
            sut.ShouldClose.Should().BeFalse();
        }


        [Fact(DisplayName = "Reviewing newly closed exits app")]
        public void Reviewingnewlyclosedexitsapp()
        {
            var args = new MockDBsDir();
            var sut  = new MainWindowVM(3.May(2018), args);
            sut.ShouldClose.Should().BeTrue();
        }


        [Fact(DisplayName = "Detects privileged Reviewer")]
        public void DetectsprivilegedReviewer()
        {
            var args = new MockDBsDir();
            args.Credentials.Roles = "Admin";
            var sut = new MainWindowVM(3.May(2018), args);
            sut.CanReview.Should().BeTrue();
        }


        [Fact(DisplayName = "Detects non-Reviewer user")]
        public void DetectsnonRevieweruser()
        {
            var args = new MockDBsDir();
            args.Credentials.Roles = "Cashier";
            var sut = new MainWindowVM(3.May(2018), args);
            sut.CanReview.Should().BeFalse();
        }


        [Fact(DisplayName = "Detects privileged Encoder")]
        public void DetectsprivilegedEncoder()
        {
            var args = new MockDBsDir();
            args.Credentials.Roles = "Cashier";
            var sut = new MainWindowVM(3.May(2018), args);
            sut.CanEncode.Should().BeTrue();
        }


        [Fact(DisplayName = "Detects non-Encoder user")]
        public void DetectsnonEncoderuser()
        {
            var args = new MockDBsDir();
            args.Credentials.Roles = "Supervisor";
            var sut = new MainWindowVM(3.May(2018), args);
            sut.CanEncode.Should().BeFalse();
        }


        [Fact(DisplayName = "Rejects non-privileged user")]
        public void Rejectsnonprivilegeduser()
        {
            var args = new MockDBsDir();
            var date = 2.May(2018);

            args.MoqCollectionsDir.Setup(_
                => _.For(date))
                .Returns(Mock.Of<ICollectionsDB>());

            args.Credentials.Roles = "impostor";

            var sut = new MainWindowVM(date, args, false);
            sut.ShouldClose.Should().BeTrue();
        }


        [Fact(DisplayName = "Encoding newly closed won't exit")]
        public void EncodingnewlyclosedcreatesDBfilefornextday()
        {
            var args = new MockDBsDir();
            args.Credentials.Roles = "Cashier";
            var sut  = new MainWindowVM(3.May(2018), args);
            sut.ShouldClose.Should().BeFalse();
        }
    }
}
