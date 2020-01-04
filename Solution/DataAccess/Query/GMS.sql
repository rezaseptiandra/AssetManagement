USE [REZADB]
GO
/****** Object:  StoredProcedure [dbo].[GetActionPermission_SP_SELECT]    Script Date: 9/17/2019 4:43:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
-- =============================================
-- Author:		Reza
-- Create date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[GetActionPermission_SP_SELECT] 
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
					+ " FROM (SELECT 
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
			left join TActionPermission B on A.ControllerName = B.ControllerName) ActionRole "				
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
/****** Object:  StoredProcedure [dbo].[GetAdmin_SP_SELECT]    Script Date: 9/17/2019 4:43:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
-- =============================================
-- Author:		Reza
-- Create date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[GetAdmin_SP_SELECT] 
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

	IF (@filter ="")	
		BEGIN
			SET @filter = " WHERE Flag = 0 "
		END
	ELSE
		BEGIN 
			SET @filter = @filter + " AND Flag = 0 "
		END
	
	SET @QUERY = "WITH CTE_ AS ("
					+ @selectedfield 
					+ " FROM (select r.RoleID,m.Username, r.Flag  from  muser M join TUserRole r on m.Username = r.Username
								left join TControllerRoleAccess A on a.RoleID = r.RoleID where r.RoleID = 'ADM')AdminList "				
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
/****** Object:  StoredProcedure [dbo].[JoinedUserRoleVM_SP_SELECT]    Script Date: 9/17/2019 4:43:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
-- =============================================
-- Author:		Reza
-- Create date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[JoinedUserRoleVM_SP_SELECT] 
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
					+ " FROM (SELECT T.*, R.Descriptions FROM TUserRole T LEFT JOIN MRole R ON R.RoleID = T.RoleID)TB "				
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
/****** Object:  StoredProcedure [dbo].[MABPVM_SP_SELECT]    Script Date: 9/17/2019 4:43:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
-- =============================================
-- Author:		Reza
-- Create date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[MABPVM_SP_SELECT] 
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

	IF (@filter ="")	
		BEGIN
			SET @filter = " WHERE Flag = 0 "
		END
	ELSE
		BEGIN 
			SET @filter = @filter + " AND Flag = 0 "
		END
	
	SET @QUERY = "WITH CTE_ AS ("
					+ @selectedfield 
					+ " FROM MABP "				
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
/****** Object:  StoredProcedure [dbo].[MKaryawan_SP_DELETE]    Script Date: 9/17/2019 4:43:31 PM ******/
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
/****** Object:  StoredProcedure [dbo].[MKaryawan_SP_INSERT]    Script Date: 9/17/2019 4:43:31 PM ******/
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
/****** Object:  StoredProcedure [dbo].[MKaryawan_SP_SELECT]    Script Date: 9/17/2019 4:43:31 PM ******/
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
/****** Object:  StoredProcedure [dbo].[MKaryawan_SP_UPDATE]    Script Date: 9/17/2019 4:43:31 PM ******/
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
/****** Object:  StoredProcedure [dbo].[MKaryawanVM_sel]    Script Date: 9/17/2019 4:43:31 PM ******/
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
/****** Object:  StoredProcedure [dbo].[MKaryawanVM_SP_SELECT]    Script Date: 9/17/2019 4:43:31 PM ******/
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
/****** Object:  StoredProcedure [dbo].[MUserVM_sel]    Script Date: 9/17/2019 4:43:31 PM ******/
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
/****** Object:  StoredProcedure [dbo].[SampleTableMaster_INT_IDENTITYVM_sel]    Script Date: 9/17/2019 4:43:31 PM ******/
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
/****** Object:  StoredProcedure [dbo].[SampleTableMaster_INTVM_sel]    Script Date: 9/17/2019 4:43:31 PM ******/
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
/****** Object:  StoredProcedure [dbo].[SampleTableMasterVM_sel]    Script Date: 9/17/2019 4:43:31 PM ******/
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
/****** Object:  StoredProcedure [dbo].[SampleTableMultipleKeyVM_sel]    Script Date: 9/17/2019 4:43:31 PM ******/
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
/****** Object:  StoredProcedure [dbo].[selcustomsp]    Script Date: 9/17/2019 4:43:31 PM ******/
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
/****** Object:  StoredProcedure [dbo].[spTest]    Script Date: 9/17/2019 4:43:31 PM ******/
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
/****** Object:  StoredProcedure [dbo].[TActionPermission_SP_GetActionPermission]    Script Date: 9/17/2019 4:43:31 PM ******/
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
/****** Object:  StoredProcedure [dbo].[TActionPermission_SP_GetRolePermission]    Script Date: 9/17/2019 4:43:31 PM ******/
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
/****** Object:  StoredProcedure [dbo].[TigaKey_SP_INSERT]    Script Date: 9/17/2019 4:43:31 PM ******/
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
/****** Object:  StoredProcedure [dbo].[TigaKey_SP_SELECT]    Script Date: 9/17/2019 4:43:31 PM ******/
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
/****** Object:  StoredProcedure [dbo].[TPembelian_SP_INSERT]    Script Date: 9/17/2019 4:43:31 PM ******/
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
/****** Object:  StoredProcedure [dbo].[TRefreshToken_updateinsert]    Script Date: 9/17/2019 4:43:31 PM ******/
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
/****** Object:  StoredProcedure [dbo].[TUserRole_SP_GetRoleAdmin]    Script Date: 9/17/2019 4:43:31 PM ******/
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
/****** Object:  Table [dbo].[DUser]    Script Date: 9/17/2019 4:43:31 PM ******/
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
/****** Object:  Table [dbo].[MABP]    Script Date: 9/17/2019 4:43:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MABP](
	[ABPID] [varchar](50) NOT NULL,
	[Deskripsi] [varchar](50) NOT NULL,
	[PeriodeStart] [date] NOT NULL,
	[PeriodeEnd] [date] NOT NULL,
	[CreatedBy] [varchar](50) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedHost] [varchar](50) NULL,
	[ModifiedBy] [varchar](50) NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[ModifiedHost] [varchar](50) NULL,
	[Flag] [int] NOT NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MKaryawan]    Script Date: 9/17/2019 4:43:31 PM ******/
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
/****** Object:  Table [dbo].[MMenu]    Script Date: 9/17/2019 4:43:31 PM ******/
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
/****** Object:  Table [dbo].[MRole]    Script Date: 9/17/2019 4:43:31 PM ******/
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
/****** Object:  Table [dbo].[MUser]    Script Date: 9/17/2019 4:43:31 PM ******/
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
	[CreatedBy] [varchar](50) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedHost] [varchar](50) NULL,
	[ModifiedBy] [varchar](50) NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[ModifiedHost] [varchar](50) NULL,
	[Flag] [int] NOT NULL,
 CONSTRAINT [PK_MUser] PRIMARY KEY CLUSTERED 
