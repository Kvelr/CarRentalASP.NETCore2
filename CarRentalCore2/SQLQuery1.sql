/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP 100 u.FirstName ,u.LastName,roles.Name
FROM [CarRentalCore2].[dbo].[AspNetUsers] as u
inner join [CarRentalCore2].[dbo].[AspNetUserRoles] ur on ur.UserId = u.Id
inner join [CarRentalCore2].[dbo].[AspNetRoles] roles on roles.id = ur.RoleId
--where ur.RoleId = 'ed588b57-38ef-4d91-acfa-b8773eb96292' --user-- '271c3992-2df7-48ec-af35-c97b5b63f512'--Admin 

