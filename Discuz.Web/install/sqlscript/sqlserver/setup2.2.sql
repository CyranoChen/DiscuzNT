IF OBJECT_ID('dnt_checkemailandsecques','P') IS NOT NULL
DROP PROC dnt_checkemailandsecques
GO

CREATE PROCEDURE dnt_checkemailandsecques
@username nchar(20),
@email char(50),
@secques char(8)
AS
SELECT TOP 1 [uid] FROM [dnt_users] WHERE [username]=@username AND [email]=@email AND [secques]=@secques
GO

IF OBJECT_ID('dnt_checkpasswordandsecques','P') IS NOT NULL
DROP PROC dnt_checkpasswordandsecques
GO

CREATE PROCEDURE dnt_checkpasswordandsecques
@username nchar(20),
@password char(32),
@secques char(8)
AS
SELECT TOP 1 [uid] FROM [dnt_users] WHERE [username]=@username AND [password]=@password AND [secques]=@secques
GO

IF OBJECT_ID('dnt_checkpasswordbyuid','P') IS NOT NULL
DROP PROC dnt_checkpasswordbyuid
GO

CREATE PROCEDURE dnt_checkpasswordbyuid
@uid int,
@password char(32)
AS
SELECT TOP 1 [uid], [groupid], [adminid] FROM [dnt_users] WHERE [uid]=@uid AND [password]=@password
GO

IF OBJECT_ID('dnt_checkpasswordbyusername','P') IS NOT NULL
DROP PROC dnt_checkpasswordbyusername
GO

CREATE PROCEDURE dnt_checkpasswordbyusername
@username nchar(20),
@password char(32)
AS
SELECT TOP 1 [uid], [groupid], [adminid] FROM [dnt_users] WHERE [username]=@username AND [password]=@password
GO

IF OBJECT_ID('dnt_createadmingroup','P') IS NOT NULL
DROP PROC dnt_createadmingroup
GO

CREATE PROCEDURE dnt_createadmingroup
	@admingid smallint,
	@alloweditpost tinyint,
	@alloweditpoll tinyint,
	@allowstickthread tinyint,
	@allowmodpost tinyint,
	@allowdelpost tinyint,
	@allowmassprune tinyint,
	@allowrefund tinyint,
	@allowcensorword tinyint,
	@allowviewip tinyint,
	@allowbanip tinyint,
	@allowedituser tinyint,
	@allowmoduser tinyint,
	@allowbanuser tinyint,
	@allowpostannounce tinyint,
	@allowviewlog tinyint,
	@disablepostctrl tinyint,
	@allowviewrealname tinyint
AS
INSERT INTO dnt_admingroups 
	([admingid],[alloweditpost],[alloweditpoll],[allowstickthread],[allowmodpost],[allowdelpost],[allowmassprune],[allowrefund],[allowcensorword],[allowviewip],[allowbanip],[allowedituser],[allowmoduser],[allowbanuser],[allowpostannounce],[allowviewlog],[disablepostctrl],[allowviewrealname])
VALUES
	(@admingid,@alloweditpost,@alloweditpoll,@allowstickthread,@allowmodpost,@allowdelpost,@allowmassprune,@allowrefund,@allowcensorword,@allowviewip,@allowbanip,@allowedituser,@allowmoduser,@allowbanuser,@allowpostannounce,@allowviewlog,@disablepostctrl,@allowviewrealname)
GO

IF OBJECT_ID('[dnt_createattachment]','P') IS NOT NULL
DROP PROC [dnt_createattachment]
GO

CREATE PROCEDURE [dnt_createattachment]
@uid int,
@tid int,
@pid int,
@postdatetime datetime,
@readperm int,
@filename nchar(200),
@description nchar(200),
@filetype nchar(100),
@filesize int,
@attachment nchar(200),
@downloads int,
@extname nvarchar(50),
@attachprice int,
@width int,
@height int,
@isimage tinyint

AS
DECLARE @aid int

INSERT INTO [dnt_attachments]([uid],[tid], [pid], [postdatetime], [readperm], [filename], [description], [filetype], [filesize],  [attachment], [downloads],[attachprice],[width],[height],[isimage]) VALUES(@uid, @tid, @pid, @postdatetime, @readperm, @filename, @description, @filetype, @filesize,  @attachment, @downloads, @attachprice,@width,@height,@isimage)
SELECT SCOPE_IDENTITY()  AS 'aid'

set @aid=(SELECT SCOPE_IDENTITY()  AS 'aid')
UPDATE [dnt_posts1] SET [attachment]=1 WHERE [pid]=@pid

INSERT INTO [dnt_myattachments]([aid],[uid],[attachment],[description],[postdatetime],[downloads],[filename],[pid],[tid],[extname]) VALUES(@aid,@uid,@attachment,@description,@postdatetime,@downloads,@filename,@pid,@tid,@extname)
GO

IF OBJECT_ID('dnt_createdebatepostexpand','P') IS NOT NULL
DROP PROC dnt_createdebatepostexpand
GO

CREATE PROCEDURE dnt_createdebatepostexpand
	@tid int,
	@pid int,
	@opinion int,
	@diggs int
AS
BEGIN
	INSERT INTO [dnt_postdebatefields] VALUES(@tid, @pid, @opinion, @diggs)
	IF @opinion = 1
		UPDATE [dnt_debates] SET [positivediggs] = [positivediggs] + 1 WHERE [tid] = @tid
	ELSE IF @opinion = 2
		UPDATE [dnt_debates] SET [negativediggs] = [negativediggs] + 1 WHERE [tid] = @tid
END
GO

IF OBJECT_ID('dnt_createfavorite','P') IS NOT NULL
DROP PROC dnt_createfavorite
GO

CREATE PROCEDURE dnt_createfavorite
@uid int,
@tid int,
@type tinyint

AS
	
	INSERT INTO [dnt_favorites] ([uid],[tid],[typeid]) VALUES(@uid,@tid,@type)
	RETURN
GO

IF OBJECT_ID('dnt_createpm','P') IS NOT NULL
DROP PROC dnt_createpm
GO

CREATE PROCEDURE dnt_createpm
@pmid int,
@msgfrom nvarchar(20),
@msgto nvarchar(20),
@msgfromid int,
@msgtoid int,
@folder smallint=0,
@new int=0,
@subject nvarchar(60),
@postdatetime datetime,
@message ntext,
@savetosentbox smallint=1
AS

IF @folder<>0
	BEGIN
		SET @msgfrom=@msgto
	END
ELSE
	BEGIN
		UPDATE [dnt_users] SET [newpmcount]=ABS(ISNULL([newpmcount],0)*1)+1,[newpm] = 1 WHERE [uid]=@msgtoid
	END

	

INSERT INTO [dnt_pms] 
	([msgfrom],[msgfromid],[msgto],[msgtoid],[folder],[new],[subject],[postdatetime],[message])
VALUES
	(@msgfrom,@msgfromid,@msgto,@msgtoid,@folder,@new,@subject,@postdatetime,@message)
	
SELECT SCOPE_IDENTITY() AS 'pmid'

IF @savetosentbox=1 AND @folder=0
	BEGIN
		INSERT INTO [dnt_pms]
			([msgfrom],[msgfromid],[msgto],[msgtoid],[folder],[new],[subject],[postdatetime],[message])
		VALUES
			(@msgfrom,@msgfromid,@msgto,@msgtoid,1,@new,@subject,@postdatetime,@message)
	END
GO

IF OBJECT_ID('dnt_createpost1','P') IS NOT NULL
DROP PROC [dnt_createpost1]
GO

CREATE PROCEDURE [dnt_createpost1]
@fid int,
@tid int,
@parentid int,
@layer int,
@poster varchar(20),
@posterid int,
@title nvarchar(60),
@topictitle nvarchar(60),
@postdatetime datetime,
@message ntext,
@ip varchar(15),
@lastedit varchar(50),
@invisible int,
@usesig int,
@htmlon int,
@smileyoff int,
@bbcodeoff int,
@parseurloff int,
@attachment int,
@rate int,
@ratetimes int

AS


DEClARE @postid int

DELETE FROM [dnt_postid] WHERE DATEDIFF(n, postdatetime, GETDATE()) >5

INSERT INTO [dnt_postid] ([postdatetime]) VALUES(GETDATE())

SELECT @postid=SCOPE_IDENTITY()

INSERT INTO [dnt_posts1]([pid], [fid], [tid], [parentid], [layer], [poster], [posterid], [title], [postdatetime], [message], [ip], [lastedit], [invisible], [usesig], [htmlon], [smileyoff], [bbcodeoff], [parseurloff], [attachment], [rate], [ratetimes]) VALUES(@postid, @fid, @tid, @parentid, @layer, @poster, @posterid, @title, @postdatetime, @message, @ip, @lastedit, @invisible, @usesig, @htmlon, @smileyoff, @bbcodeoff, @parseurloff, @attachment, @rate, @ratetimes)

IF @parentid=0
    BEGIN
        UPDATE [dnt_posts1] SET [parentid]=@postid WHERE [pid]=@postid
    END

IF @@ERROR=0
    BEGIN
        IF  @invisible = 0
        BEGIN
            UPDATE [dnt_statistics] SET [totalpost]=[totalpost] + 1
		
            DECLARE @fidlist AS VARCHAR(1000)
            DECLARE @strsql AS VARCHAR(4000)
			
            SET @fidlist = '';
			
            SELECT @fidlist = ISNULL([parentidlist],'') FROM [dnt_forums] WHERE [fid] = @fid
            IF RTRIM(@fidlist)<>''
                BEGIN
                    SET @fidlist = RTRIM(@fidlist) + ',' + CAST(@fid AS VARCHAR(10))
                END
            ELSE
                BEGIN
                    SET @fidlist = CAST(@fid AS VARCHAR(10))
                END
        
			
            UPDATE [dnt_forums] SET 
	                                [posts]=[posts] + 1, 
	                                [todayposts]=CASE 
					                                WHEN DATEDIFF(day, [lastpost], GETDATE())=0 THEN [todayposts] + 1 
				                                 ELSE 1 
				                                 END,
	                                [lasttid]=@tid,	
									[lasttitle]=@topictitle,
	                                [lastpost]=@postdatetime,
	                                [lastposter]=@poster,
	                                [lastposterid]=@posterid 
							
                            		WHERE fid IN (SELECT [item] FROM [dnt_split](@fidlist, ','))
			
			
            UPDATE [dnt_users] SET
                [lastpost] = @postdatetime,
                [lastpostid] = @postid,
                [lastposttitle] = @title,
                [posts] = [posts] + 1,
                [lastactivity] = GETDATE()
            WHERE [uid] = @posterid
       
            IF @layer<=0
                BEGIN
                    UPDATE [dnt_topics] SET [replies]=0,[lastposter]=@poster,[lastpost]=@postdatetime,[lastposterid]=@posterid WHERE [tid]=@tid
                END
            ELSE
                BEGIN
                    UPDATE [dnt_topics] SET [replies]=[replies] + 1,[lastposter]=@poster,[lastpost]=@postdatetime,[lastposterid]=@posterid WHERE [tid]=@tid
                END
            UPDATE [dnt_topics] SET [lastpostid]=@postid WHERE [tid]=@tid
        END
        ELSE IF @layer<=0
			BEGIN
				UPDATE [dnt_topics] SET [replies]=0,[lastposter]=@poster,[lastpost]=@postdatetime,[lastposterid]=@posterid,[lastpostid]=@postid WHERE [tid]=@tid
			END
    IF @posterid <> -1
        BEGIN
            INSERT [dnt_myposts]([uid], [tid], [pid], [dateline]) VALUES(@posterid, @tid, @postid, @postdatetime)
        END
    END
SELECT @postid AS postid
GO

IF OBJECT_ID('dnt_createsearchcache','P') IS NOT NULL
DROP PROC dnt_createsearchcache
GO

CREATE PROCEDURE dnt_createsearchcache
	@keywords varchar(255),
	@searchstring varchar(255),
	@ip varchar(15),
	@uid int,
	@groupid int,
	@postdatetime varchar(19),
	@expiration varchar(19),
	@topics int,
	@tids text
AS
INSERT INTO dnt_searchcaches 
	([keywords],[searchstring],[ip],[uid],[groupid],[postdatetime],[expiration],[topics],[tids])
VALUES
	(@keywords,@searchstring,@ip,@uid,@groupid,@postdatetime,@expiration,@topics,@tids)
SELECT SCOPE_IDENTITY()  AS 'searchid'
GO

IF OBJECT_ID('[dnt_createtags]','P') IS NOT NULL
DROP PROC [dnt_createtags]
GO

CREATE PROCEDURE [dnt_createtags]
@tags nvarchar(55),
@userid int,
@postdatetime datetime
AS
BEGIN	
	INSERT INTO [dnt_tags]([tagname], [userid], [postdatetime], [orderid], [color], [count], [fcount], [pcount], [scount], [vcount]) 
		SELECT [item], @userid, @postdatetime, 0, '', 0, 0, 0, 0, 0 FROM [dnt_split](@tags, ' ') AS [newtags] 
		WHERE NOT EXISTS (SELECT [tagname] FROM [dnt_tags] WHERE [newtags].[item] = [tagname])
END
GO

IF OBJECT_ID('dnt_createtopic','P') IS NOT NULL
DROP PROC dnt_createtopic
GO

CREATE PROCEDURE dnt_createtopic
@fid smallint,
@iconid smallint,
@title nchar(80),
@typeid smallint,
@readperm int,
@price smallint,
@poster char(20),
@posterid int,
@postdatetime datetime,
@lastpost smalldatetime,
@lastpostid int,
@lastposter char(20),
@views int,
@replies int,
@displayorder int,
@highlight varchar(500),
@digest int,
@rate int,
@hide int,
@attachment int,
@moderated int,
@closed int,
@magic int,
@special tinyint,
@attention int
AS

DECLARE @topicid int

DELETE FROM [dnt_topics] WHERE [tid]>(SELECT ISNULL(max(tid),0)-100 FROM [dnt_topics]) AND [lastpostid]=0

INSERT INTO [dnt_topics]([fid], [iconid], [title], [typeid], [readperm], [price], [poster], [posterid], [postdatetime], [lastpost], [lastpostid], [lastposter], [views], [replies], [displayorder], [highlight], [digest], [rate], [hide], [attachment], [moderated], [closed], [magic], [special],[attention]) VALUES(@fid, @iconid, @title, @typeid, @readperm, @price, @poster, @posterid, @postdatetime, @lastpost, @lastpostid, @lastposter, @views, @replies, @displayorder, @highlight, @digest, @rate, @hide, @attachment, @moderated, @closed, @magic, @special,@attention)

SET @topicid=SCOPE_IDENTITY()

IF @@ERROR=0 AND @displayorder=0
	BEGIN
		UPDATE [dnt_statistics] SET [totaltopic]=[totaltopic] + 1


		UPDATE [dnt_forums] SET [topics] = [topics] + 1,[curtopics] = [curtopics] + 1 WHERE [fid] = @fid
		
		IF @posterid <> -1
		BEGIN
			INSERT INTO [dnt_mytopics]([tid],[uid],[dateline]) VALUES(@topicid,  @posterid,  @postdatetime)
		END
		
	END
	
SELECT @topicid as topicid
GO

IF OBJECT_ID('[dnt_createuser]','P') IS NOT NULL
DROP PROC [dnt_createuser]
GO

CREATE PROCEDURE [dnt_createuser]
@username nchar(20),
@nickname nchar(20),
@password char(32),
@secques char(8),
@gender int,
@adminid int,
@groupid smallint,
@groupexpiry int,
@extgroupids char(60),
@regip char(15),
@joindate char(19),
@lastip char(15),
@lastvisit char(19),
@lastactivity char(19),
@lastpost char(19),
@lastpostid int,
@lastposttitle nchar(60),
@posts int,
@digestposts smallint,
@oltime int,
@pageviews int,
@credits int,
@extcredits1 float,
@extcredits2 float,
@extcredits3 float,
@extcredits4 float,
@extcredits5 float,
@extcredits6 float,
@extcredits7 float,
@extcredits8 float,
@avatarshowid int,
@email char(50),
@bday char(19),
@sigstatus int,
@salt nchar(6),
@tpp int,
@ppp int,
@templateid smallint,
@pmsound int,
@showemail int,
@newsletter int,
@invisible int,
@newpm int,
@accessmasks int,
@website varchar(80),
@icq varchar(12),
@qq varchar(12),
@yahoo varchar(40),
@msn varchar(40),
@skype varchar(40),
@location nvarchar(30),
@customstatus varchar(30),
@avatar varchar(255),
@avatarwidth int,
@avatarheight int,
@medals varchar(300),
@bio nvarchar(500),
@signature nvarchar(500),
@sightml nvarchar(1000),
@authstr varchar(20),
@realname nvarchar(10),
@idcard varchar(20),
@mobile varchar(20),
@phone varchar(20)
AS
DECLARE @uid int

INSERT INTO [dnt_users]([username],[nickname], [password], [secques], [gender], [adminid], [groupid], [groupexpiry], [extgroupids], [regip], [joindate], [lastip], [lastvisit], [lastactivity], [lastpost], [lastpostid], [lastposttitle], [posts], [digestposts], [oltime], [pageviews], [credits], [extcredits1], [extcredits2], [extcredits3], [extcredits4], [extcredits5], [extcredits6], [extcredits7], [extcredits8], [avatarshowid], [email], [bday], [sigstatus], [salt], [tpp], [ppp], [templateid], [pmsound], [showemail], [newsletter], [invisible], [newpm], [accessmasks]) VALUES(@username,@nickname, @password, @secques, @gender, @adminid, @groupid, @groupexpiry, @extgroupids, @regip, @joindate, @lastip, @lastvisit, @lastactivity, @lastpost, @lastpostid, @lastposttitle, @posts, @digestposts, @oltime, @pageviews, @credits, @extcredits1, @extcredits2, @extcredits3, @extcredits4, @extcredits5, @extcredits6, @extcredits7, @extcredits8, @avatarshowid, @email, @bday, @sigstatus, @salt, @tpp, @ppp, @templateid, @pmsound, @showemail, @newsletter, @invisible, @newpm, @accessmasks)
SET @uid = SCOPE_IDENTITY()
SELECT @uid AS 'userid'

IF @@ERROR=0
	BEGIN
		UPDATE [dnt_statistics] SET [totalusers]=[totalusers] + 1,[lastusername]=@username,[lastuserid]=@uid
	END

INSERT INTO dnt_userfields 
	([uid],[website],[icq],[qq],[yahoo],[msn],[skype],[location],[customstatus],[avatar],[avatarwidth],[avatarheight],[medals],[bio],[signature],[sightml],[authstr],[realname],[idcard],[mobile],[phone])
VALUES
	(@uid,@website,@icq,@qq,@yahoo,@msn,@skype,@location,@customstatus,@avatar,@avatarwidth,@avatarheight,@medals,@bio,@signature,@sightml,@authstr,@realname,@idcard,@mobile,@phone)	
GO

IF OBJECT_ID('[dnt_deletepost1bypid]','P') IS NOT NULL
DROP PROC [dnt_deletepost1bypid]
GO

CREATE PROCEDURE [dnt_deletepost1bypid]
                        @pid int,
			@chanageposts AS BIT
                    AS

                        DECLARE @fid int
                        DECLARE @tid int
                        DECLARE @posterid int
                        DECLARE @lastforumposterid int
                        DECLARE @layer int
                        DECLARE @postdatetime smalldatetime
                        DECLARE @poster varchar(50)
                        DECLARE @postcount int
                        DECLARE @title nchar(60)
                        DECLARE @lasttid int
                        DECLARE @postid int
                        DECLARE @todaycount int
                    	
                    	
                        SELECT @fid = [fid],@tid = [tid],@posterid = [posterid],@layer = [layer], @postdatetime = [postdatetime] FROM [dnt_posts1] WHERE pid = @pid

                        DECLARE @fidlist AS VARCHAR(1000)
                    	
                        SET @fidlist = '';
                    	
                        SELECT @fidlist = ISNULL([parentidlist],'') FROM [dnt_forums] WHERE [fid] = @fid
                        IF RTRIM(@fidlist)<>''
                            BEGIN
                                SET @fidlist = RTRIM(@fidlist) + ',' + CAST(@fid AS VARCHAR(10))
                            END
                        ELSE
                            BEGIN
                                SET @fidlist = CAST(@fid AS VARCHAR(10))
                            END


                        IF @layer<>0

                            BEGIN
                    			
								IF @chanageposts = 1
									BEGIN
										UPDATE [dnt_statistics] SET [totalpost]=[totalpost] - 1

										UPDATE [dnt_forums] SET 
											[posts]=[posts] - 1, 
											[todayposts]=CASE 
																WHEN DATEPART(yyyy, @postdatetime)=DATEPART(yyyy,GETDATE()) AND DATEPART(mm, @postdatetime)=DATEPART(mm,GETDATE()) AND DATEPART(dd, @postdatetime)=DATEPART(dd,GETDATE()) THEN [todayposts] - 1
																ELSE [todayposts]
														END						
										WHERE (CHARINDEX(',' + RTRIM([fid]) + ',', ',' +
															(SELECT @fidlist AS [fid]) + ',') > 0)
                    			
										UPDATE [dnt_users] SET [posts] = [posts] - 1 WHERE [uid] = @posterid

										UPDATE [dnt_topics] SET [replies]=[replies] - 1 WHERE [tid]=@tid
									END
                    			
                                DELETE FROM [dnt_posts1] WHERE [pid]=@pid
                    			
                            END
                        ELSE
                            BEGIN
                    		
                                SELECT @postcount = COUNT([pid]) FROM [dnt_posts1] WHERE [tid] = @tid
                                SELECT @todaycount = COUNT([pid]) FROM [dnt_posts1] WHERE [tid] = @tid AND DATEDIFF(d, [postdatetime], GETDATE()) = 0
                    			
								IF @chanageposts = 1
									BEGIN
										UPDATE [dnt_statistics] SET [totaltopic]=[totaltopic] - 1, [totalpost]=[totalpost] - @postcount
		                    			
										UPDATE [dnt_forums] SET [posts]=[posts] - @postcount, [topics]=[topics] - 1,[todayposts]=[todayposts] - @todaycount WHERE (CHARINDEX(',' + RTRIM([fid]) + ',', ',' +(SELECT @fidlist AS [fid]) + ',') > 0)
		                    			
										UPDATE [dnt_users] SET [posts] = [posts] - @postcount WHERE [uid] = @posterid
                    			
									END

                                DELETE FROM [dnt_posts1] WHERE [tid] = @tid
                    			
                                DELETE FROM [dnt_topics] WHERE [tid] = @tid
                    			
                            END	
                    		

                        IF @layer<>0
                            BEGIN
                                SELECT TOP 1 @pid = [pid], @posterid = [posterid], @postdatetime = [postdatetime], @title = [title], @poster = [poster] FROM [dnt_posts1] WHERE [tid]=@tid ORDER BY [pid] DESC
                                UPDATE [dnt_topics] SET [lastposter]=@poster,[lastpost]=@postdatetime,[lastpostid]=@pid,[lastposterid]=@posterid WHERE [tid]=@tid
                            END



                        SELECT @lasttid = [lasttid] FROM [dnt_forums] WHERE [fid] = @fid

                    	
                        IF @lasttid = @tid
                            BEGIN

                    			
                    			

                                SELECT TOP 1 @pid = [pid], @tid = [tid],@lastforumposterid = [posterid], @title = [title], @postdatetime = [postdatetime], @poster = [poster] FROM [dnt_posts1] WHERE [fid] = @fid ORDER BY [pid] DESC
                    			
                            
                            
                                UPDATE [dnt_forums] SET 
                    			
	                                [lastpost]=@postdatetime,
	                                [lastposter]=ISNULL(@poster,''),
	                                [lastposterid]=ISNULL(@lastforumposterid,'0')

                                WHERE (CHARINDEX(',' + RTRIM([fid]) + ',', ',' +
					                                (SELECT @fidlist AS [fid]) + ',') > 0)


                    			
                                SELECT TOP 1 @pid = [pid], @tid = [tid],@posterid = [posterid], @postdatetime = [postdatetime], @title = [title], @poster = [poster] FROM [dnt_posts1] WHERE [posterid]=@posterid ORDER BY [pid] DESC
                    			
                                UPDATE [dnt_users] SET
                    			
	                                [lastpost] = @postdatetime,
	                                [lastpostid] = @pid,
	                                [lastposttitle] = ISNULL(@title,'')
                    				
                                WHERE [uid] = @posterid
                    			
                            END
GO

IF OBJECT_ID('[dnt_deletetopictags]','P') IS NOT NULL
DROP PROC [dnt_deletetopictags]
GO

CREATE PROCEDURE [dnt_deletetopictags]
	@tid int
 AS
BEGIN       
	UPDATE [dnt_tags] SET [count]=[count]-1,[fcount]=[fcount]-1 
	WHERE EXISTS (SELECT [tagid] FROM [dnt_topictags] WHERE [tid] = @tid AND [tagid] = [dnt_tags].[tagid])

    DELETE FROM [dnt_topictags] WHERE [tid] = @tid	
END
GO

IF OBJECT_ID('dnt_getalltopiccount','P') IS NOT NULL
DROP PROC dnt_getalltopiccount
GO

CREATE PROCEDURE dnt_getalltopiccount
@subfidList nchar(400)
AS

SELECT COUNT(tid) FROM [dnt_topics] WHERE [fid]  IN (SELECT [item] FROM [dnt_split](@subfidList, ','))  AND [displayorder]>=0
GO

IF OBJECT_ID('[dnt_getdebatepostlist1]','P') IS NOT NULL
DROP PROC [dnt_getdebatepostlist1]
GO

CREATE PROCEDURE [dnt_getdebatepostlist1] 
	@tid int,
	@opinion int,
	@pagesize int,
	@pageindex int
