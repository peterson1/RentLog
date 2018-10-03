//using CommonTools.Lib11.DTOs;
//using FluentAssertions;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Xunit;

//namespace CommonTools.Tests.ComparisonsAlignerTests
//{
//    [Trait("Align By IDs", "Solitary")]
//    public class ComparisonsAlignerFacts
//    {
//        [Fact(DisplayName = "Equivalent")]
//        public void TestMethod00001()
//        {
//            var list1 = SampleList(1, 2, 3);
//            var list2 = SampleList(1, 2, 3);

//            var diffs = (list1, list2).AlignByIDs();

//            diffs.Should().HaveCount(3);
//            (diffs[0].Document1 as IDocumentDTO).Id.Should().Be(1);
//            (diffs[0].Document2 as IDocumentDTO).Id.Should().Be(1);
//            (diffs[1].Document1 as IDocumentDTO).Id.Should().Be(2);
//            (diffs[1].Document2 as IDocumentDTO).Id.Should().Be(2);
//            (diffs[2].Document1 as IDocumentDTO).Id.Should().Be(3);
//            (diffs[2].Document2 as IDocumentDTO).Id.Should().Be(3);
//        }


//        [Fact(DisplayName = "With Gaps")]
//        public void TestMethod00002()
//        {
//            var list1 = SampleList(1, 3, 5);
//            var list2 = SampleList(2, 4, 6);

//            var diffs = (list1, list2).AlignByIDs();

//            diffs.Should().HaveCount(6);
//            (diffs[0].Document1 as IDocumentDTO).Id.Should().Be(1);
//            (diffs[0].Document2 as IDocumentDTO).Should().BeNull();

//            (diffs[1].Document1 as IDocumentDTO).Should().BeNull();
//            (diffs[1].Document2 as IDocumentDTO).Id.Should().Be(2);

//            (diffs[2].Document1 as IDocumentDTO).Id.Should().Be(3);
//            (diffs[2].Document2 as IDocumentDTO).Should().BeNull();

//            (diffs[3].Document1 as IDocumentDTO).Should().BeNull();
//            (diffs[3].Document2 as IDocumentDTO).Id.Should().Be(4);

//            (diffs[4].Document1 as IDocumentDTO).Id.Should().Be(5);
//            (diffs[4].Document2 as IDocumentDTO).Should().BeNull();

//            (diffs[5].Document1 as IDocumentDTO).Should().BeNull();
//            (diffs[5].Document2 as IDocumentDTO).Id.Should().Be(6);
//        }


//        private List<IDocumentDTO> SampleList(params int?[] ids) 
//            => ids.Select(_ => SampleDTO(_)).ToList();


//        private IDocumentDTO SampleDTO(int? id)
//            => !id.HasValue ? null : new TestDTO
//                {
//                    Id = id.Value
//                };


//        public class TestDTO : IDocumentDTO
//        {
//            public int       Id         { get; set; }
//            public string    Author     { get; set; }
//            public DateTime  Timestamp  { get; set; }
//            public string    Remarks    { get; set; }
//        }
//    }
//}
