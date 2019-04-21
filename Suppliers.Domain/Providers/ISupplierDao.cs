namespace Suppliers.Domain.Providers
{
    using Suppliers.Domain.Models;
    using System.Collections.Generic;

    public interface ISupplierDao
    {
        IList<SupplierDTO> GetAll();

        SupplierDTO Get(int supplierId);

        IList<SupplierDTO> FindSuppliersBy(int supplierId, decimal rate);

        SupplierRateDTO SaveSupplierRate(int supplierId, SupplierRateDTO supplierRate);
    }
}
