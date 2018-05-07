using CommonTools.Lib11.ExceptionTools;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Repositories;
using System.Linq;

namespace RentLog.DomainLib11.Validations
{
    internal static class StallsRepoValidations
    {
        internal static void RejectDuplicateName(this IStallsRepo repo, StallDTO newRecord)
        {
            var matches = repo.GetAll().Where(_ => _.Name == newRecord.Name);

            if (matches.Any())
                throw DuplicateRecordsException.For(matches, nameof(newRecord.Name), newRecord.Name);
        }


        internal static void DontDeleteIfOccupied(this IStallsRepo repo, StallDTO stall, ILeasesRepo activeLses)
        {
            var lse = activeLses.GetAll().SingleOrDefault(_ => _.Stall.Id == stall.Id);
            if (lse != null)
                throw InvalidDeletionException.For(stall, $"Occupied by {lse}");
        }
    }
}
