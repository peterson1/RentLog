using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib11.ExceptionTools;
using System;
using System.Linq;

namespace RentLog.DomainLib11.Validations
{
    internal static class SharedValidations
    {
        internal static void RejectDuplicateRecord<T>(this ISimpleRepo<T> repo, Func<T, bool> predicate, string field, T record, Func<T, int> idGetter = null)
            //where T : IDocumentDTO
        {
            var matches = repo.GetAll().Where(predicate);
            if (!matches.Any()) return;

            if (matches.Count() > 1)
                throw DuplicateRecordsException
                    .For(matches, field, record.ToString());

            if (matches.Count() == 1 && idGetter != null)
            {
                var recId   = idGetter(record);
                var matchId = idGetter(matches.Single());
                if (recId == matchId) return;
            }

            throw DuplicateRecordsException
                .For(matches, field, record.ToString());
        }
    }
}
