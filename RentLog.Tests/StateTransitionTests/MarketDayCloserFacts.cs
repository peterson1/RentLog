using FluentAssertions;
using FluentAssertions.Extensions;
using RentLog.DomainLib11.StateTransitions;
using RentLog.Tests.TestTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RentLog.Tests.StateTransitionTests
{
    [Trait("Post & Close", "Solitary")]
    public class MarketDayCloserFacts
    {
        [Fact(DisplayName = "Posts unclosed day", Skip = "Undone")]
        public void Postsunclosedday()
        {
            var arg = new MockDBsDir();

            arg.Collections.LastPostedDate()
                .Should().Be(2.May(2018));

            //MarketDayCloser.Run(arg);

            arg.Collections.LastPostedDate()
                .Should().Be(3.May(2018));
        }
    }
}
