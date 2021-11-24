namespace OrderSystem.Models
{
    public static class Constant
    {
        /// <summary>
        /// ProductInventoryChangeCode
        /// </summary>
        public static class ProductInventoryChangeCode
        {
            public static string Create = "商品建立初始化";
            public static string ManualModify= "手動異動商品數量";
            public static string ShipmentOrder = "出貨訂單";

        }

        /// <summary>
        /// OrderType
        /// </summary>
        public static class OrderType
        {
            public static int Shipment = 1;
            public static int Return =2;
        }
        /// <summary>
        /// OrderStatus
        /// </summary>
        public static class OrderStatus
        {
            public static int InProgress = 1;
            public static int Return = 2;
            public static int Completed = 3;
        }
    }
}
