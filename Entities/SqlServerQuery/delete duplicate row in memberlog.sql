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
       Conic_Erp.dbo.[MemberLog]
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
        Conic_Erp.dbo.Massage
)
DELETE FROM msg
WHERE row_num > 1;