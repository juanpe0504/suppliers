namespace Suppliers.Domain.Models
{
    using System;

    public class SupplierRateDTO
    {
        public int SupplierRateId { get; set; }

        public decimal Rate { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}
