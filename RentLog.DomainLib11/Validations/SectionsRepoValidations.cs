using CommonTools.Lib11.ExceptionTools;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Repositories;
using System.Linq;

namespace RentLog.DomainLib11.Validations
{
    internal static class SectionsRepoValidations
    {
        //internal static void RejectDuplicateName(this ISectionsRepo repo, SectionDTO newRecord)
        //{
        //    var matches = repo.GetAll().Where(_ => _.Name == newRecord.Name);

        //    if (matches.Any())
        //        throw DuplicateRecordsException.For(matches, nameof(newRecord.Name), newRecord.Name);
        //}
    }
}
