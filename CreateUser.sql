use DocStorage
go
CREATE PROCEDURE [dbo].[sp_InsertFile]
    @Name nvarchar(9223372036854775807),
    @Type nvarchar(50)
	@CreationDate DateTime2
	@Author_id bigint
	@Path 
AS
    INSERT INTO Users (Name, Age)
    VALUES (@name, @age)
  
    SELECT SCOPE_IDENTITY()
GO