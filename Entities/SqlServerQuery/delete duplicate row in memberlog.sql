WITH cte AS (
    SELECT 
       ID ,
        ROW_NUMBER() OVER (
            PARTITION BY 
                MemberID, 
                DateTime
            ORDER BY 
                MemberID, 
                DateTime
        ) row_num
     FROM 
        [Gym].[Gym].[MemberLog]
)
DELETE FROM cte
WHERE row_num > 1;

WITH msg AS (
    SELECT 
       ID ,
        ROW_NUMBER() OVER (
            PARTITION BY 
                TableName,
      FKTable, 
                SendDate
            ORDER BY 
                TableName,
      FKTable, 
                SendDate
        ) row_num
     FROM 
        [Gym].Config.Massage
)
DELETE FROM msg
WHERE row_num > 1;