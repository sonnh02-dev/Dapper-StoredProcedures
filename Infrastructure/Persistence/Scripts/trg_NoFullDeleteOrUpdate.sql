DECLARE @tbl NVARCHAR(128), @pk NVARCHAR(128), @sql NVARCHAR(MAX);

-- Cursor duyệt qua tất cả bảng có Primary Key
DECLARE cur CURSOR FOR
SELECT t.name AS TableName, c.name AS PKName
FROM sys.tables t
JOIN sys.indexes i ON t.object_id = i.object_id AND i.is_primary_key = 1
JOIN sys.index_columns ic ON i.object_id = ic.object_id AND i.index_id = ic.index_id
JOIN sys.columns c ON ic.object_id = c.object_id AND ic.column_id = c.column_id
ORDER BY t.name;

OPEN cur;
FETCH NEXT FROM cur INTO @tbl, @pk;

WHILE @@FETCH_STATUS = 0
BEGIN
    SET @sql = N'
    IF OBJECT_ID(''trg_NoFullDeleteOrUpdate_' + @tbl + ''') IS NOT NULL
        DROP TRIGGER trg_NoFullDeleteOrUpdate_' + @tbl + ';
    
    EXEC(''CREATE TRIGGER trg_NoFullDeleteOrUpdate_' + @tbl + '
    ON ' + @tbl + '
    AFTER DELETE, UPDATE
    AS
    BEGIN
        SET NOCOUNT ON;

        -- Nếu không có dòng nào bị xóa/ghi thì thoát
        IF NOT EXISTS(SELECT 1 FROM deleted) RETURN;

        -- Nếu sau DELETE mà bảng trống => DELETE toàn bảng
        IF NOT EXISTS(SELECT 1 FROM ' + @tbl + ')
        BEGIN
            RAISERROR (''''Cấm DELETE toàn bảng, cần WHERE!'''', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END

        -- Nếu UPDATE toàn bảng: mọi PK hiện tại đều có trong deleted
        IF NOT EXISTS (
            SELECT ' + @pk + ' FROM ' + @tbl + '
            EXCEPT
            SELECT ' + @pk + ' FROM deleted
        )
        BEGIN
            RAISERROR (''''Cấm UPDATE toàn bảng, cần WHERE!'''', 16, 1);
            ROLLBACK TRANSACTION;
        END
    END'');
    ';

    EXEC sp_executesql @sql;

    FETCH NEXT FROM cur INTO @tbl, @pk;
END

CLOSE cur;
DEALLOCATE cur;
