namespace Suppliers.Domain.Models
{
    using System.Collections.Generic;

    public class Supplier
    {
        public int SupplierId { get; set; }

        public string Name { get; set; }

        public IList<SupplierRate> SupplierRates { get; set; }
    }
}
