-- 为ExternalProcessingApplications表添加TotalQuantity字段
IF NOT EXISTS (
    SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_NAME = 'ExternalProcessingApplications' 
    AND COLUMN_NAME = 'TotalQuantity'
)
BEGIN
    ALTER TABLE ExternalProcessingApplications 
    ADD TotalQuantity INT DEFAULT 0;
    
    PRINT 'TotalQuantity字段已添加成功';
END
ELSE
BEGIN
    PRINT 'TotalQuantity字段已存在';
END
GO

-- 验证字段是否添加成功
SELECT COLUMN_NAME, DATA_TYPE, IS_NULLABLE, COLUMN_DEFAULT
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'ExternalProcessingApplications'
AND COLUMN_NAME = 'TotalQuantity';
