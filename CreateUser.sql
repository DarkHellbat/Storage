USE [DocStorage]
GO
DECLARE @HashThis nvarchar(32);  
SET @HashThis = CONVERT(nvarchar(32),'dslfdkjLK85kldhnv$n000#knf');  
insert into [User] (UserName, Email, PasswordHash) values ('defaultUser', 'default.test.org', (SELECT HASHBYTES('SHA2_256', @HashThis))) --c хэшированием пароля
insert into [User] (UserName, Email, PasswordHash) values ('defaultUser2', 'default2.test.org', 'ABlZ35lIoEAaGlp+URrctF2K08bSR8I1iZNHaHnGdEBznMOFId6tRmfo185e4pbXOQ==')