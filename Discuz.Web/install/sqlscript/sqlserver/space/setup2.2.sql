CREATE  PROCEDURE [dnt_createspaceposttags]
@tags nvarchar(55),
@postid int,
@userid int,
@postdatetime datetime
AS
BEGIN
	exec [dnt_createtags] @tags, @userid, @postdatetime

	UPDATE [dnt_tags] SET [scount]=[scount]+1,[count]=[count]+1
	WHERE EXISTS (SELECT [item] FROM [dnt_split](@tags, ' ') AS [newtags] WHERE [newtags].[item] = [tagname])

	INSERT INTO [dnt_spaceposttags] (tagid, spacepostid)
	SELECT tagid, @postid FROM [dnt_tags] WHERE EXISTS (SELECT * FROM [dnt_split](@tags, ' ') WHERE [item] = [dnt_tags].[tagname])
END
GO

CREATE PROCEDURE dnt_deletespace
	@uid int
AS

DELETE FROM [dnt_spacealbums] WHERE userid=@uid
DELETE FROM [dnt_spaceattachments] WHERE uid=@uid
DELETE FROM [dnt_spacecategorys] WHERE uid=@uid
DELETE FROM [dnt_spacecomments] WHERE uid=@uid
DELETE FROM [dnt_spaceconfigs] WHERE userid=@uid
DELETE FROM [dnt_spacelinks] WHERE userid=@uid
DELETE FROM [dnt_spacemodules] WHERE uid=@uid
DELETE FROM [dnt_spacephotos] WHERE userid=@uid
DELETE FROM [dnt_spaceposts] WHERE uid=@uid
DELETE FROM [dnt_spacetabs] WHERE uid=@uid
GO

CREATE PROCEDURE [dnt_deletespaceposttags]
	@spacepostid int
 AS
BEGIN       
	UPDATE [dnt_tags] SET [count]=[count]-1,[scount]=[scount]-1 
	WHERE EXISTS (SELECT [tagid] FROM [dnt_spaceposttags] WHERE [spacepostid] = @spacepostid AND [tagid] = [dnt_tags].[tagid])

    DELETE FROM [dnt_spaceposttags] WHERE [spacepostid] = @spacepostid	
END
GO

CREATE PROCEDURE [dnt_getspacepostlistbytag]	
	@tagid int,
	@pageindex int,
	@pagesize int
AS
BEGIN
	DECLARE @strSQL varchar(2000)
	IF @pageindex = 1
	BEGIN
		SET @strSQL='SELECT TOP ' + STR(@pagesize) + ' [sp].[postid], [sp].[title],[sp].[author],[sp].[uid], [sp].[postdatetime],[sp].[commentcount],[sp].[views]  
		FROM [dnt_spaceposttags] AS [spt], [dnt_spaceposts] AS [sp] 
		WHERE [sp].[postid] = [spt].[spacepostid] AND [sp].[poststatus] = 1 AND [spt].[tagid] = ' + STR(@tagid) + ' 
		ORDER BY [sp].[postid] DESC'
	END
	ELSE
	BEGIN
		SET @strSQL='SELECT TOP ' + STR(@pagesize) + ' [sp].[postid], [sp].[title],[sp].[author],[sp].[uid], [sp].[postdatetime],[sp].[commentcount],[sp].[views] 
		FROM [dnt_spaceposttags] AS [spt], [dnt_spaceposts] AS [sp] 
		WHERE [sp].[postid] = [spt].[spacepostid] AND [sp].[poststatus] = 1 AND [spt].[tagid] = ' + STR(@tagid) + ' 
		AND [sp].[postid] < (SELECT MIN([postid]) FROM (SELECT TOP ' + STR((@pageindex-1)*@pagesize) + ' 
		[postid] FROM [dnt_spaceposttags] AS [spt], [dnt_spaceposts] AS [sp] 
		WHERE [sp].[postid] = [spt].[spacepostid] AND [sp].[poststatus] = 1 AND [spt].[tagid] = ' + STR(@tagid) + ' 
		ORDER BY [sp].[postid] DESC) as tblTmp) 
		ORDER BY [sp].[postid] DESC'
	END
	EXEC(@strSQL)
END
GO

CREATE PROCEDURE [dnt_getfavoriteslistbyspacepost]
@uid int,
@pagesize int,
@pageindex int
AS
DECLARE @strSQL varchar(5000)


SET @strSQL='SELECT [f].[tid], [f].[uid], [f].[favtime] , [postid], [author] AS [poster], [spaceposts].[uid] AS [posterid], [postdatetime], [title], [category], [poststatus], [commentstatus], [postupdatetime], [commentcount], [views] FROM [dnt_favorites] [f],[dnt_spaceposts] [spaceposts] WHERE [f].[tid]=[spaceposts].[postid] AND [f].[typeid]=2 AND [f].[uid]=' + STR(@uid)

IF @pageindex = 1
	BEGIN
		SET @strSQL = 'SELECT TOP ' + STR(@pagesize) +'  [tid], [postid], [poster], [posterid], [uid], [postdatetime], [title], [category], [poststatus], [commentstatus], [postupdatetime], [commentcount], [views] ,[favtime]  FROM (' + @strSQL + ') f' + '  ORDER BY [tid] DESC'
	END
ELSE
	BEGIN
		SET @strSQL = 'SELECT TOP ' + STR(@pagesize) +'  [tid], [postid], [poster], [posterid], [uid], [postdatetime], [title], [category], [poststatus], [commentstatus], [postupdatetime], [commentcount], [views] ,[favtime] FROM (' + @strSQL + ') f1 WHERE [tid] < (SELECT MIN([tid]) FROM (SELECT TOP ' + STR((@pageindex-1)*@pagesize) + ' [tid] FROM (' + @strSQL + ') f2' + '  ORDER BY [tid] DESC) AS tblTmp)' + '  ORDER BY [tid] DESC'
	END
EXEC(@strSQL)
