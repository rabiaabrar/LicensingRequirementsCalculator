
/*
-- This is the table in which I loaded the csv file

	CREATE TABLE [dbo].[Installations](
		[ComputerId] [int] NOT NULL,
		[UserId] [int] NOT NULL,
		[ApplicationId] [int] NOT NULL,
		[ComputerType] [varchar](8) NOT NULL,
		[Comment] [varchar](50) NOT NULL
	) ON [PRIMARY]
*/

-- This is the query to Calculate Licensing Requirements
SELECT		ApplicationId,
			[DESKTOP] AS Desktops, 
			[LAPTOP] AS Laptops,
			(CASE 
				WHEN [DESKTOP]>=[LAPTOP] THEN [DESKTOP] 
				WHEN ([LAPTOP]-[DESKTOP])%2 = 0 THEN [DESKTOP]+([LAPTOP]-[DESKTOP])/2
				ELSE [DESKTOP]+([LAPTOP]+1-[DESKTOP])/2 
			END) AS NumberOfLicenses
FROM
(
   SELECT DISTINCT ApplicationId, ComputerType, ComputerId
   FROM Installations
) inst
PIVOT 
(
  COUNT(ComputerId) 
  FOR ComputerType IN ([DESKTOP], [LAPTOP])
) AS computers;