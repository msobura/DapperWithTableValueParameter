﻿CREATE PROCEDURE [dbo].[TestProcedure]
	@idList udttIdList READONLY
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		IL.Id
	FROM @idList AS IL
END