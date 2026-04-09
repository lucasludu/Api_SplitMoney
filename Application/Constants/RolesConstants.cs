namespace Application.Constants
{
    public static class RolesConstants
    {
        public const string Admin = "Admin";
        public const string User = "User";
        public const string PremiumUser = "PremiumUser";
        public const string Guest = "Guest";

        public static List<string> ValidRoles => new List<string>
        {
            Admin,
            User,
            PremiumUser,
            Guest
        };
    }
}
