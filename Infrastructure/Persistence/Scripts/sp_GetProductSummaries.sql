CREATE PROCEDURE sp_GetProductSummaries
    @CategoryId INT = NULL,
    @SKU NVARCHAR(100) = NULL,
    @SortBy NVARCHAR(50) = 'SoldDisplay', 
    @SortDirection NVARCHAR(4) = 'DESC'
AS
BEGIN
    SET NOCOUNT ON;

    WITH Sold AS (
        SELECT 
            oi.ProductId,
            SUM(oi.Quantity) AS TotalBuy
        FROM OrderItems oi
        INNER JOIN Orders o ON oi.OrderId = o.Id
        INNER JOIN Payments p ON p.OrderId = o.Id
        WHERE p.Status = 'Completed'
        GROUP BY oi.ProductId
    ),
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
    )
    SELECT 
        pr.Id AS ProductId,
        pr.Name,
        pr.Price,
        CAST(COALESCE(s.TotalBuy,0) AS VARCHAR(10)) + ' / ' + CAST(pr.Quantity AS VARCHAR(10)) AS SoldDisplay,
        CASE 
            WHEN COALESCE(s.TotalBuy,0) >= pr.Quantity THEN 'Out of Stock'
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
      AND (@SKU IS NULL OR pr.Name LIKE '%' + @SKU + '%' )
    ORDER BY
        CASE WHEN @SortBy = 'SoldDisplay' AND @SortDirection = 'ASC' THEN CAST(COALESCE(s.TotalBuy,0) AS FLOAT) END ASC,
        CASE WHEN @SortBy = 'SoldDisplay' AND @SortDirection = 'DESC' THEN CAST(COALESCE(s.TotalBuy,0) AS FLOAT) END DESC,
        CASE WHEN @SortBy = 'AvgSoldPerMonth' AND @SortDirection = 'ASC' THEN COALESCE(ms.QuantitySold,0)/12.0 END ASC,
        CASE WHEN @SortBy = 'AvgSoldPerMonth' AND @SortDirection = 'DESC' THEN COALESCE(ms.QuantitySold,0)/12.0 END DESC;
END
