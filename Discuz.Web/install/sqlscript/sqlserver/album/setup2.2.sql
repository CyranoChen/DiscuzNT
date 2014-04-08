CREATE  PROCEDURE [dnt_createphototags]
@tags nvarchar(55),
@photoid int,
@userid int,
@postdatetime datetime
AS
BEGIN
	exec [dnt_createtags] @tags, @userid, @postdatetime

	UPDATE [dnt_tags] SET [pcount]=[pcount]+1,[count]=[count]+1
	WHERE EXISTS (SELECT [item] FROM [dnt_split](@tags, ' ') AS [newtags] WHERE [newtags].[item] = [tagname])
	
	INSERT INTO [dnt_phototags] (tagid, photoid)
	SELECT tagid, @photoid FROM [dnt_tags] WHERE EXISTS (SELECT * FROM [dnt_split](@tags, ' ') WHERE [item] = [dnt_tags].[tagname])
END
GO

CREATE PROCEDURE [dnt_deletephototags]
	@photoid int
 AS
BEGIN       
	UPDATE [dnt_tags] SET [count]=[count]-1,[fcount]=[fcount]-1 
	WHERE EXISTS (SELECT [tagid] FROM [dnt_phototags] WHERE [photoid] = @photoid AND [tagid] = [dnt_tags].[tagid])

    DELETE FROM [dnt_phototags] WHERE [photoid] = @photoid	
END
GO

CREATE PROCEDURE [dnt_getgoodslistByCid]
@categoryid int,
@pagesize int,
@pageindex int,
@condition varchar(500),
@orderby varchar(100),
@ascdesc int
AS

DECLARE @strSQL varchar(5000)
DECLARE @sorttype varchar(5)

IF @ascdesc=0
   SET @sorttype='ASC'
ELSE
   SET @sorttype='DESC'

IF @pageindex <= 1
	SET @strSQL = 'SELECT TOP '+STR(@pagesize)+' * FROM [dnt_goods] WHERE ([categoryid] = '+STR(@categoryid)+' OR CHARINDEX('',''+CAST('+STR(@categoryid)+' AS VARCHAR(10))+'','' , '',''+RTRIM([parentcategorylist])+'','')>0)  '+ @condition +' ORDER BY '+@orderby+' '+@sorttype
ELSE
	IF @sorttype = 'DESC'
		SET @strSQL = 'SELECT TOP '+STR(@pagesize)+' * FROM [dnt_goods] WHERE [goodsid] < (SELECT MIN([goodsid])  FROM (SELECT TOP '+ STR((@pageindex - 1) * @pagesize) + ' [goodsid] FROM [dnt_goods]  WHERE  ([categoryid] = '+STR(@categoryid)+' OR CHARINDEX('',''+CAST('+STR(@categoryid)+' AS VARCHAR(10))+'','' , '',''+RTRIM([parentcategorylist])+'','')>0)  '+ @condition +' ORDER BY '+@orderby+' '+@sorttype+') AS tblTmp ) AND ([categoryid] = '+STR(@categoryid)+' OR CHARINDEX('',''+CAST('+STR(@categoryid)+' AS VARCHAR(10))+'','' , '',''+RTRIM([parentcategorylist])+'','')>0) '+@condition+' ORDER BY '+@orderby+' '+@sorttype
	ELSE
		SET @strSQL = 'SELECT TOP '+STR(@pagesize)+' * FROM [dnt_goods] WHERE [goodsid] > (SELECT MAX([goodsid])  FROM (SELECT TOP '+ STR((@pageindex - 1) * @pagesize) + ' [goodsid] FROM [dnt_goods]  WHERE  ([categoryid] = '+STR(@categoryid)+' OR CHARINDEX('',''+CAST('+STR(@categoryid)+' AS VARCHAR(10))+'','' , '',''+RTRIM([parentcategorylist])+'','')>0) '+ @condition +' ORDER BY '+@orderby+' '+@sorttype+') AS tblTmp ) AND ([categoryid] = '+STR(@categoryid)+' OR CHARINDEX('',''+CAST('+STR(@categoryid)+' AS VARCHAR(10))+'','' , '',''+RTRIM([parentcategorylist])+'','')>0) '+@condition+' ORDER BY '+@orderby+' '+@sorttype	
EXEC(@strSQL)
GO


CREATE PROCEDURE dnt_getphotolist
@albumid int,
@pagesize int,
@pageindex int
AS
DECLARE @strSQL VARCHAR(5000)

IF @pageindex = 1
BEGIN
	SET @strSQL = 'SELECT TOP ' + STR(@pagesize) +' [photoid], [filename], [attachment], [filesize], [description], [postdate], [albumid], [userid] FROM [dnt_photos] WHERE [albumid] =' +STR(@albumid)
END
ELSE
BEGIN
	SET @strSQL = 'SELECT TOP ' + STR(@pagesize) +' [photoid], [filename], [attachment], [filesize], [description], [postdate], [albumid], [userid] FROM [dnt_photos] WHERE [photoid] < (SELECT MIN([photoid]) FROM (SELECT TOP ' + STR((@pageindex-1)*@pagesize) + ' [photoid] FROM [dnt_photos] WHERE [albumid] =' +STR(@albumid) + ') AS tblTmp) AND [albumid]=' +STR(@albumid)
END


EXEC(@strSQL)


GO


CREATE PROCEDURE dnt_getalbumlist
@uid int,
@pageindex int,
@pagesize int,
@albumcateid int
 AS
