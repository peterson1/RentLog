using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib11.ExceptionTools;
using System;
using System.Linq;

namespace RentLog.DomainLib11.Validations
{
    internal static class SharedValidations
    {
        internal static void RejectDuplicateRecord<T>(this ISimpleRepo<T> repo, Func<T, bool> predicate, string field, T record)
            //where T : IDocumentDTO
        {
            var matches = repo.GetAll().Where(predicate);

            if (matches.Any())
                throw DuplicateRecordsException
                    .For(matches, field, record.ToString());
        }
    }
}