AS
BEGIN
	DECLARE @pagetop int
	SET @pagetop = (@pageindex-1)*@pagesize

	IF @pageindex = 1 
		EXEC('SELECT 
[dnt_posts1].[attachment],
[dnt_posts1].[bbcodeoff],
[dnt_posts1].[fid],
[dnt_posts1].[htmlon],
[dnt_posts1].[invisible],
[dnt_posts1].[ip],
[dnt_posts1].[lastedit],
[dnt_posts1].[layer],
[dnt_posts1].[message],
[dnt_posts1].[parentid],
[dnt_posts1].[parseurloff],
[dnt_posts1].[pid],
[dnt_posts1].[postdatetime],
[dnt_posts1].[poster],
[dnt_posts1].[posterid],
[dnt_posts1].[rate],
[dnt_posts1].[ratetimes],
[dnt_posts1].[smileyoff],
[dnt_posts1].[tid],
[dnt_posts1].[title],
[dnt_posts1].[usesig],
[dnt_users].[accessmasks], 
[dnt_users].[adminid],
[dnt_users].[avatarshowid],
[dnt_users].[bday],
[dnt_users].[credits],
[dnt_users].[digestposts],
[dnt_users].[email],
[dnt_users].[extcredits1],
[dnt_users].[extcredits2],
[dnt_users].[extcredits3],
[dnt_users].[extcredits4],
[dnt_users].[extcredits5],
[dnt_users].[extcredits6],
[dnt_users].[extcredits7],
[dnt_users].[extcredits8],
[dnt_users].[extgroupids],
[dnt_users].[gender],
[dnt_users].[groupexpiry],
[dnt_users].[groupid],
[dnt_users].[joindate],
[dnt_users].[lastactivity],
[dnt_users].[lastip],
[dnt_users].[lastpost],
[dnt_users].[lastpostid],
[dnt_users].[lastposttitle],
[dnt_users].[lastvisit],
[dnt_users].[newpm],
[dnt_users].[newpmcount],
[dnt_users].[newsletter],
[dnt_users].[nickname],
[dnt_users].[oltime],
[dnt_users].[onlinestate],
[dnt_users].[pageviews],
[dnt_users].[password],
[dnt_users].[pmsound],
[dnt_users].[posts],
[dnt_users].[ppp],
[dnt_users].[regip],
[dnt_users].[secques],
[dnt_users].[showemail],
[dnt_users].[sigstatus],
[dnt_users].[spaceid],
[dnt_users].[templateid],
[dnt_users].[tpp],
[dnt_users].[uid],
[dnt_users].[username],
[dnt_userfields].[authflag],
[dnt_userfields].[authstr],
[dnt_userfields].[authtime],
[dnt_userfields].[avatar],
[dnt_userfields].[avatarheight],
[dnt_userfields].[avatarwidth],
[dnt_userfields].[bio],
[dnt_userfields].[customstatus],
[dnt_userfields].[icq],
[dnt_userfields].[idcard],
[dnt_userfields].[ignorepm],
[dnt_userfields].[location],
[dnt_userfields].[medals],
[dnt_userfields].[mobile],
[dnt_userfields].[msn],
[dnt_userfields].[phone],
[dnt_userfields].[qq],
[dnt_userfields].[realname],
[dnt_userfields].[sightml],
[dnt_userfields].[signature],
[dnt_userfields].[skype],
[dnt_userfields].[website],
[dnt_userfields].[yahoo] FROM [dnt_posts1] 
				LEFT JOIN dnt_users ON dnt_users.uid = [dnt_posts1].posterid 
				LEFT JOIN [dnt_userfields] ON [dnt_userfields].[uid]=[dnt_users].[uid] 
				WHERE [dnt_posts1].invisible=0 AND [dnt_posts1].pid IN 
					(SELECT TOP ' + @pagesize + ' pid FROM dnt_postdebatefields 
					 WHERE opinion=' + @opinion + ' AND 
							tid=' + @tid+')')
	ELSE
		EXEC('SELECT 
[dnt_posts1].[attachment],
[dnt_posts1].[bbcodeoff],
[dnt_posts1].[fid],
[dnt_posts1].[htmlon],
[dnt_posts1].[invisible],
[dnt_posts1].[ip],
[dnt_posts1].[lastedit],
[dnt_posts1].[layer],
[dnt_posts1].[message],
[dnt_posts1].[parentid],
[dnt_posts1].[parseurloff],
[dnt_posts1].[pid],
[dnt_posts1].[postdatetime],
[dnt_posts1].[poster],
[dnt_posts1].[posterid],
[dnt_posts1].[rate],
[dnt_posts1].[ratetimes],
[dnt_posts1].[smileyoff],
[dnt_posts1].[tid],
[dnt_posts1].[title],
[dnt_posts1].[usesig],
[dnt_users].[accessmasks], 
[dnt_users].[adminid],
[dnt_users].[avatarshowid],
[dnt_users].[bday],
[dnt_users].[credits],
[dnt_users].[digestposts],
[dnt_users].[email],
[dnt_users].[extcredits1],
[dnt_users].[extcredits2],
[dnt_users].[extcredits3],
[dnt_users].[extcredits4],
[dnt_users].[extcredits5],
[dnt_users].[extcredits6],
[dnt_users].[extcredits7],
[dnt_users].[extcredits8],
[dnt_users].[extgroupids],
[dnt_users].[gender],
[dnt_users].[groupexpiry],
[dnt_users].[groupid],
[dnt_users].[joindate],
[dnt_users].[lastactivity],
[dnt_users].[lastip],
[dnt_users].[lastpost],
[dnt_users].[lastpostid],
[dnt_users].[lastposttitle],
[dnt_users].[lastvisit],
[dnt_users].[newpm],
[dnt_users].[newpmcount],
[dnt_users].[newsletter],
[dnt_users].[nickname],
[dnt_users].[oltime],
[dnt_users].[onlinestate],
[dnt_users].[pageviews],
[dnt_users].[password],
[dnt_users].[pmsound],
[dnt_users].[posts],
[dnt_users].[ppp],
[dnt_users].[regip],
[dnt_users].[secques],
[dnt_users].[showemail],
[dnt_users].[sigstatus],
[dnt_users].[spaceid],
[dnt_users].[templateid],
[dnt_users].[tpp],
[dnt_users].[uid],
[dnt_users].[username],
[dnt_userfields].[authflag],
[dnt_userfields].[authstr],
[dnt_userfields].[authtime],
[dnt_userfields].[avatar],
[dnt_userfields].[avatarheight],
[dnt_userfields].[avatarwidth],
[dnt_userfields].[bio],
[dnt_userfields].[customstatus],
[dnt_userfields].[icq],
[dnt_userfields].[idcard],
[dnt_userfields].[ignorepm],
[dnt_userfields].[location],
[dnt_userfields].[medals],
[dnt_userfields].[mobile],
[dnt_userfields].[msn],
[dnt_userfields].[phone],
[dnt_userfields].[qq],
[dnt_userfields].[realname],
[dnt_userfields].[sightml],
[dnt_userfields].[signature],
[dnt_userfields].[skype],
[dnt_userfields].[website],
[dnt_userfields].[yahoo] FROM [dnt_posts1] 
				LEFT JOIN dnt_users ON dnt_users.uid = [dnt_posts1].posterid 
				LEFT JOIN [dnt_userfields] ON [dnt_userfields].[uid]=[dnt_users].[uid] 
				WHERE [dnt_posts1].invisible=0 AND [dnt_posts1].pid IN 
					(SELECT TOP ' + @pagesize + ' pid FROM dnt_postdebatefields 
					 WHERE opinion=' + @opinion + ' AND 
							tid=' + @tid+' AND pid > (SELECT MAX(pid) FROM (
								SELECT TOP ' + @pagetop + ' pid FROM dnt_postdebatefields 
								WHERE opinion=' + @opinion + ' AND 
										tid=' + @tid+' ORDER BY pid) AS tblTmp) ORDER BY pid)')
	
END
GO

IF OBJECT_ID('dnt_getfavoritescount','P') IS NOT NULL
DROP PROC dnt_getfavoritescount
GO

CREATE PROCEDURE dnt_getfavoritescount
@uid int,
@typeid int
AS
SELECT COUNT(uid) as c FROM [dnt_favorites] WHERE [uid]=@uid and typeid=@typeid
GO

IF OBJECT_ID('dnt_getfavoritescountbytype','P') IS NOT NULL
DROP PROC dnt_getfavoritescountbytype
GO

CREATE PROCEDURE dnt_getfavoritescountbytype
@uid int,
@typeid tinyint
AS
SELECT COUNT(uid) as c FROM [dnt_favorites] WHERE [uid]=@uid AND [typeid]=@typeid
GO

IF OBJECT_ID('dnt_getfavoriteslist','P') IS NOT NULL
DROP PROC dnt_getfavoriteslist
GO

CREATE PROCEDURE [dnt_getfavoriteslist]
@uid int,
@pagesize int,
@pageindex int
AS

IF @pageindex = 1
	BEGIN
		EXEC('
		SELECT TOP '+@pagesize+' [uid],[tid],[fid],[title],[poster],[favtime],[replies],[views],[posterid],(CASE WHEN [viewtime]<[lastpost] THEN 1 ELSE 0 END) AS [new]
		FROM (SELECT [f].[uid],[f].[tid],[f].[favtime],[f].[viewtime],[topics].[title],[topics].[poster],[topics].[replies],[topics].[views],[topics].[posterid],[topics].[lastpost],[topics].[fid] FROM [dnt_favorites] [f] LEFT JOIN [dnt_topics] [topics]  ON   [f].[tid]=[topics].[tid] WHERE  [f].[typeid]=0 AND [f].[uid]='+@uid+') favorites ORDER BY [tid] DESC
		')
	END
ELSE
	BEGIN
		DECLARE @topsize int;
		SET @topsize = (@pageindex-1)*@pagesize;
		EXEC('
		SELECT TOP '+ @pagesize +' [uid],[tid],[fid],[title],[poster],[favtime],[replies],[views],[posterid],(CASE WHEN [viewtime]<[lastpost] THEN 1 ELSE 0 END) AS [new]
	    FROM (SELECT [f].[uid],[f].[tid],[f].[favtime],[f].[viewtime],[topics].[title],[topics].[poster],[topics].[lastpost],[topics].[replies],[topics].[views],[topics].[posterid],[topics].[fid] FROM [dnt_favorites] [f] LEFT JOIN [dnt_topics] [topics]  ON   [f].[tid]=[topics].[tid] WHERE  [f].[typeid]=0 AND [f].[uid]='+@uid+') f1 WHERE [tid] < (SELECT MIN([tid]) FROM (SELECT TOP('+@topsize+') [tid] 
	    FROM (SELECT [f].[uid],[f].[tid],[f].[favtime],[f].[viewtime],[topics].[title],[topics].[poster],[topics].[lastpost],[topics].[replies],[topics].[views],[topics].[posterid],[topics].[fid] FROM [dnt_favorites] [f] LEFT JOIN [dnt_topics] [topics]  ON   [f].[tid]=[topics].[tid] WHERE  [f].[typeid]=0 AND [f].[uid]='+@uid+') f2 ORDER BY [tid] DESC) AS tblTmp)  ORDER BY [tid] DESC
	    ')
	END
GO

IF OBJECT_ID('dnt_getfirstpost1id','P') IS NOT NULL
DROP PROC dnt_getfirstpost1id
GO

CREATE PROCEDURE dnt_getfirstpost1id
@tid int
AS
SELECT TOP 1 [pid] FROM [dnt_posts1] WHERE [tid]=@tid ORDER BY [pid]
GO

IF OBJECT_ID('[dnt_getlastexecutescheduledeventdatetime]','P') IS NOT NULL
DROP PROC [dnt_getlastexecutescheduledeventdatetime]
GO

CREATE PROCEDURE [dnt_getlastexecutescheduledeventdatetime]
(
	@key VARCHAR(100),
	@servername VARCHAR(100),
	@lastexecuted DATETIME OUTPUT
)
AS
SELECT @lastexecuted = MAX([lastexecuted]) FROM [dnt_scheduledevents] WHERE [key] = @key AND [servername] = @servername

IF(@lastexecuted IS NULL)
BEGIN
	SET @lastexecuted = DATEADD(YEAR,-1,GETDATE())
END
GO

IF OBJECT_ID('[dnt_getlastpostlist1]','P') IS NOT NULL
DROP PROC [dnt_getlastpostlist1]
GO

CREATE PROCEDURE [dnt_getlastpostlist1]
	@tid int,
	@postnum int
AS

EXEC('SELECT TOP ' + @postnum + ' [dnt_posts1].[pid], [dnt_posts1].[fid], [dnt_posts1].[layer], [dnt_posts1].[posterid], [dnt_posts1].[title], [dnt_posts1].[message], [dnt_posts1].[postdatetime], [dnt_posts1].[attachment], [dnt_posts1].[poster], [dnt_posts1].[posterid], [dnt_posts1].[invisible], [dnt_posts1].[usesig], [dnt_posts1].[htmlon], [dnt_posts1].[smileyoff], [dnt_posts1].[parseurloff], [dnt_posts1].[bbcodeoff], [dnt_posts1].[rate], [dnt_posts1].[ratetimes], [dnt_users].[username], [dnt_users].[email], [dnt_users].[showemail], [dnt_userfields].[avatar], [dnt_userfields].[avatarwidth], [dnt_userfields].[avatarheight], [dnt_userfields].[sightml] AS signature, [dnt_userfields].[location], [dnt_userfields].[customstatus] FROM [dnt_posts1] LEFT JOIN [dnt_users] ON [dnt_users].[uid]=[dnt_posts1].[posterid] LEFT JOIN [dnt_userfields] ON [dnt_userfields].[uid]=[dnt_users].[uid] WHERE [dnt_posts1].[tid]=' + @tid + '  AND  [dnt_posts1].[invisible] <=0 AND [dnt_posts1].layer <> 0 ORDER BY [dnt_posts1].[pid] DESC')
GO

IF OBJECT_ID('[dnt_getmyattachments]','P') IS NOT NULL
DROP PROC [dnt_getmyattachments]
GO

CREATE PROCEDURE [dnt_getmyattachments]
@uid int,
@pageindex int,
@pagesize int
 AS
DECLARE @strSQL varchar(5000)
IF @pageindex = 1
BEGIN
	SET @strSQL ='select TOP  '+ STR(@pagesize) + ' [aid],[uid],[attachment],[description],[downloads],[extname],[filename],[pid],[postdatetime],[tid]  from [dnt_myattachments]  where  [uid]='+STR(@uid)+'  order by [aid] desc'

END
ELSE
BEGIN
	SET @strSQL = 'SELECT TOP  '+STR(@pagesize)+' [aid],[uid],[attachment],[description],[downloads],[extname],[filename],[pid],[postdatetime],[tid] FROM [dnt_myattachments] WHERE [aid] >(SELECT ISNULL(MAX([aid]),0) FROM (SELECT TOP '+STR((@pageindex-1)*@pagesize)+' [aid] FROM [dnt_myattachments]  ORDER BY aid) as A) and [uid]='+ STR(@uid)+'  ORDER BY aid '

END
EXEC(@strSQL)
GO

IF OBJECT_ID('[dnt_getmyattachmentsbytype]','P') IS NOT NULL
DROP PROC [dnt_getmyattachmentsbytype]
GO

CREATE PROCEDURE [dnt_getmyattachmentsbytype]
@uid int,
@pageindex int,
@pagesize int,
@extlist as varchar(100)
 AS
DECLARE @strSQL varchar(5000)
IF @pageindex = 1
BEGIN
	SET @strSQL ='select TOP '+ STR(@pagesize) + ' [aid],[uid],[attachment],[description],[downloads],[extname],[filename],[pid],[postdatetime],[tid]  from [dnt_myattachments] where  [extname] in ('+@extlist+') and [uid]='+STR(@uid)+' order by [aid] desc'

END
ELSE
BEGIN
	SET @strSQL = 'select TOP '+ STR(@pagesize) + ' [aid],[uid],[attachment],[description],[downloads],[extname],[filename],[pid],[postdatetime],[tid]  from dnt_myattachments where [extname] in ('+@extlist+')  and [aid] >(SELECT ISNULL(MAX([aid]),0) FROM (SELECT TOP '+STR((@pageindex-1)*@pagesize)+' [aid] FROM [dnt_myattachments]  where [extname] in ('+@extlist+') ORDER BY aid) as A) and [uid]='+STR(@uid)+' ORDER BY aid '

END
EXEC(@strSQL)
GO

IF OBJECT_ID('[dnt_getmyposts]','P') IS NOT NULL
DROP PROC [dnt_getmyposts]
GO

CREATE PROCEDURE [dnt_getmyposts]
@uid int,
@pageindex int,
@pagesize int
 AS
DECLARE @strSQL varchar(5000)
IF @pageindex = 1
BEGIN
	SET @strSQL = 'SELECT [tid], [fid], [iconid], [typeid], [readperm], [price], [poster], [posterid], [title], [postdatetime], [lastpost], [lastpostid], [lastposter], [lastposterid], [views], [replies], [displayorder], [highlight], [digest], [rate], [hide], [special], [attachment], [moderated], [closed], [magic]
					 FROM [dnt_topics] 
					WHERE [tid] IN (
							SELECT DISTINCT TOP ' + STR(@pagesize) + ' [tid] 
							FROM [dnt_myposts] 
							WHERE [uid]=' + STR(@uid) + ' 
							ORDER BY [tid] DESC
									) 
					ORDER BY [tid] DESC'
END
ELSE
BEGIN
	SET @strSQL = 'SELECT [tid], [fid], [iconid], [typeid], [readperm], [price], [poster], [posterid], [title], [postdatetime], [lastpost], [lastpostid], [lastposter], [lastposterid], [views], [replies], [displayorder], [highlight], [digest], [rate], [hide], [special], [attachment], [moderated], [closed], [magic]
					 FROM [dnt_topics] 
					WHERE [tid] IN (
							SELECT DISTINCT TOP ' + STR(@pagesize) + ' [tid] 
							FROM [dnt_myposts] 
							WHERE [uid]=' + STR(@uid) + ' 
							AND [tid] < (
										SELECT MIN([tid])
										FROM (
											SELECT DISTINCT TOP ' + STR((@pageindex-1)*@pagesize) + ' [tid] 
											FROM [dnt_myposts]
											WHERE [uid]=' + STR(@uid) + ' 
											ORDER BY [tid] DESC
											 ) AS [ttt]
										)
							ORDER BY [tid] DESC
									) 
					ORDER BY [tid] DESC'

END
EXEC(@strSQL)
GO

IF OBJECT_ID('[dnt_getmytopics]','P') IS NOT NULL
DROP PROC [dnt_getmytopics]
GO

CREATE PROCEDURE [dnt_getmytopics]
@uid int,
@pageindex int,
@pagesize int
 AS
DECLARE @strSQL varchar(5000)
IF @pageindex = 1
BEGIN
	SET @strSQL = 'SELECT [tid], [fid], [iconid], [typeid], [readperm], [price], [poster], [posterid], [title], [postdatetime], [lastpost], [lastpostid], [lastposter], [lastposterid], [views], [replies], [displayorder], [highlight], [digest], [rate], [hide], [special], [attachment], [moderated], [closed], [magic]
					 FROM [dnt_topics] 
					WHERE [tid] IN (
							SELECT TOP ' + STR(@pagesize) + ' [tid] 
							FROM [dnt_mytopics] 
							WHERE [uid]=' + STR(@uid) + ' 
							ORDER BY [tid] DESC
									) 
					ORDER BY [tid] DESC'
END
ELSE
BEGIN
	SET @strSQL = 'SELECT [tid], [fid], [iconid], [typeid], [readperm], [price], [poster], [posterid], [title], [postdatetime], [lastpost], [lastpostid], [lastposter], [lastposterid], [views], [replies], [displayorder], [highlight], [digest], [rate], [hide], [special], [attachment], [moderated], [closed], [magic]
					 FROM [dnt_topics] 
					WHERE [tid] IN (
							SELECT TOP ' + STR(@pagesize) + ' [tid] 
							FROM [dnt_mytopics] 
							WHERE [uid]=' + STR(@uid) + ' 
							AND [tid] < (
										SELECT MIN([tid])
										FROM (
											SELECT TOP ' + STR((@pageindex-1)*@pagesize) + ' [tid] 
											FROM [dnt_mytopics]
											WHERE [uid]=' + STR(@uid) + ' 
											ORDER BY [tid] DESC
											 ) AS [ttt]
										)
							ORDER BY [tid] DESC
									) 
					ORDER BY [tid] DESC'

END
EXEC(@strSQL)
GO

IF OBJECT_ID('dnt_getpmcount','P') IS NOT NULL
DROP PROC dnt_getpmcount
GO

CREATE PROCEDURE dnt_getpmcount
@userid int,
@folder int=0,
@state int=-1
AS

IF @folder=-1
	BEGIN
	  SELECT COUNT(pmid) AS [pmcount] FROM [dnt_pms] WHERE ([msgtoid]=@userid AND [folder]=0) OR ([msgfromid] = @userid AND [folder] = 1) OR ([msgfromid] = @userid AND [folder] = 2)
	END
ELSE
    BEGIN
		IF @folder=0
			BEGIN
				IF @state=-1
					BEGIN
						SELECT COUNT(pmid) AS [pmcount] FROM [dnt_pms] WHERE [msgtoid]=@userid AND [folder]=@folder
					END
				ELSE IF @state=2
					BEGIN
						SELECT COUNT(pmid) AS [pmcount] FROM [dnt_pms] WHERE [msgtoid]=@userid AND [folder]=@folder AND [new]=1 AND GETDATE()-[postdatetime]<3
					END
				ELSE
					BEGIN
						SELECT COUNT(pmid) AS [pmcount] FROM [dnt_pms] WHERE [msgtoid]=@userid AND [folder]=@folder AND [new]=@state
					END
			END
		ELSE
			BEGIN
				IF @state=-1
					BEGIN
						SELECT COUNT(pmid) AS [pmcount] FROM [dnt_pms] WHERE [msgfromid]=@userid AND [folder]=@folder
					END
				ELSE IF @state=2
					BEGIN
						SELECT COUNT(pmid) AS [pmcount] FROM [dnt_pms] WHERE [msgfromid]=@userid AND [folder]=@folder AND [new]=1 AND GETDATE()-[postdatetime]<3
					END
				ELSE
					BEGIN
						SELECT COUNT(pmid) AS [pmcount] FROM [dnt_pms] WHERE [msgfromid]=@userid AND [folder]=@folder AND [new]=@state
					END
			END
	END
GO

IF OBJECT_ID('dnt_getnoticecount','P') IS NOT NULL
DROP PROC dnt_getnoticecount
GO

CREATE PROCEDURE dnt_getnoticecount
@userid int,
@type int = -1,
@state int=-1
AS

	IF @type = -1
		BEGIN
			IF @state = -1
				BEGIN
					SELECT COUNT(nid) AS [pmcount] FROM [dnt_notices] WHERE [uid]=@userid
				END
			ELSE
				BEGIN
					SELECT COUNT(nid) AS [pmcount] FROM [dnt_notices] WHERE [uid]=@userid AND [new]=@state
				END
		END
	ELSE
		BEGIN
			IF @state = -1
				BEGIN
					SELECT COUNT(nid) AS [pmcount] FROM [dnt_notices] WHERE [uid]=@userid AND [type]=@type
				END
			ELSE
				BEGIN
					SELECT COUNT(nid) AS [pmcount] FROM [dnt_notices] WHERE [uid]=@userid AND [new]=@state AND [type]=@type
				END
		END
GO

IF OBJECT_ID('dnt_getpmlist','P') IS NOT NULL
DROP PROC dnt_getpmlist
GO

CREATE PROCEDURE dnt_getpmlist
@userid int,
@folder int,
@pagesize int,
@pageindex int,
@inttype int
AS
DECLARE @strSQL varchar(5000)

DECLARE @msgformORtoID varchar(10)
SET @msgformortoid='msgtoid'

IF @folder=1 OR @folder=2
	BEGIN
		SET @msgformortoid='msgfromid'
	END

IF @pageindex = 1
	BEGIN
		IF (@inttype <> 1)
			SET @strSQL = 'SELECT TOP ' + STR(@pagesize) +' [pmid],[msgfrom],[msgfromid],[msgto],[msgtoid],[folder],[new],[subject],[postdatetime],[message] FROM [dnt_pms] WHERE [' + @msgformortoid + ']=' +STR(@userid) + ' AND [folder]=' +STR(@folder)  + ' ORDER BY [pmid] DESC'
		ELSE
			SET @strSQL = 'SELECT TOP ' + STR(@pagesize) +' [pmid],[msgfrom],[msgfromid],[msgto],[msgtoid],[folder],[new],[subject],[postdatetime],[message] FROM [dnt_pms] WHERE [' + @msgformortoid + ']=' +STR(@userid) + ' AND [folder]=' +STR(@folder) + ' ORDER BY [pmid] DESC'
		
	END
ELSE
	BEGIN
		SET @strSQL = 'SELECT TOP ' + STR(@pagesize) +' [pmid],[msgfrom],[msgfromid],[msgto],[msgtoid],[folder],[new],[subject],[postdatetime],[message] FROM [dnt_pms] WHERE [pmid] < (SELECT MIN([pmid]) FROM (SELECT TOP ' + STR((@pageindex-1)*@pagesize) + ' [pmid] FROM [dnt_pms] WHERE [' + @msgformortoid + ']=' +STR(@userid) + ' AND [folder]=' +STR(@folder) + ' ORDER BY [pmid] DESC) AS tblTmp) AND [' + @msgformortoid + ']=' +STR(@userid) + ' AND [folder]=' + STR(@folder) + ' ORDER BY [pmid] DESC'
		IF (@inttype <> 1)
			BEGIN
				SET @strSQL = 'SELECT TOP ' + STR(@pagesize) +' [pmid],[msgfrom],[msgfromid],[msgto],[msgtoid],[folder],[new],[subject],[postdatetime],[message] FROM [dnt_pms] WHERE [pmid] < (SELECT MIN([pmid]) FROM (SELECT TOP ' + STR((@pageindex-1)*@pagesize) + ' [pmid] FROM [dnt_pms] WHERE [' + @msgformortoid + ']=' +STR(@userid) + ' AND [folder]=' +STR(@folder) + ' ORDER BY [pmid] DESC) AS tblTmp) AND [' + @msgformortoid + ']=' +STR(@userid) + ' AND [folder]=' +STR(@folder) +  ' ORDER BY [pmid] DESC'
			END
	END

EXEC(@strSQL)
GO

IF OBJECT_ID('dnt_getpost1count','P') IS NOT NULL
DROP PROC dnt_getpost1count
GO

CREATE PROCEDURE dnt_getpost1count
@tid int
AS
SELECT COUNT(pid) FROM [dnt_posts1] WHERE [tid]=@tid AND [invisible]=0 AND layer>0
GO

IF OBJECT_ID('dnt_getpost1tree','P') IS NOT NULL
DROP PROC dnt_getpost1tree
GO

CREATE PROCEDURE dnt_getpost1tree
@tid int
AS
SELECT [pid], [layer], [title], [poster], [posterid],[postdatetime],[message] FROM [dnt_posts1] WHERE [tid]=@tid AND [invisible]=0 ORDER BY [parentid];
GO

IF OBJECT_ID('[dnt_getpostcountbycondition1]','P') IS NOT NULL
DROP PROC [dnt_getpostcountbycondition1]
GO

CREATE PROCEDURE [dnt_getpostcountbycondition1]
@tid int,
@posterid int
AS
SELECT COUNT(pid) FROM [dnt_posts1] WHERE [tid] = @tid AND [posterid] = @posterid  AND [layer]>=0
GO

IF OBJECT_ID('dnt_getshortuserinfo','P') IS NOT NULL
DROP PROC dnt_getshortuserinfo
GO

CREATE PROCEDURE dnt_getshortuserinfo
@uid int
AS
SELECT TOP 1 * FROM [dnt_users] WHERE [uid]=@uid
GO

if exists (select * from sysobjects where id = object_id(N'[dnt_getsinglepost1]') and OBJECTPROPERTY(id,N'IsProcedure') = 1)
drop procedure [dnt_getsinglepost1]
GO

CREATE PROCEDURE [dnt_getsinglepost1]
                    @tid int,
                    @pid int
                    AS
                    SELECT [aid], [tid], [pid], [postdatetime], [readperm], [filename], [description], [filetype], [filesize], [attachment], [downloads], [attachprice], [uid], [width], [height] FROM [dnt_attachments] WHERE [tid]=@tid

                    SELECT TOP 1 
	                                [dnt_posts1].[pid], 
	                                [dnt_posts1].[fid], 
	                                [dnt_posts1].[title], 
	                                [dnt_posts1].[layer],
	                                [dnt_posts1].[message], 
	                                [dnt_posts1].[ip], 
	                                [dnt_posts1].[lastedit], 
	                                [dnt_posts1].[postdatetime], 
	                                [dnt_posts1].[attachment], 
	                                [dnt_posts1].[poster], 
	                                [dnt_posts1].[invisible], 
	                                [dnt_posts1].[usesig], 
	                                [dnt_posts1].[htmlon], 
	                                [dnt_posts1].[smileyoff], 
	                                [dnt_posts1].[parseurloff], 
	                                [dnt_posts1].[bbcodeoff], 
	                                [dnt_posts1].[rate], 
	                                [dnt_posts1].[ratetimes], 
	                                [dnt_posts1].[posterid], 
	                                [dnt_users].[nickname],  
	                                [dnt_users].[username], 
	                                [dnt_users].[groupid],
                                    [dnt_users].[spaceid],
									[dnt_users].[gender],
									[dnt_users].[bday], 
	                                [dnt_users].[email], 
	                                [dnt_users].[showemail], 
	                                [dnt_users].[digestposts], 
	                                [dnt_users].[credits], 
	                                [dnt_users].[extcredits1], 
	                                [dnt_users].[extcredits2], 
	                                [dnt_users].[extcredits3], 
	                                [dnt_users].[extcredits4], 
	                                [dnt_users].[extcredits5], 
	                                [dnt_users].[extcredits6], 
	                                [dnt_users].[extcredits7], 
	                                [dnt_users].[extcredits8], 
	                                [dnt_users].[posts], 
	                                [dnt_users].[joindate], 
	                                [dnt_users].[onlinestate], 
	                                [dnt_users].[lastactivity], 
	                                [dnt_users].[invisible],
	                                [dnt_users].[oltime],
	                                [dnt_users].[lastvisit],
	                                [dnt_userfields].[avatar], 
	                                [dnt_userfields].[avatarwidth], 
	                                [dnt_userfields].[avatarheight], 
	                                [dnt_userfields].[medals], 
	                                [dnt_userfields].[sightml] AS signature, 
	                                [dnt_userfields].[location], 
	                                [dnt_userfields].[customstatus], 
	                                [dnt_userfields].[website], 
	                                [dnt_userfields].[icq], 
	                                [dnt_userfields].[qq], 
	                                [dnt_userfields].[msn], 
	                                [dnt_userfields].[yahoo], 
	                                [dnt_userfields].[skype] 
                    FROM [dnt_posts1] LEFT JOIN [dnt_users] ON [dnt_users].[uid]=[dnt_posts1].[posterid] LEFT JOIN [dnt_userfields] ON [dnt_userfields].[uid]=[dnt_users].[uid] WHERE [dnt_posts1].[pid]=@pid
GO

IF OBJECT_ID('dnt_getsitemapnewtopics','P') IS NOT NULL
DROP PROC dnt_getsitemapnewtopics
GO

CREATE PROCEDURE dnt_getsitemapnewtopics
@fidlist VARCHAR(500)
AS
IF @fidlist<>''
     BEGIN
      DECLARE @strSQL VARCHAR(5000)
      SET @strSQL = 'SELECT TOP 20 [tid], [fid], [title], [poster], [postdatetime], [lastpost], [replies], [views], [digest] FROM [dnt_topics] WHERE [fid] 
NOT IN ('+@fidlist +') ORDER BY [tid] DESC' 
     END
ELSE
     BEGIN
      SET @strSQL = 'SELECT TOP 20 [tid], [fid], [title], [poster], [postdatetime], [lastpost], [replies], [views], [digest] FROM [dnt_topics] ORDER BY [tid] 
DESC'
     END
  EXEC(@strSQL)
GO

IF OBJECT_ID('dnt_gettopiccount','P') IS NOT NULL
DROP PROC dnt_gettopiccount
GO

CREATE PROCEDURE dnt_gettopiccount
@fid int
AS
SELECT [curtopics] FROM [dnt_forums] WHERE [fid]=@fid
GO

IF OBJECT_ID('dnt_gettopiccountbycondition','P') IS NOT NULL
DROP PROC dnt_gettopiccountbycondition
GO

CREATE PROCEDURE dnt_gettopiccountbycondition
@fid int,
@state int=0,
@condition varchar(80)=null
AS
DECLARE @sql varchar(500)
IF @state=-1
	BEGIN
		set @sql ='SELECT COUNT(tid) FROM [dnt_topics] WHERE [fid]='+str(@fid)+' AND [displayorder]>-1 AND [closed]<=1'+@condition
	END
ELSE
	BEGIN
set @sql ='SELECT COUNT(tid) FROM [dnt_topics] WHERE [fid]='+str(@fid)+' AND [displayorder]>-1 AND [closed]='+str(@state)+' AND [closed]<=1'+@condition
	END
exec(@sql)
GO

IF OBJECT_ID('dnt_gettopiccountbytype','P') IS NOT NULL
DROP PROC dnt_gettopiccountbytype
GO

CREATE PROCEDURE dnt_gettopiccountbytype
@condition varchar(4000)
AS
DECLARE @sql varchar(4100)

set @sql ='SELECT COUNT(tid) FROM [dnt_topics] WITH (NOLOCK)WHERE [displayorder]>-1 AND [closed]<=1 '+@condition
exec(@sql)
GO

IF OBJECT_ID('[dnt_gettopiclistbycondition]','P') IS NOT NULL
DROP PROC [dnt_gettopiclistbycondition]
GO

CREATE PROCEDURE [dnt_gettopiclistbycondition]
@fid int,
@pagesize int,
@pageindex int,
@startnum int,
@condition varchar(80)
AS
DECLARE @strSQL varchar(5000)

IF @pageindex = 1
	BEGIN
		SET @strSQL = 'SELECT TOP ' + STR(@pagesize) +' [rate],[tid],[iconid],[typeid],[title],[price],[hide],[readperm],

[special],[poster],[posterid],[replies],[views],[postdatetime],[lastpost],[lastposter],

[lastpostid],[lastposterid],[replies],[highlight],[digest],[displayorder],[attachment],[closed],[magic],[special] FROM 

[dnt_topics] WHERE [fid]=' +STR(@fid) + ' AND [displayorder]=0' + @condition + ' ORDER BY [lastpostid] DESC'
	END
ELSE
	BEGIN
		SET @strSQL = 'SELECT TOP ' + STR(@pagesize) +'[rate], [tid],[iconid],[typeid],[title],[price],[hide],[readperm],

[special],[poster],[posterid],[replies],[views],[postdatetime],[lastpost],[lastposter],

[lastpostid],[lastposterid],[replies],[highlight],[digest],[displayorder],[attachment],[closed],[magic],[special] FROM 

[dnt_topics] WHERE [lastpostid] < (SELECT min([lastpostid])  FROM (SELECT TOP ' + STR

((@pageindex-1)*@pagesize-@startnum) + ' [lastpostid] FROM [dnt_topics] WHERE [fid]=' +STR

(@fid) + ' AND [displayorder]=0' + @condition + ' ORDER BY [lastpostid] DESC) AS tblTmp ) 

AND [fid]=' +STR(@fid) + ' AND [displayorder]=0' + @condition + ' ORDER BY [lastpostid] DESC'
	END
EXEC(@strSQL)
GO

IF OBJECT_ID('[dnt_gettopiclist]','P') IS NOT NULL
DROP PROC [dnt_gettopiclist]
GO

CREATE PROCEDURE [dnt_gettopiclist]
@fid int,
@pagesize int,
@startnum int,
@pageindex int
AS
DECLARE @strSQL varchar(5000)

IF @pageindex = 1
	BEGIN
		SET @strSQL = 'SELECT TOP ' + STR(@pagesize) +' [rate],[tid],[iconid],[typeid],[title],[price],[hide],[readperm],

[special],[poster],[posterid],[replies],[views],[postdatetime],[lastpost],[lastposter],

[lastpostid],[lastposterid],[replies],[highlight],[digest],[displayorder],[attachment],[closed],[magic],[special] FROM 

[dnt_topics] WHERE [fid]=' +STR(@fid) + ' AND [displayorder]=0  ORDER BY [lastpostid] DESC'
	END
ELSE
	BEGIN
		SET @strSQL = 'SELECT TOP ' + STR(@pagesize) +'[rate], [tid],[iconid],[typeid],[title],[price],[hide],[readperm],

[special],[poster],[posterid],[replies],[views],[postdatetime],[lastpost],[lastposter],

[lastpostid],[lastposterid],[replies],[highlight],[digest],[displayorder],[attachment],[closed],[magic],[special] FROM 

[dnt_topics] WHERE [lastpostid] < (SELECT min([lastpostid])  FROM (SELECT TOP ' + STR

((@pageindex-1)*@pagesize-@startnum) + ' [lastpostid] FROM [dnt_topics] WHERE [fid]=' +STR

(@fid) + ' AND [displayorder]=0 ORDER BY [lastpostid] DESC) AS tblTmp ) 

AND [fid]=' +STR(@fid) + ' AND [displayorder]=0 ORDER BY [lastpostid] DESC'
	END
EXEC(@strSQL)
GO

IF OBJECT_ID('dnt_gettopiclistbydate','P') IS NOT NULL
DROP PROC dnt_gettopiclistbydate
GO

CREATE PROCEDURE dnt_gettopiclistbydate
@fid int,
@pagesize int,
@pageindex int,
@startnum int,
@condition varchar(100),
@orderby varchar(100),
@ascdesc int
AS

DECLARE @strsql varchar(5000)
DECLARE @sorttype varchar(5)

IF @ascdesc=0
   SET @sorttype='ASC'
ELSE
   SET @sorttype='DESC'

IF @pageindex = 1
	BEGIN
		SET @strsql = 'SELECT TOP ' + STR(@pagesize) +' [tid],[iconid],[title],[price],[typeid],[readperm],[hide],[special],[poster],[posterid],[replies],[views],[postdatetime],[lastpost],[lastposter],[lastpostid],[lastposterid],[replies],[highlight],[digest],[displayorder],[attachment],[closed],[magic],[rate] FROM [dnt_topics] WHERE [fid]=' +STR(@fid) + ' AND [displayorder]=0'+@condition+' ORDER BY '+@orderby+' '+@sorttype
	END
ELSE
           IF @sorttype='DESC'
	BEGIN
		SET @strSQL = 'SELECT TOP ' + STR(@pagesize) +' [tid],[iconid],[title],[price],[typeid],[hide],[readperm],[special],[poster],[posterid],[replies],[views],[postdatetime],[lastpost],[lastposter],[lastpostid],[lastposterid],[replies],[highlight],[digest],[displayorder],[attachment],[closed],[magic],[rate] FROM [dnt_topics] WHERE ['+@orderby+'] < (SELECT min(['+@orderby+']) FROM (SELECT TOP ' + STR((@pageindex-1)*@pagesize-@startnum) + ' ['+@orderby+']  FROM [dnt_topics] WHERE [fid]=' +STR(@fid) + ' AND [displayorder]=0'+@condition+' ORDER BY  '+@orderby+' '+@sorttype+')AS tblTmp ) AND [fid]=' +STR(@fid) + ' AND [displayorder]=0'+@condition+' ORDER BY  '+@orderby+' '+@sorttype
	END
      ELSE
             BEGIN
		SET @strSQL = 'SELECT TOP ' + STR(@pagesize) +' [tid],[iconid],[title],[price],[hide],[typeid],[readperm],[special],[poster],[posterid],[replies],[views],[postdatetime],[lastpost],[lastposter],[lastpostid],[lastposterid],[replies],[highlight],[digest],[displayorder],[attachment],[closed],[magic],[rate] FROM [dnt_topics] WHERE ['+@orderby+'] > (SELECT MAX(['+@orderby+']) FROM (SELECT TOP ' + STR((@pageindex-1)*@pagesize-@startnum) + ' ['+@orderby+'] FROM [dnt_topics] WHERE [fid]=' +STR(@fid) + ' AND [displayorder]=0'+@condition+' ORDER BY  '+@orderby+' '+@sorttype+')AS tblTmp ) AND [fid]=' +STR(@fid) + ' AND [displayorder]=0'+@condition+' ORDER BY '+@orderby+' '+@sorttype
            END
EXEC(@strsql)
GO

IF OBJECT_ID('[dnt_gettopiclistbytag]','P') IS NOT NULL
DROP PROC [dnt_gettopiclistbytag]
GO

CREATE PROCEDURE [dnt_gettopiclistbytag]	
	@tagid int,
	@pageindex int,
	@pagesize int
AS
BEGIN
	DECLARE @strSQL varchar(2000)
	IF @pageindex = 1
	BEGIN
		SET @strSQL='SELECT TOP ' + STR(@pagesize) + ' [t].[tid], [t].[title],[t].[poster],[t].[posterid],[t].[fid],[t].[postdatetime],[t].[replies],[t].[views],[t].[lastposter],[t].[lastposterid],[t].[lastpost] 
		FROM [dnt_topictags] AS [tt], [dnt_topics] AS [t] 
		WHERE [t].[tid] = [tt].[tid] AND [t].[displayorder] >=0 AND [tt].[tagid] = ' + STR(@tagid) + ' 
		ORDER BY [t].[lastpostid] DESC'
	END
	ELSE
	BEGIN
		SET @strSQL='SELECT TOP ' + STR(@pagesize) + ' [t].[tid], [t].[title],[t].[poster],[t].[posterid],[t].[fid],[t].[postdatetime],[t].[replies],[t].[views],[t].[lastposter],[t].[lastposterid],[t].[lastpost] 
		FROM [dnt_topictags] AS [tt], [dnt_topics] AS [t] 
		WHERE [t].[tid] = [tt].[tid] AND [t].[displayorder] >=0 AND [tt].[tagid] = ' + STR(@tagid) + ' 
		AND [t].[lastpostid] < (SELECT MIN([lastpostid]) FROM (SELECT TOP ' + STR((@pageindex-1)*@pagesize) + ' 
		[lastpostid] FROM [dnt_topictags] AS [tt], [dnt_topics] AS [t] 
		WHERE [t].[tid] = [tt].[tid] AND [t].[displayorder] >=0 AND [tt].[tagid] = ' + STR(@tagid) + ' 
		ORDER BY [t].[lastpostid] DESC) as tblTmp) 
		ORDER BY [t].[lastpostid] DESC'
	END
	EXEC(@strSQL)
END
GO

IF OBJECT_ID('[dnt_gettopiclistbytype]','P') IS NOT NULL
DROP PROC [dnt_gettopiclistbytype]
GO

CREATE PROCEDURE [dnt_gettopiclistbytype]
@pagesize int,
@pageindex int,
@startnum int,
@condition varchar(1000),
@ascdesc int
AS
DECLARE @strSQL varchar(5000)

DECLARE @sorttype varchar(5)

IF @ascdesc=0
   SET @sorttype='ASC'
ELSE
   SET @sorttype='DESC'

IF @pageindex = 1
	BEGIN
		SET @strSQL = 'SELECT TOP ' + STR(@pagesize) +' [tid],[iconid],[typeid],[title],[price],[hide],[readperm],

[special],[poster],[posterid],[replies],[views],[postdatetime],[lastpost],[lastposter],

[lastpostid],[lastposterid],[replies],[highlight],[digest],[displayorder],[attachment],[closed],[magic],[rate] FROM 

[dnt_topics] WITH (NOLOCK) WHERE  [displayorder]>=0' + @condition + ' ORDER BY [lastpostid] '+@sorttype +',  [tid] '+@sorttype
	END
ELSE
	BEGIN
		IF @sorttype='DESC'
		BEGIN
			SET @strSQL = 'SELECT TOP ' + STR(@pagesize) +' [tid],[iconid],[typeid],[title],[price],[hide],[readperm],
	
	[special],[poster],[posterid],[replies],[views],[postdatetime],[lastpost],[lastposter],
	
	[lastpostid],[lastposterid],[replies],[highlight],[digest],[displayorder],[attachment],[closed],[magic],[rate] FROM 
	
	[dnt_topics] WITH (NOLOCK) WHERE [lastpostid] < (SELECT min([lastpostid])  FROM (SELECT TOP ' + STR
	
	((@pageindex-1)*@pagesize-@startnum) + ' [lastpostid] FROM [dnt_topics] WHERE  [displayorder]>=0' + @condition + ' ORDER BY [lastpostid] ' + @sorttype + ', [tid] ' + @sorttype + ') AS tblTmp ) 
	
	AND  [displayorder]>=0' + @condition +' ORDER BY [lastpostid] '+@sorttype +',  [tid] '+@sorttype
		END
		ELSE
		BEGIN
			SET @strSQL = 'SELECT TOP ' + STR(@pagesize) +' [tid],[iconid],[typeid],[title],[price],[hide],[readperm],
	
	[special],[poster],[posterid],[replies],[views],[postdatetime],[lastpost],[lastposter],
	
	[lastpostid],[lastposterid],[replies],[highlight],[digest],[displayorder],[attachment],[closed],[magic] FROM 
	
	[dnt_topics] WITH (NOLOCK) WHERE [lastpostid] > (SELECT MAX([lastpostid])  FROM (SELECT TOP ' + STR
	
	((@pageindex-1)*@pagesize-@startnum) + ' [lastpostid] FROM [dnt_topics] WHERE  [displayorder]>=0' + @condition + ' ORDER BY [lastpostid] ' + @sorttype + ' , [tid] ' + @sorttype + ') AS tblTmp ) 
	
	AND  [displayorder]>=0' + @condition +' ORDER BY [lastpostid] '+@sorttype +',  [tid] '+@sorttype
		END
	END
EXEC(@strSQL)
GO

IF OBJECT_ID('dnt_gettopiclistbytypedate','P') IS NOT NULL
DROP PROC dnt_gettopiclistbytypedate
GO

CREATE PROCEDURE dnt_gettopiclistbytypedate
@pagesize int,
@pageindex int,
@startnum int,
@condition varchar(1000),
@orderby varchar(100),
@ascdesc int
AS

DECLARE @strsql varchar(5000)
DECLARE @sorttype varchar(5)

IF @ascdesc=0
   SET @sorttype='ASC'
ELSE
   SET @sorttype='DESC'

IF @pageindex = 1
	BEGIN
		SET @strsql = 'SELECT TOP ' + STR(@pagesize) +' [tid],[iconid],[typeid],[title],[special],[price],[hide],[readperm],[poster],[posterid],[replies],[views],[postdatetime],[lastpost],[lastposter],[lastpostid],[lastposterid],[highlight],[digest],[displayorder],[closed],[attachment],[magic],[rate] FROM [dnt_topics] WHERE [displayorder]>=0'+@condition+' ORDER BY '+@orderby+' '+@sorttype
	END
ELSE
           IF @sorttype='DESC'
	BEGIN
		SET @strSQL = 'SELECT TOP ' + STR(@pagesize) +' [tid],[iconid],[typeid],[title],[special],[price],[hide],[readperm],[poster],[posterid],[replies],[views],[postdatetime],[lastpost],[lastposter],[lastpostid],[lastposterid],[highlight],[digest],[displayorder],[closed],[attachment],[magic],[rate] FROM [dnt_topics] WHERE ['+@orderby+'] < (SELECT min(['+@orderby+']) FROM (SELECT TOP ' + STR((@pageindex-1)*@pagesize-@startnum) + ' ['+@orderby+']  FROM [dnt_topics] WHERE  [displayorder]>=0'+@condition+' ORDER BY  '+@orderby+' '+@sorttype+')AS tblTmp ) AND [displayorder]>=0'+@condition+' ORDER BY  '+@orderby+' '+@sorttype
	END
      ELSE
             BEGIN
		SET @strSQL = 'SELECT TOP ' + STR(@pagesize) +' [tid],[iconid],[typeid],[title],[special],[price],[hide],[readperm],[poster],[posterid],[replies],[views],[postdatetime],[lastpost],[lastposter],[lastpostid],[lastposterid],[highlight],[digest],[displayorder],[closed],[attachment],[magic],[rate] FROM [dnt_topics] WHERE ['+@orderby+'] > (SELECT MAX(['+@orderby+']) FROM (SELECT TOP ' + STR((@pageindex-1)*@pagesize-@startnum) + ' ['+@orderby+'] FROM [dnt_topics] WHERE [displayorder]>=0'+@condition+' ORDER BY  '+@orderby+' '+@sorttype+')AS tblTmp ) AND [displayorder]>=0'+@condition+' ORDER BY '+@orderby+' '+@sorttype
            END

EXEC(@strsql)
GO

IF OBJECT_ID('dnt_gettoptopiclist','P') IS NOT NULL
DROP PROC [dnt_gettoptopiclist]
GO

CREATE PROCEDURE dnt_gettoptopiclist
@fid int,
@pagesize int,
@pageindex int,
@tids varchar(500)
AS
DECLARE @strSQL varchar(5000)

IF @pageindex = 1
	BEGIN
		SET @strSQL = 'SELECT TOP ' + STR(@pagesize) + '[rate], [tid],[fid],[typeid],[iconid],[title],[price],[hide],[readperm], [special],[poster],[posterid],[views],[postdatetime],[lastpost],[lastposter],[lastpostid],[lastposterid],[replies],[highlight],[digest],[displayorder],[closed],[attachment],[magic] FROM [dnt_topics] WHERE [displayorder]>0 AND charindex('',''+CAST([tid] AS varchar(10))+'','' , '','+@tids+','')>0  ORDER BY [displayorder] DESC, [lastpost] DESC'
	END
ELSE
	BEGIN
		SET @strSQL = 'SELECT TOP ' + STR(@pagesize) + ' [rate], [tid],[fid],[typeid],[iconid],[title],[price],[hide],[readperm], [special],[poster],[posterid],[views],[postdatetime],[lastpost],[lastposter],[lastpostid],[lastposterid],[replies],[highlight],[digest],[displayorder],[closed],[attachment],[magic] FROM [dnt_topics]
WHERE (EXISTS   (SELECT TOP ' + STR((@pageindex-1)*@pagesize) + ' [tid]
                 FROM [dnt_topics]
                 WHERE [displayorder] > 0 AND  charindex('',''+CAST([tid] AS varchar(10))+'','' , '','+@tids+','')>0
                 ORDER BY [lastpost] DESC)) AND (displayorder > 0) AND 
      ( charindex('',''+CAST([tid] AS varchar(10))+'','' , '','+@tids+','')>0)
ORDER BY [displayorder] DESC, [lastpost] DESC'
	
	END

EXEC(@strSQL)
GO

IF OBJECT_ID('[dnt_getuserinfo]','P') IS NOT NULL
DROP PROC [dnt_getuserinfo]
GO

CREATE PROCEDURE [dnt_getuserinfo]
@uid int
AS
SELECT TOP 1 
[dnt_users].[accessmasks], 
[dnt_users].[adminid],
[dnt_users].[avatarshowid],
[dnt_users].[bday],
[dnt_users].[credits],
[dnt_users].[digestposts],
[dnt_users].[email],
[dnt_users].[extcredits1],
[dnt_users].[extcredits2],
[dnt_users].[extcredits3],
[dnt_users].[extcredits4],
[dnt_users].[extcredits5],
[dnt_users].[extcredits6],
[dnt_users].[extcredits7],
[dnt_users].[extcredits8],
[dnt_users].[extgroupids],
[dnt_users].[gender],
[dnt_users].[groupexpiry],
[dnt_users].[groupid],
[dnt_users].[invisible],
[dnt_users].[joindate],
[dnt_users].[lastactivity],
[dnt_users].[lastip],
[dnt_users].[lastpost],
[dnt_users].[lastpostid],
[dnt_users].[lastposttitle],
[dnt_users].[lastvisit],
[dnt_users].[newpm],
[dnt_users].[newpmcount],
[dnt_users].[newsletter],
[dnt_users].[nickname],
[dnt_users].[oltime],
[dnt_users].[onlinestate],
[dnt_users].[pageviews],
[dnt_users].[password],
[dnt_users].[pmsound],
[dnt_users].[posts],
[dnt_users].[ppp],
[dnt_users].[regip],
[dnt_users].[secques],
[dnt_users].[showemail],
[dnt_users].[sigstatus],
[dnt_users].[spaceid],
[dnt_users].[templateid],
[dnt_users].[tpp],
[dnt_users].[uid],
[dnt_users].[username],
[dnt_users].[salt],
[dnt_userfields].[authflag],
[dnt_userfields].[authstr],
[dnt_userfields].[authtime],
[dnt_userfields].[avatar],
[dnt_userfields].[avatarheight],
[dnt_userfields].[avatarwidth],
[dnt_userfields].[bio],
[dnt_userfields].[customstatus],
[dnt_userfields].[icq],
[dnt_userfields].[idcard],
[dnt_userfields].[ignorepm],
[dnt_userfields].[location],
[dnt_userfields].[medals],
[dnt_userfields].[mobile],
[dnt_userfields].[msn],
[dnt_userfields].[phone],
[dnt_userfields].[qq],
[dnt_userfields].[realname],
[dnt_userfields].[sightml],
[dnt_userfields].[signature],
[dnt_userfields].[skype],
[dnt_userfields].[uid],
[dnt_userfields].[website],
[dnt_userfields].[yahoo]
FROM [dnt_users] LEFT JOIN [dnt_userfields] ON [dnt_users].[uid]=[dnt_userfields].[uid] 
WHERE [dnt_users].[uid]=@uid
GO

IF OBJECT_ID('dnt_getuserlist','P') IS NOT NULL
DROP PROC [dnt_getuserlist]
GO

CREATE PROCEDURE [dnt_getuserlist]
@pagesize int,
@pageindex int,
@column varchar(20),
@ordertype AS varchar(5)
AS
DECLARE @pagetop int
DECLARE @sql varchar(1000)

SET @pagetop = (@pageindex-1)*@pagesize

IF @column = 'uid'
BEGIN
	SET @column = '[dnt_users].[uid]'
END
ELSE IF @column = 'admin'
BEGIN
	SET @column = '[adminid]'
END

IF @pageindex = 1
	BEGIN
	SET @sql = 'SELECT TOP ' + CAST(@pagesize AS varchar(10)) + ' [dnt_users].[uid], 
									[dnt_users].[groupid],
									[dnt_users].[username], 
									[dnt_users].[nickname], 
									[dnt_users].[joindate], 
									[dnt_users].[credits], 
									[dnt_users].[posts], 
									[dnt_users].[lastactivity], 
									[dnt_users].[email], 
									[dnt_users].[oltime], 
									[dnt_userfields].[location] 
					FROM [dnt_users] LEFT JOIN [dnt_userfields] ON [dnt_userfields].[uid] = [dnt_users].[uid] ORDER BY ' + @column + ' ' + @ordertype
	END
ELSE
	BEGIN
		SET @sql = 'SELECT TOP ' + CAST(@pagesize AS varchar(10)) + ' [dnt_users].[uid], 
									[dnt_users].[groupid], 
									[dnt_users].[username], 
									[dnt_users].[nickname], 
									[dnt_users].[joindate], 
									[dnt_users].[credits], 
									[dnt_users].[posts], 
									[dnt_users].[email], 
									[dnt_users].[lastactivity], 
									[dnt_users].[oltime], 
									[dnt_userfields].[location] 
					FROM [dnt_users] LEFT JOIN [dnt_userfields] ON [dnt_userfields].[uid] = [dnt_users].[uid] WHERE [dnt_users].[uid] NOT IN (SELECT TOP ' + CAST(@pagetop AS varchar(10)) + ' [uid] FROM [dnt_users] ORDER BY ' + @column + ' ' + @ordertype + ')ORDER BY '+ @column + ' ' + @ordertype

	END
	
	EXEC(@sql)
GO

IF OBJECT_ID('[dnt_neatenrelatetopic]','P') IS NOT NULL
DROP PROC [dnt_neatenrelatetopic]
GO

CREATE PROCEDURE [dnt_neatenrelatetopic]
AS
BEGIN
	DECLARE @tagid int
	DECLARE [tag_cursor] CURSOR FOR	
	SELECT DISTINCT [tagid] FROM [dnt_topictags]
	OPEN [tag_cursor]
	FETCH NEXT FROM [tag_cursor] INTO @tagid
	WHILE @@FETCH_STATUS = 0
		BEGIN
			INSERT INTO [dnt_topictagcaches] 
			SELECT [t1].[tid],[t2].[tid],[t2].[title] FROM (SELECT [tid] FROM [dnt_topictags]
				WHERE [tagid] = @tagid) AS [t1],(SELECT [t].[tid],[t].[title] FROM [dnt_topics] AS [t],[dnt_topictags] AS [tt] 
				WHERE [tt].[tagid] = @tagid AND [t].[tid] = [tt].[tid] AND [t].[displayorder] >=0) AS [t2] 
			WHERE [t1].[tid] <> [t2].[tid] AND NOT EXISTS (SELECT 1 FROM [dnt_topictagcaches] WHERE [tid]=[t1].[tid] AND [linktid]=[t2].[tid])
			

			FETCH NEXT FROM [tag_cursor] INTO @tagid
		END;
	CLOSE [tag_cursor]
	DEALLOCATE [tag_cursor]

END
GO

IF OBJECT_ID('[dnt_revisedebatetopicdiggs]','P') IS NOT NULL
DROP PROC [dnt_revisedebatetopicdiggs]
GO

CREATE PROCEDURE [dnt_revisedebatetopicdiggs]
	@tid int,
	@opinion int,
	@count int out
AS
BEGIN
	SELECT @count=COUNT(1) FROM [dnt_postdebatefields] WHERE [tid] = @tid AND [opinion] = @opinion
	
	IF @opinion=1
	BEGIN
		UPDATE [dnt_debates] SET [positivediggs]=(SELECT SUM(diggs + 1) FROM [dnt_postdebatefields] WHERE [tid] = @tid AND [opinion] = @opinion) WHERE [tid] = @tid
	END
	ELSE
	BEGIN
		UPDATE [dnt_debates] SET [negativediggs]=(SELECT SUM(diggs + 1) FROM [dnt_postdebatefields] WHERE [tid] = @tid AND [opinion] = @opinion) WHERE [tid] = @tid
	END
END
GO

IF OBJECT_ID('[dnt_setlastexecutescheduledeventdatetime]','P') IS NOT NULL
DROP PROC [dnt_setlastexecutescheduledeventdatetime]
GO

CREATE PROCEDURE [dnt_setlastexecutescheduledeventdatetime]
(
	@key VARCHAR(100),
	@servername VARCHAR(100),
	@lastexecuted DATETIME
)
AS
DELETE FROM [dnt_scheduledevents] WHERE ([key]=@key) AND ([lastexecuted] < DATEADD([day], - 7, GETDATE()))

INSERT [dnt_scheduledevents] ([key], [servername], [lastexecuted]) Values (@key, @servername, @lastexecuted)
GO

IF OBJECT_ID('dnt_shrinklog','P') IS NOT NULL
DROP PROC dnt_shrinklog
GO

CREATE PROCEDURE dnt_shrinklog  
@DBName  nchar(50) 
AS
Begin
	exec('BACKUP LOG ['+@DBName+']  WITH NO_LOG')
	exec('DBCC  SHRINKDATABASE(['+@DBName+'])')
End
GO

IF OBJECT_ID('dnt_updateadmingroup','P') IS NOT NULL
DROP PROC dnt_updateadmingroup
GO

CREATE PROCEDURE dnt_updateadmingroup
	@admingid smallint,
	@alloweditpost tinyint,
	@alloweditpoll tinyint,
	@allowstickthread tinyint,
	@allowmodpost tinyint,
	@allowdelpost tinyint,
	@allowmassprune tinyint,
	@allowrefund tinyint,
	@allowcensorword tinyint,
	@allowviewip tinyint,
	@allowbanip tinyint,
	@allowedituser tinyint,
	@allowmoduser tinyint,
	@allowbanuser tinyint,
	@allowpostannounce tinyint,
	@allowviewlog tinyint,
	@disablepostctrl tinyint,
	@allowviewrealname tinyint
AS
UPDATE dnt_admingroups SET 
	[alloweditpost]=@alloweditpost,
	[alloweditpoll]=@alloweditpoll,
	[allowstickthread]=@allowstickthread,
	[allowmodpost]=@allowmodpost,
	[allowdelpost]=@allowdelpost,
	[allowmassprune]=@allowmassprune,
	[allowrefund]=@allowrefund,
	[allowcensorword]=@allowcensorword,
	[allowviewip]=@allowviewip,
	[allowbanip]=@allowbanip,
	[allowedituser]=@allowedituser,
	[allowmoduser]=@allowmoduser,
	[allowbanuser]=@allowbanuser,
	[allowpostannounce]=@allowpostannounce,
	[allowviewlog]=@allowviewlog,
	[disablepostctrl]=@disablepostctrl,
	[allowviewrealname]=@allowviewrealname
WHERE [admingid]=@admingid
GO

IF OBJECT_ID('dnt_updatepost1','P') IS NOT NULL
DROP PROC dnt_updatepost1
GO

CREATE PROCEDURE dnt_updatepost1
	@pid int,
	@title nvarchar(160),
	@message ntext,
	@lastedit nvarchar(50),
	@invisible int,
	@usesig int,
	@htmlon int,
	@smileyoff int,
	@bbcodeoff int,
	@parseurloff int
AS
UPDATE dnt_posts1 SET 
	[title]=@title,
	[message]=@message,
	[lastedit]=@lastedit,
	[invisible]=@invisible,
	[usesig]=@usesig,
	[htmlon]=@htmlon,
	[smileyoff]=@smileyoff,
	[bbcodeoff]=@bbcodeoff,
	[parseurloff]=@parseurloff WHERE [pid]=@pid
GO

IF OBJECT_ID('dnt_updatetopic','P') IS NOT NULL
DROP PROC dnt_updatetopic
GO

CREATE PROCEDURE dnt_updatetopic
	@tid int,
	@fid smallint,
	@iconid smallint,
	@title nchar(60),
	@typeid smallint,
	@readperm int,
	@price smallint,
	@poster char(20),
	@posterid int,
	@postdatetime smalldatetime,
	@lastpostid int,
	@lastpost smalldatetime,
	@lastposter char(20),
	@replies int,
	@displayorder int,
	@highlight varchar(500),
	@digest int,
	@rate int,
	@hide int,
        @special int,
	@attachment int,
	@moderated int,
	@closed int,
	@magic int
AS
UPDATE dnt_topics SET
	[fid]=@fid,
	[iconid]=@iconid,
	[title]=@title,
	[typeid]=@typeid,
	[readperm]=@readperm,
	[price]=@price,
	[poster]=@poster,
	[posterid]=@posterid,
	[postdatetime]=@postdatetime,
	[lastpostid]=@lastpostid,
	[lastpost]=@lastpost,
	[lastposter]=@lastposter,
	[replies]=@replies,
	[displayorder]=@displayorder,
	[highlight]=@highlight,
	[digest]=@digest,
	[rate]=@rate,
	[hide]=@hide,
    [special]=@special,
	[attachment]=@attachment,
	[moderated]=@moderated,
	[closed]=@closed,
	[magic]=@magic WHERE [tid]=@tid 
GO

IF OBJECT_ID('dnt_updatetopicviewcount','P') IS NOT NULL
DROP PROC dnt_updatetopicviewcount
GO

CREATE PROCEDURE dnt_updatetopicviewcount
@tid int,
@viewcount int
AS
UPDATE [dnt_topics]  SET [views]= [views] + @viewcount WHERE [tid]=@tid
GO

IF OBJECT_ID('dnt_updateuserauthstr','P') IS NOT NULL
DROP PROC dnt_updateuserauthstr
GO

CREATE PROCEDURE dnt_updateuserauthstr
	@uid int,
	@authstr char(20),
	@authflag int =1
AS
UPDATE [dnt_userfields] SET [authstr]=@authstr, [authtime] = getdate(), [authflag]=@authflag WHERE [uid]=@uid
GO

IF OBJECT_ID('[dnt_updateuserforumsetting]','P') IS NOT NULL
DROP PROC [dnt_updateuserforumsetting]
GO

CREATE PROCEDURE [dnt_updateuserforumsetting]
	@uid int,
	@tpp int,
	@ppp int,
	@invisible int,
	@customstatus varchar(30)
AS
UPDATE [dnt_users] SET [tpp]=@tpp, [ppp]=@ppp, [invisible]=@invisible WHERE [uid]=@uid
UPDATE [dnt_userfields] SET [customstatus]=@customstatus WHERE [uid]=@uid
GO

IF OBJECT_ID('dnt_updateuserpassword','P') IS NOT NULL
DROP PROC dnt_updateuserpassword
GO

CREATE PROCEDURE dnt_updateuserpassword
	@uid int,
	@password char(44)
AS
UPDATE [dnt_users] SET [password]=@password WHERE [uid]=@uid
GO

IF OBJECT_ID('dnt_updateuserpreference','P') IS NOT NULL
DROP PROC dnt_updateuserpreference
GO

CREATE PROCEDURE dnt_updateuserpreference
	@uid int,
	@avatar varchar(255),
	@avatarwidth int,
	@avatarheight int,
	@templateid int
AS
UPDATE [dnt_userfields] SET [avatar]=@avatar, [avatarwidth]=@avatarwidth, [avatarheight]=@avatarheight WHERE [uid]=@uid
UPDATE [dnt_users] SET [templateid]=@templateid WHERE [uid]=@uid
GO

IF OBJECT_ID('[dnt_updateuserprofile]','P') IS NOT NULL
DROP PROC [dnt_updateuserprofile]
GO

CREATE PROCEDURE [dnt_updateuserprofile]
	@uid int,
	@nickname nchar(20),
	@gender int,
	@email char(50),
	@bday char(10),
	@showemail int,
	@website nvarchar(80),
	@icq varchar(12),
	@qq varchar(12),
	@yahoo varchar(40),
	@msn varchar(40),
	@skype varchar(40),
	@location nvarchar(30),
	@bio nvarchar(500),
	@signature nvarchar(500),
	@sigstatus int,
	@sightml nvarchar(1000),
	@realname nvarchar(10),
	@idcard varchar(20),
	@mobile varchar(20),
	@phone varchar(20)
AS
UPDATE [dnt_users] SET [nickname]=@nickname, [gender]=@gender , [email]=@email , [bday]=@bday, [sigstatus]=@sigstatus, [showemail]=@showemail WHERE [uid]=@uid
UPDATE [dnt_userfields] SET [website]=@website , [icq]=@icq , [qq]=@qq , [yahoo]=@yahoo , [msn]=@msn , [skype]=@skype , [location]=@location , [bio]=@bio, [signature]=@signature, [sightml]=@sightml, [realname]=@realname,[idcard]=@idcard,[mobile]=@mobile,[phone]=@phone  WHERE [uid]=@uid
GO

IF OBJECT_ID('dnt_getforumnewtopics','P') IS NOT NULL
DROP PROC dnt_getforumnewtopics
GO

CREATE PROCEDURE dnt_getforumnewtopics
@fid int
AS
SELECT TOP 20 [dnt_topics].[tid],[dnt_topics].[title],[dnt_topics].[poster],[dnt_topics].[postdatetime],[dnt_posts1].[message] FROM [dnt_topics] LEFT JOIN [dnt_posts1] ON [dnt_topics].[tid]=[dnt_posts1].[tid]  WHERE [dnt_posts1].[layer]=0 AND  [dnt_topics].[fid]=@fid ORDER BY [lastpost] DESC
GO

IF OBJECT_ID('[dnt_createtopictags]','P') IS NOT NULL
DROP PROC [dnt_createtopictags]
GO

CREATE PROCEDURE [dnt_createtopictags]
@tags nvarchar(55),
@tid int,
@userid int,
@postdatetime datetime
AS
BEGIN
	exec [dnt_createtags] @tags, @userid, @postdatetime

	UPDATE [dnt_tags] SET [fcount]=[fcount]+1,[count]=[count]+1
	WHERE EXISTS (SELECT [item] FROM [dnt_split](@tags, ' ') AS [newtags] WHERE [newtags].[item] = [tagname])

	INSERT INTO [dnt_topictags] (tagid, tid)
	SELECT tagid, @tid FROM [dnt_tags] WHERE EXISTS (SELECT * FROM [dnt_split](@tags, ' ') WHERE [item] = [dnt_tags].[tagname])
END
GO

IF OBJECT_ID('dnt_getfavoriteslistbyalbum','P') IS NOT NULL
DROP PROC dnt_getfavoriteslistbyalbum
GO

CREATE PROCEDURE dnt_getfavoriteslistbyalbum
@uid int,
@pagesize int,
@pageindex int
AS
DECLARE @strSQL varchar(5000)


SET @strSQL='SELECT [f].[tid], [f].[uid], [albumid], [albumcateid], [userid] AS [posterid], [username] AS [poster], [title], [description], [logo], [password], [imgcount], [views], [type], [createdatetime] AS [postdatetime] FROM [dnt_favorites] [f],[dnt_albums] [albums] WHERE [f].[tid]=[albums].[albumid] AND [f].[typeid]=1 AND [f].[uid]=' + STR(@uid)

IF @pageindex = 1
	BEGIN
		SET @strSQL = 'SELECT TOP ' + STR(@pagesize) +'  [tid], [uid], [albumid], [albumcateid], [posterid], [poster], [title], [description], [logo], [password], [imgcount], [views], [type], [postdatetime]  FROM (' + @strSQL + ') f' + '  ORDER BY [tid] DESC'
	END
ELSE
	BEGIN
		SET @strSQL = 'SELECT TOP ' + STR(@pagesize) +'  [tid], [uid], [albumid], [albumcateid], [posterid], [poster], [title], [description], [logo], [password], [imgcount], [views], [type], [postdatetime]  FROM (' + @strSQL + ') f1 WHERE [tid] < (SELECT MIN([tid]) FROM (SELECT TOP ' + STR((@pageindex-1)*@pagesize) + ' [tid] FROM (' + @strSQL + ') f2' + '  ORDER BY [tid] DESC) AS tblTmp)' + '  ORDER BY [tid] DESC'
	END

EXEC(@strSQL)
GO

if exists(select * from sysobjects where id=object_id(N'[dnt_getnewtopics1]')and OBJECTPROPERTY(id,N'IsProcedure') = 1) 
drop procedure [dnt_getnewtopics1]
GO

CREATE PROCEDURE [dnt_getnewtopics1]
	@fidlist VARCHAR(500)
	AS
	IF @fidlist <> ''
	BEGIN
	SELECT TOP 20 
		[dnt_posts1].[tid], 
        [dnt_posts1].[title], 
        [dnt_posts1].[poster], 
        [dnt_posts1].[postdatetime], 
        [dnt_posts1].[message],
        [dnt_forums].[name] 
        FROM [dnt_posts1]  
        LEFT JOIN [dnt_forums] ON [dnt_posts1].[fid]=[dnt_forums].[fid]
        LEFT JOIN [dnt_topics] ON [dnt_posts1].[tid]=[dnt_topics].[tid]
        WHERE CHARINDEX(','+RTRIM([dnt_forums].[fid])+',', ','+@fidlist+',') > 0 
        AND [dnt_posts1].[layer]=0 AND [dnt_topics].[displayorder] >= 0
        ORDER BY [dnt_posts1].[tid] DESC
    END 
    ELSE
    BEGIN
	SELECT TOP 20 
		[dnt_posts1].[tid], 
        [dnt_posts1].[title], 
        [dnt_posts1].[poster], 
        [dnt_posts1].[postdatetime], 
        [dnt_posts1].[message],
        [dnt_forums].[name] 
        FROM [dnt_posts1]
        LEFT JOIN [dnt_forums] ON [dnt_posts1].[fid]=[dnt_forums].[fid]
        LEFT JOIN [dnt_topics] ON [dnt_posts1].[tid]=[dnt_topics].[tid]
        WHERE [dnt_posts1].[layer]=0 AND [dnt_topics].[displayorder] >= 0
        ORDER BY [dnt_posts1].[tid] DESC
    END
GO

IF OBJECT_ID('[dnt_getattentiontopics]','P') IS NOT NULL
DROP PROC [dnt_getattentiontopics]
GO

CREATE PROCEDURE [dnt_getattentiontopics] 

@fid varchar(255)='',
@tpp int,
@pageid int,
@condition nvarchar(255)=''
AS

DECLARE @pagetop int,@strSQL varchar(5000)

SET @pagetop = (@pageid-1)*@tpp
IF @pageid = 1
	BEGIN
		SET @strSQL = 'SELECT TOP  ' +STR( @tpp) + '  * FROM [dnt_topics] WHERE [displayorder]>=0  AND [attention]=1'
                     	
		IF @fid<>'0'
                            SELECT  @strSQL=@strSQL+'  AND [fid] IN ('+@fid+')'


                            IF @condition<>''
                            SELECT  @strSQL=@strSQL+@condition

                           SELECT @strSQL=@strSQL+'  ORDER BY [lastpost] DESC'
                            

      
	END
ELSE
	BEGIN
		SET @strSQL = 'SELECT TOP  ' +STR( @tpp) + '  * FROM [dnt_topics]  WHERE [tid] < (SELECT MIN([tid])  FROM (SELECT TOP '+STR(@pagetop)+' [tid] FROM [dnt_topics]   WHERE [displayorder]>=0  AND [attention]=1'
		

		 IF @fid<>'0'
 
                            SELECT  @strSQL=@strSQL+'  AND [fid] IN ('+@fid+')'
                          


                            IF @condition<>''
                            SELECT  @strSQL=@strSQL+@condition
                     

                          SELECT @strSQL=@strSQL+'   ORDER BY [tid] DESC'


                          SELECT @strSQL=@strSQL+'  )  AS T) '

		 IF @fid<>'0'
 
                           SELECT  @strSQL=@strSQL+'  AND [fid] IN ('+@fid+')'

			    IF @condition<>''
                            SELECT  @strSQL=@strSQL+@condition


                           SELECT @strSQL=@strSQL+'  AND [displayorder]>=0  AND [attention]=1 ORDER BY [tid] DESC'

                                 
	END
EXEC(@strSQL)
GO

IF OBJECT_ID('[dnt_updateuser]','P') IS NOT NULL
DROP PROC [dnt_updateuser]
GO

CREATE PROCEDURE [dnt_updateuser]
@username nchar(20),
@nickname nchar(20),
@password char(32),
@secques char(8),
@spaceid int,
@gender int,
@adminid int,
@groupid smallint,
@groupexpiry int,
@extgroupids char(60),
@regip char(15),
@joindate char(19),
@lastip char(15),
@lastvisit char(19),
@lastactivity char(19),
@lastpost char(19),
@lastpostid int,
@lastposttitle nchar(60),
@posts int,
@digestposts smallint,
@oltime int,
@pageviews int,
@credits int,
@extcredits1 float,
@extcredits2 float,
@extcredits3 float,
@extcredits4 float,
@extcredits5 float,
@extcredits6 float,
@extcredits7 float,
@extcredits8 float,
@avatarshowid int,
@email char(50),
@bday char(19),
@sigstatus int,
@tpp int,
@ppp int,
@templateid smallint,
@pmsound int,
@showemail int,
@newsletter int,
@invisible int,
@newpm int,
@newpmcount int,
@accessmasks int,
@onlinestate int,
@website varchar(80),
@icq varchar(12),
@qq varchar(12),
@yahoo varchar(40),
@msn varchar(40),
@skype varchar(40),
@location nvarchar(30),
@customstatus varchar(30),
@avatar varchar(255),
@avatarwidth int,
@avatarheight int,
@medals varchar(300),
@bio nvarchar(500),
@signature nvarchar(500),
@sightml nvarchar(1000),
@authstr varchar(20),
@authtime smalldatetime,
@authflag tinyint,
@realname nvarchar(10),
@idcard varchar(20),
@mobile varchar(20),
@phone varchar(20),
@ignorepm nvarchar(1000),
@uid int
AS

UPDATE [dnt_users] SET [username]=@username,[nickname]=@nickname, [password]=@password, [secques]=@secques, [spaceid]=@spaceid, [gender]=@gender, [adminid]=@adminid, [groupid]=@groupid, [groupexpiry]=@groupexpiry, 
[extgroupids]=@extgroupids, [regip]= @regip, [joindate]= @joindate, [lastip]=@lastip, [lastvisit]=@lastvisit, [lastactivity]=@lastactivity, [lastpost]=@lastpost, 
[lastpostid]=@lastpostid, [lastposttitle]=@lastposttitle, [posts]=@posts, [digestposts]=@digestposts, [oltime]=@oltime, [pageviews]=@pageviews, [credits]=@credits, 
[extcredits1]=@extcredits1, [extcredits2]=@extcredits2, [extcredits3]=@extcredits3, [extcredits4]=@extcredits4, [extcredits5]=@extcredits5, [extcredits6]=@extcredits6, 
[extcredits7]=@extcredits7, [extcredits8]=@extcredits8, [avatarshowid]=@avatarshowid, [email]=@email, [bday]=@bday, [sigstatus]=@sigstatus, [tpp]=@tpp, [ppp]=@ppp, 
[templateid]=@templateid, [pmsound]=@pmsound, [showemail]=@showemail, [newsletter]=@newsletter, [invisible]=@invisible, [newpm]=@newpm, [newpmcount]=@newpmcount, [accessmasks]=@accessmasks, [onlinestate]=@onlinestate 
WHERE [uid]=@uid

UPDATE [dnt_userfields] SET [website]=@website,[icq]=@icq,[qq]=@qq,[yahoo]=@yahoo,[msn]=@msn,[skype]=@skype,[location]=@location,[customstatus]=@customstatus,
[avatar]=@avatar,[avatarwidth]=@avatarwidth,[avatarheight]=@avatarheight,[medals]=@medals,[bio]=@bio,[signature]=@signature,[sightml]=@sightml,[authstr]=@authstr,
[authtime]=@authtime,[authflag]=@authflag,[realname]=@realname,[idcard]=@idcard,[mobile]=@mobile,[phone]=@phone,[ignorepm]=@ignorepm 
WHERE [uid]=@uid
GO

IF OBJECT_ID('[dnt_split]') IS NOT NULL
DROP FUNCTION [dnt_split]
GO

CREATE FUNCTION [dnt_split]
(
 @splitstring NVARCHAR(4000),
 @separator CHAR(1) = ','
)
RETURNS @splitstringstable TABLE
(
 [item] NVARCHAR(200)
)
AS
BEGIN
    DECLARE @currentindex INT
    DECLARE @nextindex INT
    DECLARE @returntext NVARCHAR(200)

    SELECT @currentindex=1

    WHILE(@currentindex<=datalength(@splitstring)/2)
    BEGIN
        SELECT @nextindex=charindex(@separator,@splitstring,@currentindex)
        IF(@nextindex=0 OR @nextindex IS NULL)
            SELECT @nextindex=datalength(@splitstring)/2+1
        
        SELECT @returntext=substring(@splitstring,@currentindex,@nextindex-@currentindex)

        INSERT INTO @splitstringstable([item])
        VALUES(@returntext)
        
        SELECT @currentindex=@nextindex+1
    END
    RETURN
END
GO

if exists (select * from sysobjects where id = object_id(N'[dnt_getindexforumlist]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_getindexforumlist]
GO

if exists (select * from sysobjects where id = object_id(N'[dnt_getonlineuser]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_getonlineuser]
GO

if exists (select * from sysobjects where id = object_id(N'[dnt_getonlineuserlist]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_getonlineuserlist]
GO

if exists (select * from sysobjects where id = object_id(N'[dnt_getonlineuserlistbyfid]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_getonlineuserlistbyfid]
GO

if exists (select * from sysobjects where id = object_id(N'[dnt_getrelatedtopics]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_getrelatedtopics]
GO

if exists (select * from sysobjects where id = object_id(N'[dnt_createonlineuser]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_createonlineuser]
GO

if exists (select * from sysobjects where id = object_id(N'[dnt_deleteonlineusers]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_deleteonlineusers]
GO

if exists (select * from sysobjects where id = object_id(N'[dnt_getexpiredonlineuserlist]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_getexpiredonlineuserlist]
GO

if exists (select * from sysobjects where id = object_id(N'[dnt_updateuseronlinestates]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_updateuseronlinestates]
GO

if exists (select * from sysobjects where id = object_id(N'[dnt_updateonlineaction]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_updateonlineaction]
GO

CREATE PROCEDURE [dnt_updateonlineaction] 
@action smallint,
@lastupdatetime datetime,
@forumid int,
@forumname nvarchar(100),
@titleid int,
@title nvarchar(160),
@olid int
AS

IF (@action =5 OR @action =6)
BEGIN
   UPDATE [dnt_online] SET [lastactivity]=[action],[action]=@action,[lastupdatetime]=@lastupdatetime,[lastposttime]= @lastupdatetime,[forumid]=@forumid,[forumname]=@forumname,[titleid]=@titleid,[title]=@title WHERE [olid]=@olid
END
ELSE
BEGIN
   UPDATE [dnt_online] SET [lastactivity]=[action],[action]=@action,[lastupdatetime]=@lastupdatetime,[forumid]=@forumid,[forumname]=@forumname,[titleid]=@titleid,[title]=@title WHERE [olid]=@olid
END
GO

if exists (select * from sysobjects where id = object_id(N'[dnt_gettopictags]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_gettopictags]
GO

CREATE PROCEDURE [dnt_getindexforumlist]
AS
SELECT CASE WHEN DATEDIFF(n, [lastpost], GETDATE())<600 THEN 'new' ELSE 'old' END AS [havenew],[dnt_forums].*, [dnt_forumfields].* FROM [dnt_forums] LEFT JOIN [dnt_forumfields] ON [dnt_forums].[fid]=[dnt_forumfields].[fid] WHERE [dnt_forums].[parentid] NOT IN (SELECT [fid] FROM [dnt_forums] WHERE [status] < 1 AND [layer] = 0) AND [dnt_forums].[status] > 0 AND [layer] <= 1 ORDER BY [displayorder]
GO

IF OBJECT_ID('[dnt_getonlineuser]','P') IS NOT NULL
DROP PROC [dnt_getonlineuser]
GO

CREATE PROCEDURE [dnt_getonlineuser]
@userid int,
@password char(32)
AS
SELECT TOP 1 [olid]
      ,[userid]
      ,[ip]
      ,[username]
      ,[nickname]
      ,[password]
      ,[groupid]
      ,[olimg]
      ,[adminid]
      ,[invisible]
      ,[action]
      ,[lastactivity]
      ,[lastposttime]
      ,[lastpostpmtime]
      ,[lastsearchtime]
      ,[lastupdatetime]
      ,[forumid]
      ,[forumname]
      ,[titleid]
      ,[title]
      ,[verifycode]
      ,[newpms]
      ,[newnotices] FROM [dnt_online] WHERE [userid]=@userid AND [password]=@password
GO

IF OBJECT_ID('[dnt_getonlineuserlist]','P') IS NOT NULL
DROP PROC [dnt_getonlineuserlist]
GO

CREATE PROCEDURE [dnt_getonlineuserlist]
AS
SELECT [olid]
      ,[userid]
      ,[ip]
      ,[username]
      ,[nickname]
      ,[password]
      ,[groupid]
      ,[olimg]
      ,[adminid]
      ,[invisible]
      ,[action]
      ,[lastactivity]
      ,[lastposttime]
      ,[lastpostpmtime]
      ,[lastsearchtime]
      ,[lastupdatetime]
      ,[forumid]
      ,[forumname]
      ,[titleid]
      ,[title]
      ,[verifycode]
      ,[newpms]
      ,[newnotices] FROM [dnt_online]
GO

IF OBJECT_ID('[dnt_getonlineuserlistbyfid]','P') IS NOT NULL
DROP PROC [dnt_getonlineuserlistbyfid]
GO

CREATE PROCEDURE [dnt_getonlineuserlistbyfid]
@fid int
AS
SELECT [olid]
      ,[userid]
      ,[ip]
      ,[username]
      ,[nickname]
      ,[password]
      ,[groupid]
      ,[olimg]
      ,[adminid]
      ,[invisible]
      ,[action]
      ,[lastactivity]
      ,[lastposttime]
      ,[lastpostpmtime]
      ,[lastsearchtime]
      ,[lastupdatetime]
      ,[forumid]
      ,[forumname]
      ,[titleid]
      ,[title]
      ,[verifycode]
      ,[newpms]
      ,[newnotices] FROM [dnt_online] WHERE [forumid]=@fid
GO

IF OBJECT_ID('[dnt_getrelatedtopics]','P') IS NOT NULL
DROP PROC [dnt_getrelatedtopics]
GO

CREATE PROCEDURE [dnt_getrelatedtopics]
@count int,
@tid int
AS

EXEC('SELECT TOP ' + @count + ' [linktid],[linktitle],[tid] FROM [dnt_topictagcaches] WHERE [tid]=' + @tid + ' ORDER BY [linktid] DESC')
GO

IF OBJECT_ID('[dnt_gettopicinfo]','P') IS NOT NULL
DROP PROC [dnt_gettopicinfo]
GO

CREATE PROCEDURE [dnt_gettopicinfo]
@tid int,
@fid int,
@mode int
AS
IF @mode = 1
	BEGIN
       SELECT TOP 1 [tid]
      ,[fid]
      ,[iconid]
      ,[readperm]
      ,[price]
      ,[poster]
      ,[posterid]
      ,[title]
      ,[postdatetime]
      ,[lastpost]
      ,[lastpostid]
      ,[lastposter]
      ,[lastposterid]
      ,[views]
      ,[replies]
      ,[displayorder]
      ,[highlight]
      ,[digest]
      ,[hide]
      ,[attachment]
      ,[moderated]
      ,[closed]
      ,[magic]
      ,[identify]
      ,[special]
      ,[typeid]
      ,[rate]
      ,[attention] FROM [dnt_topics] WITH (NOLOCK) WHERE [fid]=@fid AND [lastpostid]>(SELECT [lastpostid] FROM [dnt_topics] WHERE [tid]=@tid) AND [displayorder]>=0  ORDER BY [lastpostid] ASC
	END
ELSE IF @mode = 2
	BEGIN
       SELECT TOP 1 [tid]
      ,[fid]
      ,[iconid]
      ,[readperm]
      ,[price]
      ,[poster]
      ,[posterid]
      ,[title]
      ,[postdatetime]
      ,[lastpost]
      ,[lastpostid]
      ,[lastposter]
      ,[lastposterid]
      ,[views]
      ,[replies]
      ,[displayorder]
      ,[highlight]
      ,[digest]
      ,[hide]
      ,[attachment]
      ,[moderated]
      ,[closed]
      ,[magic]
      ,[identify]
      ,[special]
      ,[typeid]
      ,[rate]
      ,[attention] FROM [dnt_topics] WITH (NOLOCK) WHERE [fid]=@fid AND [lastpostid]<(SELECT [lastpostid] FROM [dnt_topics] WHERE [tid]=@tid) AND [displayorder]>=0  ORDER BY [lastpostid] DESC
	END
ELSE
	BEGIN
       SELECT TOP 1 [tid]
      ,[fid]
      ,[iconid]
      ,[readperm]
      ,[price]
      ,[poster]
      ,[posterid]
      ,[title]
      ,[postdatetime]
      ,[lastpost]
      ,[lastpostid]
      ,[lastposter]
      ,[lastposterid]
      ,[views]
      ,[replies]
      ,[displayorder]
      ,[highlight]
      ,[digest]
      ,[hide]
      ,[attachment]
      ,[moderated]
      ,[closed]
      ,[magic]
      ,[identify]
      ,[special]
      ,[typeid]
      ,[rate]
      ,[attention] FROM [dnt_topics] WITH (NOLOCK) WHERE [tid]=@tid
	END
GO

IF OBJECT_ID('[dnt_gettopictags]','P') IS NOT NULL
DROP PROC [dnt_gettopictags]
GO

CREATE PROCEDURE [dnt_gettopictags]
@topicid int
AS

SELECT t.* FROM [dnt_tags] t, [dnt_topictags] tt WHERE tt.[tagid] = t.[tagid] AND tt.[tid] = @topicid ORDER BY [orderid]
GO

IF OBJECT_ID('[dnt_createonlineuser]','P') IS NOT NULL
DROP PROC [dnt_createonlineuser]
GO

CREATE PROCEDURE [dnt_createonlineuser] 
@onlinestate int,
@userid int,
@ip varchar(15),
@username nvarchar(40),
@nickname nvarchar(40),
@password char(32),
@groupid smallint,
@olimg varchar(80),
@adminid smallint,
@invisible smallint,
@action smallint,
@lastactivity smallint,
@lastposttime datetime,
@lastpostpmtime datetime,
@lastsearchtime datetime,
@lastupdatetime datetime,
@forumid int,
@forumname nvarchar(50),
@titleid int,
@title nvarchar(80),
@verifycode varchar(10),
@newpms smallint,
@newnotices smallint

AS

IF @onlinestate = 0
BEGIN
	UPDATE [dnt_users] SET [onlinestate]=1 WHERE [uid]=@userid
END


INSERT INTO [dnt_online] ([userid],[ip],[username],[nickname],[password],[groupid],[olimg],[adminid],[invisible],[action],[lastactivity],[lastposttime],[lastpostpmtime],[lastsearchtime],[lastupdatetime],[forumid],[forumname],[titleid],[title],[verifycode],[newpms],[newnotices])VALUES(@userid,@ip,@username,@nickname,@password,@groupid,@olimg,@adminid,@invisible,@action,@lastactivity,@lastposttime,@lastpostpmtime,@lastsearchtime,@lastupdatetime,@forumid,@forumname,@titleid,@title,@verifycode,@newpms,@newnotices);SELECT SCOPE_IDENTITY()
GO

IF OBJECT_ID('[dnt_deleteonlineusers]','P') IS NOT NULL
DROP PROC [dnt_deleteonlineusers]
GO

CREATE PROCEDURE [dnt_deleteonlineusers] 
@olidlist varchar(5000) = ''
AS

DELETE FROM [dnt_online] WHERE CHARINDEX(','+RTRIM([olid])+',', ','+@olidlist+',') > 0
GO

IF OBJECT_ID('[dnt_getexpiredonlineuserlist]','P') IS NOT NULL
DROP PROC [dnt_getexpiredonlineuserlist]
GO

CREATE PROCEDURE [dnt_getexpiredonlineuserlist]
@expires datetime
AS

SELECT [olid], [userid] FROM [dnt_online] WHERE [lastupdatetime]<@expires
GO

IF OBJECT_ID('[dnt_updateuseronlinestates]','P') IS NOT NULL
DROP PROC [dnt_updateuseronlinestates]
GO

CREATE PROCEDURE [dnt_updateuseronlinestates] 
@uidlist varchar(5000) = '' 
AS

UPDATE [dnt_users] SET [onlinestate]=0,[lastactivity]=GETDATE() WHERE CHARINDEX(','+RTRIM([uid])+',', ','+@uidlist+',') > 0
GO

if exists (select * from sysobjects where id = object_id(N'[dnt_getonlineuserbyip]') and OBJECTPROPERTY(id,N'IsProcedure') = 1)
drop procedure [dnt_getonlineuserbyip]
GO

CREATE PROCEDURE [dnt_getonlineuserbyip]
@userid int,
@ip varchar(15)
AS

SELECT TOP 1 [olid]
      ,[userid]
      ,[ip]
      ,[username]
      ,[nickname]
      ,[password]
      ,[groupid]
      ,[olimg]
      ,[adminid]
      ,[invisible]
      ,[action]
      ,[lastactivity]
      ,[lastposttime]
      ,[lastpostpmtime]
      ,[lastsearchtime]
      ,[lastupdatetime]
      ,[forumid]
      ,[forumname]
      ,[titleid]
      ,[title]
      ,[verifycode]
      ,[newpms]
      ,[newnotices] FROM [dnt_online] WHERE [userid]=@userid AND [ip]=@ip
GO

if exists (select * from sysobjects where id = object_id(N'[dnt_getreplypid]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_getreplypid]
GO

if exists (select * from sysobjects where id = object_id(N'[dnt_gettodayuploadedfilesize]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_gettodayuploadedfilesize]
GO

if exists (select * from sysobjects where id = object_id(N'[dnt_getuseridbyemail]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_getuseridbyemail]
GO

if exists (select * from sysobjects where id = object_id(N'[dnt_getuserinfobyip]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_getuserinfobyip]
GO

IF OBJECT_ID('[dnt_getreplypid1]','P') IS NOT NULL
DROP PROC [dnt_getreplypid1]
GO

CREATE PROCEDURE [dnt_getreplypid1]
@uid int,
@tid int

AS

SELECT TOP 1 [pid] FROM [dnt_posts1] WHERE [tid] =@tid AND [posterid]=@uid
GO

IF OBJECT_ID('dnt_gettodayuploadedfilesize','P') IS NOT NULL
DROP PROC dnt_gettodayuploadedfilesize
GO

CREATE PROCEDURE dnt_gettodayuploadedfilesize 
@uid int

AS

SELECT SUM([filesize]) AS [todaysize] FROM [dnt_attachments] WHERE [uid]=@uid AND DATEDIFF(d,[postdatetime],GETDATE())=0
GO

IF OBJECT_ID('dnt_getuseridbyemail','P') IS NOT NULL
DROP PROC dnt_getuseridbyemail
GO

CREATE PROCEDURE dnt_getuseridbyemail
@email char(50)
AS

SELECT TOP 1 [uid] FROM [dnt_users] WHERE [email]=@email
GO

IF OBJECT_ID('[dnt_getuserinfobyip]','P') IS NOT NULL
DROP PROC [dnt_getuserinfobyip]
GO

CREATE PROCEDURE [dnt_getuserinfobyip]
@regip char(15)
AS

SELECT TOP 1 [u].[uid], [u].[username], [u].[nickname], [u].[password], [u].[secques], [u].[spaceid], [u].[gender], [u].[adminid], [u].[groupid], [u].[groupexpiry], [u].[extgroupids], [u].[regip], [u].[joindate], [u].[lastip], [u].[lastvisit], [u].[lastactivity], [u].[lastpost], [u].[lastpostid], [u].[lastposttitle], [u].[posts], [u].[digestposts], [u].[oltime], [u].[pageviews], [u].[credits], [u].[extcredits1], [u].[extcredits2], [u].[extcredits3], [u].[extcredits4], [u].[extcredits5], [u].[extcredits6], [u].[extcredits7], [u].[extcredits8], [u].[avatarshowid], [u].[email], [u].[bday], [u].[sigstatus], [u].[tpp], [u].[ppp], [u].[templateid], [u].[pmsound], [u].[showemail], [u].[invisible], [u].[newpm], [u].[newpmcount], [u].[accessmasks], [u].[onlinestate], [u].[newsletter],[u].[salt], [uf].[website], [uf].[icq], [uf].[qq], [uf].[yahoo], [uf].[msn], [uf].[skype], [uf].[location], [uf].[customstatus], [uf].[avatar], [uf].[avatarwidth], [uf].[avatarheight], [uf].[medals], [uf].[bio], [uf].[signature], [uf].[sightml], [uf].[authstr], [uf].[authtime], [uf].[authflag], [uf].[realname], [uf].[idcard], [uf].[mobile], [uf].[phone], [uf].[ignorepm] FROM [dnt_users] [u] LEFT JOIN [dnt_userfields] [uf] ON [u].[uid]=[uf].[uid] WHERE [u].[regip]=@regip ORDER BY [u].[uid] DESC
GO

if exists (select * from sysobjects where id = object_id(N'[dnt_getnoticebyuid]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_getnoticebyuid]
GO

CREATE PROCEDURE [dnt_getnoticebyuid]
@uid int,
@type int

AS

IF @type = 0
     SELECT nid, uid, type, new, posterid, poster, note, postdatetime FROM [dnt_notices] WHERE [uid] = @uid  ORDER BY [postdatetime] DESC
ELSE
    SELECT nid, uid, type, new, posterid, poster, note, postdatetime  FROM [dnt_notices] WHERE [uid] = @uid AND [type] = @type ORDER BY [postdatetime] DESC
GO

IF OBJECT_ID('dnt_createnotice','P') IS NOT NULL
DROP PROC [dnt_createnotice]
GO

CREATE PROCEDURE [dnt_createnotice]
@uid int,
@type int,
@new  int,
@posterid int,
@poster nchar(20),
@note ntext,
@postdatetime datetime,
@fromid int

AS
DECLARE @count INT
SET @count=(SELECT COUNT(1) FROM [dnt_notices] WHERE [posterid]=@posterid AND [uid]=@uid AND [type]=@type AND [fromid]=@fromid)

IF(@count=0)
	BEGIN
		INSERT INTO [dnt_notices] ([uid], [type], [new], [posterid], [poster], [note], [postdatetime],[fromid]) VALUES (@uid, @type, @new, @posterid, @poster, @note, @postdatetime,@fromid)
		SELECT SCOPE_IDENTITY()  AS 'nid'
		RETURN
	END
ELSE
	BEGIN
		DELETE [dnt_notices] WHERE [posterid]=@posterid AND [uid]=@uid AND [type]=@type AND [fromid]=@fromid
		INSERT INTO [dnt_notices] ([uid], [type], [new], [posterid], [poster], [note], [postdatetime],[fromid]) VALUES (@uid, @type, @new, @posterid, @poster, @note, @postdatetime,@fromid)
		SELECT SCOPE_IDENTITY()  AS 'nid'
		RETURN
	END
GO

if exists (select * from sysobjects where id = object_id(N'[dnt_getnewnoticecountbyuid]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_getnewnoticecountbyuid]
GO

CREATE PROCEDURE [dnt_getnewnoticecountbyuid]
@uid int
AS

SELECT COUNT(nid) FROM [dnt_notices] WHERE [uid] =  @uid  AND [new] = 1
GO

if exists (select * from sysobjects where id = object_id(N'[dnt_getnoticecountbyuid]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_getnoticecountbyuid]
GO

CREATE PROCEDURE [dnt_getnoticecountbyuid]
@uid int,
@type int
AS
IF @type = -1
   SELECT COUNT(nid) FROM [dnt_notices] WHERE [uid] = @uid
ELSE
    SELECT COUNT(nid) FROM [dnt_notices] WHERE [uid] = @uid AND [type]=@type
GO

if exists (select * from sysobjects where id = object_id(N'[dnt_getpostlist1]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_getpostlist1]
GO

CREATE PROCEDURE [dnt_getpostlist1]
@tid int,
@pagesize int,
@pageindex int
AS
DECLARE @pagetop int

SET @pagetop = (@pageindex-1)*@pagesize

IF @pageindex = 1

EXEC('SELECT TOP ' + @pagesize + '
									[dnt_posts1].[pid], 
									[dnt_posts1].[fid], 
									[dnt_posts1].[title], 
									[dnt_posts1].[layer],
									[dnt_posts1].[message], 
									[dnt_posts1].[ip], 
									[dnt_posts1].[lastedit], 
									[dnt_posts1].[postdatetime], 
									[dnt_posts1].[attachment], 
									[dnt_posts1].[poster], 
									[dnt_posts1].[posterid], 
									[dnt_posts1].[invisible], 
									[dnt_posts1].[usesig], 
									[dnt_posts1].[htmlon], 
									[dnt_posts1].[smileyoff], 
									[dnt_posts1].[parseurloff], 
									[dnt_posts1].[bbcodeoff], 
									[dnt_posts1].[rate], 
									[dnt_posts1].[ratetimes], 
									[dnt_users].[nickname],  
									[dnt_users].[username], 
									[dnt_users].[groupid], 
									[dnt_users].[spaceid],
									[dnt_users].[gender],
									[dnt_users].[bday],
									[dnt_users].[email], 
									[dnt_users].[showemail], 
									[dnt_users].[digestposts], 
									[dnt_users].[credits], 
									[dnt_users].[extcredits1], 
									[dnt_users].[extcredits2], 
									[dnt_users].[extcredits3], 
									[dnt_users].[extcredits4], 
									[dnt_users].[extcredits5], 
									[dnt_users].[extcredits6], 
									[dnt_users].[extcredits7], 
									[dnt_users].[extcredits8], 
									[dnt_users].[posts], 
									[dnt_users].[joindate], 
									[dnt_users].[onlinestate],
									[dnt_users].[lastactivity], 
									[dnt_users].[invisible],
									[dnt_users].[oltime],
									[dnt_users].[lastvisit], 
									[dnt_userfields].[avatar], 
									[dnt_userfields].[avatarwidth], 
									[dnt_userfields].[avatarheight],
									[dnt_userfields].[medals],
									[dnt_userfields].[sightml] AS signature, 
									[dnt_userfields].[location], 
									[dnt_userfields].[customstatus], 
									[dnt_userfields].[website], 
									[dnt_userfields].[icq], 
									[dnt_userfields].[qq], 
									[dnt_userfields].[msn], 
									[dnt_userfields].[yahoo], 
									[dnt_userfields].[skype] 
	 FROM [dnt_posts1] LEFT JOIN [dnt_users] ON [dnt_users].[uid]=[dnt_posts1].[posterid] LEFT JOIN [dnt_userfields] ON [dnt_userfields].[uid]=[dnt_users].[uid] WHERE [dnt_posts1].[tid]=' + @tid + ' AND [dnt_posts1].[invisible]<=0 ORDER BY [dnt_posts1].[pid]')

ELSE

EXEC('SELECT TOP ' + @pagesize + ' 
									[dnt_posts1].[pid], 
									[dnt_posts1].[fid], 
									[dnt_posts1].[title], 
									[dnt_posts1].[layer],
									[dnt_posts1].[message], 
									[dnt_posts1].[ip], 
									[dnt_posts1].[lastedit], 
									[dnt_posts1].[postdatetime], 
									[dnt_posts1].[attachment], 
									[dnt_posts1].[poster], 
									[dnt_posts1].[posterid], 
									[dnt_posts1].[invisible], 
									[dnt_posts1].[usesig], 
									[dnt_posts1].[htmlon], 
									[dnt_posts1].[smileyoff], 
									[dnt_posts1].[parseurloff], 
									[dnt_posts1].[bbcodeoff], 
									[dnt_posts1].[rate], 
									[dnt_posts1].[ratetimes],
									[dnt_users].[nickname],  
									[dnt_users].[username], 
									[dnt_users].[groupid], 
									[dnt_users].[spaceid],
									[dnt_users].[gender],
									[dnt_users].[bday],
									[dnt_users].[email], 
									[dnt_users].[showemail], 
									[dnt_users].[digestposts], 
									[dnt_users].[credits], 
									[dnt_users].[extcredits1], 
									[dnt_users].[extcredits2], 
									[dnt_users].[extcredits3], 
									[dnt_users].[extcredits4], 
									[dnt_users].[extcredits5], 
									[dnt_users].[extcredits6], 
									[dnt_users].[extcredits7], 
									[dnt_users].[extcredits8], 
									[dnt_users].[posts], 
									[dnt_users].[joindate], 
									[dnt_users].[onlinestate],
									[dnt_users].[lastactivity],
									[dnt_users].[oltime],
									[dnt_users].[lastvisit],  
									[dnt_users].[invisible] AS [userinvisible], 
									[dnt_userfields].[avatar], 
									[dnt_userfields].[avatarwidth], 
									[dnt_userfields].[avatarheight], 
									[dnt_userfields].[medals],
									[dnt_userfields].[sightml] AS [signature], 
									[dnt_userfields].[location], 
									[dnt_userfields].[customstatus], 
									[dnt_userfields].[website], 
									[dnt_userfields].[icq], 
									[dnt_userfields].[qq], 
									[dnt_userfields].[msn], 
									[dnt_userfields].[yahoo], 
									[dnt_userfields].[skype] 
	FROM [dnt_posts1] LEFT JOIN [dnt_users] ON [dnt_users].[uid]=[dnt_posts1].[posterid] LEFT JOIN [dnt_userfields] ON [dnt_userfields].[uid]=[dnt_users].[uid] WHERE [dnt_posts1].[tid]=' + @tid + ' AND [pid] > (SELECT MAX([pid]) FROM (SELECT TOP ' + @pagetop + ' [dnt_posts1].[pid] FROM [dnt_posts1] WHERE [dnt_posts1].[tid]=' + @tid + ' AND [dnt_posts1].[invisible]<=0 ORDER BY [dnt_posts1].[pid]) AS tblTmp) AND [dnt_posts1].[invisible]<=0 ORDER BY [dnt_posts1].[pid]')
GO

IF OBJECT_ID('[dnt_getpostlistbycondition1]','P') IS NOT NULL
DROP PROC [dnt_getpostlistbycondition1]
GO

CREATE PROCEDURE [dnt_getpostlistbycondition1]
@tid int,
@pagesize int,
@pageindex int,
@posterid int
AS
DECLARE @pagetop int

SET @pagetop = (@pageindex-1)*@pagesize

IF @pageindex = 1

EXEC('SELECT TOP ' + @pagesize + '
									[dnt_posts1].[pid], 
									[dnt_posts1].[fid], 
									[dnt_posts1].[title], 
									[dnt_posts1].[layer],
									[dnt_posts1].[message], 
									[dnt_posts1].[ip], 
									[dnt_posts1].[lastedit], 
									[dnt_posts1].[postdatetime], 
									[dnt_posts1].[attachment], 
									[dnt_posts1].[poster], 
									[dnt_posts1].[posterid], 
									[dnt_posts1].[invisible], 
									[dnt_posts1].[usesig], 
									[dnt_posts1].[htmlon], 
									[dnt_posts1].[smileyoff], 
									[dnt_posts1].[parseurloff], 
									[dnt_posts1].[bbcodeoff], 
									[dnt_posts1].[rate], 
									[dnt_posts1].[ratetimes], 
									[dnt_users].[nickname],  
									[dnt_users].[username], 
									[dnt_users].[groupid], 
									[dnt_users].[spaceid],
									[dnt_users].[gender],
									[dnt_users].[bday],
									[dnt_users].[email], 
									[dnt_users].[showemail], 
									[dnt_users].[digestposts], 
									[dnt_users].[credits], 
									[dnt_users].[extcredits1], 
									[dnt_users].[extcredits2], 
									[dnt_users].[extcredits3], 
									[dnt_users].[extcredits4], 
									[dnt_users].[extcredits5], 
									[dnt_users].[extcredits6], 
									[dnt_users].[extcredits7], 
									[dnt_users].[extcredits8], 
									[dnt_users].[posts], 
									[dnt_users].[joindate], 
									[dnt_users].[onlinestate],
									[dnt_users].[lastactivity],
									[dnt_users].[oltime],
									[dnt_users].[lastvisit],  
									[dnt_users].[invisible], 
									[dnt_userfields].[avatar], 
									[dnt_userfields].[avatarwidth],
									[dnt_userfields].[avatarheight],
									[dnt_userfields].[medals],
									[dnt_userfields].[sightml] AS signature, 
									[dnt_userfields].[location], 
									[dnt_userfields].[customstatus], 
									[dnt_userfields].[website], 
									[dnt_userfields].[icq], 
									[dnt_userfields].[qq], 
									[dnt_userfields].[msn], 
									[dnt_userfields].[yahoo], 
									[dnt_userfields].[skype] 
	 FROM [dnt_posts1] LEFT JOIN [dnt_users] ON [dnt_users].[uid]=[dnt_posts1].[posterid] LEFT JOIN [dnt_userfields] ON [dnt_userfields].[uid]=[dnt_users].[uid] WHERE [dnt_posts1].[tid]=' + @tid + ' AND [dnt_posts1].[invisible]=0 AND [posterid] = '+@posterid+' ORDER BY [dnt_posts1].[pid]')

ELSE

EXEC('SELECT TOP ' + @pagesize + ' 
									[dnt_posts1].[pid], 
									[dnt_posts1].[fid], 
									[dnt_posts1].[title], 
									[dnt_posts1].[layer],
									[dnt_posts1].[message], 
									[dnt_posts1].[ip], 
									[dnt_posts1].[lastedit], 
									[dnt_posts1].[postdatetime], 
									[dnt_posts1].[attachment], 
									[dnt_posts1].[poster], 
									[dnt_posts1].[posterid], 
									[dnt_posts1].[invisible], 
									[dnt_posts1].[usesig], 
									[dnt_posts1].[htmlon], 
									[dnt_posts1].[smileyoff], 
									[dnt_posts1].[parseurloff], 
									[dnt_posts1].[bbcodeoff], 
									[dnt_posts1].[rate], 
									[dnt_posts1].[ratetimes],
									[dnt_users].[nickname],  
									[dnt_users].[username], 
									[dnt_users].[groupid], 
									[dnt_users].[spaceid],
									[dnt_users].[gender],
									[dnt_users].[bday],
									[dnt_users].[email], 
									[dnt_users].[showemail], 
									[dnt_users].[digestposts], 
									[dnt_users].[credits], 
									[dnt_users].[extcredits1], 
									[dnt_users].[extcredits2], 
									[dnt_users].[extcredits3], 
									[dnt_users].[extcredits4], 
									[dnt_users].[extcredits5], 
									[dnt_users].[extcredits6], 
									[dnt_users].[extcredits7], 
									[dnt_users].[extcredits8], 
									[dnt_users].[posts], 
									[dnt_users].[joindate], 
									[dnt_users].[onlinestate], 
									[dnt_users].[lastactivity], 
									[dnt_users].[invisible],
									[dnt_users].[oltime],
									[dnt_users].[lastvisit], 
									[dnt_userfields].[avatar], 
									[dnt_userfields].[avatarwidth], 
									[dnt_userfields].[avatarheight], 
									[dnt_userfields].[medals],
									[dnt_userfields].[sightml] AS signature, 
									[dnt_userfields].[location], 
									[dnt_userfields].[customstatus], 
									[dnt_userfields].[website], 
									[dnt_userfields].[icq], 
									[dnt_userfields].[qq], 
									[dnt_userfields].[msn], 
									[dnt_userfields].[yahoo], 
									[dnt_userfields].[skype] 
	FROM [dnt_posts1] LEFT JOIN [dnt_users] ON [dnt_users].[uid]=[dnt_posts1].[posterid] LEFT JOIN [dnt_userfields] ON [dnt_userfields].[uid]=[dnt_users].[uid] WHERE [dnt_posts1].[tid]=' + @tid + ' AND [dnt_posts1].[invisible]=0 AND [posterid] = '+@posterid+'  AND [pid] > (SELECT MAX([pid]) FROM (SELECT TOP ' + @pagetop + ' [dnt_posts1].[pid] FROM [dnt_posts1] LEFT JOIN [dnt_users] ON [dnt_users].[uid]=[dnt_posts1].[posterid] LEFT JOIN [dnt_userfields] ON [dnt_userfields].[uid]=[dnt_users].[uid] WHERE [dnt_posts1].[tid]=@tid  AND [posterid] = '+@posterid+'  ORDER BY [dnt_posts1].[pid]) AS tblTmp) ORDER BY [dnt_posts1].[pid]')
GO

IF OBJECT_ID('dnt_getattachpaymentlogbyaid','P') IS NOT NULL
DROP PROC [dnt_getattachpaymentlogbyaid]
GO

CREATE PROCEDURE [dnt_getattachpaymentlogbyaid]
@aid int
AS
SELECT 
[id],
[uid],
[username],
[aid],
[authorid],
[postdatetime],
[amount],
[netamount]
FROM [dnt_attachpaymentlog]
WHERE [aid] = @aid
GO

IF OBJECT_ID('dnt_getonlineuercount','P') IS NOT NULL
DROP PROC [dnt_getonlineuercount]
GO

CREATE PROCEDURE [dnt_getonlineuercount]
AS
SELECT COUNT(olid) FROM [dnt_online]
GO

IF OBJECT_ID('dnt_getalltoptopiclist','P') IS NOT NULL
DROP PROC [dnt_getalltoptopiclist]
GO

CREATE PROCEDURE [dnt_getalltoptopiclist]
AS
SELECT 
[tid],
[displayorder],
[fid] 
FROM [dnt_topics] 
WHERE [displayorder] > 0 ORDER BY [fid]
GO

IF OBJECT_ID('dnt_getnousedforumattachment','P') IS NOT NULL
DROP PROC [dnt_getnousedforumattachment]
GO

CREATE PROCEDURE [dnt_getnousedforumattachment]
AS
SELECT 
[aid],
[uid],
[tid],
[pid],
[postdatetime],
[readperm],
[filename],
[description],
[filetype],
[filesize],
[attachment],
[downloads],
[attachprice],
[width],
[height],
[isimage]
FROM [dnt_attachments] 
WHERE [tid]= 0 AND [pid]=0 AND DATEDIFF(n, postdatetime, GETDATE()) > 30 
GO

IF OBJECT_ID('dnt_deletenousedforumattachment','P') IS NOT NULL
DROP PROC [dnt_deletenousedforumattachment]
GO

CREATE PROCEDURE [dnt_deletenousedforumattachment]
AS
DELETE FROM [dnt_attachments] 
WHERE [tid]= 0 AND [pid]=0 AND DATEDIFF(n, postdatetime, GETDATE()) > 30
GO

IF OBJECT_ID('dnt_getnousedattachmentlistbytid','P') IS NOT NULL
DROP PROC [dnt_getnousedattachmentlistbytid]
GO

IF OBJECT_ID('dnt_getnousedattachmentlistbyuid','P') IS NOT NULL
DROP PROC [dnt_getnousedattachmentlistbyuid]
GO

CREATE PROC [dnt_getnousedattachmentlistbyuid]@uid INT,@posttime DATETIME,@isimage INTASIF @posttime IS NULLBEGIN	IF @isimage in (0,1)		SELECT [aid], [uid], [tid], [pid], [postdatetime], [readperm], [filename], [description], [filetype], [filesize], [attachment], [downloads], [width], [height], [attachprice],[isimage]		FROM [dnt_attachments] WITH (NOLOCK) WHERE [uid] = @uid AND [pid]=0 AND [tid]=0 AND [isimage]=@isimage ORDER BY [aid] ASC	ELSE		SELECT [aid], [uid], [tid], [pid], [postdatetime], [readperm], [filename], [description], [filetype], [filesize], [attachment], [downloads], [width], [height], [attachprice],[isimage]		FROM [dnt_attachments] WITH (NOLOCK) WHERE [uid] = @uid AND [pid]=0 AND [tid]=0 ORDER BY [aid] ASCENDELSEBEGIN	IF @isimage in (0,1)		SELECT [aid], [uid], [tid], [pid], [postdatetime], [readperm], [filename], [description], [filetype], [filesize], [attachment], [downloads], [width], [height], [attachprice],[isimage]		FROM [dnt_attachments] WITH (NOLOCK) WHERE [uid] = @uid AND [postdatetime]>@posttime AND [pid]=0 AND [tid]=0 AND [isimage]=@isimage ORDER BY [aid] ASC	ELSE
		SELECT [aid], [uid], [tid], [pid], [postdatetime], [readperm], [filename], [description], [filetype], [filesize], [attachment], [downloads], [width], [height], [attachprice],[isimage]		FROM [dnt_attachments] WITH (NOLOCK) WHERE [uid] = @uid AND [postdatetime]>@posttime AND [pid]=0 AND [tid]=0 ORDER BY [aid] ASC
END
GO

IF OBJECT_ID('dnt_updateattachmenttidtoanothertopic','P') IS NOT NULL
DROP PROC [dnt_updateattachmenttidtoanothertopic]
GO

CREATE PROCEDURE [dnt_updateattachmenttidtoanothertopic]
@tid INT,
@oldtid INT
AS
UPDATE [dnt_attachments] SET [tid]=@tid WHERE [tid]=@oldtid
GO

IF OBJECT_ID('dnt_getfirstimageattachbytid','P') IS NOT NULL
DROP PROC [dnt_getfirstimageattachbytid]
GO

CREATE PROCEDURE [dnt_getfirstimageattachbytid]
@tid INT
AS
SELECT TOP 1 
[aid],
[uid],
[tid],
[pid],
[postdatetime],
[readperm],
[filename],
[description],
[filetype],
[filesize],
[attachment],
[downloads],
[attachprice],
[width],
[height],
[isimage]
FROM [dnt_attachments] 
WHERE [tid]=@tid AND LEFT([filetype], 5)='image' ORDER BY [aid]
GO

IF OBJECT_ID('dnt_getattachmentlistbypid','P') IS NOT NULL
DROP PROC [dnt_getattachmentlistbypid]
GO

CREATE PROCEDURE [dnt_getattachmentlistbypid]
@pid INT
AS
SELECT 
[aid],
[uid],
[tid],
[pid],
[postdatetime],
[readperm],
[filename],
[description],
[filetype],
[filesize],
[attachment],
[downloads],
[attachprice],
[width],
[height],
[isimage]
FROM [dnt_attachments] 
WHERE [pid]=@pid
GO

IF OBJECT_ID('dnt_getattachmentlistbyaid','P') IS NOT NULL
DROP PROC [dnt_getattachmentlistbyaid]
GO

CREATE PROCEDURE [dnt_getattachmentlistbyaid]
@aidlist varchar(500)
AS
SELECT 
[aid],
[tid],
[pid],
[filename]
FROM [dnt_attachments] 
WHERE CHARINDEX(','+RTRIM([dnt_attachments].[aid])+',', ','+@aidlist+',')>0
GO

IF OBJECT_ID('dnt_updateattachment','P') IS NOT NULL
DROP PROC [dnt_updateattachment]
GO

CREATE PROCEDURE [dnt_updateattachment]
@readperm INT,
@description NCHAR(100),
@attachprice INT,
@aid INT
AS
UPDATE [dnt_attachments] 
SET [readperm] = @readperm, [description] = @description, [attachprice] = @attachprice 
WHERE [aid] = @aid
GO

IF OBJECT_ID('dnt_updateattachmentinfo','P') IS NOT NULL
DROP PROC [dnt_updateattachmentinfo]
GO

CREATE PROCEDURE [dnt_updateattachmentinfo]
@readperm INT,
@description NCHAR(100),
@aid INT
AS
UPDATE [dnt_attachments] 
SET [readperm] = @readperm, [description] = @description 
WHERE [aid] = @aid
GO

IF OBJECT_ID('dnt_updateallfieldattachmentinfo','P') IS NOT NULL
DROP PROC [dnt_updateallfieldattachmentinfo]
GO

CREATE PROCEDURE [dnt_updateallfieldattachmentinfo]
@postdatetime DATETIME,
@readperm INT,
@filename NCHAR(100),
@filetype NCHAR(50),
@description NCHAR(100),
@filesize INT,
@attachment NCHAR(100),
@downloads INT,
@tid INT,
@pid INT,
@aid INT,
@attachprice INT,
@width INT,
@height INT,
@isimage TINYINT
AS
UPDATE [dnt_attachments] 
SET 
[postdatetime] = @postdatetime, 
[readperm] = @readperm, 
[filename] = @filename, 
[description] = @description, 
[filetype] = @filetype, 
[filesize] = @filesize, 
[attachment] = @attachment, 
[downloads] = @downloads, 
[tid]=@tid, 
[pid]=@pid, 
[attachprice]=@attachprice, 
[width]=@width, 
[height]=@height,
[isimage]=@isimage 
WHERE [aid]=@aid;
UPDATE [dnt_myattachments] SET [tid]=@tid,[pid]=@pid WHERE [aid]=@aid
GO

IF OBJECT_ID('dnt_deleteattachmentbypid','P') IS NOT NULL
DROP PROC [dnt_deleteattachmentbypid]
GO

CREATE PROCEDURE [dnt_deleteattachmentbypid]
@pid INT
AS
DELETE FROM [dnt_attachments] 
WHERE [pid]=@pid
GO

IF OBJECT_ID('dnt_deleteattachmentbyaidlist','P') IS NOT NULL
DROP PROC [dnt_deleteattachmentbyaidlist]
GO

CREATE PROCEDURE [dnt_deleteattachmentbyaidlist]
@aidlist VARCHAR(500)
AS
DELETE FROM [dnt_attachments] WHERE CHARINDEX(','+RTRIM([aid])+',',','+@aidlist+',') > 0
GO

IF OBJECT_ID('dnt_deleteattachmentbyaid','P') IS NOT NULL
DROP PROC [dnt_deleteattachmentbyaid]
GO

CREATE PROCEDURE [dnt_deleteattachmentbyaid]
@aid INT
AS
DELETE FROM [dnt_attachments] 
WHERE [aid]=@aid
GO

IF OBJECT_ID('dnt_deleteattachmentbytid','P') IS NOT NULL
DROP PROC [dnt_deleteattachmentbytid]
GO

CREATE PROCEDURE [dnt_deleteattachmentbytid]
@tid INT
AS
DELETE FROM [dnt_attachments] WHERE [tid] = @tid
GO

IF OBJECT_ID('dnt_deleteattachmentbytidlist','P') IS NOT NULL
DROP PROC [dnt_deleteattachmentbytidlist]
GO

CREATE PROCEDURE [dnt_deleteattachmentbytidlist]
@tidlist VARCHAR(500)
AS
DELETE FROM [dnt_attachments] WHERE CHARINDEX(','+RTRIM([tid])+',',','+@tidlist+',') > 0
GO

IF OBJECT_ID('dnt_getattachmentlistbytidlist','P') IS NOT NULL
DROP PROC [dnt_getattachmentlistbytidlist]
GO

CREATE PROCEDURE [dnt_getattachmentlistbytidlist]
@tidlist VARCHAR(500)
AS
SELECT 
[aid],
[filename] 
FROM [dnt_attachments] 
WHERE CHARINDEX(','+RTRIM([tid])+',',''+@tidlist+',') > 0
GO

IF OBJECT_ID('dnt_getattachmentlistbytid','P') IS NOT NULL
DROP PROC [dnt_getattachmentlistbytid]
GO

CREATE PROCEDURE [dnt_getattachmentlistbytid]
@tid INT
AS
SELECT 
[aid],
[filename] 
FROM [dnt_attachments] 
WHERE [tid]	= @tid
GO

IF OBJECT_ID('dnt_updateattachmentdownloads','P') IS NOT NULL
DROP PROC [dnt_updateattachmentdownloads]
GO

CREATE PROCEDURE [dnt_updateattachmentdownloads]
@aid INT
AS
UPDATE [dnt_attachments] SET [downloads]=[downloads]+1 WHERE [aid]=@aid
GO

IF OBJECT_ID('dnt_getattachenmtlistbypid','P') IS NOT NULL
DROP PROC [dnt_getattachenmtlistbypid]
GO

CREATE PROCEDURE [dnt_getattachenmtlistbypid]
@pid INT
AS
SELECT 
[aid],
[uid],
[tid],
[pid],
[postdatetime],
[readperm],
[filename],
[description],
[filetype],
[filesize],
[attachment],
[downloads],
[attachprice],
[width],
[height] 
FROM [dnt_attachments] 
WHERE [pid]=@pid
GO

IF OBJECT_ID('dnt_getattachmentcountbytid','P') IS NOT NULL
DROP PROC [dnt_getattachmentcountbytid]
GO

CREATE PROCEDURE [dnt_getattachmentcountbytid]
@tid INT
AS
SELECT COUNT([aid]) AS [acount] FROM [dnt_attachments] WHERE [tid]=@tid
GO

IF OBJECT_ID('dnt_getattachmentcountbypid','P') IS NOT NULL
DROP PROC [dnt_getattachmentcountbypid]
GO

CREATE PROCEDURE [dnt_getattachmentcountbypid]
@pid INT
AS
SELECT COUNT([aid]) AS [acount] FROM [dnt_attachments] WHERE [pid]=@pid
GO

IF OBJECT_ID('dnt_getattachenmtinfobyaid','P') IS NOT NULL
DROP PROC [dnt_getattachenmtinfobyaid]
GO

CREATE PROCEDURE [dnt_getattachenmtinfobyaid]
@aid INT
AS
SELECT TOP 1
[aid],
[uid],
[tid],
[pid],
[postdatetime],
[readperm],
[filename],
[description],
[filetype],
[filesize],
[attachment],
[downloads],
[attachprice],
[width],
[height],
[isimage]
FROM [dnt_attachments] 
WHERE [aid]=@aid
GO

IF OBJECT_ID('dnt_getpolllist','P') IS NOT NULL
DROP PROC [dnt_getpolllist]
GO

CREATE PROCEDURE [dnt_getpolllist]
@tid int
AS
SELECT 
[pollid],
[tid],
[displayorder],
[multiple],
[visible],
[allowview],
[maxchoices],
[expiration],
[uid],
[voternames] 
FROM [dnt_polls] 
WHERE [tid]=@tid
GO

IF OBJECT_ID('dnt_gettaginfo','P') IS NOT NULL
DROP PROC [dnt_gettaginfo]
GO

CREATE PROCEDURE [dnt_gettaginfo]
@tagid int
AS
SELECT 
[tagid],
[tagname],
[userid],
[postdatetime],
[orderid],
[color],
[count],
[fcount],
[pcount],
[scount],
[vcount],
[gcount] 
FROM [dnt_tags] 
WHERE [tagid]=@tagid
GO

IF OBJECT_ID('dnt_setcurrenttopics','P') IS NOT NULL
DROP PROC [dnt_setcurrenttopics]
GO

CREATE PROCEDURE [dnt_setcurrenttopics]
@fid int
AS
UPDATE 
[dnt_forums] 
SET [curtopics] = (SELECT COUNT(tid) FROM [dnt_topics] WHERE [displayorder] >= 0 AND [fid]=@fid) WHERE [fid]=@fid
GO

IF OBJECT_ID('[dnt_createinvitecode]','P') IS NOT NULL
DROP PROC [dnt_createinvitecode]
GO

CREATE PROCEDURE [dnt_createinvitecode]
                    @code char(8),
                    @creatorid int,
                    @creator nchar(20),
                    @createtime smalldatetime,
                    @expiretime smalldatetime,
                    @maxcount int,
                    @invitetype int

                    AS
                    
                    INSERT INTO [dnt_invitation]([invitecode],[creatorid],[creator],[createdtime],[expiretime],[maxcount],[invitetype])
                     VALUES(@code,@creatorid,@creator,@createtime,@expiretime,@maxcount,@invitetype);SELECT SCOPE_IDENTITY()

GO

IF OBJECT_ID('[dnt_deleteinvitecode]','P') IS NOT NULL
DROP PROC [dnt_deleteinvitecode]
GO

CREATE PROCEDURE [dnt_deleteinvitecode]
@id int
AS
BEGIN
UPDATE [dnt_invitation] SET [isdeleted]=1 WHERE [inviteid]=@id
END
GO

IF OBJECT_ID('[dnt_clearexpireinvitecode]','P') IS NOT NULL
DROP PROC [dnt_clearexpireinvitecode]
GO

CREATE PROCEDURE [dnt_clearexpireinvitecode]
AS
BEGIN
DELETE [dnt_invitation] WHERE [invitetype]=3 AND [createdtime]<>[expiretime] AND ([expiretime]-GETDATE())<=0;
DELETE [dnt_invitation] WHERE [isdeleted]=1 AND [createdtime]<(GETDATE()-1)
END
GO

if exists (select * from sysobjects where id = object_id(N'[dnt_clearexpirecreditorders]') and OBJECTPROPERTY(id,N'IsProcedure') = 1)
drop procedure [dnt_clearexpirecreditorders]
GO

CREATE PROCEDURE [dnt_clearexpirecreditorders]
AS
BEGIN
   DELETE [dnt_orders] WHERE [orderstatus] < 1 AND [createdtime] < (GETDATE()-15)
END
GO

--2009-10-30 后台优化----------------------------------------------
IF OBJECT_ID('dnt_getmaxandmintid','P') IS NOT NULL
DROP PROC [dnt_getmaxandmintid]
GO

CREATE PROC [dnt_getmaxandmintid]
@fid INT
AS
SELECT 
MAX([tid]) AS [maxtid],
MIN([tid]) AS [mintid] 
FROM [dnt_topics] 
WHERE 
[fid] IN (SELECT [fid] 
		  FROM [dnt_forums] 
		  WHERE [fid]=@fid 
		  OR (CHARINDEX(',' + RTRIM(@fid) + ',', ',' + RTRIM(parentidlist) + ',') > 0))
GO

IF OBJECT_ID('dnt_getdesignatepostcount','P') IS NOT NULL
DROP PROC [dnt_getdesignatepostcount]
GO

CREATE PROC [dnt_getdesignatepostcount]
@fid INT,
@posttablename VARCHAR(50)
AS
EXEC ('SELECT COUNT([pid]) AS [postcount] FROM [' + @posttablename + '] WHERE [fid] = ' + @fid)
GO

IF OBJECT_ID('dnt_getpostcountbyuid','P') IS NOT NULL
DROP PROC [dnt_getpostcountbyuid]
GO

CREATE PROC [dnt_getpostcountbyuid]
@uid INT,
@posttablename VARCHAR(50)
AS
EXEC ('SELECT COUNT([pid]) AS [postcount] FROM [' + @posttablename + '] WHERE [posterid] = ' + @uid)
GO

IF OBJECT_ID('dnt_gettodaypostcountbyuid','P') IS NOT NULL
DROP PROC [dnt_gettodaypostcountbyuid]
GO

CREATE PROC [dnt_gettodaypostcountbyuid]
@uid INT,
@posttablename VARCHAR(50)
AS
EXEC ('SELECT COUNT([pid]) AS [postcount] 
FROM [' + @posttablename + '] 
WHERE [posterid] = ' + @uid + ' AND DATEDIFF(day, [postdatetime], GETDATE()) = 0')
GO

IF OBJECT_ID('dnt_gettotaltopiccount','P') IS NOT NULL
DROP PROC [dnt_gettotaltopiccount]
GO

CREATE PROC [dnt_gettotaltopiccount]
AS
SELECT COUNT([tid]) AS [topicscount] FROM [dnt_topics]
GO

IF OBJECT_ID('dnt_resetstatistic','P') IS NOT NULL
DROP PROC [dnt_resetstatistic]
GO

CREATE PROC [dnt_resetstatistic]
@totaltopic INT,
@totalpost INT,
@totalusers INT,
@lastusername VARCHAR(20),
@lastuserid INT
AS
UPDATE [dnt_statistics] 
SET 
[totaltopic]=@totaltopic,[totalpost]=@totalpost,[totalusers]=@totalusers,[lastusername]=@lastusername,[lastuserid]=@lastuserid
GO

IF OBJECT_ID('dnt_gettopusers','P') IS NOT NULL
DROP PROC [dnt_gettopusers]
GO

CREATE PROC [dnt_gettopusers]
@statcont INT,
@lastuid INT
AS
EXEC('
		SELECT TOP '+@statcont+' [uid] FROM [dnt_users] WHERE [uid] > @lastuid
	')
GO

IF OBJECT_ID('dnt_resetuserdigestposts','P') IS NOT NULL
DROP PROC [dnt_resetuserdigestposts]
GO

CREATE PROC [dnt_resetuserdigestposts]
@userid INT
AS
UPDATE [dnt_users] 
SET [digestposts]=(SELECT COUNT(tid) AS [digestposts] FROM [dnt_topics] WHERE [dnt_topics].[posterid] = [dnt_users].[uid] AND [digest] > 0) WHERE [dnt_users].[uid] = @userid
GO

IF OBJECT_ID('dnt_getusers','P') IS NOT NULL
DROP PROC [dnt_getusers]
GO

CREATE PROC [dnt_getusers]
@start_uid INT,
@end_uid INT
AS
SELECT [uid] FROM [dnt_users] WHERE [uid] >= @start_uid AND [uid]<=@end_uid
GO

IF OBJECT_ID('dnt_updateuserpostcount','P') IS NOT NULL
DROP PROC [dnt_updateuserpostcount]
GO

CREATE PROC [dnt_updateuserpostcount]
@postcount INT,
@userid INT
AS
UPDATE [dnt_users] SET [posts]=@postcount WHERE [dnt_users].[uid] = @userid
GO

IF OBJECT_ID('dnt_getonlineregistercount','P') IS NOT NULL
DROP PROC [dnt_getonlineregistercount]
GO

CREATE PROC [dnt_getonlineregistercount]
AS
SELECT COUNT(olid) FROM [dnt_online] WHERE [userid]>0
GO

IF OBJECT_ID('dnt_gettopictids','P') IS NOT NULL
DROP PROC [dnt_gettopictids]
GO

CREATE PROC [dnt_gettopictids]
@statcount INT,
@lasttid INT
AS
EXEC('
		SELECT TOP '+@statcount+' [tid] FROM [dnt_topics] WHERE [tid] > @lasttid ORDER BY [tid]
	')
GO

IF OBJECT_ID('dnt_updatelastpostoftopic','P') IS NOT NULL
DROP PROC [dnt_updatelastpostoftopic]
GO

CREATE PROC [dnt_updatelastpostoftopic]
@tid INT,
@postcount INT,
@lastpostid INT,
@lastpost VARCHAR(20),
@lastposterid INT,
@lastposter VARCHAR(20)
AS
UPDATE [dnt_topics] 
SET [lastpost]=@lastpost, [lastposterid]=@lastposterid, [lastposter]=@lastposter, [replies]=@postcount, [lastpostid]=@lastpostid 
WHERE [tid] = @tid
GO

IF OBJECT_ID('dnt_updatetopiclastposterid','P') IS NOT NULL
DROP PROC [dnt_updatetopiclastposterid]
GO

CREATE PROC [dnt_updatetopiclastposterid]
@tid INT
AS
UPDATE [dnt_topics] 
SET [lastposterid]=(SELECT ISNULL(MIN(lastpostid), -1)-1 FROM [dnt_topics] WHERE [tid] = @tid)
GO

IF OBJECT_ID('dnt_gettopics','P') IS NOT NULL
DROP PROC [dnt_gettopics]
GO

CREATE PROC [dnt_gettopics]
@start_tid INT,
@end_tid INT
AS
SELECT [tid] 
FROM [dnt_topics] 
WHERE [tid] >= @start_tid AND [tid]<=@end_tid 
ORDER BY [tid]
GO

IF OBJECT_ID('dnt_gettopforumfids','P') IS NOT NULL
DROP PROC [dnt_gettopforumfids]
GO

CREATE PROC [dnt_gettopforumfids]
@lastfid INT,
@statcount INT
AS
EXEC('
		SELECT TOP(@statcount) [fid] FROM [dnt_forums] WHERE [fid] > @lastfid
	')
GO

IF OBJECT_ID('dnt_getlastpostbyfid','P') IS NOT NULL
DROP PROC [dnt_getlastpostbyfid]
GO

CREATE PROC [dnt_getlastpostbyfid]
@fid INT,
@posttablename VARCHAR(50)
AS
EXEC ('SELECT TOP 1 [tid], [title], [postdatetime], [posterid], [poster], [pid] 
FROM [' + @posttablename + '] 
WHERE [fid] = ' + @fid +
'ORDER BY [pid] DESC')
GO

IF OBJECT_ID('dnt_updateforum','P') IS NOT NULL
DROP PROC [dnt_updateforum]
GO

CREATE PROC [dnt_updateforum]
@lastfid INT,
@topiccount INT,
@postcount INT,
@lasttid INT,
@lasttitle NCHAR(80),
@lastpost VARCHAR(20),
@lastposterid INT,
@lastposter NCHAR(20),
@todaypostcount INT 
AS
UPDATE [dnt_forums] 
SET [topics] = @topiccount, [posts]=@postcount, [todayposts] = @todaypostcount, [lasttid] = @lasttid, [lasttitle] = @lasttitle, [lastpost]=@lastpost, [lastposterid] = @lastposterid, [lastposter]=@lastposter 
WHERE [dnt_forums].[fid] = @lastfid
GO

IF OBJECT_ID('dnt_resetclearmove','P') IS NOT NULL
DROP PROC [dnt_resetclearmove]
GO

CREATE PROC [dnt_resetclearmove]
AS
DELETE FROM [dnt_topics] WHERE [closed] > 1
GO

IF OBJECT_ID('dnt_updatemytopic','P') IS NOT NULL
DROP PROC [dnt_updatemytopic]
GO

CREATE PROC [dnt_updatemytopic]
AS
DELETE FROM [dnt_mytopics]
INSERT INTO [dnt_mytopics]([uid], [tid], [dateline]) 
SELECT [posterid],[tid],[postdatetime] 
FROM [dnt_topics] 
WHERE [posterid]<>-1
GO

IF OBJECT_ID('dnt_updatemypost','P') IS NOT NULL
DROP PROC [dnt_updatemypost]
GO

CREATE PROC [dnt_updatemypost]
@tablename VARCHAR(50)
AS
DELETE FROM [dnt_myposts]
EXEC ('
		INSERT INTO [dnt_myposts]([uid], [tid], [pid], [dateline])
		SELECT [posterid],[tid],[pid],[postdatetime] FROM [' + @tablename + '] WHERE [posterid]<>-1
	')
GO

IF OBJECT_ID('dnt_updateforumsinfo','P') IS NOT NULL
DROP PROC [dnt_updateforumsinfo]
GO

CREATE PROC [dnt_updateforumsinfo]
@parentid SmallInt,
@layer Int,
@pathlist NChar(3000),
@parentidlist NChar(300),
@subforumcount Int,
@name NChar(50),
@status Int,
@colcount SmallInt,
@displayorder Int,
@templateid SmallInt,
@topics Int,
@curtopics Int,
@posts Int,
@todayposts Int,
@lasttid Int,
@lasttitle NChar(60),
@lastpost DateTime,
@lastposterid Int,
@lastposter NChar(20),
@allowsmilies Int,
@allowrss Int,
@allowhtml Int,
@allowbbcode Int,
@allowimgcode Int,
@allowblog Int,
@istrade Int,
@allowpostspecial Int,
@allowspecialonly Int,
@alloweditrules Int,
@allowthumbnail Int,
@allowtag Int,
@recyclebin Int,
@modnewposts Int,
@modnewtopics Int,
@jammer Int,
@disablewatermark Int,
@inheritedmod Int,
@autoclose SmallInt,
@password NVarChar(16),
@icon VarChar(255),
@postcredits VarChar(255),
@replycredits VarChar(255),
@redirect VarChar(255),
@attachextensions VarChar(255),
@rules NText,
@topictypes Text,
@viewperm Text,
@postperm Text,
@replyperm Text,
@getattachperm Text,
@postattachperm Text,
@moderators Text,
@description NText,
@applytopictype TinyInt,
@postbytopictype TinyInt,
@viewbytopictype TinyInt,
@topictypeprefix TinyInt,
@permuserlist NText,
@seokeywords NVarChar(500),
@seodescription NVarChar(500),
@rewritename NVarChar(20),
@fid Int

AS
UPDATE [dnt_forums] 
SET [parentid]=@parentid, [layer]=@layer, [pathlist]=@pathlist, 
[parentidlist]=@parentidlist, [subforumcount]=@subforumcount, [name]=@name, [status]=@status,
[colcount]=@colcount, [displayorder]=@displayorder,[templateid]=@templateid,[topics]=@topics,
[curtopics]=@curtopics,[posts]=@posts,[todayposts]=@todayposts,[lasttid]=@lasttid,[lasttitle]=@lasttitle,
[lastpost]=@lastpost,[lastposterid]=@lastposterid,[lastposter]=@lastposter,
[allowsmilies]=@allowsmilies ,[allowrss]=@allowrss, [allowhtml]=@allowhtml, [allowbbcode]=@allowbbcode, [allowimgcode]=@allowimgcode, 
[allowblog]=@allowblog,[istrade]=@istrade,[allowpostspecial]=@allowpostspecial,[allowspecialonly]=@allowspecialonly,
[alloweditrules]=@alloweditrules ,[allowthumbnail]=@allowthumbnail ,[allowtag]=@allowtag,
[recyclebin]=@recyclebin, [modnewposts]=@modnewposts,[modnewtopics]=@modnewtopics,[jammer]=@jammer,[disablewatermark]=@disablewatermark,[inheritedmod]=@inheritedmod,
[autoclose]=@autoclose 
WHERE [fid]=@fid

UPDATE [dnt_forumfields] 
SET [password]=@password,[icon]=@icon,[postcredits]=@postcredits,
 [replycredits]=@replycredits,[redirect]=@redirect,[attachextensions]=@attachextensions,[rules]=@rules,[topictypes]=@topictypes,
 [viewperm]=@viewperm,[postperm]=@postperm,[replyperm]=@replyperm,[getattachperm]=@getattachperm,[postattachperm]=@postattachperm,
 [moderators]=@moderators,[description]=@description,[applytopictype]=@applytopictype,[postbytopictype]=@postbytopictype,
 [viewbytopictype]=@viewbytopictype,[topictypeprefix]=@topictypeprefix,[permuserlist]=@permuserlist,[seokeywords]=@seokeywords,
 [seodescription]=@seodescription,[rewritename]=@rewritename 
 WHERE [fid]=@fid
GO

IF OBJECT_ID('dnt_insertforumsinfo','P') IS NOT NULL
DROP PROC [dnt_insertforumsinfo]
GO

CREATE PROC [dnt_insertforumsinfo]
@parentid SmallInt,
@layer Int,
@pathlist NChar(3000),
@parentidlist NChar(300),
@subforumcount Int,
@name NChar(50),
@status Int,
@colcount SmallInt,
@displayorder Int,
@templateid SmallInt,
@allowsmilies Int,
@allowrss Int,
@allowhtml Int,
@allowbbcode Int,
@allowimgcode Int,
@allowblog Int,
@istrade Int,
@alloweditrules Int,
@allowthumbnail Int,
@allowtag Int,
@recyclebin Int,
@modnewposts Int,
@modnewtopics Int,
@jammer Int,
@disablewatermark Int,
@inheritedmod Int,
@autoclose SmallInt,
@allowpostspecial Int,
@allowspecialonly Int,
@description NText,
@password VarChar(16),
@icon VarChar(255),
@postcredits VarChar(255),
@replycredits VarChar(255),
@redirect VarChar(255),
@attachextensions VarChar(255),
@moderators Text,
@rules NText,
@topictypes Text,
@viewperm Text,
@postperm Text,
@replyperm Text,
@getattachperm Text,
@postattachperm Text,
@seokeywords NVarChar(500),
@seodescription NVarChar(500),
@rewritename NVarChar(20)
AS
DECLARE @fid INT

INSERT INTO [dnt_forums] ([parentid],[layer],[pathlist],[parentidlist],[subforumcount],[name],
[status],[colcount],[displayorder],[templateid],[allowsmilies],[allowrss],[allowhtml],[allowbbcode],[allowimgcode],[allowblog],
[istrade],[alloweditrules],[recyclebin],[modnewposts],[modnewtopics],[jammer],[disablewatermark],[inheritedmod],[autoclose],[allowthumbnail],
[allowtag],[allowpostspecial],[allowspecialonly]) 
VALUES (@parentid,@layer,@pathlist,@parentidlist,@subforumcount,@name,@status, @colcount, @displayorder,
@templateid,@allowsmilies,@allowrss,@allowhtml,@allowbbcode,@allowimgcode,@allowblog,@istrade,@alloweditrules,@recyclebin,
@modnewposts,@modnewtopics,@jammer,@disablewatermark,@inheritedmod,@autoclose,@allowthumbnail,@allowtag,@allowpostspecial,@allowspecialonly)
--SET @fid=@@IDENTITY
SELECT @fid = ISNULL(MAX(fid), 0) FROM [dnt_forums]

INSERT INTO [dnt_forumfields] ([fid],[description],[password],[icon],[postcredits],[replycredits],[redirect],
[attachextensions],[moderators],[rules],[topictypes],[viewperm],[postperm],[replyperm],[getattachperm],[postattachperm],[seokeywords],[seodescription],[rewritename]) 
VALUES (@fid,@description,@password,@icon,@postcredits,@replycredits,@redirect,@attachextensions,@moderators,@rules,@topictypes,@viewperm,
@postperm,@replyperm,@getattachperm,@postattachperm,@seokeywords,@seodescription,@rewritename) 
GO

IF OBJECT_ID('dnt_getsubforumcount','P') IS NOT NULL
DROP PROC [dnt_getsubforumcount]
GO

CREATE PROC [dnt_getsubforumcount]
@fid INT
AS
SELECT COUNT(fid) FROM [dnt_forums] WHERE [parentid]=@fid
GO

IF OBJECT_ID('dnt_deleteforumsbyfid','P') IS NOT NULL
DROP PROC [dnt_deleteforumsbyfid]
GO

CREATE PROC [dnt_deleteforumsbyfid]
@fid INT,
@postname NVARCHAR(50)
AS
DECLARE @parentid INT,
		@displayorder INT		
SELECT TOP 1 @parentid=[parentid],@displayorder=[displayorder] FROM [dnt_forums] WHERE [fid]=@fid
UPDATE [dnt_forums] SET [displayorder]=[displayorder]-1 WHERE [displayorder]>@displayorder
UPDATE [dnt_forums] SET [subforumcount]=[subforumcount]-1 WHERE  [fid]=@parentid
DELETE FROM [dnt_forumfields] WHERE [fid]=@fid
DELETE FROM [dnt_polls] WHERE [tid] IN (SELECT [tid] FROM [dnt_topics] WHERE [fid]=@fid)
EXEC ('DELETE FROM [dnt_attachments] WHERE [tid] IN(SELECT [tid] FROM [dnt_topics] WHERE [fid]=' + @fid + ') OR [pid] IN(SELECT [pid] FROM [' + @postname + '] WHERE [fid]=' + @fid + ')')
EXEC ('DELETE FROM [' + @postname + '] WHERE [fid]=' + @fid)
DELETE FROM [dnt_topics] WHERE [fid]=@fid
DELETE FROM [dnt_forums] WHERE  [fid]=@fid
DELETE FROM [dnt_moderators] WHERE  [fid]=@fid
GO

IF OBJECT_ID('dnt_getparentidbyfid','P') IS NOT NULL
DROP PROC [dnt_getparentidbyfid]
GO

CREATE PROC [dnt_getparentidbyfid]
@fid INT
AS
SELECT [parentid] From [dnt_forums] WHERE [inheritedmod]=1 AND [fid]=@fid
GO

IF OBJECT_ID('dnt_updateforumsmoderators','P') IS NOT NULL
DROP PROC [dnt_updateforumsmoderators]
GO

CREATE PROC [dnt_updateforumsmoderators]
@displayorder INT,
@moderators VARCHAR(500),
@fid INT,
@inherited INT
AS
DECLARE @usernamelist VARCHAR(255),
		@username VARCHAR(255),
		@uid INT,
		@len INT,
		@b BIT,
		@begin INT,
		@end INT
SELECT @len=0,@begin=1,@end=1,@b=1,@usernamelist=''
WHILE @b=1
	BEGIN
		IF @end=1
			SET @begin=@end
		ELSE
			SET @begin=@end+1
		SET @end = CHARINDEX(',',@moderators,@begin)
		IF @end=0
			SET @end=LEN(@moderators)+1
		SET @len=@end-@begin
		IF @len>0
			BEGIN
				SET @username=SUBSTRING(@moderators,@begin,@len)
				IF @username<>''
					BEGIN
						SELECT @uid=[uid] FROM [dnt_users] WHERE [groupid]<>7 AND [groupid]<>8 AND [username]=@username
						IF @uid IS NOT NULL
							BEGIN
								INSERT INTO [dnt_moderators] ([uid],[fid],[displayorder],[inherited]) VALUES (@uid,@fid,@displayorder,@inherited)
								SET @usernamelist=@usernamelist+@username+','
								SET @displayorder=@displayorder+1
								SET @uid=NULL
							END
					END
			END
		ELSE
			SET @b=0
	END
IF RIGHT(@usernamelist,1)=','
	SET @usernamelist=LEFT(@usernamelist,LEN(@usernamelist)-1)
UPDATE [dnt_forumfields] SET [moderators]=@usernamelist WHERE [fid] =@fid
GO

IF OBJECT_ID('dnt_getforumstable','P') IS NOT NULL
DROP PROC [dnt_getforumstable]
GO

CREATE PROC [dnt_getforumstable]
AS
SELECT
[f].[fid],[f].[parentid],[f].[layer],[f].[pathlist],[f].[parentidlist],[f].[subforumcount],[f].[name],[f].[status],[f].[colcount],[f].[displayorder],[f].[templateid],[f].[topics],[f].[curtopics],[f].[posts],[f].[todayposts],[f].[lasttid],[f].[lasttitle],[f].[lastpost],[f].[lastposterid],[f].[lastposter],[f].[allowsmilies],[f].[allowrss],[f].[allowhtml],[f].[allowbbcode],[f].[allowimgcode],[f].[allowblog],[f].[istrade],[f].[allowpostspecial],[f].[allowspecialonly],[f].[alloweditrules],[f].[allowthumbnail],[f].[allowtag],[f].[recyclebin],[f].[modnewposts],[f].[modnewtopics],[f].[jammer],[f].[disablewatermark],[f].[inheritedmod],[f].[autoclose],[ff].[password],[ff].[icon],[ff].[postcredits],[ff].[replycredits],[ff].[redirect],[ff].[attachextensions],[ff].[rules],[ff].[topictypes],[ff].[viewperm],[ff].[postperm],[ff].[replyperm],[ff].[getattachperm],[ff].[postattachperm],[ff].[moderators],[ff].[description],[ff].[applytopictype],[ff].[postbytopictype],[ff].[viewbytopictype],[ff].[topictypeprefix],[ff].[permuserlist],[ff].[seokeywords],[ff].[seodescription],[ff].[rewritename] 
FROM [dnt_forums] AS [f] 
LEFT JOIN [dnt_forumfields] AS [ff] 
ON [f].[fid]=[ff].[fid] 
ORDER BY [f].[displayorder]
GO

IF OBJECT_ID('dnt_getmainforum','P') IS NOT NULL
DROP PROC [dnt_getmainforum]
GO

CREATE PROC [dnt_getmainforum]
AS
SELECT
[fid],[parentid],[layer],[pathlist],[parentidlist],[subforumcount],[name],[status],[colcount],[displayorder],[templateid],[topics],[curtopics],[posts],[todayposts],[lasttid],[lasttitle],[lastpost],[lastposterid],[lastposter],[allowsmilies],[allowrss],[allowhtml],[allowbbcode],[allowimgcode],[allowblog],[istrade],[allowpostspecial],[allowspecialonly],[alloweditrules],[allowthumbnail],[allowtag],[recyclebin],[modnewposts],[jammer],[disablewatermark],[inheritedmod],[autoclose] 
FROM [dnt_forums] 
WHERE [layer]=0 
Order By [displayorder] ASC
GO

IF OBJECT_ID('dnt_getlastposttid','P') IS NOT NULL
DROP PROC [dnt_getlastposttid]
GO

CREATE PROC [dnt_getlastposttid]
@visibleforums VARCHAR(4000),
@fid INT
AS
IF @visibleforums=''
SELECT TOP 1 [tid] FROM [dnt_topics] AS t LEFT JOIN [dnt_forums] AS f  ON [t].[fid] = [f].[fid] 
WHERE [t].[closed]<>1 AND  [t].[displayorder] >=0  AND ([t].[fid] = @fid 
OR CHARINDEX(',' + CONVERT(NVARCHAR(10), @fid) + ',' , ',' + RTRIM([f].[parentidlist]) + ',') > 0 )  
ORDER BY [t].[lastpost] DESC
ELSE
EXEC('SELECT TOP 1 [tid] FROM [dnt_topics] AS t LEFT JOIN [dnt_forums] AS f  ON [t].[fid] = [f].[fid] 
WHERE [t].[closed]<>1 AND  [t].[displayorder] >=0  AND ([t].[fid] = ' + @fid +
'OR CHARINDEX('','' + CONVERT(NVARCHAR(10), ' + @fid + ') + '','' , '','' + RTRIM([f].[parentidlist]) + '','') > 0 )  
AND [t].[fid] IN ('+@visibleforums+')  ORDER BY [t].[lastpost] DESC')
GO

IF OBJECT_ID('dnt_deluserallinf','P') IS NOT NULL
DROP PROC [dnt_deluserallinf]
GO

CREATE PROC [dnt_deluserallinf]
@uid INT,
@delPosts BIT,
@delPms BIT
AS
DECLARE @existuid INT
SELECT @existuid = COUNT([uid]) FROM [dnt_users] WHERE [uid]=@uid
IF @existuid <>0
BEGIN
	DELETE FROM [dnt_users] WHERE [uid]=@uid                 
	DELETE FROM [dnt_userfields] WHERE [uid]=@uid                 
	DELETE FROM [dnt_onlinetime] WHERE [uid]=@uid                
	DELETE FROM [dnt_polls] WHERE [uid]=@uid                 
	DELETE FROM [dnt_favorites] WHERE [uid]=@uid
	DECLARE @tableid INT,
			@tablename NVARCHAR(20)
	IF @delPosts = 1
		BEGIN
			DELETE FROM [dnt_topics] WHERE [posterid]=@uid
			DECLARE tables_cursor CURSOR FOR SELECT [id] FROM [dnt_tablelist]
			OPEN tables_cursor
			FETCH NEXT FROM tables_cursor INTO @tableid
			WHILE @@FETCH_STATUS = 0
			BEGIN
				SET @tablename = 'dnt_posts' + LTRIM(STR(@tableid))
				EXEC ('DELETE FROM ' + @tablename + ' WHERE [posterid]='+@uid)
				FETCH NEXT FROM tables_cursor INTO @tableid
			END
			CLOSE tables_cursor
			DEALLOCATE tables_cursor
			SET @tableid = 0
			SET @tablename = ''
		END
	ELSE
		BEGIN
			UPDATE [dnt_topics] SET [poster]='该用户已被删除' Where [posterid]=@uid
			UPDATE [dnt_topics] SET [lastposter]='该用户已被删除' Where [lastpostid]=@uid
			DECLARE tables_cursor CURSOR FOR SELECT [id] FROM [dnt_tablelist]
			OPEN tables_cursor
			FETCH NEXT FROM tables_cursor INTO @tableid
			WHILE @@FETCH_STATUS = 0
			BEGIN
				SET @tablename = 'dnt_posts' + LTRIM(STR(@tableid))
				EXEC ('UPDATE ['+@tablename+'] SET [poster]=''该用户已被删除'' WHERE [posterid]='+@uid)
				FETCH NEXT FROM tables_cursor INTO @tableid
			END
			CLOSE tables_cursor
			DEALLOCATE tables_cursor
			SET @tableid = 0
			SET @tablename = ''
		END
	IF @delPms = 1
		BEGIN
			DELETE FROM [dnt_pms] WHERE [msgfromid]=@uid
		END
	ELSE
		BEGIN
			UPDATE [dnt_pms] SET [msgfrom]='该用户已被删除' WHERE [msgfromid]=@uid
			UPDATE [dnt_pms] SET [msgto]='该用户已被删除' WHERE [msgtoid]=@uid
		END
	DELETE FROM [dnt_moderators] WHERE [uid]=@uid
	UPDATE [dnt_statistics] SET [totalusers]=[totalusers]-1
	DECLARE @lastuserid INT,@lastusername VARCHAR(50)
	SELECT TOP 1 @lastuserid=[uid],@lastusername=[username] FROM [dnt_users] ORDER BY [uid] DESC
	IF @lastuserid IS NOT NULL
		UPDATE [dnt_Statistics] SET [lastuserid]=@lastuserid, [lastusername]=@lastusername
END
GO

IF OBJECT_ID('dnt_addusergroup','P') IS NOT NULL
DROP PROC [dnt_addusergroup]
GO

CREATE PROCEDURE [dnt_addusergroup]
@Radminid INT,
@Grouptitle NVARCHAR(50),
@Creditshigher INT,
@Creditslower INT,
@Stars INT,
@Color CHAR(7),
@Groupavatar NVARCHAR(60), 
@Readaccess INT,
@Allowvisit INT,
@Allowpost INT,
@Allowreply INT,
@Allowpostpoll INT,
@Allowdirectpost INT,
@Allowgetattach INT,
@Allowpostattach INT,
@Allowvote INT,
@Allowmultigroups INT, 
@Allowsearch INT,
@Allowavatar INT,
@Allowcstatus INT,
@Allowuseblog INT,
@Allowinvisible INT,
@Allowtransfer INT,
@Allowsetreadperm INT, 
@Allowsetattachperm INT,
@Allowhidecode INT,
@Allowhtml INT,
@Allowhtmltitle INT,
@Allowcusbbcode INT,
@Allownickname INT,
@Allowsigbbcode INT,
@Allowsigimgcode INT,
@Allowviewpro INT,
@Allowviewstats INT,
@Allowtrade INT,
@Allowdiggs INT,
@Allowdebate INT,
@Allowbonus INT,
@Minbonusprice INT,
@Maxbonusprice INT,
@Disableperiodctrl INT, 
@Reasonpm INT,
@Maxprice SMALLINT,
@Maxpmnum SMALLINT,
@Maxsigsize SMALLINT,
@Maxattachsize INT,
@Maxsizeperday INT,
@Attachextensions CHAR(100),
@Maxspaceattachsize INT,
@Maxspacephotosize INT, 
@Raterange CHAR(100),
@ModNewTopics SMALLINT,
@ModNewPosts SMALLINT,
@Ignoreseccode INT
AS
INSERT INTO [dnt_usergroups]  ([radminid],[grouptitle],[creditshigher],[creditslower],
[stars] ,[color], [groupavatar],[readaccess], [allowvisit],[allowpost],[allowreply],
[allowpostpoll], [allowdirectpost],[allowgetattach],[allowpostattach],[allowvote],[allowmultigroups],
[allowsearch],[allowavatar],[allowcstatus],[allowuseblog],[allowinvisible],[allowtransfer],
[allowsetreadperm],[allowsetattachperm],[allowhidecode],[allowhtml],[allowhtmltitle],[allowcusbbcode],[allownickname],
[allowsigbbcode],[allowsigimgcode],[allowviewpro],[allowviewstats],[allowtrade],[allowdiggs],[disableperiodctrl],[reasonpm],
[maxprice],[maxpmnum],[maxsigsize],[maxattachsize],[maxsizeperday],[attachextensions],[raterange],[maxspaceattachsize],
[maxspacephotosize],[allowdebate],[allowbonus],[minbonusprice],[maxbonusprice],[modnewtopics],[modnewposts],[ignoreseccode]) 
VALUES(
@Radminid,@Grouptitle,@Creditshigher,@Creditslower,@Stars,@Color,@Groupavatar,@Readaccess,@Allowvisit,@Allowpost,@Allowreply,
@Allowpostpoll,@Allowdirectpost,@Allowgetattach,@Allowpostattach,@Allowvote,@Allowmultigroups,@Allowsearch,@Allowavatar,@Allowcstatus,
@Allowuseblog,@Allowinvisible,@Allowtransfer,@Allowsetreadperm,@Allowsetattachperm,@Allowhidecode,@Allowhtml,@Allowhtmltitle,@Allowcusbbcode,@Allownickname,
@Allowsigbbcode,@Allowsigimgcode,@Allowviewpro,@Allowviewstats,@Allowtrade,@Allowdiggs,@Disableperiodctrl,@Reasonpm,@Maxprice,@Maxpmnum,@Maxsigsize,@Maxattachsize, 
@Maxsizeperday,@Attachextensions,@Raterange,@Maxspaceattachsize,@Maxspacephotosize,@Allowdebate,@Allowbonus,@Minbonusprice,@Maxbonusprice,@ModNewTopics,@ModNewPosts,@Ignoreseccode)
GO

IF OBJECT_ID('dnt_updateusergroup','P') IS NOT NULL
DROP PROC [dnt_updateusergroup]
GO

CREATE PROCEDURE [dnt_updateusergroup]
@Radminid INT,
@Grouptitle NVARCHAR(50),
@Creditshigher INT,
@Creditslower INT, 
@Stars INT,
@Color CHAR(7),
@Groupavatar NVARCHAR(60),
@Readaccess INT,
@Allowvisit INT,
@Allowpost INT,
@Allowreply INT,
@Allowpostpoll INT,
@Allowdirectpost INT,
@Allowgetattach INT,
@Allowpostattach INT,
@Allowvote INT,
@Allowmultigroups INT,
@Allowsearch INT,
@Allowavatar INT,
@Allowcstatus INT,
@Allowuseblog INT,
@Allowinvisible INT,
@Allowtransfer INT,
@Allowsetreadperm INT,
@Allowsetattachperm INT,
@Allowhidecode INT,
@Allowhtml INT,
@Allowhtmltitle INT,
@Allowcusbbcode INT,
@Allownickname INT,
@Allowsigbbcode INT,
@Allowsigimgcode INT,
@Allowviewpro INT,
@Allowviewstats INT,
@Allowtrade INT,
@Allowdiggs INT,
@Disableperiodctrl INT,
@Allowdebate INT,
@Allowbonus INT,
@Minbonusprice INT,
@Maxbonusprice INT,
@Reasonpm INT,
@Maxprice SMALLINT,
@Maxpmnum SMALLINT,
@Maxsigsize SMALLINT,
@Maxattachsize INT,
@Maxsizeperday INT,
@Attachextensions CHAR(100),
@Maxspaceattachsize INT,
@Maxspacephotosize INT,
@Groupid INT,
@ModNewTopics SMALLINT,
@ModNewPosts SMALLINT,
@Ignoreseccode INT
AS
UPDATE [dnt_usergroups]  
SET [radminid]=@Radminid,[grouptitle]=@Grouptitle,[creditshigher]=@Creditshigher,[creditslower]=@Creditslower,
[stars]=@Stars,[color]=@Color,[groupavatar]=@Groupavatar,[readaccess]=@Readaccess, 
[allowvisit]=@Allowvisit,[allowpost]=@Allowpost,[allowreply]=@Allowreply,[allowpostpoll]=@Allowpostpoll, 
[allowdirectpost]=@Allowdirectpost,[allowgetattach]=@Allowgetattach,[allowpostattach]=@Allowpostattach,
[allowvote]=@Allowvote,[allowmultigroups]=@Allowmultigroups,[allowsearch]=@Allowsearch,[allowavatar]=@Allowavatar,
[allowcstatus]=@Allowcstatus,[allowuseblog]=@Allowuseblog,[allowinvisible]=@Allowinvisible,
[allowtransfer]=@Allowtransfer,[allowsetreadperm]=@Allowsetreadperm,[allowsetattachperm]=@Allowsetattachperm,
[allowhidecode]=@Allowhidecode,[allowhtml]=@Allowhtml,[allowhtmltitle]=@Allowhtmltitle,[allowcusbbcode]=@Allowcusbbcode,
[allownickname]=@Allownickname,[allowsigbbcode]=@Allowsigbbcode,[allowsigimgcode]=@Allowsigimgcode,
[allowviewpro]=@Allowviewpro,[allowviewstats]=@Allowviewstats,[allowtrade]=@Allowtrade,[allowdiggs]=@Allowdiggs,
[disableperiodctrl]=@Disableperiodctrl,[allowdebate]=@Allowdebate,[allowbonus]=@Allowbonus,
[minbonusprice]=@Minbonusprice,[maxbonusprice]=@Maxbonusprice,[reasonpm]=@Reasonpm,[maxprice]=@Maxprice,
[maxpmnum]=@Maxpmnum,[maxsigsize]=@Maxsigsize,[maxattachsize]=@Maxattachsize,[maxsizeperday]=@Maxsizeperday,
[attachextensions]=@Attachextensions,[maxspaceattachsize]=@Maxspaceattachsize,
[maxspacephotosize]=@Maxspacephotosize,[modnewtopics]=@ModNewTopics,[modnewposts]=@ModNewPosts,[ignoreseccode]=@Ignoreseccode
WHERE [groupid]=@Groupid
GO

IF OBJECT_ID('dnt_getonlinegroupicontable','P') IS NOT NULL
DROP PROC [dnt_getonlinegroupicontable]
GO

CREATE PROC [dnt_getonlinegroupicontable]
AS
SELECT [groupid], [displayorder], [title], [img] FROM [dnt_onlinelist] WHERE [img] <> '' ORDER BY [displayorder]
GO

IF OBJECT_ID('dnt_getuserlistbygroupid','P') IS NOT NULL
DROP PROC [dnt_getuserlistbygroupid]
GO

CREATE PROC [dnt_getuserlistbygroupid]
@groupIdList VARCHAR(500)
AS
EXEC('
	SELECT 
	[uid],
	[username],
	[nickname],
	[password],
	[secques],
	[spaceid],
	[gender],
	[adminid],
	[groupid],
	[groupexpiry],
	[extgroupids],
	[regip],
	[joindate],
	[lastip],
	[lastvisit],
	[lastactivity],
	[lastpost],
	[lastpostid],
	[lastposttitle],
	[posts],
	[digestposts],
	[oltime],
	[pageviews],
	[credits],
	[extcredits1],
	[extcredits2],
	[extcredits3],
	[extcredits4],
	[extcredits5],
	[extcredits6],
	[extcredits7],
	[extcredits8],
	[avatarshowid],
	[email],
	[bday],
	[sigstatus],
	[tpp],
	[ppp],
	[templateid],
	[pmsound],
	[showemail],
	[invisible],
	[newpm],
	[newpmcount],
	[accessmasks],
	[onlinestate],
	[newsletter],
	[salt]
	FROM [dnt_users] WHERE [groupid] IN ('+@groupIdList+')
')
GO

IF OBJECT_ID('dnt_passpost','P') IS NOT NULL 
DROP PROC [dnt_passpost]
GO

CREATE PROC [dnt_passpost]@currentPostTableId INT,@postcount INT,@pidList NVARCHAR(500)ASEXEC('DECLARE @count INT,		@begin INT,		@postdatetime DATETIME,		@poster NVARCHAR(50),		@posterid INT,		@fid INT,		@title NVARCHAR(20),		@tid INT,		@pid INT		 SET @begin=1UPDATE  [dnt_posts'+@currentPostTableId+']   SET [invisible]=0 WHERE [pid] IN ('+@pidList+')UPDATE  [dnt_statistics] SET [totalpost]= [totalpost] + ' + @postcount + 'DECLARE @tempposttable TABLE 						(							[ROWID] INT IDENTITY(1,1),
							[pid] INT,
							[postdatetime] DATETIME,
							[poster] NVARCHAR(50),
							[posterid] INT,
							[fid] INT,
							[title] NVARCHAR(20),
							[tid] INT						)INSERT INTO @tempposttable SELECT [pid],[postdatetime],[poster],[posterid],[fid],[title],[tid] FROM [dnt_posts'+@currentPostTableId+'] WHERE [pid] IN ('+@pidList+')SELECT @count=COUNT(1) FROM @tempposttableWHILE @begin <= @countBEGIN	SELECT @postdatetime = [postdatetime],			@poster = [poster],			@posterid = [posterid],			@fid = [fid],			@title = [title],			@tid = [tid],			@pid = [pid]	FROM @tempposttable  WHERE [ROWID]=@begin	UPDATE [dnt_forums] 	SET [posts]=[posts] + 1, 	[todayposts]=CASE WHEN datediff(day,@postdatetime,getdate())=0 	THEN [todayposts] + 1		ELSE [todayposts] END,	[lastpost]=@postdatetime,	[lastposter]=@poster,	[lastposterid]=@posterid 	WHERE [fid]=@fid		UPDATE [dnt_users] 	SET [lastpost] = @postdatetime, 	[lastpostid] = @posterid, 	[lastposttitle] = @title, 	[posts] = [posts] + 1  	WHERE [uid] = @posterid		UPDATE [dnt_topics] 	SET [replies]=[replies]+1,	[lastposter]=@poster,	[lastposterid]=@posterid,	[lastpost]=@postdatetime,	[lastpostid]=@pid	WHERE [tid]=@tid		SET @begin = @begin + 1END')
GO

IF OBJECT_ID('dnt_getunauditnewtopicbycondition','P') IS NOT NULL
DROP PROC [dnt_getunauditnewtopicbycondition]
GO

CREATE PROCEDURE [dnt_getunauditnewtopicbycondition]
@fidlist NVARCHAR(500),
@pagesize int,
@displayorder INT,
@pageindex int
AS
DECLARE @startRow NVARCHAR(3)
SET @startRow = (@pageindex - 1) * @pagesize
IF @fidlist = '0'
BEGIN
	IF @pageindex = 1
	BEGIN
	EXEC('
		SELECT 
			TOP '+@pagesize+'
			[dnt_topics].[rate], 
			[dnt_topics].[fid], 
			[dnt_topics].[tid],
			[dnt_topics].[iconid],
			[dnt_topics].[typeid],
			[dnt_topics].[title],
			[dnt_topics].[price],
			[dnt_topics].[hide],
			[dnt_topics].[readperm],
			[dnt_topics].[poster],
			[dnt_topics].[posterid],
			[dnt_topics].[replies],
			[dnt_topics].[views],
			[dnt_topics].[postdatetime],
			[dnt_topics].[lastpost],
			[dnt_topics].[lastposter],
			[dnt_topics].[lastpostid],
			[dnt_topics].[lastposterid],
			[dnt_topics].[highlight],
			[dnt_topics].[digest],
			[dnt_topics].[displayorder],
			[dnt_topics].[attachment],
			[dnt_topics].[closed],
			[dnt_topics].[magic],
			[dnt_topics].[special]		
		FROM [dnt_topics]
		WHERE [displayorder] = '+@displayorder+'
		')
	END
	ELSE
	BEGIN
	EXEC('
		SELECT 
			TOP '+@pagesize+'
			[dnt_topics].[rate], 
			[dnt_topics].[fid], 
			[dnt_topics].[tid],
			[dnt_topics].[iconid],
			[dnt_topics].[typeid],
			[dnt_topics].[title],
			[dnt_topics].[price],
			[dnt_topics].[hide],
			[dnt_topics].[readperm],
			[dnt_topics].[poster],
			[dnt_topics].[posterid],
			[dnt_topics].[replies],
			[dnt_topics].[views],
			[dnt_topics].[postdatetime],
			[dnt_topics].[lastpost],
			[dnt_topics].[lastposter],
			[dnt_topics].[lastpostid],
			[dnt_topics].[lastposterid],
			[dnt_topics].[highlight],
			[dnt_topics].[digest],
			[dnt_topics].[displayorder],
			[dnt_topics].[attachment],
			[dnt_topics].[closed],
			[dnt_topics].[magic],
			[dnt_topics].[special]
		FROM [dnt_topics]
		WHERE [displayorder] = '+@displayorder+' 
		AND tid < (SELECT MIN(tid) FROM (SELECT TOP '+@startRow+' tid FROM [dnt_topics] WHERE displayorder='+@displayorder+') AS T)
		ORDER BY tid DESC
		')
	END
END
ELSE
BEGIN
	IF @pageindex = 1
	BEGIN
		EXEC('
		SELECT 
			TOP '+@pagesize+'
			[dnt_topics].[rate], 
			[dnt_topics].[fid], 
			[dnt_topics].[tid],
			[dnt_topics].[iconid],
			[dnt_topics].[typeid],
			[dnt_topics].[title],
			[dnt_topics].[price],
			[dnt_topics].[hide],
			[dnt_topics].[readperm],
			[dnt_topics].[poster],
			[dnt_topics].[posterid],
			[dnt_topics].[replies],
			[dnt_topics].[views],
			[dnt_topics].[postdatetime],
			[dnt_topics].[lastpost],
			[dnt_topics].[lastposter],
			[dnt_topics].[lastpostid],
			[dnt_topics].[lastposterid],
			[dnt_topics].[highlight],
			[dnt_topics].[digest],
			[dnt_topics].[displayorder],
			[dnt_topics].[attachment],
			[dnt_topics].[closed],
			[dnt_topics].[magic],
			[dnt_topics].[special]		
		FROM [dnt_topics]
		WHERE [displayorder] = '+@displayorder+' AND fid IN ('+@fidlist+')
		')
	END
	ELSE
	BEGIN
		EXEC('
		SELECT 
			TOP '+@pagesize+'
			[dnt_topics].[rate], 
			[dnt_topics].[fid], 
			[dnt_topics].[tid],
			[dnt_topics].[iconid],
			[dnt_topics].[typeid],
			[dnt_topics].[title],
			[dnt_topics].[price],
			[dnt_topics].[hide],
			[dnt_topics].[readperm],
			[dnt_topics].[poster],
			[dnt_topics].[posterid],
			[dnt_topics].[replies],
			[dnt_topics].[views],
			[dnt_topics].[postdatetime],
			[dnt_topics].[lastpost],
			[dnt_topics].[lastposter],
			[dnt_topics].[lastpostid],
			[dnt_topics].[lastposterid],
			[dnt_topics].[highlight],
			[dnt_topics].[digest],
			[dnt_topics].[displayorder],
			[dnt_topics].[attachment],
			[dnt_topics].[closed],
			[dnt_topics].[magic],
			[dnt_topics].[special]
		FROM [dnt_topics]
		WHERE [displayorder] = '+@displayorder+' AND fid IN ('+@fidlist+')
		AND tid < (SELECT MIN(tid) FROM (SELECT TOP '+@startRow+' tid FROM [dnt_topics] WHERE displayorder='+@displayorder+' fid IN ('+@fidlist+')) AS T)
		ORDER BY tid DESC
		')
	END
END
GO

IF OBJECT_ID('dnt_getunauditpost','P') IS NOT NULL
DROP PROC [dnt_getunauditpost]
GO

CREATE PROC [dnt_getunauditpost]
@lastposter NVARCHAR(20),
@lastposterid INT
AS
UPDATE [dnt_topics] 
SET [lastposter]=@lastposter  
WHERE [lastposterid]=@lastposterid
GO

IF OBJECT_ID('dnt_updatetopicposter','P') IS NOT NULL
DROP PROC [dnt_updatetopicposter]
GO

CREATE PROCEDURE [dnt_updatetopicposter]
@poster NVARCHAR(20),
@posterid INT
AS
UPDATE [dnt_topics] 
SET [poster]=@poster 
WHERE [posterid]=@posterid
GO

IF OBJECT_ID('dnt_updatepostposter','P') IS NOT NULL
DROP PROC [dnt_updatepostposter]
GO

CREATE PROCEDURE [dnt_updatepostposter]
@poster NVARCHAR(20),
@posterid INT
AS
DECLARE @count INT,
		@begin INT,
		@tableid INT
SET @begin = 1
DECLARE @tempposttable TABLE 
						(
							[ROWID] INT IDENTITY(1,1),
							[tableid] INT
						)
INSERT INTO @tempposttable 
SELECT id FROM dnt_tablelist
SELECT @count=COUNT(1) FROM @tempposttable
WHILE @begin <= @count
BEGIN
	SELECT  @tableid = [tableid] FROM @tempposttable WHERE [ROWID] = @begin
	EXEC('UPDATE [dnt_posts' + @tableid + '] SET [poster]=''' + @poster + ''' WHERE [posterid]=' + @posterid )
	SET @begin = @begin + 1
END
GO

IF OBJECT_ID('dnt_updatemoderatorname','P') IS NOT NULL
DROP PROC [dnt_updatemoderatorname]
GO

CREATE PROC [dnt_updatemoderatorname]
@oldname NVARCHAR(20),
@newname VARCHAR(20)
AS
IF @newname <> ''
	UPDATE dnt_forumfields 
	SET MODERATORS=REPLACE(LTRIM(RTRIM(REPLACE(' '+REPLACE(CONVERT(NVARCHAR(500),MODERATORS),',',' ')+' ',' '+@oldname+' ',' '+@newname+' '))),' ',',')
	WHERE CHARINDEX(','+@oldname+',',','+CONVERT(NVARCHAR(500),MODERATORS)+',') > 0
ELSE
	UPDATE dnt_forumfields 
	SET MODERATORS = REPLACE(LTRIM(RTRIM(REPLACE(REPLACE(','+CONVERT(NVARCHAR(500),MODERATORS)+',',','+@oldname+',',','),',',' '))),' ',',')
	WHERE CHARINDEX(','+@oldname+',',','+CONVERT(NVARCHAR(500),MODERATORS)+',') > 0
GO

IF OBJECT_ID('dnt_updateonlinelist','P') IS NOT NULL
DROP PROC [dnt_updateonlinelist]
GO

CREATE PROC [dnt_updateonlinelist]
@title NVARCHAR(50),
@groupid INT
AS
UPDATE [dnt_onlinelist] SET [title]=@title WHERE [groupid]=@groupid
GO

--此存储过程的作用是更新总帖子数
IF OBJECT_ID('dnt_resetforumsposts','P') IS NOT NULL
DROP PROC [dnt_resetforumsposts]
GO

CREATE PROC [dnt_resetforumsposts]
AS
--清空forums表中的帖子数，以便重新统计
UPDATE dnt_forums SET posts=0
DECLARE @i INT,
		@maxlayer INT,
		@maxtableid INT,
		@tablename NVARCHAR(50)
SET @i = 1
--取分表数
SELECT @maxtableid = MAX(id) FROM dnt_tablelist
--依次处理分表
WHILE @i <= @maxtableid
BEGIN
	SET @tablename = 'dnt_posts' + CONVERT(NVARCHAR(2),@i)
	--如果分表不存在则继续处理下一个
	IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE id = OBJECT_ID(@tablename) AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
		CONTINUE
	--取得每个版块在当前分表中的帖子数，累加并更新forums表中版块帖子数
	EXEC('
		UPDATE dnt_forums 
		SET posts=posts+rightTable.fidcount 
		FROM dnt_forums 
		INNER JOIN (SELECT fid,COUNT(fid) fidcount FROM '+ @tablename +' GROUP BY fid) AS rightTable
		ON dnt_forums.fid=rightTable.fid
		WHERE dnt_forums.fid=rightTable.fid
		')
	SET @i = @i + 1
END

--以下操作将子版块中的帖子总数累加并更新到父版块中
--取得版块的最大层数
SELECT @maxlayer = MAX(layer) FROM dnt_forums
--倒序更新，从最底层版块更新到最顶层
WHILE @maxlayer > 0
BEGIN
	--将子版块中的帖子总数累加并更新到父版块中
	UPDATE dnt_forums 
	SET posts=posts+rightTable.sumposts 
	FROM dnt_forums AS leftTable 
	INNER JOIN (SELECT parentid,SUM(posts) sumposts FROM dnt_forums WHERE layer=@maxlayer GROUP BY parentid) AS rightTable
	ON leftTable.fid=rightTable.parentid
	WHERE leftTable.fid=rightTable.parentid
	SET @maxlayer = @maxlayer - 1
END
GO

--此存储过程的作用是更新最后发帖
--测试代码：
--SELECT parentid,fid,lasttid,lasttitle,lastpost,lastposterid,lastposter FROM #tempTable2 ORDER BY parentid
--SELECT fid,lasttid,lasttitle,lastpost,parentid,lastposterid,lastposter FROM dnt_forums WHERE layer = 6 AND fid IN (245,256,260,259,273)
IF OBJECT_ID('dnt_resetlastpostinfo','P') IS NOT NULL
DROP PROC [dnt_resetlastpostinfo]
GO

CREATE PROC [dnt_resetlastpostinfo]
AS
--创建两个临时表#tempTable1，#tempTable2
CREATE TABLE #tempTable1 (
							fid INT,
							lasttid INT,
							lasttitle NVARCHAR(60),
							lastpost DATETIME,
							parentid INT,
							lastposterid INT,
							lastposter NVARCHAR(30)
						 )

CREATE table #tempTable2 (
							fid INT,
							lasttid INT,
							lasttitle NVARCHAR(60),
							lastpost DATETIME,
							parentid INT,
							lastposterid INT,
							lastposter NVARCHAR(30)
						)
DECLARE @maxlayer INT  -- 用来存放最大layer的变量
SELECT @maxlayer = MAX(layer) FROM dnt_forums;
--先初始化dnt_forums表中最后发帖等的信息
UPDATE dnt_forums SET lasttid = 0, lasttitle = '从未',lastpost = '1900-1-1',lastposterid=0,lastposter=''
--更新所有有主题的版块的最后发帖人等信息
UPDATE dnt_forums 
SET lasttid = rightTable.tid, lasttitle = rightTable.title, lastpost = rightTable.lastpost, lastposterid=rightTable.lastposterid, lastposter=rightTable.lastposter
FROM dnt_forums AS leftTable
INNER JOIN (SELECT fid,tid,title,lastpost,lastposter,lastposterid FROM dnt_topics WHERE tid IN( SELECT MAX(tid) FROM dnt_topics GROUP BY fid)) AS rightTable
ON leftTable.fid = rightTable.fid
WHERE leftTable.fid = rightTable.fid

WHILE @maxlayer > 0
BEGIN
	--清空临时表#tempTable1，为了保证第二次循环时，表是空的
	DELETE FROM #tempTable1;
	--首先以dnt_forums表中的parentid字段分组，查询出每组最后回复时间最晚的记录；
	--然后LEFT JOIN表dnt_forums，以两个表中parentid相等并且lastpost相等为条件，进行查询，
	--并把结果插入到临时表#tempTable1中，以备临时表#tempTable2用
	INSERT INTO #tempTable1 
				SELECT fid,lasttid,lasttitle,a.lastpost,a.parentid,a.lastposterid,a.lastposter 
				FROM (
						SELECT parentid,MAX(lastpost) lastpost FROM dnt_forums a WHERE layer = @maxlayer GROUP BY parentid
					 ) b 
				LEFT JOIN dnt_forums a
				ON a.parentid=b.parentid and a.lastpost=b.lastpost						

	--清空临时表#tempTable2，为了保证第二次循环时，表是空的
	DELETE FROM #tempTable2;
	--首先以#tempTable1表中的parentid分组，查询出组中的MAX(fid)；
	--然后以fid IN （子查询中查出的MAX(fid)）为条件，查询出结果并插入到#tempTable2中
	INSERT INTO #tempTable2 
				SELECT * FROM #tempTable1
				WHERE fid IN (SELECT MAX(fid) FROM #tempTable1 GROUP BY parentid,lastpost)
	--根据临时表#tempTable2中的内容，更新dnt_forums表的最后发帖人等信息
	UPDATE dnt_forums 
	SET lastpost = CASE WHEN #tempTable2.lastpost>dnt_forums.lastpost THEN #tempTable2.lastpost
				   ELSE dnt_forums.lastpost END,
		lasttitle = CASE WHEN #tempTable2.lastpost>dnt_forums.lastpost THEN #tempTable2.lasttitle
					ELSE dnt_forums.lasttitle END,
		lasttid = CASE WHEN #tempTable2.lastpost>dnt_forums.lastpost THEN #tempTable2.lasttid
					ELSE dnt_forums.lasttid END,
		lastposterid = CASE WHEN #tempTable2.lastpost>dnt_forums.lastpost THEN #tempTable2.lastposterid
					ELSE dnt_forums.lastposterid END,
		lastposter = CASE WHEN #tempTable2.lastpost>dnt_forums.lastpost THEN #tempTable2.lastposter
					ELSE dnt_forums.lastposter END			
	FROM dnt_forums 
	INNER JOIN #tempTable2 
	ON dnt_forums.fid=#tempTable2.parentid 
	WHERE dnt_forums.fid=#tempTable2.parentid;
	SET @maxlayer = @maxlayer - 1;
END
DROP TABLE #tempTable2;
DROP TABLE #tempTable1;
GO

/*
此存储过程的作用：
根据参数中传递过来的分表名更新主题的最后回复等信息
*/
IF OBJECT_ID('dnt_resetLastRepliesInfoOftopics','P') IS NOT NULL
DROP PROC [dnt_resetLastRepliesInfoOftopics]
GO

CREATE PROC [dnt_resetLastRepliesInfoOftopics]
@posttable NVARCHAR(20)
AS
EXEC('
		UPDATE dnt_topics 
		SET dnt_topics.replies = lastPostTable.posts, 
			lastpost = lastPostTable.postdatetime, 
			lastposterid = lastPostTable.posterid, 
			lastposter = lastPostTable.poster, 
			lastpostid = lastPostTable.pid 
		FROM dnt_topics
		INNER JOIN  (
						SELECT leftTable.tid,leftTable.posterid,leftTable.pid,leftTable.poster,leftTable.postdatetime,rightTable.posts FROM '+@posttable+' AS leftTable 
						LEFT JOIN (SELECT tid,COUNT(*)-1 posts,MAX(pid) maxpid FROM '+@posttable+' GROUP BY tid) AS rightTable
						ON leftTable.pid = rightTable.maxpid
						WHERE leftTable.pid = rightTable.maxpid
					) AS lastPostTable
		ON dnt_topics.tid = lastPostTable.tid
		WHERE dnt_topics.tid = lastPostTable.tid AND dnt_topics.lastpostid <> lastPostTable.pid
	')
GO
	
/*
此存储过程的作用：
更新所有用户的精华帖数
*/
IF OBJECT_ID('dnt_resetuserdigestposts','P') IS NOT NULL
DROP PROC [dnt_resetuserdigestposts]
GO

CREATE PROC [dnt_resetuserdigestposts]
AS
UPDATE dnt_users
SET [digestposts] = leftTable.topiccount 
FROM dnt_users 
INNER JOIN (SELECT COUNT(tid) topiccount,posterid FROM dnt_topics WHERE [digest] > 0 GROUP BY posterid) AS leftTable
ON dnt_users.[uid] = leftTable.posterid
WHERE dnt_users.[uid] = leftTable.posterid AND dnt_users.posts <> 0
GO

/*
此存储过程的作用：
根据参数中传递过来的分表名更新用户的帖子数
后台更新所有用户帖子数时使用
*/
IF OBJECT_ID('dnt_resetuserspostcount','P') IS NOT NULL
DROP PROC [dnt_resetuserspostcount]
GO

CREATE PROC [dnt_resetuserspostcount]
@posttablename NVARCHAR(50)
AS
EXEC('
		UPDATE dnt_users
		SET posts = posts + leftTable.postcount 
		FROM dnt_users 
		INNER JOIN (SELECT posterid,COUNT(pid) postcount FROM '+@posttablename+' GROUP BY posterid) AS leftTable
		ON dnt_users.[uid] = leftTable.posterid
		WHERE dnt_users.[uid] = leftTable.posterid
	')
GO

IF OBJECT_ID('dnt_createinvitecode','P') IS NOT NULL
DROP PROC [dnt_createinvitecode]
GO

CREATE PROCEDURE [dnt_createinvitecode]
                    @code char(8),
                    @creatorid int,
                    @creator nchar(20),
                    @createtime smalldatetime,
                    @expiretime smalldatetime,
                    @maxcount int,
                    @invitetype int

                    AS
                    
                    INSERT INTO [dnt_invitation]([invitecode],[creatorid],[creator],[createdtime],[expiretime],[maxcount],[invitetype])
                     VALUES(@code,@creatorid,@creator,@createtime,@expiretime,@maxcount,@invitetype);SELECT SCOPE_IDENTITY()

GO

IF OBJECT_ID('[dnt_deleteinvitecode]','P') IS NOT NULL
DROP PROC [dnt_deleteinvitecode]
GO

CREATE PROCEDURE [dnt_deleteinvitecode]
@id int
AS
BEGIN
UPDATE [dnt_invitation] SET [isdeleted]=1 WHERE [inviteid]=@id
END
GO

IF OBJECT_ID('dnt_clearexpireinvitecode','P') IS NOT NULL
DROP PROC [dnt_clearexpireinvitecode]
GO

CREATE PROCEDURE [dnt_clearexpireinvitecode]
AS
BEGIN
DELETE [dnt_invitation] WHERE [invitetype]=3 AND [createdtime]<>[expiretime] AND ([expiretime]-GETDATE())<=0;
DELETE [dnt_invitation] WHERE [isdeleted]=1 AND [createdtime]<(GETDATE()-1)
END
GO

IF OBJECT_ID('dnt_getinvitecode','P') IS NOT NULL
DROP PROC [dnt_getinvitecode]
GO

CREATE PROCEDURE [dnt_getinvitecode]
@searchtype nchar(10),
@searchkey nchar(20)
AS
BEGIN
IF @searchtype='uid'
   SELECT TOP 1 [inviteid],[invitecode],[invitetype],[createdtime],[creator],[creatorid],[expiretime],[maxcount],[successcount] 
FROM [dnt_invitation] WHERE [creatorid]=@searchkey AND [invitetype]=2 AND [isdeleted]=0 ORDER BY [inviteid] DESC
ELSE IF @searchtype='id'
   SELECT [inviteid],[invitecode],[invitetype],[createdtime],[creator],[creatorid],[expiretime],[maxcount],[successcount] FROM 

[dnt_invitation] WHERE [inviteid]=@searchkey AND [isdeleted]=0
ELSE IF @searchtype='code'
   SELECT [inviteid],[invitecode],[invitetype],[createdtime],[creator],[creatorid],[expiretime],[maxcount],[successcount] FROM 

[dnt_invitation] WHERE [invitecode]=@searchkey AND [isdeleted]=0
ELSE
   SELECT (1)
END

GO

IF OBJECT_ID('dnt_getinvitecodelistbyuid','P') IS NOT NULL
DROP PROC [dnt_getinvitecodelistbyuid]
GO

CREATE PROCEDURE [dnt_getinvitecodelistbyuid]
@creatorid int,
@pageindex int
AS
DECLARE @startRow int,
		@endRow int
SET @startRow = (@pageIndex - 1) * 10
IF @pageindex = 1
BEGIN
	SELECT 
	TOP 10
	[inviteid],
	[invitecode],
	[invitetype],
	[createdtime],
	[creator],
	[creatorid],
	[expiretime],
	[maxcount],
	[successcount]
	FROM [dnt_invitation]
	WHERE [creatorid]=@creatorid AND [invitetype]=3 AND [isdeleted]=0 ORDER BY [inviteid] DESC
END
ELSE
BEGIN
	EXEC('
		SELECT 
		TOP 10
		[inviteid],
		[invitecode],
		[invitetype],
		[createdtime],
		[creator],
		[creatorid],
		[expiretime],
		[maxcount],
		[successcount]
		FROM [dnt_invitation]
		WHERE [creatorid]=@creatorid AND [invitetype]=3 AND [isdeleted]=0
		AND inviteid < (SELECT MIN(inviteid) FROM ( SELECT TOP '+@startRow+' inviteid 
													FROM [dnt_invitation] 
													WHERE [creatorid]=@creatorid AND [invitetype]=3 AND [isdeleted]=0
													RDER BY [inviteid] DESC
												   ) AS T
						)
		ORDER BY [inviteid] DESC
		')
END
GO

IF OBJECT_ID('dnt_isinvitecodeexist','P') IS NOT NULL
DROP PROC [dnt_isinvitecodeexist]
GO

CREATE PROCEDURE [dnt_isinvitecodeexist]
@code nchar(8)
AS
BEGIN
SELECT COUNT(1) FROM [dnt_invitation] WHERE [invitecode]=@code AND [isdeleted] =0
END
GO

IF OBJECT_ID('dnt_updateinvitecodesuccesscount','P') IS NOT NULL
DROP PROC [dnt_updateinvitecodesuccesscount]
GO

CREATE PROCEDURE [dnt_updateinvitecodesuccesscount]
@id int
AS
BEGIN
UPDATE [dnt_invitation] SET [successcount]=[successcount]+1 WHERE [inviteid]=@id
END
GO

IF OBJECT_ID('dnt_getuserinvitecodecount','P') IS NOT NULL
DROP PROC [dnt_getuserinvitecodecount]
GO

CREATE PROCEDURE [dnt_getuserinvitecodecount]
@creatorid int
AS
BEGIN
SELECT COUNT(1) FROM [dnt_invitation] WHERE [creatorid]=@creatorid AND [invitetype]=3 AND [isdeleted]=0
END
GO

IF OBJECT_ID('dnt_gettodayusercreatedinvitecode','P') IS NOT NULL
DROP PROC [dnt_gettodayusercreatedinvitecode]
GO

CREATE PROCEDURE [dnt_gettodayusercreatedinvitecode]
@creatorid int
AS
BEGIN
SELECT COUNT(1) FROM [dnt_invitation] WHERE [creatorid]=@creatorid AND [invitetype]=2 AND (GETDATE()- [createdtime])<1
END
GO

IF OBJECT_ID('dnt_getattachmentlistbypid','P') IS NOT NULL
DROP PROC [dnt_getattachmentlistbypid]
GO

CREATE PROCEDURE [dnt_getattachmentlistbypid]
@pidlist varchar(500)
AS
EXEC('SELECT 
[aid],
[uid],
[tid],
[pid],
[postdatetime],
[readperm],
[filename],
[description],
[filetype],
[filesize],
[attachment],
[downloads],
[attachprice],
[width],
[height] 
FROM [dnt_attachments] 
WHERE [pid] IN ('+@pidlist+')')
GO

IF OBJECT_ID('dnt_getattachmentlistbyaid','P') IS NOT NULL
DROP PROC [dnt_getattachmentlistbyaid]
GO

CREATE PROCEDURE [dnt_getattachmentlistbyaid]
@aidlist varchar(500)
AS
SELECT [aid], [tid], [pid], [filename] FROM [dnt_attachments] WITH (NOLOCK) WHERE [aid] IN (SELECT [item] FROM [dnt_split](@aidlist, ',') )
GO

IF OBJECT_ID('dnt_deleteattachmentbyaidlist','P') IS NOT NULL
DROP PROC [dnt_deleteattachmentbyaidlist]
GO

CREATE PROCEDURE [dnt_deleteattachmentbyaidlist]
@aidlist VARCHAR(500)
AS
EXEC('DELETE FROM [dnt_attachments] WHERE [aid] IN ('+@aidlist+')')
GO

IF OBJECT_ID('dnt_deleteattachmentbytidlist','P') IS NOT NULL
DROP PROC [dnt_deleteattachmentbytidlist]
GO

CREATE PROCEDURE [dnt_deleteattachmentbytidlist]
@tidlist VARCHAR(500)
AS
EXEC('DELETE FROM [dnt_attachments] WHERE [tid] IN ('+@tidlist+')')
GO

IF OBJECT_ID('dnt_getattachmentlistbytidlist','P') IS NOT NULL
DROP PROC [dnt_getattachmentlistbytidlist]
GO

CREATE PROCEDURE [dnt_getattachmentlistbytidlist]
@tidlist VARCHAR(500)
AS
SELECT 
[aid],
[filename] 
FROM [dnt_attachments] WITH (NOLOCK)
WHERE [tid] IN (SELECT [item]FROM [dnt_split](@tidlist, ','))
GO

IF OBJECT_ID('dnt_updateuseronlinestates','P') IS NOT NULL
DROP PROCEDURE [dnt_updateuseronlinestates]
GO

CREATE PROCEDURE [dnt_updateuseronlinestates] 
@uidlist varchar(5000) = '' 
AS

EXEC ('UPDATE [dnt_users] SET [onlinestate]=0,[lastactivity]=GETDATE() WHERE [uid] IN ('+@uidlist+')')
GO


/*
修改时间：2009-11-23
修改原因：将CHARINDEX替换成了IN
测试代码：
exec [dnt_deletetopicbytidlist1] '1246471,1246472',1
*/

IF OBJECT_ID('dnt_deletetopicbytidlist1','P') IS NOT NULL
DROP PROCEDURE [dnt_deletetopicbytidlist1]
GO

CREATE PROCEDURE [dnt_deletetopicbytidlist1]
    @tidlist AS VARCHAR(2000),
    @chanageposts AS BIT
AS

    DECLARE @postcount int
    DECLARE @topiccount int
    DECLARE @todaycount int
    DECLARE @sqlstr nvarchar(4000)
    DECLARE @fid varchar(2000)
    DECLARE @posterid varchar(200)
    DECLARE @tempFid int
    DECLARE @tempPosterid int
    DECLARE @tempLayer int
    DECLARE @temppostdatetime datetime

    DECLARE @tempfidlist AS VARCHAR(1000)	

    SET @fid = ''
    SET @posterid = ''
    SET @postcount=0
    SET @topiccount=0
    SET @todaycount=0

    SET @tempfidlist = '';


    IF @tidlist<>''
        BEGIN
            EXEC('DECLARE cu_dnt_posts CURSOR FOR SELECT [fid],[posterid],[layer],[postdatetime] FROM [dnt_posts1] WHERE [dnt_posts1].[tid] IN ('+@tidlist+')')
			
            OPEN cu_dnt_posts
            FETCH NEXT FROM cu_dnt_posts into @tempFid,@tempPosterid,@tempLayer,@temppostdatetime
            WHILE @@FETCH_STATUS = 0
                BEGIN
                    SET @postcount = @postcount + 1
                    IF @tempLayer = 0
	                    BEGIN
		                    SET @topiccount = @topiccount + 1
							
	                    END

                    IF DATEDIFF(d,@temppostdatetime,GETDATE()) = 0
	                    BEGIN
		                    SET @todaycount = @todaycount + 1
	                    END


                    IF CHARINDEX(',' + LTRIM(STR(@tempFid)) + ',',@fid + ',') = 0
	                    BEGIN
		                    --SET @fid = @fid + ',' + LTRIM(STR(@tempFid))	
		                    SELECT @tempfidlist = ISNULL([parentidlist],'') FROM [dnt_forums] WHERE [fid] = @tempFid
		                    IF RTRIM(@tempfidlist)<>''
			                    BEGIN
				                    SET @fid = RTRIM(@fid) + ',' +  RTRIM(@tempfidlist) + ',' + CAST(@tempFid AS VARCHAR(10))
			                    END
		                    ELSE
			                    BEGIN
				                    SET @fid =RTRIM(@fid) + ',' +  CAST(@tempFid AS VARCHAR(10))
			                    END

					
	                    END
                    IF @chanageposts = 1
	                    BEGIN
		                    UPDATE [dnt_users] SET [posts] = [posts] - 1 WHERE [uid] = @tempPosterid
	                    END
				
                    FETCH NEXT FROM cu_dnt_posts into @tempFid,@tempPosterid,@tempLayer,@temppostdatetime
                END

            CLOSE cu_dnt_posts
            DEALLOCATE cu_dnt_posts

	
            IF LEN(@fid)>0
                BEGIN	

			
                    SET @fid = SUBSTRING(@fid,2,LEN(@fid)-1)
		
                    IF @chanageposts = 1
	                    BEGIN
		
		                    UPDATE [dnt_statistics] SET [totaltopic]=[totaltopic] - @topiccount, [totalpost]=[totalpost] - @postcount

		                    EXEC('UPDATE [dnt_forums] 
		                    SET [posts]=[posts] - ' + @postcount + ',  
		                    [topics]=[topics] - ' + @topiccount + ', 
		                    [todayposts] = [todayposts] - ' + @todaycount + ' 
		                    WHERE [fid] IN ('+@fid+')')
	                    END
		
                    EXEC('DELETE FROM [dnt_favorites] WHERE [tid] IN ('+@tidlist+') AND [typeid]=0')
					
                    EXEC('DELETE FROM [dnt_polls] WHERE [tid] IN ('+@tidlist+')')

                    EXEC('DELETE FROM [dnt_posts1] WHERE [tid] IN ('+@tidlist+')')

                    EXEC('DELETE FROM [dnt_mytopics] WHERE [tid] IN ('+@tidlist+')')
                END
            EXEC('DELETE FROM [dnt_topics] WHERE [closed] IN ('+@tidlist+') OR [tid] IN ('+@tidlist+')')
			
            EXEC('UPDATE [dnt_tags] SET [count]=[count]-1, [fcount]=[fcount]-1 WHERE [tagid] IN (SELECT [tagid] FROM [dnt_topictags] WHERE [tid] IN ('+@tidlist+'))') 
			
            EXEC('DELETE FROM [dnt_topictags] WHERE [tid] IN ('+@tidlist+')')
			
            EXEC('DELETE FROM [dnt_topictagcaches] WHERE [tid] IN ('+@tidlist+') OR [linktid] IN ('+@tidlist+')')
END
GO

/*修改存储过程的名字*/
IF OBJECT_ID('dnt_getattachmentlistbypid','P') IS NOT NULL
DROP PROC [dnt_getattachmentlistbypid]
GO

IF OBJECT_ID('dnt_getattachmentlistbypidlist','P') IS NOT NULL
DROP PROC [dnt_getattachmentlistbypidlist]
GO

CREATE PROCEDURE [dnt_getattachmentlistbypidlist]
@pidlist varchar(500)
AS
SELECT 
[aid],
[uid],
[tid],
[pid],
[postdatetime],
[readperm],
[filename],
[description],
[filetype],
[filesize],
[attachment],
[downloads],
[attachprice],
[width],
[height],
[isimage] 
FROM [dnt_attachments] WITH (NOLOCK)
WHERE [dnt_attachments].[pid] IN (SELECT [item]FROM [dnt_split](@pidlist, ',') )
GO

/*修改存储过程的名字*/
IF OBJECT_ID('dnt_getattachenmtlistbypid','P') IS NOT NULL
DROP PROC [dnt_getattachenmtlistbypid]
GO

IF OBJECT_ID('dnt_getattachmentlistbypid','P') IS NOT NULL
DROP PROC [dnt_getattachmentlistbypid]
GO

CREATE PROCEDURE [dnt_getattachmentlistbypid]
@pid INT
AS
SELECT 
[aid],
[uid],
[tid],
[pid],
[postdatetime],
[readperm],
[filename],
[description],
[filetype],
[filesize],
[attachment],
[downloads],
[attachprice],
[width],
[height],
[isimage]
FROM [dnt_attachments] 
WHERE [pid]=@pid
GO

IF OBJECT_ID('dnt_getattachpaymentlogbyuid','P') IS NOT NULL
DROP PROC [dnt_getattachpaymentlogbyuid]
GO

CREATE PROCEDURE [dnt_getattachpaymentlogbyuid]
@attachidlist varchar(2000),
@uid int
AS
EXEC('
		SELECT 
		[aid]
		FROM [dnt_attachpaymentlog] 
		WHERE [uid] = '+@uid+' AND [dnt_attachpaymentlog].[aid] IN ('+@attachidlist+')
	')
GO

IF OBJECT_ID('dnt_getindexforumlist','P') IS NOT NULL
DROP PROC [dnt_getindexforumlist]
GO

CREATE PROCEDURE [dnt_getindexforumlist]
AS
SELECT CASE WHEN DATEDIFF(n, [lastpost], GETDATE())<600 THEN 'new' ELSE 'old' END AS [havenew],
[dnt_forums].[allowbbcode],
[dnt_forums].[allowblog],
[dnt_forums].[alloweditrules],
[dnt_forums].[allowhtml],
[dnt_forums].[allowimgcode],
[dnt_forums].[allowpostspecial],
[dnt_forums].[allowrss],
[dnt_forums].[allowsmilies],
[dnt_forums].[allowspecialonly],
[dnt_forums].[allowtag],
[dnt_forums].[allowthumbnail],	
[dnt_forums].[autoclose],	
[dnt_forums].[colcount],
[dnt_forums].[curtopics],
[dnt_forums].[disablewatermark],
[dnt_forums].[displayorder],
[dnt_forums].[fid],
[dnt_forums].[inheritedmod],
[dnt_forums].[istrade],
[dnt_forums].[jammer],
[dnt_forums].[lastpost],
[dnt_forums].[lastposter],
[dnt_forums].[lastposterid],
[dnt_forums].[lasttid],
[dnt_forums].[lasttitle],
[dnt_forums].[layer],
[dnt_forums].[modnewposts],
[dnt_forums].[name],
[dnt_forums].[parentid],
[dnt_forums].[parentidlist],
[dnt_forums].[pathlist],
[dnt_forums].[posts],
[dnt_forums].[recyclebin],
[dnt_forums].[status],
[dnt_forums].[subforumcount],
[dnt_forums].[templateid],
[dnt_forums].[todayposts],
[dnt_forums].[topics],
[dnt_forumfields].[applytopictype],
[dnt_forumfields].[attachextensions],
[dnt_forumfields].[description],
[dnt_forumfields].[fid],
[dnt_forumfields].[getattachperm],
[dnt_forumfields].[icon],
[dnt_forumfields].[moderators],
[dnt_forumfields].[password],
[dnt_forumfields].[permuserlist],
[dnt_forumfields].[postattachperm],
[dnt_forumfields].[postbytopictype],
[dnt_forumfields].[postcredits],
[dnt_forumfields].[postperm],
[dnt_forumfields].[redirect],
[dnt_forumfields].[replycredits],
[dnt_forumfields].[replyperm],
[dnt_forumfields].[rewritename],
[dnt_forumfields].[rules],
[dnt_forumfields].[seodescription],
[dnt_forumfields].[seokeywords],
[dnt_forumfields].[topictypeprefix],
[dnt_forumfields].[topictypes],
[dnt_forumfields].[viewbytopictype],
[dnt_forumfields].[viewperm]
FROM [dnt_forums] 
LEFT JOIN [dnt_forumfields] ON [dnt_forums].[fid]=[dnt_forumfields].[fid] 
WHERE [dnt_forums].[status] > 0 AND [layer] <= 1 
AND [dnt_forums].[parentid] NOT IN (SELECT [fid] FROM [dnt_forums] WHERE [status] < 1 AND [layer] = 0) ORDER BY [displayorder]
GO

IF OBJECT_ID('dnt_resetforumstopics','P') IS NOT NULL
DROP PROC [dnt_resetforumstopics]
GO

CREATE PROC [dnt_resetforumstopics]
AS
--更新所有版块的主题数,不包含子版块
UPDATE dnt_forums 
		SET curtopics=ISNULL(rightTable.sumtopics,0),topics = ISNULL(rightTable.sumtopics,0)
		FROM dnt_forums 
		INNER JOIN (SELECT fid,COUNT(tid) sumtopics FROM dnt_topics GROUP BY fid) AS rightTable
		ON dnt_forums.fid=rightTable.fid
		WHERE dnt_forums.fid=rightTable.fid

DECLARE @maxlayer INT
SELECT @maxlayer = MAX(layer) FROM dnt_forums


--倒序更新，从最底层版块更新到最顶层
WHILE @maxlayer > 0
BEGIN
	--将子版块中的帖子总数累加并更新到父版块中
	UPDATE dnt_forums 
	SET topics=topics+ISNULL(rightTable.sumtopics,0) 
	FROM dnt_forums AS leftTable 
	INNER JOIN (SELECT parentid,SUM(topics) sumtopics FROM dnt_forums WHERE layer=@maxlayer GROUP BY parentid) AS rightTable
	ON leftTable.fid=rightTable.parentid
	WHERE leftTable.fid=rightTable.parentid
	SET @maxlayer = @maxlayer - 1
END
GO

if exists (select * from sysobjects where id = object_id(N'[dnt_createorder]') and OBJECTPROPERTY(id,N'IsProcedure') = 1)
drop procedure [dnt_createorder]
GO

CREATE PROCEDURE [dnt_createorder]
@ordercode char(32),
@uid int,
@buyer char(20),
@paytype tinyint,
@price decimal(18,2),
@orderstatus tinyint,
@credit tinyint,
@amount int
AS
BEGIN
INSERT INTO [dnt_orders]([ordercode],[uid],[buyer],[paytype],[price],[orderstatus],
[credit],[amount]) VALUES(@ordercode,@uid,@buyer,@paytype,@price,@orderstatus,@credit,@amount)
END
GO

if exists (select * from sysobjects where id = object_id(N'[dnt_getorderbyordercode]') and OBJECTPROPERTY(id,N'IsProcedure') = 1)
drop procedure [dnt_getorderbyordercode]
GO

CREATE PROCEDURE [dnt_getorderbyordercode]
@ordercode char(32)
AS
BEGIN
	SELECT [orderid],[ordercode],[uid],[buyer],[paytype],[tradeno],[price],[orderstatus],[createdtime],[confirmedtime]
,[credit],[amount] FROM [dnt_orders] WHERE [ordercode]=@ordercode
END
GO

if exists (select * from sysobjects where id = object_id(N'[dnt_getorderlist]') and OBJECTPROPERTY(id,N'IsProcedure') = 1)
drop procedure [dnt_getorderlist]
GO

CREATE PROCEDURE [dnt_getorderlist]
@searchcondition varchar(1000)
AS
BEGIN
DECLARE @script VARCHAR(1000)
SET @script= 'SELECT TOP 20 [orderid],[ordercode],[uid],[buyer],[paytype],[tradeno],[price],[orderstatus],[createdtime],[confirmedtime]
,[credit],[amount] FROM [dnt_orders] '+@searchcondition+' ORDER BY [orderid] DESC'
EXEC(@script)
END
GO

if exists (select * from sysobjects where id = object_id(N'[dnt_getorderscount]') and OBJECTPROPERTY(id,N'IsProcedure') = 1)
drop procedure [dnt_getorderscount]
GO

CREATE PROCEDURE [dnt_getorderscount]
@searchcondition varchar(1000)
AS
BEGIN
DECLARE @script VARCHAR(1000)
SET @script= 'SELECT COUNT([orderid]) FROM [dnt_orders] '+@searchcondition
EXEC(@script)
END
GO

if exists (select * from sysobjects where id = object_id(N'[dnt_updateorderinfo]') and OBJECTPROPERTY(id,N'IsProcedure') = 1)
drop procedure [dnt_updateorderinfo]
GO

CREATE PROCEDURE [dnt_updateorderinfo]
@orderid int,
@tradeno char(32),
@orderstatus tinyint,
@confirmedtime smalldatetime
AS
BEGIN
    UPDATE [dnt_orders] SET [tradeno]=@tradeno,[orderstatus]=@orderstatus,[confirmedtime]=@confirmedtime WHERE [orderid]=@orderid
END
GO

if exists (select * from sysobjects where id = object_id(N'[dnt_clearexpirecreditorders]') and OBJECTPROPERTY(id,N'IsProcedure') = 1)
drop procedure [dnt_clearexpirecreditorders]
GO

CREATE PROCEDURE [dnt_clearexpirecreditorders]
AS
BEGIN
   DELETE [dnt_orders] WHERE [orderstatus] < 1 AND [createdtime] < (GETDATE()-15)
END
GO

IF OBJECT_ID('dnt_addparentforumtopics','P') IS NOT NULL
DROP PROC [dnt_addparentforumtopics]
GO

CREATE PROCEDURE [dnt_addparentforumtopics]
@topics int,
@fpidlist nvarchar(100)
AS
   UPDATE [dnt_forums] SET [topics] = [topics] + @topics WHERE [fid] IN (SELECT [item]FROM [dnt_split](@fpidlist, ',') )
GO

IF OBJECT_ID('dnt_updateusercredits','P') IS NOT NULL
DROP PROC [dnt_updateusercredits]
GO

CREATE PROCEDURE [dnt_updateusercredits]
@uid INT
AS
UPDATE [dnt_users] SET [credits] = extcredits1 + posts + digestposts* 5 WHERE [uid] = @uid
GO

IF OBJECT_ID('[dnt_gethottopicscount]','P')  IS NOT NULL
DROP PROC [dnt_gethottopicscount]
GO

CREATE PROCEDURE [dnt_gethottopicscount]
@fid int,
@timebetween int
AS
DECLARE @strSQL varchar(4000)
DECLARE @strSQLByFid nvarchar(200)
DECLARE @strSQLByDate nvarchar(200)
DECLARE @pagetop int
IF @fid<>0
SET @strSQLByFid=' AND [fid]='+STR(@fid)+'' 

IF @timebetween<>0
SET @strSQLByDate=' AND DATEDIFF(DAY,[postdatetime],GETDATE())<=' + STR(@timebetween)

SET @strSQL = 'SELECT COUNT(1) FROM [dnt_topics] WHERE 1=1 '+@strSQLByFid+@strSQLByDate
EXEC(@strSQL)
GO

IF OBJECT_ID('[dnt_gethottopicslist]','P')  IS NOT NULL
DROP PROC [dnt_gethottopicslist]
GO

CREATE PROCEDURE [dnt_gethottopicslist]
@pagesize int,
@pageindex int, 
@fid int,
@showtype varchar(100) , 
@timebetween int
AS
DECLARE @strSQL varchar(4000)
DECLARE @strSQLByFid nvarchar(200)
DECLARE @strSQLByDate nvarchar(200)
DECLARE @pagetop int
SET @pagetop = (@pageindex-1)*@pagesize
SET @strSQLByFid = ''
SET @strSQLByDate = ''

IF @fid<>0
BEGIN
SET @strSQLByFid='AND t.[fid]='+STR(@fid)
END

IF @timebetween<>0
SET @strSQLByDate=' AND DATEDIFF(DAY,[postdatetime],GETDATE())<=' + STR(@timebetween)


IF @pageindex=1
BEGIN
SET @strSQL = 'SELECT TOP ' + STR(@pagesize) +'  [t].[tid],[t].[fid],[t].[iconid],[t].[typeid],
[t].[readperm],[t].[price],[t].[poster],[t].[posterid],[t].[title],
[t].[attention],[t].[postdatetime],[t].[lastpost],[t].[lastpostid],[t].[lastposter],[t].[lastposterid],
[t].[views],[t].[replies],[t].[displayorder],[t].[highlight],[t].[digest],[t].[rate],[t].[hide],
[t].[attachment],[t].[moderated],[t].[closed],[t].[magic],[t].[identify],[t].[special],
f.[name] FROM [dnt_topics] t LEFT JOIN [dnt_forums] f ON t.[fid] = f.[fid] 
WHERE 1=1 '+@strSQLByFid+@strSQLByDate+' ORDER BY ['+@showtype+'] DESC'
END

ELSE
BEGIN
SET @strSQL = 'SELECT TOP ' + STR(@pagesize) +' [t].[tid],[t].[fid],[t].[iconid],[t].[typeid],[t].[readperm],[t].[price],
[t].[poster],[t].[posterid],[t].[title],[t].[attention],[t].[postdatetime],[t].[lastpost],[t].[lastpostid],
[t].[lastposter],[t].[lastposterid],[t].[views],[t].[replies],[t].[displayorder],[t].[highlight],[t].[digest],
[t].[rate],[t].[hide],[t].[attachment],[t].[moderated],[t].[closed],[t].[magic],[t].[identify],[t].[special],
f.[name] FROM [dnt_topics] t LEFT JOIN [dnt_forums] f ON t.[fid] = f.[fid] 
WHERE 1=1 ' +@strSQLByFid +@strSQLByDate+' AND [tid] NOT IN (SELECT TOP '+ STR(@pagetop)+' [tid] FROM [dnt_topics] 
WHERE 1=1 ' +@strSQLByFid +@strSQLByDate+' ORDER BY ['+@showtype+'] DESC) ORDER BY ['+@showtype+'] DESC'
END
EXEC(@strSQL)
GO

IF OBJECT_ID('dnt_resettodayposts','P') IS NOT NULL
DROP PROC [dnt_resettodayposts]
GO

CREATE PROC [dnt_resettodayposts]
AS
UPDATE dnt_forums SET todayposts = 0;

DECLARE @tableid int;
SELECT @tableid = MAX(id) FROM dnt_tablelist;
EXEC('
UPDATE dnt_forums 
SET todayposts=rightTable.pidcount 
FROM dnt_forums 
INNER JOIN (SELECT fid,COUNT(pid) pidcount FROM dnt_posts'+ @tableid +' WHERE DATEDIFF(DAY,postdatetime,GETDATE())=0 GROUP BY fid ) AS rightTable
ON dnt_forums.fid=rightTable.fid
WHERE dnt_forums.fid=rightTable.fid
')
GO

IF OBJECT_ID('dnt_updateonlinenewinfo','P') IS NOT NULL
DROP PROC dnt_updateonlinenewinfo
GO

CREATE PROCEDURE [dnt_updateonlinenewinfo]
@action NCHAR(30),
@olid INT,
@count SMALLINT
AS
BEGIN
IF(@action='newpms')
	UPDATE [dnt_online] SET [newpms]=@count WHERE [olid]=@olid
ELSE IF(@action='newnotice')
	IF(@count = 0)
		UPDATE [dnt_online] SET [newnotices]=(SELECT COUNT(nid) FROM [dnt_notices] WHERE [uid] = (SELECT [userid] FROM [dnt_online] WHERE [olid]=@olid)  AND [new] = 1) WHERE [olid]=@olid
	ELSE
		UPDATE [dnt_online] SET [newnotices]=[newnotices]+@count WHERE [olid]=@olid
ELSE
	SELECT 1
END
GO

IF OBJECT_ID('dnt_getmodpostcountbypidlist','P') IS NOT NULL
DROP PROC [dnt_getmodpostcountbypidlist]
GO

CREATE PROCEDURE [dnt_getmodpostcountbypidlist]
@fidlist NVARCHAR(500),
@posttableid NVARCHAR(5),
@pidlist NVARCHAR(500)
AS
BEGIN	
	DECLARE @sql NVARCHAR(500)
	SET @sql = 'SELECT COUNT([pid]) FROM [dnt_posts'+@posttableid+'] where [pid] IN ('+@pidlist+') AND [fid] IN ('+@fidlist+')'
	EXEC(@sql)
END
GO

IF OBJECT_ID('dnt_getmodtopiccountbytidlist','P') IS NOT NULL
DROP PROC [dnt_getmodtopiccountbytidlist]
GO

CREATE PROCEDURE [dnt_getmodtopiccountbytidlist]
@fidlist NCHAR(500),
@tidlist NCHAR(500)
AS
BEGIN	
	SELECT COUNT([tid]) FROM [dnt_topics] WHERE [tid] IN (SELECT [item] FROM dnt_split(@tidlist, ',')) AND [fid] IN (SELECT [item] FROM dnt_split(@fidlist, ','))
END
GO

IF OBJECT_ID('dnt_geteditpostattachlist','P') IS NOT NULL
DROP PROC [dnt_geteditpostattachlist]
GO

CREATE PROCEDURE [dnt_geteditpostattachlist]
@uid INT,
@aidlist VARCHAR(2000)
AS
BEGIN
IF @uid=0
SELECT [aid], [uid], [tid], [pid], [postdatetime], [readperm], [filename], [description], [filetype], [filesize], [attachment], [downloads], [width], [height], [attachprice],[isimage]
FROM [dnt_attachments] WITH (NOLOCK) WHERE aid in (SELECT [item] FROM [dnt_split](@aidlist, ','))
ELSE
SELECT [aid], [uid], [tid], [pid], [postdatetime], [readperm], [filename], [description], [filetype], [filesize], [attachment], [downloads], [width], [height], [attachprice],[isimage]
FROM [dnt_attachments] WITH (NOLOCK) WHERE aid in (SELECT [item] FROM [dnt_split](@aidlist, ',')) and UID=@uid
END
GO

IF OBJECT_ID('dnt_deletepostsbyuidanddays','P') IS NOT NULL
DROP PROC [dnt_deletepostsbyuidanddays]
GO

CREATE PROC [dnt_deletepostsbyuidanddays]
@uid	INT,
@days	INT
AS

DECLARE		@maxid 			INT,
			@minid 			INT,
			@postcount 			INT
SELECT @maxid=MAX(id), @minid=MIN(id) FROM dnt_tablelist WHERE DATEDIFF(dd,createdatetime,GETDATE()) <= @days;
DELETE [dnt_topics] WHERE [posterid]=@uid AND DATEDIFF(dd,postdatetime,GETDATE()) <= @days;

IF @maxid is NULL
BEGIN
	SELECT @maxid=MAX(id) FROM dnt_tablelist;
	EXEC(' DELETE FROM [dnt_posts'+@maxid+'] WHERE [posterid]='+@uid+' AND DATEDIFF(dd,postdatetime,GETDATE()) <= '+@days)
	SELECT @postcount = @@ROWCOUNT;
END
ELSE
BEGIN
	WHILE @maxid >= @minid
	BEGIN
		EXEC(' DELETE FROM [dnt_posts'+@maxid+'] WHERE [posterid]='+@uid+' AND DATEDIFF(dd,postdatetime,GETDATE()) <= '+@days)
		SELECT @postcount = @postcount+@@ROWCOUNT;
		SET @maxid = @maxid - 1
	END
END
UPDATE [dnt_users] SET [posts]=[posts]-@postcount WHERE [uid]=@uid
GO

IF OBJECT_ID('dnt_getratecount','P') IS NOT NULL
DROP PROC dnt_getratecount
GO

CREATE PROCEDURE [dnt_getratecount]
@pid int
AS
BEGIN
	SELECT COUNT(1) FROM [dnt_ratelog] WHERE pid=@pid
END
GO

IF OBJECT_ID('dnt_getrateloglist','P') IS NOT NULL
DROP PROC dnt_getrateloglist
GO

CREATE PROCEDURE [dnt_getrateloglist]
@pid int,
@pagesize int,
@pageindex int
AS
DECLARE @startRow int,
		@endRow int
SET @startRow = (@pageIndex - 1) * @pagesize
	IF @pageindex = 1
		BEGIN
			EXEC(
			 'SELECT TOP '+@pagesize+' [id],[pid],[uid],[username],[extcredits],[postdatetime],[score],[reason] FROM [dnt_ratelog] WHERE [pid]='+@pid+' ORDER BY [id] DESC'
			)
		END
	ELSE
		BEGIN
	 		EXEC('
			SELECT 
			TOP '+@pagesize+' 
			[id],[pid],[uid],[username],[extcredits],[postdatetime],[score],[reason] FROM [dnt_ratelog]
			WHERE [pid]='+@pid+' 
			AND [id] < (SELECT MIN([id]) FROM ( SELECT TOP '+@startRow+' [id] 
														FROM [dnt_ratelog] 
														WHERE [pid]='+@pid+'  
														ORDER BY [id] DESC
													   ) AS T
							)
			ORDER BY [id] DESC
			')
		END
GO

IF OBJECT_ID ('dnt_getUnAuditPostList','P') IS NOT NULL
DROP PROCEDURE [dnt_getUnAuditPostList]
GO

CREATE PROCEDURE [dnt_getUnAuditPostList]
@fidlist		VARCHAR(255),
@tableid		INT,
@filter			INT,
@pagesize		INT,
@pageindex		INT
AS
DECLARE @pagetop INT
SET @pagetop = (@pageindex-1)*@pagesize

IF @pageindex = 1
BEGIN
	IF @fidlist = '0'
		EXEC('SELECT TOP ' + @pagesize + '
										[pid],
										[tid],
										[fid], 
										[parentid],
										[title], 
										[layer],
										[message], 
										[ip], 
										[lastedit], 
										[postdatetime], 
										[attachment], 
										[poster], 
										[posterid], 
										[invisible], 
										[usesig], 
										[htmlon], 
										[smileyoff], 
										[parseurloff], 
										[bbcodeoff], 
										[rate], 
										[ratetimes] 									
		 FROM [dnt_posts'+@tableid+'] WHERE [invisible]='+@filter+' AND [layer]>0 ORDER BY [pid] DESC')
	ELSE
		EXEC('SELECT TOP ' + @pagesize + '
										[pid], 
										[tid],
										[fid],
										[parentid], 
										[title], 
										[layer],
										[message], 
										[ip], 
										[lastedit], 
										[postdatetime], 
										[attachment], 
										[poster], 
										[posterid], 
										[invisible], 
										[usesig], 
										[htmlon], 
										[smileyoff], 
										[parseurloff], 
										[bbcodeoff], 
										[rate], 
										[ratetimes] 									
		 FROM [dnt_posts'+@tableid+'] WHERE [fid] IN ('+@fidlist+') AND [invisible]='+@filter+' AND [layer]>0 ORDER BY [pid] DESC')
END
ELSE
BEGIN
	IF @fidlist = '0'
		EXEC('SELECT TOP ' + @pagesize + ' 
										[pid],
										[tid], 
										[fid],
										[parentid], 
										[title], 
										[layer],
										[message], 
										[ip], 
										[lastedit], 
										[postdatetime], 
										[attachment], 
										[poster], 
										[posterid], 
										[invisible], 
										[usesig], 
										[htmlon], 
										[smileyoff], 
										[parseurloff], 
										[bbcodeoff], 
										[rate], 
										[ratetimes]
		FROM [dnt_posts'+@tableid+'] WHERE [pid] < (SELECT MIN([pid])  FROM (SELECT TOP '+@pagetop+' [pid] FROM [dnt_posts'+@tableid+'] WHERE [invisible]='+@filter+' AND [layer]>0 ORDER BY [pid] DESC) AS T) AND [invisible]=1 AND [layer]>0 ORDER BY [pid] DESC')
	ELSE
		EXEC('SELECT TOP ' + @pagesize + ' 
										[pid],
										[tid], 
										[fid],
										[parentid], 
										[title], 
										[layer],
										[message], 
										[ip], 
										[lastedit], 
										[postdatetime], 
										[attachment], 
										[poster], 
										[posterid], 
										[invisible], 
										[usesig], 
										[htmlon], 
										[smileyoff], 
										[parseurloff], 
										[bbcodeoff], 
										[rate], 
										[ratetimes]
		FROM [dnt_posts'+@tableid+'] WHERE [pid] < (SELECT MIN([pid])  FROM (SELECT TOP '+@pagetop+' [pid] FROM [dnt_posts'+@tableid+'] WHERE [fid] IN ('+@fidlist+') AND [invisible]='+@filter+' AND [layer]>0 ORDER BY [pid] DESC) AS T) AND [fid] IN ('+@fidlist+') AND [invisible]=1 AND [layer]>0 ORDER BY [pid] DESC')
END
GO

IF OBJECT_ID('dnt_updateBanUser','P') IS NOT NULL
DROP PROC dnt_updateBanUser
GO

CREATE PROC dnt_updateBanUser
@groupid		INT,
@groupexpiry	VARCHAR(50), 
@uid			INT
AS

UPDATE [dnt_users] SET [groupid]=@groupid, [groupexpiry]=@groupexpiry WHERE [uid]=@uid;
UPDATE [dnt_online] SET [groupid]=@groupid WHERE userid=@uid;
GO

IF OBJECT_ID('dnt_getattAchmentlistByUid','P') IS NOT NULL
DROP PROC [dnt_getattAchmentlistByUid]
GO

CREATE PROCEDURE [dnt_getattAchmentlistByUid]
@uid	INT,
@days	INT
AS
IF @days=0
	SELECT [aid], [tid], [pid], [filename] FROM [dnt_attachments] WITH (NOLOCK) WHERE [uid] = @uid
ELSE
    SELECT [aid], [tid], [pid], [filename] FROM [dnt_attachments] WITH (NOLOCK) WHERE [uid] = @uid AND DATEDIFF(dd,postdatetime,GETDATE()) <= @days
GO

IF OBJECT_ID('dnt_deleteAttachmentByUid','P') IS NOT NULL
DROP PROC [dnt_deleteAttachmentByUid]
GO

CREATE PROC [dnt_deleteAttachmentByUid]
@uid	INT,
@days	INT
AS
IF @days = 0
BEGIN
	DELETE FROM [dnt_attachments] WHERE [uid] = @uid;
	DELETE FROM [dnt_myattachments] WHERE [uid] = @uid;
END
ELSE
BEGIN
	DELETE FROM [dnt_attachments] WHERE [uid] = @uid AND DATEDIFF(dd,postdatetime,GETDATE()) <= @days;
	DELETE FROM [dnt_myattachments] WHERE [uid] = @uid AND DATEDIFF(dd,postdatetime,GETDATE()) <= @days;
END
GO

IF OBJECT_ID('dnt_getuseridbyemail','P') IS NOT NULL
DROP PROC [dnt_getuseridbyemail]
GO

CREATE PROCEDURE [dnt_getuseridbyemail]
@email char(50)
AS

SELECT TOP 1 [uid] FROM [dnt_users] WHERE [email]=@email
GO

IF OBJECT_ID('dnt_getuserinfobyemail','P') IS NOT NULL
DROP PROC [dnt_getuserinfobyemail]
GO

CREATE PROCEDURE [dnt_getuserinfobyemail]
@email char(50)
AS

SELECT [username],[email] FROM [dnt_users] WHERE [email]=@email
GO

IF OBJECT_ID('dnt_gettopiclistbyvieworreplies','P') IS NOT NULL
DROP PROC dnt_gettopiclistbyvieworreplies
GO


CREATE PROCEDURE [dnt_gettopiclistbyvieworreplies]
@fid int,
@pagesize int,
@pageindex int,
@startnum int,
@condition varchar(100),
@orderby varchar(100),
@ascdesc int

AS
DECLARE @strsql varchar(5000)
DECLARE @sorttype varchar(5)

IF @ascdesc=0
   SET @sorttype='ASC'
ELSE
   SET @sorttype='DESC'

IF @pageindex = 1
	BEGIN
		SET @strsql = 'SELECT TOP ' + STR(@pagesize) +' [tid],[iconid],[title],[price],[typeid],[readperm],[hide],[special],[poster],[posterid],[replies],[views],[postdatetime],[lastpost],[lastposter],[lastpostid],[lastposterid],[highlight],[digest],[displayorder],[attachment],[closed],[magic],[rate] FROM [dnt_topics] WHERE [fid]=' +STR(@fid) + ' AND [displayorder]=0'+@condition+' ORDER BY '+@orderby+' '+@sorttype
	END
ELSE
           IF @sorttype='DESC'
	BEGIN
		SET @strSQL = 'SELECT TOP ' + STR(@pagesize) +' [tid],[iconid],[title],[price],[typeid],[hide],[readperm],[special],[poster],[posterid],[replies],[views],[postdatetime],[lastpost],[lastposter],[lastpostid],[lastposterid],[highlight],[digest],[displayorder],[attachment],[closed],[magic],[rate] FROM [dnt_topics] WHERE [tid] NOT IN (SELECT TOP ' + STR((@pageindex-1)*@pagesize-@startnum) + ' [tid]  FROM [dnt_topics] WHERE [fid]=' +STR(@fid) + ' AND [displayorder]=0'+@condition+' ORDER BY  '+@orderby+' '+@sorttype+') AND [fid]=' +STR(@fid) + ' AND [displayorder]=0'+@condition+' ORDER BY  '+@orderby+' '+@sorttype
	END
      ELSE
             BEGIN
		SET @strSQL = 'SELECT TOP ' + STR(@pagesize) +' [tid],[iconid],[title],[price],[hide],[typeid],[readperm],[special],[poster],[posterid],[replies],[views],[postdatetime],[lastpost],[lastposter],[lastpostid],[lastposterid],[highlight],[digest],[displayorder],[attachment],[closed],[magic],[rate] FROM [dnt_topics] WHERE [tid] NOT IN (SELECT TOP ' + STR((@pageindex-1)*@pagesize-@startnum) + ' [tid] FROM [dnt_topics] WHERE [fid]=' +STR(@fid) + ' AND [displayorder]=0'+@condition+' ORDER BY  '+@orderby+' '+@sorttype+') AND [fid]=' +STR(@fid) + ' AND [displayorder]=0'+@condition+' ORDER BY '+@orderby+' '+@sorttype
            END
EXEC(@strsql)
GO

IF OBJECT_ID('dnt_updateuserfavoriteviewtime','P') IS NOT NULL
DROP PROC [dnt_updateuserfavoriteviewtime]
GO

CREATE PROCEDURE [dnt_updateuserfavoriteviewtime]
@uid int,
@tid int
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE [dnt_favorites] SET [viewtime]=GETDATE() WHERE [uid]=@uid AND [tid]=@tid
END
GO

IF OBJECT_ID('dnt_updatedebate','P') IS NOT NULL
DROP PROC [dnt_updatedebate]
GO

CREATE PROC [dnt_updatedebate]
@tid INT,
@positiveopinion NVARCHAR(200),
@positivediggs INT,
@negativeopinion NVARCHAR(200),
@negativediggs INT,
@terminaltime DATETIME
AS
UPDATE [dnt_debates]
SET [positiveopinion]=@positiveopinion,[negativeopinion]=@negativeopinion,[terminaltime]=@terminaltime,[positivediggs]=@positivediggs,[negativediggs]=@negativediggs
WHERE [tid]=@tid
GO

IF OBJECT_ID('dnt_gethotimages','P') IS NOT NULL
DROP PROC [dnt_gethotimages]
GO

CREATE PROCEDURE [dnt_gethotimages]
@fidlist VARCHAR(4000),
@count INT,
@orderby VARCHAR(50),
@continuous INT
AS

IF @continuous = 1
	BEGIN
		IF @fidlist <> ''
			SET @fidlist = 'AND [fid] IN('+@fidlist+') '
		EXEC('SELECT TOP '+ @count +' [attach].[aid],[attach].[tid],[attach].[filename],[attach].[attachment],[topic].[title] FROM [dnt_attachments] AS [attach] LEFT JOIN [dnt_topics] AS [topic] ON [attach].[tid] = [topic].[tid] AND [topic].[displayorder]>=0 WHERE
aid = (SELECT MIN(aid) from [dnt_attachments] where [width] > 360 AND [height] > 240 and [tid]=[topic].[tid]) '+ @fidlist +' ORDER BY [attach].['+ @orderby +'] DESC')
	END
ELSE
	BEGIN
		IF @fidlist <> ''
			SET @fidlist = 'AND [topic].[fid] IN('+@fidlist+') '
		EXEC('SELECT TOP '+ @count +' [attach].[tid],[attach].[filename],[attach].[attachment],[topic].[title] FROM [dnt_attachments] AS [attach] LEFT JOIN [dnt_topics] AS [topic] ON [attach].[tid] = [topic].[tid] AND [topic].[displayorder]>=0  WHERE [attach].[width] > 360  AND [height] > 240 '+ @fidlist+' ORDER BY ['+ @orderby +'] DESC')
	END
GO

IF OBJECT_ID('dnt_updatetrendstat','P') IS NOT NULL
DROP PROC [dnt_updatetrendstat]
GO

CREATE PROC [dnt_updatetrendstat]
@daytime INT,
@login INT,
@register INT,
@topic INT,
@post INT,
@poll INT,
@bonus INT,
@debate INT
AS
BEGIN	
	IF NOT EXISTS(SELECT [daytime] FROM [dnt_trendstat] WHERE [daytime]=@daytime)
		INSERT INTO [dnt_trendstat] ([daytime],[login],[register],[topic],[post]) VALUES(@daytime,0,0,0,0)		
		
		UPDATE [dnt_trendstat] SET [login]=[login]+@login,[register]=[register]+@register,[topic]=[topic]+@topic,[post]=[post]+@post,
			[poll]=[poll]+@poll,[bonus]=[bonus]+@bonus,[debate]=[debate]+@debate WHERE [daytime]=@daytime
END