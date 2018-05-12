using CommonTools.Lib11.DatabaseTools;
using Moq;
using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.DTOs;
using Xunit;

namespace RentLog.Tests.BalAdjsRepoTests
{
    [Trait("Bal Adjs. Repo", "Solitary")]
    public class BalAdjsRepoFacts
    {
        [Fact(DisplayName = "Insert calls BatchBalUpdate", Skip ="Undone")]
        public void InsertcallsBatchBalUpdate()
        {
            //var moq = new Mock<ISimpleRepo<BalanceAdjustmentDTO>>();
            //var sut = new BalanceAdjsRepo1(moq.Object, );
            //sut.Insert(new BalanceAdjustmentDTO());
            //moq.Verify(_ => _.)
        }
    }
}
