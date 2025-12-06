namespace CoreLayer
{
    public class SD
    {
        public const string SuperAdmin = "Super Admin";
        public const string StockManager = "Stock Manager";
        public const string BranchManager = "Branch Manager";
        public const string Cashier = "Cashier";
        public const string Workers = "Workers";
        public const string Managers = "Managers";
        public static readonly List<string> AllRoles = new List<string>()
        {
            SuperAdmin,
            StockManager,
            BranchManager,
            Cashier
        };
    }
}
