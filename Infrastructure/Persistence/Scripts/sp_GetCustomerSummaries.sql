CREATE PROCEDURE sp_GetCustomerSummaries
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        c.Id,
        c.Name,
        c.Phone,
        COUNT(DISTINCT o.Id) AS TotalOrders,
        ISNULL(SUM(oi.Quantity), 0) AS TotalOrders,

        -- Sản phẩm yêu thích
        (SELECT TOP 1 p.Name
         FROM OrderItems oi2
         INNER JOIN Orders o2 ON o2.Id = oi2.OrderId
         INNER JOIN Products p ON p.Id = oi2.ProductId
         WHERE o2.CustomerId = c.Id
         GROUP BY p.Name
         ORDER BY SUM(oi2.Quantity) DESC) AS FavoriteProduct,

        -- Số lần thanh toán thất bại
        (SELECT COUNT(*) 
         FROM Payments p
         INNER JOIN Orders o3 ON o3.Id = p.OrderId
         WHERE o3.CustomerId = c.Id AND p.Status = 'Failed') AS TotalFailedPayments,

        -- Số lần thanh toán thành công
        (SELECT COUNT(*) 
         FROM Payments p
         INNER JOIN Orders o4 ON o4.Id = p.OrderId
         WHERE o4.CustomerId = c.Id AND p.Status = 'Completed') AS TotalSuccessfulPayments,

        -- Tổng tiền đã chi
        (SELECT ISNULL(SUM(o5.TotalAmount), 0)
         FROM Orders o5
         WHERE o5.CustomerId = c.Id) AS TotalSpent,

        -- Ngày mua gần nhất
        (SELECT MAX(o6.OrderDate)
         FROM Orders o6
         WHERE o6.CustomerId = c.Id) AS LastOrderDate

    FROM Customers c
    LEFT JOIN Orders o ON o.CustomerId = c.Id
    LEFT JOIN OrderItems oi ON oi.OrderId = o.Id
    GROUP BY c.Id, c.Name, c.Phone
    ORDER BY c.Name;
END
