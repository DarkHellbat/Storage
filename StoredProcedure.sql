use DocStorage
go
CREATE PROCEDURE [dbo].[sp_InsertFile]

    @Name nvarchar(100),
    @Type nvarchar(50),
	@CreationDate DateTime2,
	@Author_id bigint,
	@Path nvarchar(255)
AS
    INSERT INTO [File] (Name, Type, CreationDate, Author_id, Path )
    VALUES ( @Name,
    @Type,
	@CreationDate,
	@Author_id,
	@Path)
  
    SELECT SCOPE_IDENTITY()
GO