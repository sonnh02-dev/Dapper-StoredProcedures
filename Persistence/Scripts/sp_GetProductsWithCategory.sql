CREATE PROCEDURE sp_GetProductsWithCategory
AS
BEGIN
    SELECT p.Id, p.Name, p.Price, p.Quantity,
           p.CategoryId, c.Name AS CategoryName
    FROM Products p
    INNER JOIN Categories c ON p.CategoryId = c.Id
END
