using CommonTools.Lib11.DatabaseTools;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace CommonTools.Tests.DatabaseToolsTests
{
    [Trait("CombinedReadOnlyRepo", "Solitary")]
    public class CombinedReadOnlyRepoFacts
    {
        [Fact(DisplayName = "2 Repos")]
        public void TestMethod00001()
        {
            var rp1 = new Mock<ISimpleRepo<int>>();
            var rp2 = new Mock<ISimpleRepo<int>>();
            var sut = new CombinedReadOnlyRepo<int>
                        (rp1.Object, rp2.Object);

            rp1.Setup(_ => _.GetAll()).Returns
                (new List<int> { 1, 2, 3 });
            rp2.Setup(_ => _.GetAll()).Returns
                (new List<int> { 4, 5 });

            sut.GetAll().Should()
                .BeEquivalentTo(1, 2, 3, 4, 5);
        }


        [Fact(DisplayName = "3 Repos")]
        public void TestMethod00002()
        {
            var rp1 = new Mock<ISimpleRepo<int>>();
            var rp2 = new Mock<ISimpleRepo<int>>();
            var rp3 = new Mock<ISimpleRepo<int>>();
            var sut = new CombinedReadOnlyRepo<int>
                        (rp1.Object, rp2.Object, rp3.Object);

            rp1.Setup(_ => _.GetAll()).Returns
                (new List<int> { 1, 2, 3 });
            rp2.Setup(_ => _.GetAll()).Returns
                (new List<int> { 4, 5 });
            rp3.Setup(_ => _.GetAll()).Returns
                (new List<int> { 6 });

            sut.GetAll().Should()
                .BeEquivalentTo(1, 2, 3, 4, 5, 6);
        }
    }
}
