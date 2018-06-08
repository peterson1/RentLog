using CommonTools.Lib11.GoogleTools;
using CommonTools.Lib11.StringTools;
using System;

namespace RentLog.DomainLib11.Authorization
{
    public static class AccessControlExtensions
    {
        public static Action<string> OnUnauthorizedAccess;


        public static bool CanInputChequeDetails(this ICredentialsProvider creds, bool alertIfNotAllowed) => creds.Check(alertIfNotAllowed,
            "Input Cheque Details", "Admin", "Supervisor");



        public static bool CanPostAndClose(this ICredentialsProvider creds, bool alertIfNotAllowed) => creds.Check(alertIfNotAllowed,
            "Post & Close Market Day", "Admin", "Supervisor");

        public static bool CanEncodeCollections(this ICredentialsProvider creds, bool alertIfNotAllowed) => creds.Check(alertIfNotAllowed,
            "Encode Collections", "Cashier");



        public static bool CanAddLease(this ICredentialsProvider creds, bool alertIfNotAllowed) => creds.Check(alertIfNotAllowed,
            "Add Lease", "Supervisor", "Admin");

        public static bool CanEditLease(this ICredentialsProvider creds, bool alertIfNotAllowed) => creds.Check(alertIfNotAllowed,
            "Edit Lease", "Admin");

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
