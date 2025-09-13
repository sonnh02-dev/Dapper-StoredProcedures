CREATE PROCEDURE sp_GetProductsByFilterAndSort
    @CategoryId INT = NULL,             -- lọc theo CategoryId (nếu NULL thì lấy tất cả)
    @SKU NVARCHAR(100) = NULL,          -- lọc theo tên sản phẩm (LIKE)
    @SortBy NVARCHAR(50) = 'SoldTotal', -- cột sắp xếp mặc định = SoldTotal
    @SortDirection NVARCHAR(4) = 'DESC',-- chiều sắp xếp mặc định = DESC
    @PageIndex INT = 1,                 -- trang hiện tại
    @PageSize INT = 10                  -- số dòng trên 1 trang
AS
BEGIN
    SET NOCOUNT ON;

    -------------------------------------------------------
    -- CTE 1: Tính tổng số lượng đã bán (tất cả thời gian)
    -------------------------------------------------------
    ;WITH Sold AS (
        SELECT 
            oi.ProductId,
            SUM(oi.Quantity) AS SoldTotal
        FROM OrderItems oi
        INNER JOIN Orders o ON oi.OrderId = o.Id
        INNER JOIN Payments p ON p.OrderId = o.Id
        WHERE p.Status = 'Completed'
        GROUP BY oi.ProductId
    ),
    -------------------------------------------------------
    -- CTE 2: Tính số lượng đã bán trong 12 tháng gần nhất
    -------------------------------------------------------
    MonthlySold AS (
        SELECT 
            oi.ProductId,
            SUM(oi.Quantity) AS QuantitySold
        FROM OrderItems oi
        INNER JOIN Orders o ON oi.OrderId = o.Id
        INNER JOIN Payments p ON p.OrderId = o.Id
        WHERE p.Status = 'Completed'
          AND o.OrderDate >= DATEADD(MONTH, -12, GETDATE())
        GROUP BY oi.ProductId
    ),
    -------------------------------------------------------
    -- CTE 3: Ghép dữ liệu sản phẩm + Sold + MonthlySold
    --        Tính trạng thái tồn kho + AvgSoldPerMonth
    -------------------------------------------------------
    ProductWithStats AS (
        SELECT 
            pr.Id AS ProductId,
            pr.Name,
            pr.Price,
            COALESCE(s.SoldTotal,0) AS SoldTotal,  -- tổng số đã bán
            pr.Quantity,                            -- số lượng tồn kho ban đầu
            CASE 
                WHEN COALESCE(s.SoldTotal,0) >= pr.Quantity THEN 'Out of Stock'
                ELSE 'In Stock'
            END AS StockStatus,
            pr.CategoryId,
            ca.Name AS CategoryName,
            COALESCE(ms.QuantitySold,0)/12.0 AS AvgSoldPerMonth
        FROM Products pr
        LEFT JOIN Sold s ON pr.Id = s.ProductId
        LEFT JOIN MonthlySold ms ON pr.Id = ms.ProductId
        INNER JOIN Categories ca ON pr.CategoryId = ca.Id
        WHERE (@CategoryId IS NULL OR pr.CategoryId = @CategoryId)
          AND (@SKU IS NULL OR pr.Name LIKE '%' + @SKU + '%')
    )

    -------------------------------------------------------
    -- Chọn dữ liệu cuối cùng với OFFSET/FETCH
    -------------------------------------------------------
    SELECT *
    FROM ProductWithStats
    ORDER BY
        CASE WHEN @SortBy = 'SoldTotal' AND @SortDirection = 'ASC'  THEN SoldTotal END ASC,
        CASE WHEN @SortBy = 'SoldTotal' AND @SortDirection = 'DESC' THEN SoldTotal END DESC,
        CASE WHEN @SortBy = 'AvgSoldPerMonth' AND @SortDirection = 'ASC'  THEN AvgSoldPerMonth END ASC,
        CASE WHEN @SortBy = 'AvgSoldPerMonth' AND @SortDirection = 'DESC' THEN AvgSoldPerMonth END DESC,
        CASE WHEN @SortBy = 'Price' AND @SortDirection = 'ASC'  THEN Price END ASC,
        CASE WHEN @SortBy = 'Price' AND @SortDirection = 'DESC' THEN Price END DESC
    OFFSET (@PageIndex - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END
