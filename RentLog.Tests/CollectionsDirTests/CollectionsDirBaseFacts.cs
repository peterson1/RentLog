//using FluentAssertions;
//using FluentAssertions.Extensions;
//using RentLog.DomainLib11.CollectionRepos;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Xunit;

//namespace RentLog.Tests.CollectionsDirTests
//{
//    public class CollectionsDirBaseFacts
//    {
//        [Fact(DisplayName = "Sets BusinessDate meta")]
//        public void SetsBusinessDatemeta()
//        {
//            var date = 2.May(2018);
//            var sut = new TestClass1();
//            var db = sut.CreateFor(date);

//            db.Should().NotBeNull();
//            db.int
//        }



//        private class TestClass1 : CollectionsDirBase
//        {
//            protected override ICollectionsDB ConnectToDB(DateTime date, string path)
//            {
//                throw new NotImplementedException();
//            }

//            protected override IEnumerable<DateTime> FindAllDates()
//            {
//                throw new NotImplementedException();
//            }

//            protected override bool TryFindDB(DateTime date, out string path)
//            {
//                throw new NotImplementedException();
//            }
//        }
//    }
//}
