using CommonTools.Lib11.GoogleTools;
using CommonTools.Lib11.StringTools;
using System;

namespace RentLog.DomainLib11.Authorization
{
    public static class AccessControlExtensions
    {
        public static Action<string> OnUnauthorizedAccess;


        public static bool CanForceLeaseBalanceUpdate(this ICredentialsProvider creds, bool alertIfNotAllowed) => creds.Check(alertIfNotAllowed,
            "Force Lease Balance Update", "Supervisor", "Admin");


        public static bool CanEditPostedPRNumber(this ICredentialsProvider creds, bool alertIfNotAllowed) => creds.Check(alertIfNotAllowed,
            "Edit posted PR #", "Supervisor", "Admin");


        public static bool CanAddJournalVoucher(this ICredentialsProvider creds, bool alertIfNotAllowed) => creds.Check(alertIfNotAllowed,
            "Create Journal Voucher", "Supervisor", "Admin");

        public static bool CanEditJournalVoucher(this ICredentialsProvider creds, bool alertIfNotAllowed) => creds.Check(alertIfNotAllowed,
            "Edit Journal Voucher", "Supervisor", "Admin");

        public static bool CanDeleteJournalVoucher(this ICredentialsProvider creds, bool alertIfNotAllowed) => creds.Check(alertIfNotAllowed,
            "Delete Journal Voucher", "Supervisor", "Admin");



        public static bool CanAddPassbookRow(this ICredentialsProvider creds, bool alertIfNotAllowed) => creds.Check(alertIfNotAllowed,
            "Create Passbook Row", "Cashier", "Supervisor", "Admin");

        public static bool CanEditPassbookRow(this ICredentialsProvider creds, bool alertIfNotAllowed) => creds.Check(alertIfNotAllowed,
            "Edit Passbook Row", "Cashier", "Supervisor", "Admin");

        public static bool CanDeletePassbookRow(this ICredentialsProvider creds, bool alertIfNotAllowed) => creds.Check(alertIfNotAllowed,
            "Delete Passbook Row", "Cashier", "Supervisor", "Admin");

        public static bool CanDeleteSystemGeneratedPassbookRow(this ICredentialsProvider creds, bool alertIfNotAllowed) => creds.Check(alertIfNotAllowed,
            "Delete System-generated Passbook Row", "Supervisor", "Admin");



        public static bool CanAddVoucherRequest(this ICredentialsProvider creds, bool alertIfNotAllowed) => creds.Check(alertIfNotAllowed,
            "Create Voucher Request", "Supervisor", "Admin");

        public static bool CanDeleteVoucherRequest(this ICredentialsProvider creds, bool alertIfNotAllowed) => creds.Check(alertIfNotAllowed,
            "Delete Voucher Request", "Admin");

        public static bool CanInputChequeDetails(this ICredentialsProvider creds, bool alertIfNotAllowed) => creds.Check(alertIfNotAllowed,
            "Input Cheque Details", "Supervisor", "Admin");

        public static bool CanDeletePreparedCheque(this ICredentialsProvider creds, bool alertIfNotAllowed) => creds.Check(alertIfNotAllowed,
            "Delete Prepared Cheque", "Admin");

        public static bool CanIssueChequeToPayee(this ICredentialsProvider creds, bool alertIfNotAllowed) => creds.Check(alertIfNotAllowed,
            "Issue Cheque to Payee", "Supervisor", "Admin");

        public static bool CanTakeBackIssuedCheque(this ICredentialsProvider creds, bool alertIfNotAllowed) => creds.Check(alertIfNotAllowed,
            "Take back Issued Cheque", "Cashier", "Supervisor", "Admin");

        public static bool CanMarkChequeAsCleared(this ICredentialsProvider creds, bool alertIfNotAllowed) => creds.Check(alertIfNotAllowed,
            "Mark Cheque as “Cleared”", "Cashier", "Supervisor", "Admin");

        public static bool CanMarkChequeAsCancelled(this ICredentialsProvider creds, bool alertIfNotAllowed) => creds.Check(alertIfNotAllowed,
            "Mark Cheque as “Cancelled”", "Supervisor", "Admin");

        public static bool CanEditClearedDate(this ICredentialsProvider creds, bool alertIfNotAllowed) => creds.Check(alertIfNotAllowed,
            "Edit “Cleared” date", "Cashier", "Supervisor", "Admin");

        public static bool CanEditInactiveRequest(this ICredentialsProvider creds, bool alertIfNotAllowed) => creds.Check(alertIfNotAllowed,
            "Edit Inactive Request", "Supervisor", "Admin");



        public static bool CanPostAndClose(this ICredentialsProvider creds, bool alertIfNotAllowed) => creds.Check(alertIfNotAllowed,
            "Post & Close Market Day", "Admin", "Supervisor");

        public static bool CanEncodeCollections(this ICredentialsProvider creds, bool alertIfNotAllowed) => creds.Check(alertIfNotAllowed,
            "Encode Collections", "Cashier");



        public static bool CanAddLease(this ICredentialsProvider creds, bool alertIfNotAllowed) => creds.Check(alertIfNotAllowed,
            "Add Lease", "Supervisor", "Admin");

        public static bool CanEditLease(this ICredentialsProvider creds, bool alertIfNotAllowed) => creds.Check(alertIfNotAllowed,
            "Edit Lease", "Admin");

        public static bool CanEditTenantInfo(this ICredentialsProvider creds, bool alertIfNotAllowed) => creds.Check(alertIfNotAllowed,
            "Edit Tenant Info", "Supervisor", "Admin");

        public static bool CanTerminateteLease(this ICredentialsProvider creds, bool alertIfNotAllowed) => creds.Check(alertIfNotAllowed,
            "Terminate Lease", "Supervisor", "Admin");



        public static bool CanAddStall(this ICredentialsProvider creds, bool alertIfNotAllowed) => creds.Check(alertIfNotAllowed,
            "Add Stall", "Supervisor", "Admin");

        public static bool CanEditStall(this ICredentialsProvider creds, bool alertIfNotAllowed) => creds.Check(alertIfNotAllowed,
            "Edit Stall", "Supervisor", "Admin");

        public static bool CanDeleteStall(this ICredentialsProvider creds, bool alertIfNotAllowed) => creds.Check(alertIfNotAllowed,
            "Delete Stall", "Supervisor", "Admin");



        public static bool CanAddSection(this ICredentialsProvider creds, bool alertIfNotAllowed) => creds.Check(alertIfNotAllowed,
            "Add Section", "Supervisor", "Admin");

        public static bool CanEditSection(this ICredentialsProvider creds, bool alertIfNotAllowed) => creds.Check(alertIfNotAllowed,
            "Edit Section", "Supervisor", "Admin");



        public static bool CanRunAdHocTask(this ICredentialsProvider creds, bool alertIfNotAllowed) => creds.Check(alertIfNotAllowed,
            "Run Ad Hoc Job", "Admin");



        private static bool Check(this ICredentialsProvider args,
            bool alertIfNotAllowed, string intendedAction, params string[] allowedRoles)
        {
            if (args == null) return false;
            if (!args.IsValidUser) return false;
            if (args.Credentials == null) return false;

            var creds = args.Credentials;
            if (creds.Roles.IsBlank()) return false;

            foreach (var role in allowedRoles)
                if (creds.Roles.Contains(role)) return true;

            if (alertIfNotAllowed)
                OnUnauthorizedAccess?.Invoke($"“{creds.HumanName}” ({creds.Roles}) {L.f} is not allowed to {L.f} “{intendedAction}”.");

            return false;
        }
    }
}
