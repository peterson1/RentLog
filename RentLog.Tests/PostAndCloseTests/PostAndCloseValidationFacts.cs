using FluentAssertions;
using FluentAssertions.Extensions;
using RentLog.Cashiering;
using System.Threading.Tasks;
using Xunit;

namespace RentLog.Tests.PostAndCloseTests
{
    [Trait("Post&Close", "Sample Dir")]
    public class PostAndCloseValidationFacts
    {
        [Fact(DisplayName = "Rejects missing Vacants table")]
        public async Task TestMethod00001()
        {
            var arg  = SampleDBs.SampleDir.Jul9_GRY_Open();
            var main = new MainWindowVM(9.July(2018), arg, false);
            var vm   = main.PostAndClose;

            await vm.RefreshCmd.RunAsync();
            vm.CanPostAndClose().Should().BeFalse();
            vm.PostAndCloseCmd.CurrentLabel.Should().Contain("submit");
        }
    }
}
