using DataAccessLibrary.Models;

namespace DataAccessLibrary
{
    public interface ICrud
    {
        void CreateSupplier(Supplier supplier);
        List<Supplier> GetAllSuppliers();
        Task<List<Supplier>> GetAllSuppliersAsync();
        Supplier GetSupplier(int supplierId);
        void RemoveSupplier(int supplierId);
        void UpdateSupplier(Supplier supplier);
    }
}