DECLARE @strSQL varchar(5000)
DECLARE @strWhere varchar(2000)
SET @strWhere = ' WHERE [imgcount]>0 '	
IF @albumcateid <> 0
BEGIN
	SET @strWhere = @strWhere +			' AND [albumcateid]=' + STR(@albumcateid)
END
IF @uid > 0
BEGIN
	SET @strWhere = @strWhere + 		' AND [userid]=' + STR(@uid)
END
IF @pageindex = 1
BEGIN
	SET @strSQL = 'SELECT * 
					 FROM [dnt_albums]  
					WHERE [albumid] IN (
							SELECT TOP ' + STR(@pagesize) + ' [albumid] 
							FROM [dnt_albums] 
							 ' 
							+ @strWhere + 	
							' ORDER BY [albumid] DESC
									) 
					ORDER BY [albumid] DESC'
END
ELSE
BEGIN
	SET @strSQL = 'SELECT * 
					 FROM [dnt_albums] 
					WHERE [albumid] IN (
							SELECT TOP ' + STR(@pagesize) + ' [albumid] 
							FROM [dnt_albums] ' + @strWhere+ ' AND [albumid] < (
											SELECT MIN([albumid])
											FROM (
												SELECT TOP ' + STR((@pageindex-1)*@pagesize) + ' [albumid] 
												FROM [dnt_albums] ' + @strWhere + ' ORDER BY [albumid] DESC
											 ) AS [ttt]
										)
							ORDER BY [albumid] DESC
									) 
					ORDER BY [albumid] DESC'

END
EXEC(@strSQL)
GO

CREATE PROCEDURE [dnt_getphotolistbytag]	
	@tagid int,
	@pageindex int,
	@pagesize int
AS
BEGIN
	DECLARE @strSQL varchar(2000)
	IF @pageindex = 1
	BEGIN
		SET @strSQL='SELECT TOP ' + STR(@pagesize) + ' [p].[photoid], [p].[title],[p].[filename],[p].[filesize],[p].[username],[p].[userid], [p].[postdate],[p].[comments],[p].[views]  
		FROM [dnt_phototags] AS [pt], [dnt_photos] AS [p], [dnt_albums] AS [a] 
		WHERE [p].[photoid] = [pt].[photoid] AND [p].[albumid] = [a].[albumid] AND [a].[type] = 0 AND [pt].[tagid] = ' + STR(@tagid) + ' 
		ORDER BY [p].[photoid] DESC'
	END
	ELSE
	BEGIN
		SET @strSQL='SELECT TOP ' + STR(@pagesize) + ' [p].[photoid], [p].[title],[p].[filename],[p].[filesize],[p].[username],[p].[userid], [p].[postdate],[p].[comments],[p].[views]  
		FROM [dnt_phototags] AS [pt], [dnt_photos] AS [p], [dnt_albums] AS [a] 
		WHERE [p].[photoid] = [pt].[photoid] AND [p].[albumid] = [a].[albumid] AND [a].[type] = 0 AND [pt].[tagid] = ' + STR(@tagid) + ' 
		AND [p].[photoid] < (SELECT MIN([photoid]) FROM (SELECT TOP ' + STR((@pageindex-1)*@pagesize) + ' 
		[p].[photoid] FROM [dnt_phototags] AS [pt], [dnt_photos] AS [p], [dnt_albums] AS [a] 
		WHERE [p].[photoid] = [pt].[photoid] AND [p].[albumid] = [a].[albumid] AND [a].[type] = 0 AND [pt].[tagid] = ' + STR(@tagid) + ' 
		ORDER BY [p].[photoid] DESC) as tblTmp) 
		ORDER BY [p].[photoid] DESC'
	END
	EXEC(@strSQL)
END
GO

IF OBJECT_ID('[dnt_getfavoriteslistbyalbum]','P') IS NOT NULL
DROP PROC [dnt_getfavoriteslistbyalbum]
GO

CREATE PROCEDURE [dnt_getfavoriteslistbyalbum]
@uid int,
@pagesize int,
@pageindex int
AS
DECLARE @strSQL varchar(5000)


SET @strSQL='SELECT [f].[tid], [f].[uid], [f].[favtime] ,[albumid], [albumcateid], [userid] AS [posterid], [username] AS [poster], [title], [description], [logo], [password], [imgcount], [views], [type], [createdatetime] AS [postdatetime] FROM [dnt_favorites] [f],[dnt_albums] [albums] WHERE [f].[tid]=[albums].[albumid] AND [f].[typeid]=1 AND [f].[uid]=' + STR(@uid)

IF @pageindex = 1
	BEGIN
		SET @strSQL = 'SELECT TOP ' + STR(@pagesize) +'  [tid], [uid], [albumid], [albumcateid], [posterid], [poster], [title], [description], [logo], [password], [imgcount], [views], [type], [postdatetime], [favtime]  FROM (' + @strSQL + ') f' + '  ORDER BY [tid] DESC'
	END
ELSE
	BEGIN
		SET @strSQL = 'SELECT TOP ' + STR(@pagesize) +'  [tid], [uid], [albumid], [albumcateid], [posterid], [poster], [title], [description], [logo], [password], [imgcount], [views], [type], [postdatetime], [favtime]  FROM (' + @strSQL + ') f1 WHERE [tid] < (SELECT MIN([tid]) FROM (SELECT TOP ' + STR((@pageindex-1)*@pagesize) + ' [tid] FROM (' + @strSQL + ') f2' + '  ORDER BY [tid] DESC) AS tblTmp)' + '  ORDER BY [tid] DESC'
	END

EXEC(@strSQL)
