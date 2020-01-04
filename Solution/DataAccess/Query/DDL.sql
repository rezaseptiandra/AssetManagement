USE [REZADB]
GO
/****** Object:  StoredProcedure [dbo].[MKaryawan_SP_DELETE]    Script Date: 7/29/2019 9:09:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
-- =============================================
-- Author	  :	Reza
-- Create date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[MKaryawan_SP_DELETE] 
	@filter varchar(max)="",
	@isDebug bit = 0
AS
BEGIN
	EXEC("DELETE FROM  MKaryawan WHERE("+@filter+");");	
END


GO
/****** Object:  StoredProcedure [dbo].[MKaryawan_SP_INSERT]    Script Date: 7/29/2019 9:09:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
-- =============================================
-- Author:		Reza
-- Create date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[MKaryawan_SP_INSERT] 
	@field varchar(max)="",
	@value varchar(max) = "",
	@getkey bit = 0
AS
BEGIN
IF(@getkey=0)
	BEGIN
	EXEC("INSERT INTO MKaryawan"+@field+" VALUES"+@value+";");	
	SELECT @@ROWCOUNT;
	SET NOCOUNT OFF;
	END
ELSE
	BEGIN
	EXEC("INSERT INTO MKaryawan"+@field+" VALUES"+@value+";
		  SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY];");	
	SET NOCOUNT OFF;
	END
END


GO
/****** Object:  StoredProcedure [dbo].[MKaryawan_SP_SELECT]    Script Date: 7/29/2019 9:09:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
-- =============================================
-- Author:		Reza
-- Create date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[MKaryawan_SP_SELECT] 
	@selectedField varchar(max)="SELECT *",
	@filter varchar(max) = "",
	@groupBy varchar(max) = "",
	@orderBy varchar(max) = "",
	@pageNumber int = 0,
	@pageSize int = 0,
	@isCount bit = 0,
	@isDebug bit = 0
AS
BEGIN
	DECLARE @QUERY VARCHAR(MAX);
	SET @QUERY = "WITH CTE_ AS ("
					+ @selectedfield 
					+ " FROM MKaryawan "				
					+ @filter 
					+ @groupby +")";

	IF(@iscount=1)
		SET @QUERY = @QUERY+" SELECT COUNT(*) FROM CTE_";
	ELSE
	BEGIN
		IF(@pagenumber>0)
		BEGIN
			SET @QUERY = @QUERY+" SELECT * FROM CTE_"+ @orderby + " OFFSET " 
			+ CAST(@pagesize AS VARCHAR)+ " * (" + CAST(@pagenumber AS VARCHAR) + " - 1) ROWS FETCH NEXT "
			+ CAST(@pagesize AS VARCHAR) + " ROWS ONLY;";
			
		END
		ELSE
		BEGIN
			SET @QUERY = @QUERY+" SELECT * FROM CTE_"+ @orderby;
		END
	END

	IF(@isDebug=0)
		EXEC(@QUERY);
	ELSE
		SELECT @QUERY;
END


GO
/****** Object:  StoredProcedure [dbo].[MKaryawan_SP_UPDATE]    Script Date: 7/29/2019 9:09:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
-- =============================================
-- Author	  :	Reza
-- Create date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[MKaryawan_SP_UPDATE] 
	@filter varchar(max)="",
	@setvalue varchar(max)="",
	@isDebug bit = 0
AS
BEGIN
	IF(@isDebug = 0) 
	EXEC("UPDATE MKaryawan SET "+@setvalue+" WHERE("+@filter+");");	
	ELSE
	SELECT "UPDATE MKaryawan SET "+@setvalue+" WHERE("+@filter+");";
END


GO
/****** Object:  StoredProcedure [dbo].[MKaryawanVM_sel]    Script Date: 7/29/2019 9:09:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
-- =============================================
-- Author:		Reza
-- Create date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[MKaryawanVM_sel] 
	-- Add the parameters for the stored procedure here
	@key1 varchar(max)="",
	@field varchar(max)="SELECT *",
	@filter varchar(max) = "",	
	@groupby varchar(max) = "",	
	@orderby varchar(max) = "",
	@pagenumber int = 0,
	@pagesize int = 0,
	@iscount bit = 0
AS
BEGIN
	declare
	@query_ as varchar(max)	

	set @query_ = "FROM MKaryawan "
	if (@key1!="")
		begin
			set @filter = "WHERE ID ="+ @key1;
		end
	if (@pagenumber > 0 and @pagesize > 0 and @iscount=0)
		begin
			if(@orderby = "")
				set @orderby = "order by [ID] OFFSET "+CAST(@pagesize AS varchar)+" * ("+CAST(@pagenumber AS VARCHAR)+" - 1) ROWS FETCH NEXT "+CAST(@pagesize AS varchar)+" ROWS ONLY";
			else
				set @orderby = @orderby+ " OFFSET "+CAST(@pagesize AS varchar)+" * ("+CAST(@pagenumber AS VARCHAR)+" - 1) ROWS FETCH NEXT "+CAST(@pagesize AS varchar)+" ROWS ONLY";
		end
		
	
	if(@iscount=0)
		exec(@field+" "+@query_+" "+@filter+" "+@groupby+" "+@orderby);
	else
		exec("SELECT COUNT(*) AS TOTALROW FROM ("+@field+" "+@query_+" "+@filter+" "+@groupby+" "+@orderby+")TB");
	
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT OFF;

    -- Insert statements for procedure here
END


GO
/****** Object:  StoredProcedure [dbo].[MKaryawanVM_SP_SELECT]    Script Date: 7/29/2019 9:09:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
-- =============================================
-- Author:		Reza
-- Create date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[MKaryawanVM_SP_SELECT] 
	@selectedField varchar(max)="SELECT *",
	@filter varchar(max) = "",
	@groupBy varchar(max) = "",
	@orderBy varchar(max) = "",
	@pageNumber int = 0,
	@pageSize int = 0,
	@isCount bit = 0,
	@isDebug bit = 0
AS
BEGIN
	DECLARE @QUERY VARCHAR(MAX);
	SET @QUERY = "WITH CTE_ AS ("
					+ @selectedfield 
					+ " FROM MKaryawan "				
					+ @filter 
					+ @groupby +")";

	IF(@iscount=1)
		SET @QUERY = @QUERY+" SELECT COUNT(*) FROM CTE_";
	ELSE
	BEGIN
		IF(@pagenumber>0)
		BEGIN
			SET @QUERY = @QUERY+" SELECT * FROM CTE_"+ @orderby + " OFFSET " 
			+ CAST(@pagesize AS VARCHAR)+ " * (" + CAST(@pagenumber AS VARCHAR) + " - 1) ROWS FETCH NEXT "
			+ CAST(@pagesize AS VARCHAR) + " ROWS ONLY;";
			
		END
		ELSE
		BEGIN
			SET @QUERY = @QUERY+" SELECT * FROM CTE_"+ @orderby;
		END
	END

	IF(@isDebug=0)
		EXEC(@QUERY);
	ELSE
		SELECT @QUERY;
END


GO
/****** Object:  StoredProcedure [dbo].[MUserVM_sel]    Script Date: 7/29/2019 9:09:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
-- =============================================
-- Author:		Reza
-- Create date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[MUserVM_sel] 
	-- Add the parameters for the stored procedure here
	@filter varchar(MAX)=''
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	exec("SELECT *, '1sdf' as ULOLKA FROM MUser "+ @filter)
END


GO
/****** Object:  StoredProcedure [dbo].[SampleTableMaster_INT_IDENTITYVM_sel]    Script Date: 7/29/2019 9:09:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
-- =============================================
-- Author:		Reza
-- Create date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[SampleTableMaster_INT_IDENTITYVM_sel] 
	-- Add the parameters for the stored procedure here
	@key1 varchar(max)="",
	@field varchar(max)="SELECT *",
	@filter varchar(max) = "",	
	@groupby varchar(max) = "",	
	@orderby varchar(max) = "",
	@pagenumber int = 0,
	@pagesize int = 0,
	@iscount bit = 0
AS
BEGIN
	declare
	@query_ as varchar(max)	

	set @query_ = "FROM SampleTableMaster_INT_IDENTITY"

	if (@key1!="")
		set @filter = "WHERE IDINT ="+ @key1;
		if (@pagenumber > 0 and @pagesize > 0)
		begin
			if(@orderby = "")
				set @orderby = "order by [IDINT] OFFSET "+CAST(@pagesize AS varchar)+" * ("+CAST(@pagenumber AS VARCHAR)+" - 1) ROWS FETCH NEXT "+CAST(@pagesize AS varchar)+" ROWS ONLY";
			else
				set @orderby = @orderby+ " OFFSET "+CAST(@pagesize AS varchar)+" * ("+CAST(@pagenumber AS VARCHAR)+" - 1) ROWS FETCH NEXT "+CAST(@pagesize AS varchar)+" ROWS ONLY";
		end

	exec(@field+" "+@query_+" "+@filter+" "+@groupby+" "+@orderby);
	
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
END


GO
/****** Object:  StoredProcedure [dbo].[SampleTableMaster_INTVM_sel]    Script Date: 7/29/2019 9:09:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
-- =============================================
-- Author:		Reza
-- Create date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[SampleTableMaster_INTVM_sel] 
	-- Add the parameters for the stored procedure here
	@key1 varchar(max)="",
	@field varchar(max)="SELECT *",
	@filter varchar(max) = "",	
	@groupby varchar(max) = "",	
	@orderby varchar(max) = "",
	@pagenumber int=0,
	@pagesize int = 0
AS
BEGIN
	declare
	@query_ as varchar(max)	

	set @query_ = "FROM SampleTableMaster_INT"

	if (@key1!="")
		set @filter = "WHERE IDINT ="+ @key1;
	if (@pagenumber > 0 and @pagesize > 0)
		begin
			if(@orderby = "")
				set @orderby = "order by [IDINT] OFFSET "+CAST(@pagesize AS varchar)+" * ("+CAST(@pagenumber AS VARCHAR)+" - 1) ROWS FETCH NEXT "+CAST(@pagesize AS varchar)+" ROWS ONLY";
			else
				set @orderby = @orderby+ " OFFSET "+CAST(@pagesize AS varchar)+" * ("+CAST(@pagenumber AS VARCHAR)+" - 1) ROWS FETCH NEXT "+CAST(@pagesize AS varchar)+" ROWS ONLY";
		end

	exec(@field+" "+@query_+" "+@filter+" "+@groupby+" "+@orderby);
	
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
END


GO
/****** Object:  StoredProcedure [dbo].[SampleTableMasterVM_sel]    Script Date: 7/29/2019 9:09:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
-- =============================================
-- Author:		Reza
-- Create date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[SampleTableMasterVM_sel] 
	-- Add the parameters for the stored procedure here
	@key1 varchar(max)="",
	@field varchar(max)="SELECT *",
	@filter varchar(max) = "",	
	@groupby varchar(max) = "",	
	@orderby varchar(max) = "",
	@pagenumber int=0,
	@pagesize int = 0
AS
BEGIN
	declare
	@query_ as varchar(max)	
	set @query_ = "FROM SampleTableMaster"

	if (@key1!="")
		set @filter = "WHERE IDSTR ='"+ @key1+"'";
	if (@pagenumber > 0 and @pagesize > 0)
		begin
			if(@orderby = "")
				set @orderby = "order by [IDSTR] OFFSET "+CAST(@pagesize AS varchar)+" * ("+CAST(@pagenumber AS VARCHAR)+" - 1) ROWS FETCH NEXT "+CAST(@pagesize AS varchar)+" ROWS ONLY";
			else
				set @orderby = @orderby+ " OFFSET "+CAST(@pagesize AS varchar)+" * ("+CAST(@pagenumber AS VARCHAR)+" - 1) ROWS FETCH NEXT "+CAST(@pagesize AS varchar)+" ROWS ONLY";
		end

	exec(@field+" "+@query_+" "+@filter+" "+@groupby+" "+@orderby);
	
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
END


GO
/****** Object:  StoredProcedure [dbo].[SampleTableMultipleKeyVM_sel]    Script Date: 7/29/2019 9:09:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
-- =============================================
-- Author:		Reza
-- Create date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[SampleTableMultipleKeyVM_sel] 
	-- Add the parameters for the stored procedure here
	@key1 varchar(max)="",
	@key2 varchar(max)="",
	@key3 varchar(max)="",
	@field varchar(max)="SELECT *",
	@filter varchar(max) = "",	
	@groupby varchar(max) = "",	
	@orderby varchar(max) = "",
	@pagenumber int=0,
	@pagesize int = 0
AS
BEGIN
	declare	@query_ as varchar(max)	;

	set @query_ = "FROM SampleTableMultipleKey";

	if(@filter="")
		set @filter = "WHERE 1=1 ";
			
	if (@key1!="")
		set @filter = @filter+" AND ID1 ='"+ @key1+"'";
	if (@key2!="")
		set @filter = @filter+" AND ID2 ='"+ @key2+"'";
	if (@key3!="")
		set @filter = @filter+" AND ID3 ='"+ @key3+"'";



	if (@pagenumber > 0 and @pagesize > 0)
		begin
			if(@orderby = "")
				set @orderby = "order by [ID1],[ID2],[ID3] OFFSET "+CAST(@pagesize AS varchar)+" * ("+CAST(@pagenumber AS VARCHAR)+" - 1) ROWS FETCH NEXT "+CAST(@pagesize AS varchar)+" ROWS ONLY";
			else
				set @orderby = @orderby+ " OFFSET "+CAST(@pagesize AS varchar)+" * ("+CAST(@pagenumber AS VARCHAR)+" - 1) ROWS FETCH NEXT "+CAST(@pagesize AS varchar)+" ROWS ONLY";
		end
	exec(@field+" "+@query_+" "+@filter+" "+@groupby+" "+@orderby);
	
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
END


GO
/****** Object:  StoredProcedure [dbo].[selcustomsp]    Script Date: 7/29/2019 9:09:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
-- =============================================
-- Author:		Reza
-- Create date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[selcustomsp] 
	-- Add the parameters for the stored procedure here
	@key1 varchar(max)="",
	@field varchar(max)="SELECT *",
	@filter varchar(max) = ""
AS
BEGIN
	declare
	@query_ as varchar(max)	
	set @query_ = "FROM SampleTableMaster"

	if (@key1!="")
		set @filter = "WHERE IDSTR ="+ @key1

	exec(@field+" "+@query_+" "+@filter);
	
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
END


GO
/****** Object:  StoredProcedure [dbo].[spTest]    Script Date: 7/29/2019 9:09:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Reza
-- Create date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[spTest] 
	-- Add the parameters for the stored procedure here

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SELECT 'ulol'as 'satu', 'noob' as 'dua' ;
	SET NOCOUNT ON;

    -- Insert statements for procedure here
END


GO
/****** Object:  StoredProcedure [dbo].[TActionPermission_SP_GetActionPermission]    Script Date: 7/29/2019 9:09:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
-- =============================================
-- Author:		Reza
-- Create date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[TActionPermission_SP_GetActionPermission] 
@filter varchar(max) = "",
@field varchar(max) = "",
@groupby varchar(max) = "",
	@orderby varchar(max) = ""
AS
BEGIN	
	exec( "	select * from (select distinct TActionPermission.ControllerActionName,MUser.Username AS Username from MUser
left join TUserRole on TUserRole.Username = MUser.Username
inner join TActionPermission on TActionPermission.RoleID = TUserRole.RoleID)as er "+@filter);
	
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
END


GO
/****** Object:  StoredProcedure [dbo].[TActionPermission_SP_GetRolePermission]    Script Date: 7/29/2019 9:09:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
-- =============================================
-- Author:		Reza
-- Create date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[TActionPermission_SP_GetRolePermission] 
@filter varchar(max) = "",
@field varchar(max) = "",
@groupby varchar(max) = "",
@orderby varchar(max) = ""
AS
BEGIN	
	exec( "	SELECT * FROM (
			SELECT 
						(case 
							when A.AllowCreate=1 then  cast(B.RequiredCreate as varchar(max))
							when A.AllowEdit=1 then cast( B.RequiredEdit as varchar(max))
							when A.AllowDelete=1 then cast (B.RequiredDelete as varchar(max))
							when A.AllowView=1 then cast(B.RequiredView as varchar(max))
							else
							'1'
							end ) as IsAllowed, M.Username, B.ControllerName, B.ActionName
						 FROM muser M left join TUserRole r on m.Username = r.Username
			left join TControllerRoleAccess A on a.RoleID = r.RoleID
			left join TActionPermission B on A.ControllerName = B.ControllerName) as TActionPermissions
			
			  "+@filter);
			 --where Username = 'uio'
			--and ControllerName='Account' 
			--and ActionName = 'DeleteAccount' 
			--and IsAllowed = '1'
	
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
END


GO
/****** Object:  StoredProcedure [dbo].[TigaKey_SP_INSERT]    Script Date: 7/29/2019 9:09:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
-- =============================================
-- Author:		Reza
-- Create date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[TigaKey_SP_INSERT] 
	@field varchar(max)="",
	@value varchar(max) = "",
	@getkey bit = 0
AS
BEGIN
IF(@getkey=0)
	BEGIN
	EXEC("INSERT INTO TigaKey"+@field+" VALUES"+@value+";");	
	SELECT @@ROWCOUNT;
	SET NOCOUNT OFF;
	END
ELSE
	BEGIN
	EXEC("INSERT INTO TigaKey"+@field+" VALUES"+@value+";
		  SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY];");	
	SET NOCOUNT OFF;
	END
END


GO
/****** Object:  StoredProcedure [dbo].[TigaKey_SP_SELECT]    Script Date: 7/29/2019 9:09:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
-- =============================================
-- Author:		Reza
-- Create date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[TigaKey_SP_SELECT] 
	@selectedField varchar(max)="SELECT *",
	@filter varchar(max) = "",
	@groupBy varchar(max) = "",
	@orderBy varchar(max) = "",
	@pageNumber int = 0,
	@pageSize int = 0,
	@isCount bit = 0,
	@isDebug bit = 0
AS
BEGIN
	DECLARE @QUERY VARCHAR(MAX);
	SET @QUERY = "WITH CTE_ AS ("
					+ @selectedfield 
					+ " FROM TigaKey "				
					+ @filter 
					+ @groupby +")";

	IF(@iscount=1)
		SET @QUERY = @QUERY+" SELECT COUNT(*) FROM CTE_";
	ELSE
	BEGIN
		IF(@pagenumber>0)
		BEGIN
			SET @QUERY = @QUERY+" SELECT * FROM CTE_"+ @orderby + " OFFSET " 
			+ CAST(@pagesize AS VARCHAR)+ " * (" + CAST(@pagenumber AS VARCHAR) + " - 1) ROWS FETCH NEXT "
			+ CAST(@pagesize AS VARCHAR) + " ROWS ONLY;";
			
		END
		ELSE
		BEGIN
			SET @QUERY = @QUERY+" SELECT * FROM CTE_"+ @orderby;
		END
	END

	IF(@isDebug=0)
		EXEC(@QUERY);
	ELSE
		SELECT @QUERY;
END


GO
/****** Object:  StoredProcedure [dbo].[TPembelian_SP_INSERT]    Script Date: 7/29/2019 9:09:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
-- =============================================
-- Author:		Reza
-- Create date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[TPembelian_SP_INSERT] 
	@field varchar(max)="",
	@value varchar(max) = "",
	@getkey bit = 0
AS
BEGIN
IF(@getkey=0)
	BEGIN
	EXEC("INSERT INTO TPembelian"+@field+" VALUES"+@value+";");	
	SELECT @@ROWCOUNT;
	SET NOCOUNT OFF;
	END
ELSE
	BEGIN
	EXEC("INSERT INTO TPembelian"+@field+" VALUES"+@value+";
		  SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY];");	
	SET NOCOUNT OFF;
	END
END


GO
/****** Object:  StoredProcedure [dbo].[TRefreshToken_updateinsert]    Script Date: 7/29/2019 9:09:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
-- =============================================
-- Author:		Reza
-- Create date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[TRefreshToken_updateinsert] 
	-- Add the parameters for the stored procedure here
	@userid varchar(50)="",
	@refreshtoken varchar(50)=""

AS
BEGIN
	SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
		BEGIN TRANSACTION;
		IF EXISTS (SELECT 1 FROM dbo.TRefreshToken WHERE userid = @userid)
		BEGIN
		  UPDATE TRefreshToken set refreshtoken = @refreshtoken where userid = @userid
		END
		ELSE
		BEGIN
		  INSERT into TRefreshToken values(@userid,@refreshtoken)
		END	
	COMMIT TRANSACTION;
END


GO
/****** Object:  StoredProcedure [dbo].[TUserRole_SP_GetRoleAdmin]    Script Date: 7/29/2019 9:09:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
-- =============================================
-- Author:		Reza
-- Create date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[TUserRole_SP_GetRoleAdmin] 
@filter varchar(max) = "",
@field varchar(max) = "",
@groupby varchar(max) = "",
@orderby varchar(max) = ""
AS
BEGIN	
	exec( "	 WITH TUserRole_CTE (RoleID, Username)  
				AS   
				(  
					select r.RoleID,m.Username  from  muser M join TUserRole r on m.Username = r.Username
				left join TControllerRoleAccess A on a.RoleID = r.RoleID where r.RoleID = 'ADM'

				)  
				select * from TUserRole_CTE "+@filter);
			 --where B.ControllerName = 'Account' and B.ActionName='DeleteAccount
	
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
END


GO
/****** Object:  Table [dbo].[DUser]    Script Date: 7/29/2019 9:09:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DUser](
	[ID] [int] NOT NULL,
	[MUserID] [int] NOT NULL,
	[Name] [varchar](50) NULL,
 CONSTRAINT [PK_DUser] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MKaryawan]    Script Date: 7/29/2019 9:09:53 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MKaryawan](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Age] [int] NOT NULL,
	[Office] [varchar](50) NOT NULL,
	[Position] [varchar](50) NULL,
	[Salary] [decimal](18, 0) NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[Active] [bit] NULL,
 CONSTRAINT [PK_MKaryawan] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MMenu]    Script Date: 7/29/2019 9:09:53 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MMenu](
	[ID] [varchar](50) NOT NULL,
	[ParentID] [varchar](50) NOT NULL,
	[MenuName] [varchar](50) NOT NULL,
	[Path] [varchar](200) NOT NULL,
 CONSTRAINT [PK_MMenu_1] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MRole]    Script Date: 7/29/2019 9:09:53 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MRole](
	[RoleID] [varchar](10) NOT NULL,
	[Descriptions] [varchar](50) NULL,
 CONSTRAINT [PK_MRole] PRIMARY KEY CLUSTERED 
(
	[RoleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MUser]    Script Date: 7/29/2019 9:09:53 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MUser](
	[Username] [varchar](50) NOT NULL,
	[Password] [varchar](50) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[IsLocked] [bit] NOT NULL,
	[Email] [varchar](50) NOT NULL,
	[FullName] [varchar](200) NOT NULL,
 CONSTRAINT [PK_MUser] PRIMARY KEY CLUSTERED 
(
	[Username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SampleTableMaster]    Script Date: 7/29/2019 9:09:53 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SampleTableMaster](
	[IDSTR] [varchar](50) NOT NULL,
	[NAME] [varchar](50) NULL,
 CONSTRAINT [PK_SampleTableMaster] PRIMARY KEY CLUSTERED 
(
	[IDSTR] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SampleTableMaster_INT]    Script Date: 7/29/2019 9:09:53 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SampleTableMaster_INT](
	[IDINT] [int] NOT NULL,
	[NAME] [varchar](50) NULL,
 CONSTRAINT [PK_SampleTableMaster_INT] PRIMARY KEY CLUSTERED 
(
	[IDINT] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SampleTableMaster_INT_IDENTITY]    Script Date: 7/29/2019 9:09:53 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SampleTableMaster_INT_IDENTITY](
	[IDINT] [int] IDENTITY(1,1) NOT NULL,
	[NAME] [varchar](50) NULL,
 CONSTRAINT [PK_SampleTableMaster_INT_IDENTITY] PRIMARY KEY CLUSTERED 
(
	[IDINT] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SampleTableMultipleKey]    Script Date: 7/29/2019 9:09:53 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SampleTableMultipleKey](
	[ID1] [int] NOT NULL,
	[ID2] [int] NOT NULL,
	[ID3] [varchar](50) NOT NULL,
	[NAME] [varchar](50) NULL,
 CONSTRAINT [PK__SampleTa__18DE73E03B4A10AF] PRIMARY KEY CLUSTERED 
(
	[ID1] ASC,
	[ID2] ASC,
	[ID3] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TActionPermission]    Script Date: 7/29/2019 9:09:53 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TActionPermission](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ControllerName] [varchar](50) NOT NULL,
	[ActionName] [varchar](50) NOT NULL,
	[RequiredCreate] [bit] NOT NULL,
	[RequiredEdit] [bit] NOT NULL,
	[RequiredDelete] [bit] NOT NULL,
	[RequiredView] [bit] NOT NULL,
 CONSTRAINT [PK_TActionPermission_1] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TControllerRoleAccess]    Script Date: 7/29/2019 9:09:53 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TControllerRoleAccess](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ControllerName] [varchar](50) NOT NULL,
	[RoleID] [varchar](10) NOT NULL,
	[AllowCreate] [bit] NOT NULL,
	[AllowEdit] [bit] NOT NULL,
	[AllowDelete] [bit] NOT NULL,
	[AllowView] [bit] NOT NULL,
 CONSTRAINT [PK_TControllerRoleAccess_1] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TigaKey]    Script Date: 7/29/2019 9:09:53 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TigaKey](
	[ID1] [int] NOT NULL,
	[ID2] [int] NOT NULL,
	[ID3] [int] NOT NULL,
	[Deskripsi] [varchar](50) NOT NULL,
	[Dates] [datetime] NULL,
	[IsActive] [bit] NULL,
	[Gaji] [decimal](18, 2) NULL,
 CONSTRAINT [PK_Person] PRIMARY KEY CLUSTERED 
(
	[ID1] ASC,
	[ID2] ASC,
	[ID3] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TPembelian]    Script Date: 7/29/2019 9:09:53 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TPembelian](
	[TransactionID] [varchar](50) NOT NULL,
	[KaryawanID] [int] NOT NULL,
	[Waktu] [datetime] NOT NULL,
	[TotalHarga] [decimal](18, 0) NOT NULL,
 CONSTRAINT [PK_TPembelian] PRIMARY KEY CLUSTERED 
(
	[TransactionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TRefreshToken]    Script Date: 7/29/2019 9:09:53 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TRefreshToken](
	[userid] [varchar](50) NOT NULL,
	[refreshtoken] [varchar](50) NOT NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TToken]    Script Date: 7/29/2019 9:09:53 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TToken](
	[ID] [varchar](50) NOT NULL,
	[UserID] [int] NOT NULL,
	[RoleID] [varchar](50) NOT NULL,
	[Token] [varchar](200) NULL,
	[RefreshToken] [varchar](50) NULL,
	[IsActive] [bit] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TUserRole]    Script Date: 7/29/2019 9:09:53 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TUserRole](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Username] [varchar](50) NOT NULL,
	[RoleID] [varchar](10) NOT NULL,
 CONSTRAINT [PK_TUserRole] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
