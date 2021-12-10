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

        // Product
        public const string Product_View = "Product_View";
        public const string Product_Create = "Product_Create";
        public const string Product_Modify = "Product_Modify";
        public const string Product_Delete = "Product_Delete";

        // Product Category 
        public const string ProductCategory_View = "ProductCategory_View";
        public const string ProductCategory_Create = "ProductCategory_Create";
        public const string ProductCategory_Modify = "ProductCategory_Modify";
        public const string ProductCategory_Delete = "ProductCategory_Delete";
        
        // Inventory
        public const string Inventory_View = "Inventory_View";
        public const string Inventory_Create = "Inventory_Create";
        public const string Inventory_Modify = "Inventory_Modify";
        public const string Inventory_Delete = "Inventory_Delete";

        // ShipmentOrder 
        public const string Order_Shipment_View = "Order_Shipment_View";
        public const string Order_Shipment_Create = "Order_Shipment_Create";
        public const string Order_Shipment_Modify = "Order_Shipment_Modify";
        public const string Order_Shipment_Delete = "Order_Shipment_Delete";

        // ReturnShipmentOrder 
        public const string Order_ReturnShipment_View = "Order_ReturnShipment_View";
        public const string Order_ReturnShipment_Create = "Order_ReturnShipment_Create";
        public const string Order_ReturnShipment_Modify = "Order_ReturnShipment_Modify";
        public const string Order_ReturnShipment_Delete = "Order_ReturnShipment_Delete";
    }
}
