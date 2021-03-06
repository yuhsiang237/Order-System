USE [OrderSystem]
GO
SET IDENTITY_INSERT [dbo].[Permissions] ON 

INSERT [dbo].[Permissions] ([Id], [RoleId], [Code]) VALUES (2252, 1, N'Basic_Permission_View')
INSERT [dbo].[Permissions] ([Id], [RoleId], [Code]) VALUES (2253, 1, N'ProductCategory_Modify')
INSERT [dbo].[Permissions] ([Id], [RoleId], [Code]) VALUES (2254, 1, N'Inventory_Create')
INSERT [dbo].[Permissions] ([Id], [RoleId], [Code]) VALUES (2255, 1, N'ProductCategory_Delete')
INSERT [dbo].[Permissions] ([Id], [RoleId], [Code]) VALUES (2256, 1, N'Basic_Permission_Create')
INSERT [dbo].[Permissions] ([Id], [RoleId], [Code]) VALUES (2257, 1, N'Inventory_Delete')
INSERT [dbo].[Permissions] ([Id], [RoleId], [Code]) VALUES (2258, 1, N'Inventory_Modify')
INSERT [dbo].[Permissions] ([Id], [RoleId], [Code]) VALUES (2259, 1, N'ProductCategory_Create')
INSERT [dbo].[Permissions] ([Id], [RoleId], [Code]) VALUES (2260, 1, N'Order_ReturnShipment_View')
INSERT [dbo].[Permissions] ([Id], [RoleId], [Code]) VALUES (2261, 1, N'Order_Shipment_View')
INSERT [dbo].[Permissions] ([Id], [RoleId], [Code]) VALUES (2262, 1, N'Order_ReturnShipment_Delete')
INSERT [dbo].[Permissions] ([Id], [RoleId], [Code]) VALUES (2263, 1, N'Product_Modify')
INSERT [dbo].[Permissions] ([Id], [RoleId], [Code]) VALUES (2264, 1, N'Product_Delete')
INSERT [dbo].[Permissions] ([Id], [RoleId], [Code]) VALUES (2265, 1, N'Product_View')
INSERT [dbo].[Permissions] ([Id], [RoleId], [Code]) VALUES (2266, 1, N'Product_Create')
INSERT [dbo].[Permissions] ([Id], [RoleId], [Code]) VALUES (2267, 1, N'ProductCategory_View')
INSERT [dbo].[Permissions] ([Id], [RoleId], [Code]) VALUES (2268, 1, N'Inventory_View')
INSERT [dbo].[Permissions] ([Id], [RoleId], [Code]) VALUES (2269, 1, N'Order_ReturnShipment_Modify')
INSERT [dbo].[Permissions] ([Id], [RoleId], [Code]) VALUES (2270, 1, N'Basic_Permission_Delete')
INSERT [dbo].[Permissions] ([Id], [RoleId], [Code]) VALUES (2271, 1, N'Basic_UserManagement_View')
INSERT [dbo].[Permissions] ([Id], [RoleId], [Code]) VALUES (2272, 1, N'Basic_UserManagement_Modify')
INSERT [dbo].[Permissions] ([Id], [RoleId], [Code]) VALUES (2273, 1, N'Basic_UserManagement_Delete')
INSERT [dbo].[Permissions] ([Id], [RoleId], [Code]) VALUES (2274, 1, N'Order_Shipment_Delete')
INSERT [dbo].[Permissions] ([Id], [RoleId], [Code]) VALUES (2275, 1, N'Order_ReturnShipment_Create')
INSERT [dbo].[Permissions] ([Id], [RoleId], [Code]) VALUES (2276, 1, N'Basic_Permission_Modify')
INSERT [dbo].[Permissions] ([Id], [RoleId], [Code]) VALUES (2277, 1, N'Basic_UserManagement_Create')
INSERT [dbo].[Permissions] ([Id], [RoleId], [Code]) VALUES (2278, 1, N'Order_Shipment_Create')
INSERT [dbo].[Permissions] ([Id], [RoleId], [Code]) VALUES (2279, 1, N'Order_Shipment_Modify')
SET IDENTITY_INSERT [dbo].[Permissions] OFF
GO
SET IDENTITY_INSERT [dbo].[Roles] ON 

INSERT [dbo].[Roles] ([Id], [Name], [CreatedAt], [UpdatedAt], [IsDeleted]) VALUES (1, N'一般使用者', NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[Roles] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([Id], [Name], [Salt], [Email], [Account], [Password], [RoleId], [CreateAt], [UpdatedAt], [IsDeleted]) VALUES (1, N'demouser', N'N1j*Do$NL>YPD<B', N'demouser', N'demouser', N'NVlVvtqmyafhTLuW0xs5D+Nb691hRbAfwCNbyGZ7xmVhEKGNpgHzbuPFm/XULaRUGc6AfwiA8oH+ACMJVhft+A==', 1, CAST(N'2021-12-26T03:00:45.6166667' AS DateTime2), CAST(N'2021-12-26T03:00:45.6166667' AS DateTime2), NULL)
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
