﻿namespace Dapper_StoredProcedures.Domain.Entities
{
    public class OrderItem
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
