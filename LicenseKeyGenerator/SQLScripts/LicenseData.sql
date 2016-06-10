
USE [License]
GO

SET IDENTITY_INSERT [dbo].[Users] ON
INSERT [dbo].[Users] ([ID], [LoginName], [Password], [FullName], [Email], [Status], [UserType], [SecurityQuestion1], [SecurityQuestion1Answer], 
[SecurityQuestion2], [SecurityQuestion2Answer]) VALUES (1, N'admin', N'admin1', N'VitalSigns Administrator', N'youremail@acme.com', N'Active', N'Admin',
 NULL, NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[Users] OFF
--VSPLUS 1756  Durga 
USE [License]
GO
SET IDENTITY_INSERT [dbo].[ServerTypes] ON 
INSERT [dbo].[ServerTypes] ([ID], [ServerType], [ServerTypeTable], [FeatureId], [UnitCost]) VALUES (1, N'Domino', N'DominoServers', 1, 100)
INSERT [dbo].[ServerTypes] ([ID], [ServerType], [ServerTypeTable], [FeatureId], [UnitCost]) VALUES (2, N'BES', N'', 3, 0)
INSERT [dbo].[ServerTypes] ([ID], [ServerType], [ServerTypeTable], [FeatureId], [UnitCost]) VALUES (3, N'Sametime', N'SametimeServers', 6, 0)
INSERT [dbo].[ServerTypes] ([ID], [ServerType], [ServerTypeTable], [FeatureId], [UnitCost]) VALUES (4, N'SharePoint', N'MailServices', 5, 50)
INSERT [dbo].[ServerTypes] ([ID], [ServerType], [ServerTypeTable], [FeatureId], [UnitCost]) VALUES (5, N'Exchange', N'ExchangeSettings', 2, 33)
INSERT [dbo].[ServerTypes] ([ID], [ServerType], [ServerTypeTable], [FeatureId], [UnitCost]) VALUES (6, N'Mail', N'', 4, 0)
INSERT [dbo].[ServerTypes] ([ID], [ServerType], [ServerTypeTable], [FeatureId], [UnitCost]) VALUES (7, N'URL', N'', 8, 0)
INSERT [dbo].[ServerTypes] ([ID], [ServerType], [ServerTypeTable], [FeatureId], [UnitCost]) VALUES (8, N'Network Device', N'Network Devices', 10, 0)
INSERT [dbo].[ServerTypes] ([ID], [ServerType], [ServerTypeTable], [FeatureId], [UnitCost]) VALUES (9, N'Notes Database', N'NotesDatabases', 1, 0)
INSERT [dbo].[ServerTypes] ([ID], [ServerType], [ServerTypeTable], [FeatureId], [UnitCost]) VALUES (10, N'General', N'ServerAttributes', 11, 0)
INSERT [dbo].[ServerTypes] ([ID], [ServerType], [ServerTypeTable], [FeatureId], [UnitCost]) VALUES (11, N'Mobile Users', N'Traveler_Devices', 9, 0)
INSERT [dbo].[ServerTypes] ([ID], [ServerType], [ServerTypeTable], [FeatureId], [UnitCost]) VALUES (12, N'Domino Cluster database', N'', 1, 0)
INSERT [dbo].[ServerTypes] ([ID], [ServerType], [ServerTypeTable], [FeatureId], [UnitCost]) VALUES (13, N'NotesMail Probe', N'', 4, 0)
INSERT [dbo].[ServerTypes] ([ID], [ServerType], [ServerTypeTable], [FeatureId], [UnitCost]) VALUES (14, N'Exchange Mail Flow', N'', 2, 0)
INSERT [dbo].[ServerTypes] ([ID], [ServerType], [ServerTypeTable], [FeatureId], [UnitCost]) VALUES (15, N'Lync', N'', 13, 33)
INSERT [dbo].[ServerTypes] ([ID], [ServerType], [ServerTypeTable], [FeatureId], [UnitCost]) VALUES (16, N'Windows', N'', 14, 33)
INSERT [dbo].[ServerTypes] ([ID], [ServerType], [ServerTypeTable], [FeatureId], [UnitCost]) VALUES (17, N'Cloud', N'', 15, 17)
INSERT [dbo].[ServerTypes] ([ID], [ServerType], [ServerTypeTable], [FeatureId], [UnitCost]) VALUES (18, N'Active Directory', N'', 16, 17)
INSERT [dbo].[ServerTypes] ([ID], [ServerType], [ServerTypeTable], [FeatureId], [UnitCost]) VALUES (19, N'Database Availability Group', N'DagSettings', 2, 0)
INSERT [dbo].[ServerTypes] ([ID], [ServerType], [ServerTypeTable], [FeatureId], [UnitCost]) VALUES (20, N'SNMP Devices', N'SNMP', 10, 13)
INSERT [dbo].[ServerTypes] ([ID], [ServerType], [ServerTypeTable], [FeatureId], [UnitCost]) VALUES (21, N'Office365', N'', 15, 0)
INSERT [dbo].[ServerTypes] ([ID], [ServerType], [ServerTypeTable], [FeatureId], [UnitCost]) VALUES (22, N'Websphere Server', N'Websphereserver', 11, 133)
INSERT [dbo].[ServerTypes] ([ID], [ServerType], [ServerTypeTable], [FeatureId], [UnitCost]) VALUES (23, N'Network Latency', N'NetworkLatency', NULL, 0)
INSERT [dbo].[ServerTypes] ([ID], [ServerType], [ServerTypeTable], [FeatureId], [UnitCost]) VALUES (24, N'Notes Traveler', N'Traveler_HA_Datastore', 1, 0)
INSERT [dbo].[ServerTypes] ([ID], [ServerType], [ServerTypeTable], [FeatureId], [UnitCost]) VALUES (25, N'Domino Cluster', N'DominoCluster', 1, 0)
SET IDENTITY_INSERT [dbo].[ServerTypes] OFF

