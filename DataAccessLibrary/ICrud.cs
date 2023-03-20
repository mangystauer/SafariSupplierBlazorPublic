using DataAccessLibrary.Models;

namespace DataAccessLibrary
{
    public interface ICrud
    {
        Task<ISupplier> CreateSupplierAsync(ISupplier supplier);
        Task<List<ISupplier>> GetAllSuppliersAsync();
        Task<ISupplier> GetSupplierAsync(int supplierId);
        void RemoveSupplier(int supplierId);
        Task UpdateSupplier(ISupplier supplier);
    }
}