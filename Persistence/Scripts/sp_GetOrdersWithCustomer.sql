CREATE PROCEDURE sp_GetOrdersWithCustomer
AS
BEGIN
    SELECT o.Id, o.CustomerId, o.OrderDate, o.TotalAmount,
           c.Name AS CustomerName
    FROM Orders o
    INNER JOIN Customers c ON o.CustomerId = c.Id
END
