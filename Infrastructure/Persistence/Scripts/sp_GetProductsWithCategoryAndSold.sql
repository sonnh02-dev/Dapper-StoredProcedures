CREATE PROCEDURE sp_GetProductsWithCategoryAndSold
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
    )
    SELECT 
        pr.Id AS ProductId,
        pr.Name,
        pr.Price,
        CAST(COALESCE(s.TotalBuy, 0) AS VARCHAR(10)) + ' / ' + CAST(pr.Quantity AS VARCHAR(10)) AS SoldDisplay,
        pr.CategoryId,
        ca.Name AS CategoryName
    FROM Products pr
    LEFT JOIN Sold s ON pr.Id = s.ProductId
    INNER JOIN Categories ca ON pr.CategoryId = ca.Id
    ORDER BY pr.Id;
END
