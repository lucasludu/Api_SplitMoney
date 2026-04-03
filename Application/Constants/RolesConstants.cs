namespace Application.Constants
{
    public static class RolesConstants
    {
        public const string Admin = "Admin";
        public const string User = "User";
        public const string PremiumUser = "PremiumUser";

        public static List<string> ValidRoles => new List<string>
        {
            Admin,
            User,
            PremiumUser
        };
    }
}
