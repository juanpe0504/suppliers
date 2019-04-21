namespace Suppliers.Providers
{
    using Suppliers.Domain.Exceptions;
    using Suppliers.Domain.Models;
    using Suppliers.Domain.Providers;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class SupplierDao : ISupplierDao
    {
        #region mocks of data

        private static List<SupplierDTO> suppliers;

        private static List<SupplierDTO> GetSuppliers()
        {
            return new List<SupplierDTO> {
                new SupplierDTO
                {
                    SupplierId = 1,
                    Name = "Supplier A",
                    SupplierRates =  new List<SupplierRateDTO>
                    {
                        new SupplierRateDTO
                        {
                            SupplierRateId = 1,
                            Rate = 10,
                            StartDate = new DateTime(2015,1,1),
                            EndDate = new DateTime(2015, 3, 31)
                        },
                        new SupplierRateDTO
                        {
                            SupplierRateId = 2,
                            Rate = 20,
                            StartDate = new DateTime(2015,4,1),
                            EndDate = new DateTime(2015, 5, 1)
                        },
                        new SupplierRateDTO
                        {
                            SupplierRateId = 3,
                            Rate = 10,
                            StartDate = new DateTime(2015,5,30),
                            EndDate = new DateTime(2015, 7, 25)
                        },
                        new SupplierRateDTO
                        {
                            SupplierRateId = 4,
                            Rate = 25,
                            StartDate = new DateTime(2015,10,1)
                        }
                    }
                },
                new SupplierDTO
                {
                    SupplierId = 2,
                    Name = "Supplier B",
                    SupplierRates =  new List<SupplierRateDTO>
                    {
                        new SupplierRateDTO
                        {
                            SupplierRateId = 5,
                            Rate = 100,
                            StartDate = new DateTime(2016,11,1)
                        }
                    }
                },
                new SupplierDTO
                {
                    SupplierId = 3,
                    Name = "Supplier C",
                    SupplierRates =  new List<SupplierRateDTO>
                    {
                        new SupplierRateDTO
                        {
                            SupplierRateId = 6,
                            Rate = 30,
                            StartDate = new DateTime(2016,12,1),
                            EndDate = new DateTime(2017, 1, 1)
                        },
                        new SupplierRateDTO
                        {
                            SupplierRateId = 7,
                            Rate = 30,
                            StartDate = new DateTime(2017,1,2)
                        }
                    }
                }
            };
        }

        #endregion

        public SupplierDao()
        {
            suppliers = GetSuppliers();
        }

        public IList<SupplierDTO> GetAll()
        {
            return suppliers;
        }

        public SupplierDTO Get(int supplierId)
        {
            return suppliers.Where(x => x.SupplierId == supplierId).SingleOrDefault();
        }

        public IList<SupplierDTO> FindSuppliersBy(int supplierId, decimal rate)
        {
            return suppliers
                .Where(x => 
                    x.SupplierId == supplierId && 
                    x.SupplierRates.Any(y => y.Rate == rate)).ToList();
        }

        public SupplierRateDTO SaveSupplierRate(int supplierId, SupplierRateDTO supplierRate)
        {
            var supplier = Get(supplierId);

            if (supplier == null)
            {
                throw new NotFoundException("Supplier Id does not exists");
            }

            var maxCurrentSupplierRateId = suppliers
                .SelectMany(x => x.SupplierRates)
                .Max(x => x.SupplierRateId);

            supplierRate.SupplierRateId = maxCurrentSupplierRateId + 1;
            supplier.SupplierRates.Add(supplierRate);

            return supplierRate;
        }
    }
}
