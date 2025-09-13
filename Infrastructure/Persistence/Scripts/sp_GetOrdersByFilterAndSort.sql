CREATE PROCEDURE sp_GetOrdersByFilterAndSort
    @PaymentStatus NVARCHAR(20) = NULL,      
    @SortBy NVARCHAR(20) = 'OrderDate',     
    @SortDirection NVARCHAR(4) = 'DESC',    
    @ProductName NVARCHAR(100) = NULL,      
    @CustomerId INT = NULL  ,
    @PageIndex INT = 1,              
    @PageSize INT = 10  
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        o.Id,
        o.CustomerId,
        c.Name AS CustomerName,
        o.OrderDate,
        o.TotalAmount,
        p.Status AS PaymentStatus,
        STRING_AGG(pr.Name, ', ') AS ProductNames
    FROM Orders o
    INNER JOIN Customers c ON o.CustomerId = c.Id
    INNER JOIN Payments p ON p.OrderId = o.Id
    INNER JOIN OrderItems oi ON oi.OrderId = o.Id
    INNER JOIN Products pr ON pr.Id = oi.ProductId
    WHERE (@PaymentStatus IS NULL OR p.Status = @PaymentStatus)
      AND (@ProductName IS NULL OR pr.Name LIKE '%' + @ProductName + '%')
      AND (@CustomerId IS NULL OR o.CustomerId = @CustomerId)
    GROUP BY o.Id, o.CustomerId, c.Name, o.OrderDate, o.TotalAmount, p.Status
    ORDER BY 
        CASE WHEN @SortBy = 'OrderDate' AND @SortDirection = 'ASC' THEN o.OrderDate END ASC,
        CASE WHEN @SortBy = 'OrderDate' AND @SortDirection = 'DESC' THEN o.OrderDate END DESC,
        CASE WHEN @SortBy = 'TotalAmount' AND @SortDirection = 'ASC' THEN o.TotalAmount END ASC,
        CASE WHEN @SortBy = 'TotalAmount' AND @SortDirection = 'DESC' THEN o.TotalAmount END DESC
    OFFSET (@PageIndex - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END
