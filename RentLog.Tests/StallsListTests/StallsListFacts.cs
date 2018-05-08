//using FluentAssertions;
//using Moq;
//using RentLog.DomainLib11.DTOs;
//using RentLog.DomainLib11.Repositories;
//using RentLog.DomainLib45;
//using RentLog.StallsCrud.StallsList;
//using System.Collections.Generic;
//using Xunit;

//namespace RentLog.Tests.StallsListTests
//{
//    [Trait("StallsListVM", "Solitary")]
//    public class StallsListFacts
//    {
//        [Fact(DisplayName = "Lists stalls for section")]
//        public void Listsstallsforsection()
//        {
//            var arg = new TestArguments();
//            var sut = new StallsListVM(arg);
//            sut.ReloadFromDB();
//            sut.Rows.Should().HaveCount(3);
//        }


//        class TestArguments : AppArguments
//        {
//            public TestArguments()
//            {
//                Stalls       = MockStallsRepo();
//                ActiveLeases = MockActiveLeasesRepo();
//            }


//            private IStallsRepo MockStallsRepo()
//            {
//                var moq = new Mock<IStallsRepo>();
//                moq.Setup(_ => _.ForSection(It.IsAny<SectionDTO>())).Returns(new List<StallDTO>
//                {
//                    new StallDTO(),
//                    new StallDTO(),
//                    new StallDTO(),
//                });
//                return moq.Object;
//            }


//            private ILeasesRepo MockActiveLeasesRepo()
//            {
//                var moq = new Mock<ILeasesRepo>();

//                moq.Setup(_ => _.StallsLookup()).Returns(new Dictionary<int, LeaseDTO>
//                {
//                    { 1, new LeaseDTO() },
//                    { 2, new LeaseDTO() },
//                    { 3, new LeaseDTO() },
//                });

//                return moq.Object;
//            }
//        }
//    }
//}
