USE [ASPNET-SPORTEVENTSAPP-20180106061602]
GO
SET IDENTITY_INSERT [dbo].[NestedInfoes] ON 

INSERT [dbo].[NestedInfoes] ([Id], [Header]) VALUES (3, N'Tournament Systems')
SET IDENTITY_INSERT [dbo].[NestedInfoes] OFF
SET IDENTITY_INSERT [dbo].[RegularInfoes] ON 

INSERT [dbo].[RegularInfoes] ([Id], [Title], [NestedId]) VALUES (2, N'Regulations', NULL)
INSERT [dbo].[RegularInfoes] ([Id], [Title], [NestedId]) VALUES (3, N'Payment Method', NULL)
INSERT [dbo].[RegularInfoes] ([Id], [Title], [NestedId]) VALUES (4, N'Royal Tournament  [250 EGP]', 3)
INSERT [dbo].[RegularInfoes] ([Id], [Title], [NestedId]) VALUES (5, N'Fishermen Tournament [200 EGP]', 3)
INSERT [dbo].[RegularInfoes] ([Id], [Title], [NestedId]) VALUES (6, N'Underdogs Tournament [150 EGP]', 3)
SET IDENTITY_INSERT [dbo].[RegularInfoes] OFF
SET IDENTITY_INSERT [dbo].[InfoPoints] ON 

INSERT [dbo].[InfoPoints] ([Id], [Value], [InfoId]) VALUES (2, N'you must be present at our booth in front of your event location 30 minutes before event time.', 2)
INSERT [dbo].[InfoPoints] ([Id], [Value], [InfoId]) VALUES (3, N'Time is Money, Don''t waste it.', 2)
INSERT [dbo].[InfoPoints] ([Id], [Value], [InfoId]) VALUES (4, N'Any Friends coming with you are your responsibility.', 2)
INSERT [dbo].[InfoPoints] ([Id], [Value], [InfoId]) VALUES (5, N'cursing or fighting is forbidden, Handle the pressure.', 2)
INSERT [dbo].[InfoPoints] ([Id], [Value], [InfoId]) VALUES (6, N'spend your prize wisely!', 2)
INSERT [dbo].[InfoPoints] ([Id], [Value], [InfoId]) VALUES (7, N'Vodafone Cash & Etisalat Cash', 3)
INSERT [dbo].[InfoPoints] ([Id], [Value], [InfoId]) VALUES (8, N'Registeration with full name and phone number on the requested event', 3)
INSERT [dbo].[InfoPoints] ([Id], [Value], [InfoId]) VALUES (9, N'According to your number if Vodafone you will get Vodafone phone number & if Etisalat you will get Etisalat phone number', 3)
INSERT [dbo].[InfoPoints] ([Id], [Value], [InfoId]) VALUES (10, N'reach the nearest branch of Vodafone or Etisalat to transfer Entry Fees on the requested Number', 3)
INSERT [dbo].[InfoPoints] ([Id], [Value], [InfoId]) VALUES (11, N'your status on App will be updated shortly including the requested event , location , time , group number', 3)
INSERT [dbo].[InfoPoints] ([Id], [Value], [InfoId]) VALUES (12, N'128 Players distributed on 16 groups.', 4)
INSERT [dbo].[InfoPoints] ([Id], [Value], [InfoId]) VALUES (13, N'Each group have 8 players.', 4)
INSERT [dbo].[InfoPoints] ([Id], [Value], [InfoId]) VALUES (14, N'Every player will play 7 matches with group members, 1st and 2nd place will qualify to the knockout stage.', 4)
INSERT [dbo].[InfoPoints] ([Id], [Value], [InfoId]) VALUES (15, N'32 players qualified will play knockout stage until the semi final, 3rd place & the FINAL.', 4)
INSERT [dbo].[InfoPoints] ([Id], [Value], [InfoId]) VALUES (16, N'64 players distributed on 8 groups.', 5)
INSERT [dbo].[InfoPoints] ([Id], [Value], [InfoId]) VALUES (17, N'Each group contain 8 players.', 5)
INSERT [dbo].[InfoPoints] ([Id], [Value], [InfoId]) VALUES (18, N'Every player will play 7 matches with group members, 1st and 2nd place will qualify to the knockout stage', 5)
INSERT [dbo].[InfoPoints] ([Id], [Value], [InfoId]) VALUES (19, N'32 players distributed on 4 groups', 6)
INSERT [dbo].[InfoPoints] ([Id], [Value], [InfoId]) VALUES (20, N'Each group contain 8 players', 6)
INSERT [dbo].[InfoPoints] ([Id], [Value], [InfoId]) VALUES (21, N'Each player will play 7 matches with group members, 1st and 2nd place will qualify to the knockout stage', 6)
SET IDENTITY_INSERT [dbo].[InfoPoints] OFF
SET IDENTITY_INSERT [dbo].[EntryFees] ON 

INSERT [dbo].[EntryFees] ([Id], [Name], [Value]) VALUES (1, N'25', 25)
INSERT [dbo].[EntryFees] ([Id], [Name], [Value]) VALUES (2, N'50', 50)
INSERT [dbo].[EntryFees] ([Id], [Name], [Value]) VALUES (3, N'75', 75)
INSERT [dbo].[EntryFees] ([Id], [Name], [Value]) VALUES (4, N'100', 100)
SET IDENTITY_INSERT [dbo].[EntryFees] OFF
