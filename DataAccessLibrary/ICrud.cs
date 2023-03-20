using DataAccessLibrary.Models;

namespace DataAccessLibrary
{
    public interface ICrud
    {
        Task<Supplier> CreateSupplierAsync(Supplier supplier);
        Task<List<Supplier>> GetAllSuppliersAsync();
        Task<Supplier> GetSupplierAsync(int supplierId);
        void RemoveSupplier(int supplierId);
        Task UpdateSupplier(Supplier supplier);
    }
}