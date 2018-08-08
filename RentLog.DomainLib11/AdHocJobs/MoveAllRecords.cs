using RentLog.DomainLib11.DataSources;

namespace RentLog.DomainLib11.AdHocJobs
{
    public class MoveAllRecords
    {
        public static void ToNewInactiveRequests(ITenantDBsDir dir)
        {
            var oldRepo = dir.Vouchers.InactiveRequests_old;
            var oldRecs = oldRepo.GetAll();
            var newRepo = dir.Vouchers.InactiveRequests_new;
            newRepo.Insert(oldRecs, false);
            oldRepo.Drop();
        }
    }
}
