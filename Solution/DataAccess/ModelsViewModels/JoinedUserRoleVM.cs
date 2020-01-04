using System;
using System.Collections.Generic;
using Dapper;
using System.Linq;
using DataAccess.Repository;
using DataAccess.Interface;

namespace DataAccess.ModelsViewModels
{
    /*
     *
  RoleID Descriptions
    ADM	admincoy
    AMR	hsdfuhii
    SDF	sdrfgawertg23
    SSD	dssfsdf
    STP	68uuut
    ULI	DFDFFDG
    WER	DFDFFDG
     *
     *
     * 
     * 
     * 
    USE [REZADB]

    SET ANSI_NULLS ON
    GO
    SET QUOTED_IDENTIFIER OFF
    GO
    -- =============================================
    -- Author:		Reza
    -- Create date: 
    -- Description:	
    -- =============================================
    CREATE PROCEDURE[dbo].[JoinedUserRoleVM_SP_SELECT]
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
                        + @groupby + ")";

        IF(@iscount= 1)

            SET @QUERY = @QUERY + " SELECT COUNT(*) FROM CTE_";
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


        IF(@isDebug= 0)

            EXEC(@QUERY);
        ELSE
            SELECT @QUERY;
    END
     */
    public class JoinedUserRoleVM
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string RoleID { get; set; }
        public string Descriptions { get; set; }
        private static string SP_SELECT { get => "[JoinedUserRoleVM_SP_SELECT]"; }
    }

    
    public class JoinedUserRoleVMRPO : ViewDataAccess<JoinedUserRoleVM>
    {
        public JoinedUserRoleVMRPO(IMapper _mpp)
        {
            base.Initialize(_mpp);
        }
    }
}