(
	[Username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SampleTableMaster]    Script Date: 9/17/2019 4:43:31 PM ******/
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
/****** Object:  Table [dbo].[SampleTableMaster_INT]    Script Date: 9/17/2019 4:43:31 PM ******/
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
/****** Object:  Table [dbo].[SampleTableMaster_INT_IDENTITY]    Script Date: 9/17/2019 4:43:31 PM ******/
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
/****** Object:  Table [dbo].[SampleTableMultipleKey]    Script Date: 9/17/2019 4:43:31 PM ******/
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
/****** Object:  Table [dbo].[TActionPermission]    Script Date: 9/17/2019 4:43:31 PM ******/
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
/****** Object:  Table [dbo].[TControllerRoleAccess]    Script Date: 9/17/2019 4:43:31 PM ******/
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
/****** Object:  Table [dbo].[TigaKey]    Script Date: 9/17/2019 4:43:31 PM ******/
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
/****** Object:  Table [dbo].[TPembelian]    Script Date: 9/17/2019 4:43:31 PM ******/
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
/****** Object:  Table [dbo].[TRefreshToken]    Script Date: 9/17/2019 4:43:31 PM ******/
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
/****** Object:  Table [dbo].[TToken]    Script Date: 9/17/2019 4:43:31 PM ******/
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
/****** Object:  Table [dbo].[TUserRole]    Script Date: 9/17/2019 4:43:31 PM ******/
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
	[CreatedBy] [varchar](50) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedHost] [varchar](50) NULL,
	[ModifiedBy] [varchar](50) NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[ModifiedHost] [varchar](50) NULL,
	[Flag] [int] NOT NULL,
 CONSTRAINT [PK_TUserRole] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
INSERT [dbo].[DUser] ([ID], [MUserID], [Name]) VALUES (1, 2, N'PakUlol')
GO
INSERT [dbo].[DUser] ([ID], [MUserID], [Name]) VALUES (2, 2, N'Pkethemat')
GO
INSERT [dbo].[MABP] ([ABPID], [Deskripsi], [PeriodeStart], [PeriodeEnd], [CreatedBy], [CreatedDate], [CreatedHost], [ModifiedBy], [ModifiedDate], [ModifiedHost], [Flag]) VALUES (N'AB-Plm-19', N'wsefrwerTwer', CAST(0x62400B00 AS Date), CAST(0x59400B00 AS Date), N' ', CAST(0x0000AAD900000000 AS DateTime), N' ', N' ', CAST(0x0000AAD900000000 AS DateTime), N' ', 0)
GO
INSERT [dbo].[MABP] ([ABPID], [Deskripsi], [PeriodeStart], [PeriodeEnd], [CreatedBy], [CreatedDate], [CreatedHost], [ModifiedBy], [ModifiedDate], [ModifiedHost], [Flag]) VALUES (N'AB-Glm-19', N'667T', CAST(0x34400B00 AS Date), CAST(0x34400B00 AS Date), N' ', CAST(0x0000AAD900000000 AS DateTime), N' ', N' ', CAST(0x0000AAD900000000 AS DateTime), N' ', 0)
GO
INSERT [dbo].[MABP] ([ABPID], [Deskripsi], [PeriodeStart], [PeriodeEnd], [CreatedBy], [CreatedDate], [CreatedHost], [ModifiedBy], [ModifiedDate], [ModifiedHost], [Flag]) VALUES (N'AB-3', N'TERKijang', CAST(0xF3340B00 AS Date), CAST(0xF3340B00 AS Date), N'', CAST(0x00009F9800000000 AS DateTime), N'', N'', CAST(0x00009F9800000000 AS DateTime), N'', 4)
GO
INSERT [dbo].[MABP] ([ABPID], [Deskripsi], [PeriodeStart], [PeriodeEnd], [CreatedBy], [CreatedDate], [CreatedHost], [ModifiedBy], [ModifiedDate], [ModifiedHost], [Flag]) VALUES (N'AB-4', N'TERRusa', CAST(0xF3340B00 AS Date), CAST(0xF3340B00 AS Date), N'', CAST(0x00009F9800000000 AS DateTime), N'', N'', CAST(0x00009F9800000000 AS DateTime), N'', 0)
GO
INSERT [dbo].[MABP] ([ABPID], [Deskripsi], [PeriodeStart], [PeriodeEnd], [CreatedBy], [CreatedDate], [CreatedHost], [ModifiedBy], [ModifiedDate], [ModifiedHost], [Flag]) VALUES (N'AB-5', N'TERKijang', CAST(0xF3340B00 AS Date), CAST(0xF3340B00 AS Date), N'', CAST(0x00009F9800000000 AS DateTime), N'', N'', CAST(0x00009F9800000000 AS DateTime), N'', 4)
GO
INSERT [dbo].[MABP] ([ABPID], [Deskripsi], [PeriodeStart], [PeriodeEnd], [CreatedBy], [CreatedDate], [CreatedHost], [ModifiedBy], [ModifiedDate], [ModifiedHost], [Flag]) VALUES (N'AB-6', N'TERKijangesrg', CAST(0xF3340B00 AS Date), CAST(0xF3340B00 AS Date), N'', CAST(0x00009F9800000000 AS DateTime), N'', N'', CAST(0x00009F9800000000 AS DateTime), N'', 4)
GO
INSERT [dbo].[MABP] ([ABPID], [Deskripsi], [PeriodeStart], [PeriodeEnd], [CreatedBy], [CreatedDate], [CreatedHost], [ModifiedBy], [ModifiedDate], [ModifiedHost], [Flag]) VALUES (N'AB-7', N'Terkasian', CAST(0xF3340B00 AS Date), CAST(0xF3340B00 AS Date), N'', CAST(0x00009F9800000000 AS DateTime), N'', N'', CAST(0x00009F9800000000 AS DateTime), N'', 4)
GO
INSERT [dbo].[MABP] ([ABPID], [Deskripsi], [PeriodeStart], [PeriodeEnd], [CreatedBy], [CreatedDate], [CreatedHost], [ModifiedBy], [ModifiedDate], [ModifiedHost], [Flag]) VALUES (N'AB-8', N'TERKijang', CAST(0xF3340B00 AS Date), CAST(0xF3340B00 AS Date), N'', CAST(0x00009F9800000000 AS DateTime), N'', N'', CAST(0x00009F9800000000 AS DateTime), N'', 4)
GO
INSERT [dbo].[MABP] ([ABPID], [Deskripsi], [PeriodeStart], [PeriodeEnd], [CreatedBy], [CreatedDate], [CreatedHost], [ModifiedBy], [ModifiedDate], [ModifiedHost], [Flag]) VALUES (N'AB-9', N'TERKijangju', CAST(0xF3340B00 AS Date), CAST(0xF3340B00 AS Date), N'', CAST(0x00009F9800000000 AS DateTime), N'', N'', CAST(0x00009F9800000000 AS DateTime), N'', 4)
GO
INSERT [dbo].[MABP] ([ABPID], [Deskripsi], [PeriodeStart], [PeriodeEnd], [CreatedBy], [CreatedDate], [CreatedHost], [ModifiedBy], [ModifiedDate], [ModifiedHost], [Flag]) VALUES (N'AB-10', N'UTERYDFRG', CAST(0xF3340B00 AS Date), CAST(0xF3340B00 AS Date), N'', CAST(0x00009F9800000000 AS DateTime), N'', N'', CAST(0x00009F9800000000 AS DateTime), N'', 0)
GO
INSERT [dbo].[MABP] ([ABPID], [Deskripsi], [PeriodeStart], [PeriodeEnd], [CreatedBy], [CreatedDate], [CreatedHost], [ModifiedBy], [ModifiedDate], [ModifiedHost], [Flag]) VALUES (N'AB-11', N'TERKijang', CAST(0xF3340B00 AS Date), CAST(0xF3340B00 AS Date), N'', CAST(0x00009F9800000000 AS DateTime), N'', N'', CAST(0x00009F9800000000 AS DateTime), N'', 4)
GO
INSERT [dbo].[MABP] ([ABPID], [Deskripsi], [PeriodeStart], [PeriodeEnd], [CreatedBy], [CreatedDate], [CreatedHost], [ModifiedBy], [ModifiedDate], [ModifiedHost], [Flag]) VALUES (N'AB-15', N'nuwinainuwinainuYHAAA', CAST(0xF3340B00 AS Date), CAST(0xF3340B00 AS Date), N'', CAST(0x00009F9800000000 AS DateTime), N'', N'rezas', CAST(0x0000AACB00ED2754 AS DateTime), N'::1', 0)
GO
SET IDENTITY_INSERT [dbo].[MKaryawan] ON 

GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (1, N'Jhon', 44, N'London', N'Staff', CAST(340000 AS Decimal(18, 0)), CAST(0x00008ECC00000000 AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (2, N'Titor', 33, N'Spain', N'Staff', CAST(200000 AS Decimal(18, 0)), CAST(0x00008ECC00000000 AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (3, N'asdasd', 21, N'Uruguay', N'Staff', CAST(340000 AS Decimal(18, 0)), CAST(0x000091A700000000 AS DateTime), 0)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (4, N'Ursa', 38, N'Radiant', NULL, CAST(29000 AS Decimal(18, 0)), CAST(0x000091A700000000 AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (5, N'BloodSeeker', 25, N'Dire', N'Staff', CAST(5999 AS Decimal(18, 0)), CAST(0x000091A700000000 AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (6, N'Tracsect', 25, N'Dire', N'Staff', CAST(5999 AS Decimal(18, 0)), CAST(0x000091A700000000 AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (7, N'Orb', 88, N'Radiant', N'Staff', CAST(88277 AS Decimal(18, 0)), CAST(0x00008ECC00000000 AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (10, N'Orb4', 88, N'Radiant', N'Staff', CAST(88277 AS Decimal(18, 0)), CAST(0x00008ECC00000000 AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (11, N'Orb5', 88, N'Radiant', N'Staff', CAST(88277 AS Decimal(18, 0)), CAST(0x00008ECC00000000 AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (12, N'Orb6', 88, N'Radiant', N'Staff', CAST(88277 AS Decimal(18, 0)), CAST(0x00008ECC00000000 AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (13, N'Cuka', 12, N'PT', N'Atas', CAST(0 AS Decimal(18, 0)), CAST(0x0000AA7700E7B8B2 AS DateTime), 0)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (14, N'Cuka', 12, N'PT', N'Atas', CAST(0 AS Decimal(18, 0)), CAST(0x0000AA7700EEB540 AS DateTime), 0)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (15, N'Cuka', 12, N'PT', N'Atas', CAST(0 AS Decimal(18, 0)), CAST(0x0000AA7700EF099B AS DateTime), 0)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (16, N'Cuka', 12, N'PT', N'Atas', CAST(0 AS Decimal(18, 0)), CAST(0x0000AA7700EFCFE8 AS DateTime), 0)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (17, N'Cuka', 12, N'PT', N'Atas', CAST(0 AS Decimal(18, 0)), CAST(0x0000AA7700F00900 AS DateTime), 0)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (18, N'ASDFQW', 978, N'LKHSDFL', N'kKJHKJH', CAST(8979821397 AS Decimal(18, 0)), CAST(0x0000AA77011A04E0 AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (19, N'ASDFQW', 978, N'LKHSDFL', N'kKJHKJH', CAST(8979821397 AS Decimal(18, 0)), CAST(0x0000AA77011A12F0 AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (1013, N'ASDFQW', 978, N'LKHSDFL', N'kKJHKJH', CAST(8979821397 AS Decimal(18, 0)), CAST(0x0000AA7800E1FE4C AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (1014, N'ASDFQW', 978, N'LKHSDFL', N'kKJHKJH', CAST(8979821397 AS Decimal(18, 0)), CAST(0x0000AA7800E202FC AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (1015, N'ASDFQW', 978, N'LKHSDFL', N'kKJHKJH', CAST(8979821397 AS Decimal(18, 0)), CAST(0x0000AA7800E237B8 AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (1016, N'ASDFQW', 978, N'LKHSDFL', NULL, CAST(8979821397 AS Decimal(18, 0)), CAST(0x0000AA7800E83A64 AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (1021, N'ASDFQW', 978, N'LKHSDFL', N'kKJHKJH', CAST(8979821397 AS Decimal(18, 0)), CAST(0x0000AA7800EF6334 AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (1022, N'ASDFQW', 978, N'LKHSDFL', N'kKJHKJH', CAST(8979821397 AS Decimal(18, 0)), CAST(0x0000AA7800F8B074 AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (1023, N'ASDFQW', 978, N'LKHSDFL', N'kKJHKJH', CAST(8979821397 AS Decimal(18, 0)), CAST(0x0000AA7800F8B2CC AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (1024, N'ASDFQW', 978, N'LKHSDFL', N'kKJHKJH', CAST(8979821397 AS Decimal(18, 0)), CAST(0x0000AA7800FA60E0 AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (1025, N'ASDFQW', 978, N'LKHSDFL', N'kKJHKJH', CAST(8979821397 AS Decimal(18, 0)), CAST(0x0000AA7800FA8E94 AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (1026, N'ASDFQW', 978, N'LKHSDFL', N'kKJHKJH', CAST(8979821397 AS Decimal(18, 0)), CAST(0x0000AA7800FA8E94 AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (1027, N'ASDFQW', 978, N'LKHSDFL', N'kKJHKJH', CAST(8979821397 AS Decimal(18, 0)), CAST(0x0000AA7800FA60E0 AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (1028, N'ASDFQW', 978, N'LKHSDFL', N'kKJHKJH', CAST(8979821397 AS Decimal(18, 0)), CAST(0x0000AA7800FA60E0 AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (1029, N'ASDFQW', 978, N'LKHSDFL', N'kKJHKJH', CAST(8979821397 AS Decimal(18, 0)), CAST(0x0000AA7800FC15FC AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (1030, N'ASDFQW', 978, N'LKHSDFL', N'kKJHKJH', CAST(8979821397 AS Decimal(18, 0)), CAST(0x0000AA7800FC1854 AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (1031, N'ASDFQW', 978, N'LKHSDFL', N'kKJHKJH', CAST(8979821397 AS Decimal(18, 0)), CAST(0x0000AA7800FC74E8 AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (1032, N'ASDFQW', 978, N'LKHSDFL', N'kKJHKJH', CAST(8979821397 AS Decimal(18, 0)), CAST(0x0000AA7800FCEFF4 AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (1033, N'ASDFQW', 978, N'LKHSDFL', N'kKJHKJH', CAST(8979821397 AS Decimal(18, 0)), CAST(0x0000AA7800FE2A04 AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (1034, N'ASDFQW', 978, N'LKHSDFL', N'kKJHKJH', CAST(8979821397 AS Decimal(18, 0)), CAST(0x0000AA7800FE88F0 AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (1035, N'ASDFQW', 978, N'LKHSDFL', N'kKJHKJH', CAST(8979821397 AS Decimal(18, 0)), CAST(0x0000AA7801027E9C AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (1036, N'ASDFQW', 978, N'LKHSDFL', N'kKJHKJH', CAST(8979821397 AS Decimal(18, 0)), CAST(0x0000AA780117B9C4 AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (1037, N'ASDFQW', 978, N'LKHSDFL', N'kKJHKJH', CAST(8979821397 AS Decimal(18, 0)), CAST(0x0000AA7801179318 AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (1038, N'ASDFQW', 978, N'LKHSDFL', N'kKJHKJH', CAST(8979821397 AS Decimal(18, 0)), CAST(0x0000AA7801194834 AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (1039, N'ASDFQW', 978, N'LKHSDFL', N'kKJHKJH', CAST(8979821397 AS Decimal(18, 0)), CAST(0x0000AA7900A8762C AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (1040, N'ASDFQW', 978, N'LKHSDFL', N'kKJHKJH', CAST(8979821397 AS Decimal(18, 0)), CAST(0x0000AA7900A75008 AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (1041, N'ASDFQW', 978, N'LKHSDFL', N'kKJHKJH', CAST(8979821397 AS Decimal(18, 0)), CAST(0x0000AA7900AA1504 AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (1042, N'ASDFQW', 978, N'LKHSDFL', N'kKJHKJH', CAST(8979821397 AS Decimal(18, 0)), CAST(0x0000AA7900AA6130 AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (1044, N'ASDFQW', 978, N'LKHSDFL', N'kKJHKJH', CAST(8979821397 AS Decimal(18, 0)), CAST(0x0000AA7900AAEEFC AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (1045, N'ASDFQW', 978, N'LKHSDFL', N'kKJHKJH', CAST(8979821397 AS Decimal(18, 0)), CAST(0x0000AA7900AD3B44 AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (1046, N'ASDFQW', 978, N'LKHSDFL', N'kKJHKJH', CAST(8979821397 AS Decimal(18, 0)), CAST(0x0000AA7A01161AEC AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (1047, N'ASDFQW', 978, N'LKHSDFL', N'kKJHKJH', CAST(8979821397 AS Decimal(18, 0)), CAST(0x0000AA7A01167D5C AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (1048, N'ASDFQW', 978, N'LKHSDFL', N'kKJHKJH', CAST(8979821397 AS Decimal(18, 0)), CAST(0x0000AA7A0116FAC0 AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (1049, N'ASDFQW', 978, N'LKHSDFL', N'kKJHKJH', CAST(8979821397 AS Decimal(18, 0)), CAST(0x0000AA7A0117D4B8 AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (1050, N'ASDFQW', 978, N'LKHSDFL', N'kKJHKJH', CAST(8979821397 AS Decimal(18, 0)), CAST(0x0000AA7A011900B8 AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (1051, N'ASDFQW', 978, N'LKHSDFL', N'kKJHKJH', CAST(8979821397 AS Decimal(18, 0)), CAST(0x0000AA7A011A7C68 AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (1052, N'ASDFQW', 978, N'LKHSDFL', N'kKJHKJH', CAST(8979821397 AS Decimal(18, 0)), CAST(0x0000AA7D00F5E470 AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (1053, N'ASDFQW', 978, N'LKHSDFL', N'kKJHKJH', CAST(8979821397 AS Decimal(18, 0)), CAST(0x0000AA7D00F74FB8 AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (1054, N'ASDFQW', 978, N'LKHSDFL', N'kKJHKJH', CAST(8979821397 AS Decimal(18, 0)), CAST(0x0000AA7D00F830B8 AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (1055, N'ASDFQW', 978, N'LKHSDFL', N'kKJHKJH', CAST(8979821397 AS Decimal(18, 0)), CAST(0x0000AA7D00F9A1DC AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (1056, N'ASDFQW', 978, N'LKHSDFL', N'kKJHKJH', CAST(8979821397 AS Decimal(18, 0)), CAST(0x0000AA7D00FB4438 AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (1057, N'ASDFQW', 978, N'LKHSDFL', N'kKJHKJH', CAST(8979821397 AS Decimal(18, 0)), CAST(0x0000AA7D00FC0B70 AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (1058, N'ASDFQW', 978, N'LKHSDFL', N'kKJHKJH', CAST(8979821397 AS Decimal(18, 0)), CAST(0x0000AA7D00FEFF4C AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (1059, N'ASDFQW', 978, N'LKHSDFL', N'kKJHKJH', CAST(8979821397 AS Decimal(18, 0)), CAST(0x0000AA7D00FF8E44 AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (1060, N'ASDFQW', 978, N'LKHSDFL', N'kKJHKJH', CAST(8979821397 AS Decimal(18, 0)), CAST(0x0000AA7D0101D000 AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (1061, N'ASDFQW', 978, N'LKHSDFL', N'kKJHKJH', CAST(8979821397 AS Decimal(18, 0)), CAST(0x0000AA7D010256C4 AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (2052, N'ASDFQW', 978, N'LKHSDFL', N'kKJHKJH', CAST(8979821397 AS Decimal(18, 0)), CAST(0x0000AA7E009F2FF4 AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (2053, N'ASDFQW', 978, N'LKHSDFL', N'kKJHKJH', CAST(8979821397 AS Decimal(18, 0)), CAST(0x0000AA7E00A0E18C AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (2054, N'ASDFQW', 978, N'LKHSDFL', N'kKJHKJH', CAST(8979821397 AS Decimal(18, 0)), CAST(0x0000AA7E00A215C0 AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (2055, N'ASDFQW', 978, N'LKHSDFL', N'kKJHKJH', CAST(8979821397 AS Decimal(18, 0)), CAST(0x0000AA7E00A474C8 AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (2056, N'ASDFQW', 978, N'LKHSDFL', N'kKJHKJH', CAST(8979821397 AS Decimal(18, 0)), CAST(0x0000AA7E00A55F28 AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (2057, N'ASDFQW', 978, N'LKHSDFL', N'kKJHKJH', CAST(8979821397 AS Decimal(18, 0)), CAST(0x0000AA7E00A902CC AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (2058, N'ASDFQW', 978, N'LKHSDFL', N'kKJHKJH', CAST(8979821397 AS Decimal(18, 0)), CAST(0x0000AA7E00A9FFEC AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (2059, N'ASDFQW', 978, N'LKHSDFL', N'kKJHKJH', CAST(8979821397 AS Decimal(18, 0)), CAST(0x0000AA7E00AA418C AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (2060, N'ASDFQW', 978, N'LKHSDFL', N'kKJHKJH', CAST(8979821397 AS Decimal(18, 0)), CAST(0x0000AA7E00ACBF0C AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (2061, N'ASDFQW', 978, N'LKHSDFL', N'kKJHKJH', CAST(8979821397 AS Decimal(18, 0)), CAST(0x0000AA7E00AD59BC AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (2062, N'ASDFQW', 978, N'LKHSDFL', N'kKJHKJH', CAST(8979821397 AS Decimal(18, 0)), CAST(0x0000AA7E00B08380 AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (2063, N'ASDFQW', 978, N'LKHSDFL', N'kKJHKJH', CAST(8979821397 AS Decimal(18, 0)), CAST(0x0000AA7E00B3C130 AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (2064, N'ASDFQW', 978, N'LKHSDFL', N'kKJHKJH', CAST(8979821397 AS Decimal(18, 0)), CAST(0x0000AA7E00B5D40C AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (2065, N'ASDFQW', 978, N'LKHSDFL', N'kKJHKJH', CAST(8979821397 AS Decimal(18, 0)), CAST(0x0000AA7E00B631CC AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (2066, N'ASDFQW', 978, N'LKHSDFL', N'kKJHKJH', CAST(8979821397 AS Decimal(18, 0)), CAST(0x0000AA7E00B6EFA4 AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (2067, N'ASDFQW', 978, N'LKHSDFL', N'kKJHKJH', CAST(8979821397 AS Decimal(18, 0)), CAST(0x0000AA7E00B6FDB4 AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (2068, N'ASDFQW', 978, N'LKHSDFL', N'kKJHKJH', CAST(8979821397 AS Decimal(18, 0)), CAST(0x0000AA7E00B81BA4 AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (2069, N'ASDFQW', 978, N'LKHSDFL', N'kKJHKJH', CAST(8979821397 AS Decimal(18, 0)), CAST(0x0000AA7E00F5463C AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (2070, N'ASDFQW', 978, N'LKHSDFL', N'kKJHKJH', CAST(8979821397 AS Decimal(18, 0)), CAST(0x0000AA7E00FAFF14 AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (2071, N'ASDFQW', 978, N'LKHSDFL', N'kKJHKJH', CAST(8979821397 AS Decimal(18, 0)), CAST(0x0000AA7E00FB69B8 AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (2072, N'ASDFQW', 978, N'LKHSDFL', N'kKJHKJH', CAST(8979821397 AS Decimal(18, 0)), CAST(0x0000AA7E00FD09BC AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (2073, N'ASDFQW', 978, N'LKHSDFL', N'kKJHKJH', CAST(8979821397 AS Decimal(18, 0)), CAST(0x0000AA7E00FE3814 AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (2074, N'ASDFQW', 978, N'LKHSDFL', N'kKJHKJH', CAST(8979821397 AS Decimal(18, 0)), CAST(0x0000AA7E00FEF5EC AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (2075, N'ASDFQW', 978, N'LKHSDFL', N'kKJHKJH', CAST(8979821397 AS Decimal(18, 0)), CAST(0x0000AA7E0100D1B4 AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (2076, N'ASDFQW', 978, N'LKHSDFL', N'kKJHKJH', CAST(8979821397 AS Decimal(18, 0)), CAST(0x0000AA7E01013A00 AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (2077, N'ASDFQW', 978, N'LKHSDFL', N'kKJHKJH', CAST(8979821397 AS Decimal(18, 0)), CAST(0x0000AA7E0101DF3C AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (2078, N'ASDFQW', 978, N'LKHSDFL', N'kKJHKJH', CAST(8979821397 AS Decimal(18, 0)), CAST(0x0000AA7E010285A4 AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (2079, N'ASDFQW', 978, N'LKHSDFL', N'kKJHKJH', CAST(8979821397 AS Decimal(18, 0)), CAST(0x0000AA7E0102D2FC AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (2080, N'ASDFQW', 978, N'LKHSDFL', N'kKJHKJH', CAST(8979821397 AS Decimal(18, 0)), CAST(0x0000AA7E010951E0 AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (2081, N'ASDFQW', 978, N'LKHSDFL', N'kKJHKJH', CAST(8979821397 AS Decimal(18, 0)), CAST(0x0000AA7E010973DC AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (2082, N'ASDFQW', 978, N'LKHSDFL', N'kKJHKJH', CAST(8979821397 AS Decimal(18, 0)), CAST(0x0000AA7E010A1468 AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (2083, N'ASDFQW', 978, N'LKHSDFL', N'kKJHKJH', CAST(8979821397 AS Decimal(18, 0)), CAST(0x0000AA7F008D1D64 AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (2084, N'ASDFQW', 978, N'LKHSDFL', N'kKJHKJH', CAST(8979821397 AS Decimal(18, 0)), CAST(0x0000AA7F008E8B04 AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (2085, N'ASDFQW', 978, N'LKHSDFL', N'kKJHKJH', CAST(8979821397 AS Decimal(18, 0)), CAST(0x0000AA7F009110B8 AS DateTime), 1)
GO
INSERT [dbo].[MKaryawan] ([ID], [Name], [Age], [Office], [Position], [Salary], [StartDate], [Active]) VALUES (2086, N'ASDFQW', 978, N'LKHSDFL', N'kKJHKJH', CAST(8979821397 AS Decimal(18, 0)), CAST(0x0000AA7F0091CFBC AS DateTime), 1)
GO
SET IDENTITY_INSERT [dbo].[MKaryawan] OFF
GO
INSERT [dbo].[MMenu] ([ID], [ParentID], [MenuName], [Path]) VALUES (N'MN1', N'-', N'mENU uPDATE', N'/update')
GO
INSERT [dbo].[MMenu] ([ID], [ParentID], [MenuName], [Path]) VALUES (N'MN2', N'-', N'mENU 6666', N'/update')
GO
INSERT [dbo].[MRole] ([RoleID], [Descriptions]) VALUES (N'ADM', N'admincoy')
GO
INSERT [dbo].[MRole] ([RoleID], [Descriptions]) VALUES (N'AMR', N'hsdfuhii')
GO
INSERT [dbo].[MRole] ([RoleID], [Descriptions]) VALUES (N'SDF', N'sdrfgawertg23')
GO
INSERT [dbo].[MRole] ([RoleID], [Descriptions]) VALUES (N'SSD', N'dssfsdf')
GO
INSERT [dbo].[MRole] ([RoleID], [Descriptions]) VALUES (N'STP', N'68uuut')
GO
INSERT [dbo].[MRole] ([RoleID], [Descriptions]) VALUES (N'ULI', N'DFDFFDG')
GO
INSERT [dbo].[MRole] ([RoleID], [Descriptions]) VALUES (N'WER', N'DFDFFDG')
GO
INSERT [dbo].[MUser] ([Username], [Password], [IsActive], [IsLocked], [Email], [FullName], [CreatedBy], [CreatedDate], [CreatedHost], [ModifiedBy], [ModifiedDate], [ModifiedHost], [Flag]) VALUES (N'radito', N'9dUdulCZV/zPjXG/F40XmmnJepe/ISWqceNl4Y2wddrx4Jjw', 1, 0, N'rd@sadk.asd', N'Radito', N'rezas', CAST(0x0000AACB00EC4528 AS DateTime), N'::1', N'rezas', CAST(0x0000AACB00EC4528 AS DateTime), N'::1', 0)
GO
INSERT [dbo].[MUser] ([Username], [Password], [IsActive], [IsLocked], [Email], [FullName], [CreatedBy], [CreatedDate], [CreatedHost], [ModifiedBy], [ModifiedDate], [ModifiedHost], [Flag]) VALUES (N'rezas', N'xs22jN/tAYYoqHZ6VerG/nE6mvCBW/oEZYAvaB15NZ0VKcPl', 1, 0, N'rezaseptiandra@gmail.com', N'Reza Septiandra', N'', CAST(0x0000AACB00BDC540 AS DateTime), N'', N'', CAST(0x0000AACB00BDC540 AS DateTime), N'', 0)
GO
INSERT [dbo].[SampleTableMaster] ([IDSTR], [NAME]) VALUES (N'1', N'UPDATED')
GO
INSERT [dbo].[SampleTableMaster] ([IDSTR], [NAME]) VALUES (N'3333', N'4444')
GO
INSERT [dbo].[SampleTableMaster] ([IDSTR], [NAME]) VALUES (N'a89a', N'las9j')
GO
INSERT [dbo].[SampleTableMaster] ([IDSTR], [NAME]) VALUES (N'asdklj9', N'iasd98')
GO
INSERT [dbo].[SampleTableMaster] ([IDSTR], [NAME]) VALUES (N'dfkjk', N'kjh')
GO
INSERT [dbo].[SampleTableMaster] ([IDSTR], [NAME]) VALUES (N'dsfhj', N'khsdf')
GO
INSERT [dbo].[SampleTableMaster] ([IDSTR], [NAME]) VALUES (N'fdgdsrg', N'34kj')
GO
INSERT [dbo].[SampleTableMaster] ([IDSTR], [NAME]) VALUES (N'jhhf', N'masok ndak CUHOYs')
GO
INSERT [dbo].[SampleTableMaster] ([IDSTR], [NAME]) VALUES (N'jhhfk', N'masok ndak CUHOYs')
GO
INSERT [dbo].[SampleTableMaster] ([IDSTR], [NAME]) VALUES (N'jksdi', N'kjsadfk')
GO
INSERT [dbo].[SampleTableMaster] ([IDSTR], [NAME]) VALUES (N'khsdf99', N'ulolka')
GO
INSERT [dbo].[SampleTableMaster] ([IDSTR], [NAME]) VALUES (N'lkas888', N'isdajljd')
GO
INSERT [dbo].[SampleTableMaster] ([IDSTR], [NAME]) VALUES (N'OIASDJH', N'ulolka')
GO
INSERT [dbo].[SampleTableMaster] ([IDSTR], [NAME]) VALUES (N'updateTEST2', NULL)
GO
INSERT [dbo].[SampleTableMaster_INT] ([IDINT], [NAME]) VALUES (1, N'UPDATED')
GO
INSERT [dbo].[SampleTableMaster_INT] ([IDINT], [NAME]) VALUES (8, N'Keterangan1')
GO
INSERT [dbo].[SampleTableMaster_INT] ([IDINT], [NAME]) VALUES (9, N'DSFKHDS')
GO
INSERT [dbo].[SampleTableMaster_INT] ([IDINT], [NAME]) VALUES (11, N'dsfsdf')
GO
INSERT [dbo].[SampleTableMaster_INT] ([IDINT], [NAME]) VALUES (83, N'lkjfd')
GO
INSERT [dbo].[SampleTableMaster_INT] ([IDINT], [NAME]) VALUES (99, N'sdfsf')
GO
INSERT [dbo].[SampleTableMaster_INT] ([IDINT], [NAME]) VALUES (413, N'dghdtrh')
GO
INSERT [dbo].[SampleTableMaster_INT] ([IDINT], [NAME]) VALUES (832, N'cfsdfsa')
GO
INSERT [dbo].[SampleTableMaster_INT] ([IDINT], [NAME]) VALUES (899, N'Keterangan2')
GO
INSERT [dbo].[SampleTableMaster_INT] ([IDINT], [NAME]) VALUES (2234, N'sdgsdg')
GO
INSERT [dbo].[SampleTableMaster_INT] ([IDINT], [NAME]) VALUES (6333, N'asfafsd')
GO
INSERT [dbo].[SampleTableMaster_INT] ([IDINT], [NAME]) VALUES (8568, N'dffdf')
GO
INSERT [dbo].[SampleTableMaster_INT] ([IDINT], [NAME]) VALUES (11122, N'dsfsdfds')
GO
INSERT [dbo].[SampleTableMaster_INT] ([IDINT], [NAME]) VALUES (14353, N'ulolka')
GO
INSERT [dbo].[SampleTableMaster_INT] ([IDINT], [NAME]) VALUES (77876, N'ulolka')
GO
INSERT [dbo].[SampleTableMaster_INT] ([IDINT], [NAME]) VALUES (778876, N'ulolka')
GO
SET IDENTITY_INSERT [dbo].[SampleTableMaster_INT_IDENTITY] ON 

GO
INSERT [dbo].[SampleTableMaster_INT_IDENTITY] ([IDINT], [NAME]) VALUES (1, N'UPDATED')
GO
INSERT [dbo].[SampleTableMaster_INT_IDENTITY] ([IDINT], [NAME]) VALUES (2, N'updated')
GO
INSERT [dbo].[SampleTableMaster_INT_IDENTITY] ([IDINT], [NAME]) VALUES (3, N'fxdf')
GO
INSERT [dbo].[SampleTableMaster_INT_IDENTITY] ([IDINT], [NAME]) VALUES (4, N'gg')
GO
INSERT [dbo].[SampleTableMaster_INT_IDENTITY] ([IDINT], [NAME]) VALUES (5, N'fsdfeee')
GO
INSERT [dbo].[SampleTableMaster_INT_IDENTITY] ([IDINT], [NAME]) VALUES (6, N'gfjfgj')
GO
INSERT [dbo].[SampleTableMaster_INT_IDENTITY] ([IDINT], [NAME]) VALUES (7, N'ewrtert')
GO
INSERT [dbo].[SampleTableMaster_INT_IDENTITY] ([IDINT], [NAME]) VALUES (8, N'sarw')
GO
INSERT [dbo].[SampleTableMaster_INT_IDENTITY] ([IDINT], [NAME]) VALUES (9, N'ffashfs')
GO
INSERT [dbo].[SampleTableMaster_INT_IDENTITY] ([IDINT], [NAME]) VALUES (10, N'rqqqqwew')
GO
INSERT [dbo].[SampleTableMaster_INT_IDENTITY] ([IDINT], [NAME]) VALUES (11, N'ffasf')
GO
INSERT [dbo].[SampleTableMaster_INT_IDENTITY] ([IDINT], [NAME]) VALUES (12, N'ewrqwrxzvzx')
GO
INSERT [dbo].[SampleTableMaster_INT_IDENTITY] ([IDINT], [NAME]) VALUES (13, N'fsasf')
GO
INSERT [dbo].[SampleTableMaster_INT_IDENTITY] ([IDINT], [NAME]) VALUES (14, N'Testulolka')
GO
SET IDENTITY_INSERT [dbo].[SampleTableMaster_INT_IDENTITY] OFF
GO
INSERT [dbo].[SampleTableMultipleKey] ([ID1], [ID2], [ID3], [NAME]) VALUES (1, 1, N'1', N'UPDATED')
GO
INSERT [dbo].[SampleTableMultipleKey] ([ID1], [ID2], [ID3], [NAME]) VALUES (1, 1, N'2', N'Keterangan2')
GO
INSERT [dbo].[SampleTableMultipleKey] ([ID1], [ID2], [ID3], [NAME]) VALUES (1, 1, N'233', N'fdsdgswg')
GO
INSERT [dbo].[SampleTableMultipleKey] ([ID1], [ID2], [ID3], [NAME]) VALUES (1, 1, N'3', N'sdfwfw')
GO
INSERT [dbo].[SampleTableMultipleKey] ([ID1], [ID2], [ID3], [NAME]) VALUES (1, 1, N'4', N'fgsgeewg')
GO
INSERT [dbo].[SampleTableMultipleKey] ([ID1], [ID2], [ID3], [NAME]) VALUES (1, 1, N'5', N'asdfqgfq')
GO
INSERT [dbo].[SampleTableMultipleKey] ([ID1], [ID2], [ID3], [NAME]) VALUES (1, 1, N'6', N'asdadad')
GO
INSERT [dbo].[SampleTableMultipleKey] ([ID1], [ID2], [ID3], [NAME]) VALUES (9, 77, N'asd', N'asdasd')
GO
INSERT [dbo].[SampleTableMultipleKey] ([ID1], [ID2], [ID3], [NAME]) VALUES (9, 777, N'asd', N'asdasd')
GO
INSERT [dbo].[SampleTableMultipleKey] ([ID1], [ID2], [ID3], [NAME]) VALUES (89, 777, N'asd', N'asdasd')
GO
INSERT [dbo].[SampleTableMultipleKey] ([ID1], [ID2], [ID3], [NAME]) VALUES (32423, 1, N'1', N'asfqwfqwf')
GO
SET IDENTITY_INSERT [dbo].[TActionPermission] ON 

GO
INSERT [dbo].[TActionPermission] ([ID], [ControllerName], [ActionName], [RequiredCreate], [RequiredEdit], [RequiredDelete], [RequiredView]) VALUES (1007, N'AccountController', N'DeleteAccount', 0, 0, 1, 0)
GO
INSERT [dbo].[TActionPermission] ([ID], [ControllerName], [ActionName], [RequiredCreate], [RequiredEdit], [RequiredDelete], [RequiredView]) VALUES (1008, N'AccountController', N'Index', 0, 0, 1, 0)
GO
INSERT [dbo].[TActionPermission] ([ID], [ControllerName], [ActionName], [RequiredCreate], [RequiredEdit], [RequiredDelete], [RequiredView]) VALUES (1009, N'AccountController', N'SubmitDelete', 0, 0, 0, 0)
GO
INSERT [dbo].[TActionPermission] ([ID], [ControllerName], [ActionName], [RequiredCreate], [RequiredEdit], [RequiredDelete], [RequiredView]) VALUES (1010, N'ABPController', N'Index', 0, 0, 0, 1)
GO
INSERT [dbo].[TActionPermission] ([ID], [ControllerName], [ActionName], [RequiredCreate], [RequiredEdit], [RequiredDelete], [RequiredView]) VALUES (1011, N'ABPController', N'GetList', 0, 0, 0, 1)
GO
INSERT [dbo].[TActionPermission] ([ID], [ControllerName], [ActionName], [RequiredCreate], [RequiredEdit], [RequiredDelete], [RequiredView]) VALUES (1012, N'ABPController', N'Save', 0, 0, 0, 1)
GO
INSERT [dbo].[TActionPermission] ([ID], [ControllerName], [ActionName], [RequiredCreate], [RequiredEdit], [RequiredDelete], [RequiredView]) VALUES (1013, N'ABPController', N'SubmitDelete', 0, 0, 0, 1)
GO
INSERT [dbo].[TActionPermission] ([ID], [ControllerName], [ActionName], [RequiredCreate], [RequiredEdit], [RequiredDelete], [RequiredView]) VALUES (1014, N'ABPController', N'Detail', 0, 0, 0, 1)
GO
SET IDENTITY_INSERT [dbo].[TActionPermission] OFF
GO
SET IDENTITY_INSERT [dbo].[TControllerRoleAccess] ON 

GO
INSERT [dbo].[TControllerRoleAccess] ([ID], [ControllerName], [RoleID], [AllowCreate], [AllowEdit], [AllowDelete], [AllowView]) VALUES (1, N'AccountController', N'STP', 0, 0, 1, 1)
GO
INSERT [dbo].[TControllerRoleAccess] ([ID], [ControllerName], [RoleID], [AllowCreate], [AllowEdit], [AllowDelete], [AllowView]) VALUES (3, N'AccountController', N'WER', 0, 0, 1, 0)
GO
INSERT [dbo].[TControllerRoleAccess] ([ID], [ControllerName], [RoleID], [AllowCreate], [AllowEdit], [AllowDelete], [AllowView]) VALUES (6, N'ABPController', N'ULI', 0, 0, 0, 1)
GO
SET IDENTITY_INSERT [dbo].[TControllerRoleAccess] OFF
GO
INSERT [dbo].[TigaKey] ([ID1], [ID2], [ID3], [Deskripsi], [Dates], [IsActive], [Gaji]) VALUES (1, 2, 3, N'', CAST(0x0000AA770118C4F4 AS DateTime), 1, NULL)
GO
INSERT [dbo].[TigaKey] ([ID1], [ID2], [ID3], [Deskripsi], [Dates], [IsActive], [Gaji]) VALUES (4, 2, 3, N'', CAST(0x0000AA7900AE1D70 AS DateTime), 1, CAST(3453454325.88 AS Decimal(18, 2)))
GO
INSERT [dbo].[TigaKey] ([ID1], [ID2], [ID3], [Deskripsi], [Dates], [IsActive], [Gaji]) VALUES (5, 2, 3, N'', CAST(0x0000AA7800DDF25C AS DateTime), 1, CAST(3453454325.88 AS Decimal(18, 2)))
GO
INSERT [dbo].[TigaKey] ([ID1], [ID2], [ID3], [Deskripsi], [Dates], [IsActive], [Gaji]) VALUES (57, 2, 3, N'', CAST(0x0000AA7900B2A340 AS DateTime), 1, CAST(3453454325.88 AS Decimal(18, 2)))
GO
INSERT [dbo].[TigaKey] ([ID1], [ID2], [ID3], [Deskripsi], [Dates], [IsActive], [Gaji]) VALUES (77, 2, 3, N'', CAST(0x0000AA7900B25A98 AS DateTime), 1, CAST(3453454325.88 AS Decimal(18, 2)))
GO
INSERT [dbo].[TigaKey] ([ID1], [ID2], [ID3], [Deskripsi], [Dates], [IsActive], [Gaji]) VALUES (112, 2, 3, N'', CAST(0x0000AA7900B30A60 AS DateTime), 1, CAST(3453454325.88 AS Decimal(18, 2)))
GO
INSERT [dbo].[TigaKey] ([ID1], [ID2], [ID3], [Deskripsi], [Dates], [IsActive], [Gaji]) VALUES (112, 23, 3, N'', CAST(0x0000AA7900B40078 AS DateTime), 1, CAST(3453454325.88 AS Decimal(18, 2)))
GO
INSERT [dbo].[TigaKey] ([ID1], [ID2], [ID3], [Deskripsi], [Dates], [IsActive], [Gaji]) VALUES (112, 23, 34, N'', CAST(0x0000AA7900B43D68 AS DateTime), 1, CAST(3453454325.88 AS Decimal(18, 2)))
GO
INSERT [dbo].[TRefreshToken] ([userid], [refreshtoken]) VALUES (N'user', N'6c3629ab-a8d0-437b-81d0-aa7dfd3effd4')
GO
INSERT [dbo].[TRefreshToken] ([userid], [refreshtoken]) VALUES (N'user', N'6c3629ab-a8d0-437b-81d0-aa7dfd3effd4')
GO
INSERT [dbo].[TToken] ([ID], [UserID], [RoleID], [Token], [RefreshToken], [IsActive], [ModifiedDate]) VALUES (N'1', 1, N'ADM', NULL, NULL, 0, CAST(0x0000AAE200000000 AS DateTime))
GO
INSERT [dbo].[TToken] ([ID], [UserID], [RoleID], [Token], [RefreshToken], [IsActive], [ModifiedDate]) VALUES (N'1', 1, N'ADM', NULL, NULL, 0, CAST(0x0000AAE200000000 AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[TUserRole] ON 

GO
INSERT [dbo].[TUserRole] ([ID], [Username], [RoleID], [CreatedBy], [CreatedDate], [CreatedHost], [ModifiedBy], [ModifiedDate], [ModifiedHost], [Flag]) VALUES (1, N'rezas', N'ADM', N' ', CAST(0x0000A9C800000000 AS DateTime), N' ', N' ', CAST(0x0000A9C800000000 AS DateTime), N' ', 0)
GO
INSERT [dbo].[TUserRole] ([ID], [Username], [RoleID], [CreatedBy], [CreatedDate], [CreatedHost], [ModifiedBy], [ModifiedDate], [ModifiedHost], [Flag]) VALUES (2, N'rezas', N'ULI', N'', CAST(0x0000AACB00BDE73C AS DateTime), N'', N'', CAST(0x0000AACB00BDE73C AS DateTime), N'', 0)
GO
INSERT [dbo].[TUserRole] ([ID], [Username], [RoleID], [CreatedBy], [CreatedDate], [CreatedHost], [ModifiedBy], [ModifiedDate], [ModifiedHost], [Flag]) VALUES (3, N'rezas', N'AMR', N'', CAST(0x0000AACB00BDEBEC AS DateTime), N'', N'', CAST(0x0000AACB00BDEBEC AS DateTime), N'', 0)
GO
INSERT [dbo].[TUserRole] ([ID], [Username], [RoleID], [CreatedBy], [CreatedDate], [CreatedHost], [ModifiedBy], [ModifiedDate], [ModifiedHost], [Flag]) VALUES (4, N'radito', N'ULI', N'rezas', CAST(0x0000AACB00EC56BC AS DateTime), N'::1', N'rezas', CAST(0x0000AACB00EC56BC AS DateTime), N'::1', 0)
GO
INSERT [dbo].[TUserRole] ([ID], [Username], [RoleID], [CreatedBy], [CreatedDate], [CreatedHost], [ModifiedBy], [ModifiedDate], [ModifiedHost], [Flag]) VALUES (5, N'radito', N'AMR', N'rezas', CAST(0x0000AACB00EC5914 AS DateTime), N'::1', N'rezas', CAST(0x0000AACB00EC5914 AS DateTime), N'::1', 0)
GO
SET IDENTITY_INSERT [dbo].[TUserRole] OFF
GO
