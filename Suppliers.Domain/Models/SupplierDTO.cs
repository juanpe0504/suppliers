namespace Suppliers.Domain.Models
{
    using System.Collections.Generic;

    public class SupplierDTO
    {
        public int SupplierId { get; set; }

        public string Name { get; set; }

        public IList<SupplierRateDTO> SupplierRates { get; set; }
    }
}
