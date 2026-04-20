namespace Daco.Domain.Administration.Constants
{
    public static class AdminPermissions
    {
        public static class Users
        {
            public const string View     = "users.view";
            public const string Suspend  = "users.suspend";
            public const string Ban      = "users.ban";
            public const string Activate = "users.activate";
        }

        public static class Admins
        {
            public const string View       = "admins.view";
            public const string Create     = "admins.create";
            public const string Update     = "admins.update";
            public const string AssignRole = "admins.assign_role";
        }

        public static class Sellers
        {
            public const string View      = "sellers.view";
            public const string Approve   = "sellers.approve";
            public const string Reject    = "sellers.reject";
            public const string Suspend   = "sellers.suspend";
            public const string Reinstate = "sellers.reinstate";
        }

        public static class Products
        {
            public const string View       = "products.view";
            public const string Approve    = "products.approve";
            public const string Suspend    = "products.suspend";
            public const string UnSuspend  = "products.unsuspend";
            public const string Remove     = "products.remove";
        }

        public static class Orders
        {
            public const string View   = "orders.view";
            public const string Cancel = "orders.cancel";
            public const string Refund = "orders.refund";
        }

        public static class Withdrawals
        {
            public const string View    = "withdrawals.view";
            public const string Approve = "withdrawals.approve";
            public const string Reject  = "withdrawals.reject";
        }

        public static class Violations
        {
            public const string View    = "violations.view";
            public const string Handle  = "violations.handle";
            public const string Penalty = "violations.penalty";
        }

        public static class Content
        {
            public const string Banners = "content.banners";
            public const string Pages   = "content.pages";
        }

        public static class Analytics
        {
            public const string View = "analytics.view";
        }

        public static class System
        {
            public const string Configs   = "system.configs";
            public const string AuditLogs = "system.audit_logs";
        }
    }
}
