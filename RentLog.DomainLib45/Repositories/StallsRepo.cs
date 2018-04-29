using CommonTools.Lib45.LiteDbTools;
using RentLog.DatabaseLib.StallsRepository;
using RentLog.DomainLib11.DTOs;
using System.Collections.Generic;

namespace RentLog.DomainLib45.Repositories
{
    public class StallsRepo
    {

        private StallsCollection _colxn;

        public StallsRepo(string dbFilePath, string currentUser)
        {
            _colxn = new StallsCollection(dbFilePath, currentUser);
        }


        public List<StallDTO> List() => _colxn.GetAll();
    }
}
