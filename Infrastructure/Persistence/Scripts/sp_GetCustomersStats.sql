CREATE PROCEDURE sp_GetCustomersStats
AS
BEGIN
    SET NOCOUNT ON;

    -- Bật thống kê để đo hiệu năng
    SET STATISTICS IO ON;
    SET STATISTICS TIME ON;

    SELECT 
        c.Id,

        c.Name,

        c.Email,

        -- Tổng số đơn
        ISNULL(os.TotalOrders, 0) AS TotalOrders,

        -- Tổng số sản phẩm đã mua
        ISNULL(os.TotalItemsBought, 0) AS TotalItemsBought,

        -- Sản phẩm yêu thích (mua nhiều nhất)
        ISNULL(fp.FavoriteProduct, '') AS FavoriteProduct,

        -- Thống kê thanh toán
        ISNULL(ps.TotalFailedPayments, 0) AS TotalFailedPayments,
        ISNULL(ps.TotalSuccessfulPayments, 0) AS TotalSuccessfulPayments,

        -- Tổng tiền chi tiêu
        ISNULL(os.TotalSpent, 0) AS TotalSpent,

        -- Đơn hàng gần nhất
        os.LastOrderDate

    FROM Customers c
    -- Thống kê đơn hàng + tổng tiền
    LEFT JOIN (
        SELECT 
            o.CustomerId,
            COUNT(DISTINCT o.Id) AS TotalOrders,
            SUM(oi.Quantity) AS TotalItemsBought,
            SUM(o.TotalAmount) AS TotalSpent,
            MAX(o.OrderDate) AS LastOrderDate
        FROM Orders o
        LEFT JOIN OrderItems oi ON o.Id = oi.OrderId
        GROUP BY o.CustomerId
    ) os ON c.Id = os.CustomerId

    -- Sản phẩm yêu thích 
    OUTER APPLY (
        SELECT TOP 1 p.Name AS FavoriteProduct
        FROM Orders o
        INNER JOIN OrderItems oi ON o.Id = oi.OrderId
        INNER JOIN Products p ON p.Id = oi.ProductId
        WHERE o.CustomerId = c.Id
        GROUP BY p.Name
        ORDER BY SUM(oi.Quantity) DESC
    ) fp

    -- Thống kê thanh toán
    LEFT JOIN (
        SELECT 
            o.CustomerId,
            SUM(CASE WHEN p.Status = 'Failed' THEN 1 ELSE 0 END) AS TotalFailedPayments,
            SUM(CASE WHEN p.Status = 'Completed' THEN 1 ELSE 0 END) AS TotalSuccessfulPayments
        FROM Payments p
        INNER JOIN Orders o ON o.Id = p.OrderId
        GROUP BY o.CustomerId
    ) ps ON c.Id = ps.CustomerId

    ORDER BY c.Name;
END
