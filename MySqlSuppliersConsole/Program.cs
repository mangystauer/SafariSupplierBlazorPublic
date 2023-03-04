using DataAccessLibrary;
using DataAccessLibrary.Models;
using Microsoft.Extensions.Configuration;
using Org.BouncyCastle.Security;
using System.IO;
using Microsoft.Extensions.DependencyInjection;

internal class Program
{
    private static void Main(string[] args)
    {

        var builder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory()) //<--You would need to set the path
        .AddJsonFile("appsettings.json"); //or what ever file you have the settings

        IConfiguration configuration = builder.Build();



        var serviceProvider = new ServiceCollection()
            .AddTransient<IDataAccess, MySQLDataAccess>()
            .AddTransient<ICrud, MySqlCrud>()
            .AddSingleton<IConfiguration>(configuration)
            .BuildServiceProvider();
            
        
        //MySqlCrud sql = new MySqlCrud();

        //ReadAllSuppliers(sql);
        //ReadSupplier(serviceProvider.GetService<ICrud>(), 28);

        CreateNewSupplier(serviceProvider.GetService<ICrud>());
        //UpdateSupplier(serviceProvider.GetService<ICrud>());
        //DeleteSupplier(serviceProvider.GetService<ICrud>(), 38);
        //new comment for git


        ReadAllSuppliers(serviceProvider.GetService<ICrud>());


        Console.WriteLine("Done Processing MySQL");

        Console.ReadLine();
    }





    private static string GetConnectionString(string connectionStringName = "Default")
    {
        string output = "";

        var builder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");

        var config = builder.Build();

        output = config.GetConnectionString(connectionStringName);

        return output;
    }


    private static async void ReadAllSuppliers(ICrud sql)
    {
        var rows = await sql.GetAllSuppliersAsync();

        foreach (var row in rows)
        {
            Console.WriteLine($"{row.id}: {row.prefix} {row.supplier} {row.desc_manual}");
        }
    }

    private static async void ReadSupplier(ICrud sql, int supplierId)
    {
        Supplier row = await sql.GetSupplierAsync(supplierId);

        Console.WriteLine($"{row.id}: {row.prefix} {row.supplier} {row.desc_manual}");
    }

    private async static void CreateNewSupplier(ICrud sql)
    {
        Supplier supplier = new Supplier
        {
            //string sql = "insert into suppliers (supplier, prefix, partnum_col, avail, cost, markupthreshold, markupbelow, markupabove) values (@id, @supplier, @prefix, @partnum_col, @avail, @cost, @markupthreshold, @markupbelow, @markupabove);";

            supplier = "Test Sup5",
            prefix ="TT2",
            partnum_col = 1,
            avail = 2,
            cost = 5,
            markupthreshold = 100000,
            markupbelow = 0.3m,
            markupabove= 0.2m

        };

        await sql.CreateSupplierAsync(supplier);


    }

    private static void UpdateSupplier(ICrud sql)
    {
        Supplier supplier = new Supplier
        {
            id = 38,
            supplier = "Test Sup4",
            prefix = "TT3",
            partnum_col = 1,
            avail = 2,
            cost = 5,
            markupthreshold = 100000,
            markupbelow = 0.3m,
            markupabove = 0.2m
        };
        sql.UpdateSupplier(supplier);
    }

    private static void DeleteSupplier(ICrud sql, int supplierId)
    {
        sql.RemoveSupplier(supplierId);


    }

}