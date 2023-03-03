namespace DataAccessLibrary
{
    public interface IDataAccess
    {
        List<T> LoadData<T, U>(string sqlStatement, U parameters, string connectionString);
        void SaveData<T>(string sqlStatement, T parameters, string connectionString);
    }
}