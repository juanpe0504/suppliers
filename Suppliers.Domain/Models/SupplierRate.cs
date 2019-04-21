namespace Suppliers.Domain.Models
{
    using System;

    public class SupplierRate
    {
        public int SupplierRateId { get; set; }

        public Supplier Supplier { get; set; }

        public decimal Rate { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}
