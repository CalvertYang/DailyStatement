
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		AndyYou
-- Create date: 2013/04/15
-- Description:	The DailyStatement system that employee's daily records need 'WINTRISS' record every day.
--				so, system will help everyone create a record everyday and alert user need to modify.
-- =============================================
IF OBJECT_ID('insert_wintriss_everyday') IS NOT NULL
	DROP Procedure insert_wintriss_everyday
GO
CREATE PROCEDURE insert_wintriss_everyday
AS
BEGIN
INSERT INTO [dbo].[DailyInfoes]
    ([WorkCategoryId], [WorkContent], [CreateDate], [EmployeeId], [WorkingHours], [Project_ProjectId])
SELECT
    10, 'N/A', Current_Timestamp, [EmployeeId], 0, 2
FROM [dbo].[Employees]
WHERE [EmployeeId] > 1;
END
GO
