CREATE PROCEDURE sp_GetOrderItemsWithProduct
    @OrderId INT
AS
BEGIN
    SELECT oi.OrderId, oi.ProductId, oi.Quantity,
           p.Name AS ProductName, p.Price
    FROM OrderItems oi
    INNER JOIN Products p ON oi.ProductId = p.Id
    WHERE oi.OrderId = @OrderId
END
