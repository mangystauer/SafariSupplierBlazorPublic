namespace DataAccessLibrary
{
    public interface IDataAccess
    {
        Task<List<T>> LoadData<T, U>(string sqlStatement, U parameters, string connectionString, bool isStroredProcedure = false);
        Task SaveData<T>(string sqlStatement, T parameters, string connectionString, bool isStroredProcedure = false);
    }
}