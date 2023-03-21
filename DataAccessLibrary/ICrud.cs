using DataAccessLibrary.Models;

namespace DataAccessLibrary
{
    public interface ICrud
    {
        Task<IdLookupModel> CreateSupplierAsync(ISupplier supplier);
        Task<List<ISupplier>> GetAllSuppliersAsync();
        Task<ISupplier> GetSupplierAsync(int supplierId);
        Task RemoveSupplier(int supplierId);
        Task UpdateSupplierAsync(ISupplier supplier);
    }
}