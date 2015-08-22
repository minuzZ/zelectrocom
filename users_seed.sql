USE [zelectroCom]
GO
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 08/20/2015 22:29:52 ******/
INSERT [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName], [Discriminator]) VALUES (N'2a440efe-e6cf-426e-9710-562fe3e6603c', NULL, 0, N'AITG0ocprwKapaG2XwQbWjFMx62Z4PCWSV4wtSjKBv0auDT2MCsZAoa+WBmYNa/zgg==', N'3e61e1c9-6e69-4e6b-a644-b596e73af523', NULL, 0, 0, NULL, 0, 0, N'Admin', N'ApplicationUser')
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 08/20/2015 22:29:52 ******/
INSERT [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'd10a0083-4fe5-4dcc-993c-681b8a99f87e', N'Admin')
INSERT [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'c695f27f-4df7-439b-93d0-00e3b4d2b8be', N'Member')
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 08/20/2015 22:29:52 ******/
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'2a440efe-e6cf-426e-9710-562fe3e6603c', N'c695f27f-4df7-439b-93d0-00e3b4d2b8be')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'2a440efe-e6cf-426e-9710-562fe3e6603c', N'd10a0083-4fe5-4dcc-993c-681b8a99f87e')
