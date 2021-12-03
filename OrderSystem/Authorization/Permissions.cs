namespace OrderSystem.Authorization
{
    /// <summary>
    /// permissions code
    /// </summary>
    public static class Permissions
    {
        // default permission 
        public const string Default_Login = "Default_Login";

        // Basic UserManagement
        public const string Basic_UserManagement_View = "Basic_UserManagement_View";
        public const string Basic_UserManagement_Create = "Basic_UserManagement_Create";
        public const string Basic_UserManagement_Modify = "Basic_UserManagement_Modify";
        public const string Basic_UserManagement_Delete = "Basic_UserManagement_Delete";

        // Basic Permission
        public const string Basic_Permission_View = "Basic_Permission_View";
        public const string Basic_Permission_Create = "Basic_Permission_Create";
        public const string Basic_Permission_Modify = "Basic_Permission_Modify";
        public const string Basic_Permission_Delete = "Basic_Permission_Delete";

    }
}
