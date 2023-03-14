﻿using DataAccessLibrary.Models;

namespace DataAccessLibrary
{
    public interface ICrud
    {
        Task<Supplier> CreateSupplierAsync(Supplier supplier);
        Task<List<Supplier>> GetAllSuppliersAsync();
        Task<Supplier> GetSupplierAsync(int supplierId);
        void RemoveSupplier(int supplierId);
        void UpdateSupplier(Supplier supplier);
    }
}