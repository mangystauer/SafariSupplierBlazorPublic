using DataAccessLibrary;
using DataAccessLibrary.Models;
using Microsoft.Extensions.Configuration;
using Org.BouncyCastle.Security;
using System.IO;

internal class Program
{
    private static void Main(string[] args)
    {

        MySqlCrud sql = new MySqlCrud(GetConnectionString());

        //ReadAllSuppliers(sql);
        //ReadSupplier(sql, 28);

        CreateNewSupplier(sql);
        //UpdateSupplier(sql);
        //DeleteSupplier(sql, 35);
        //ReadSupplier(sql, newSupplier);
        ReadAllSuppliers(sql);

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


    private static void ReadAllSuppliers(MySqlCrud sql)
    {
        var rows = sql.GetAllSuppliers();

        foreach (var row in rows)
        {
            Console.WriteLine($"{row.id}: {row.prefix} {row.supplier} {row.desc_manual}");
        }
    }

    private static void ReadSupplier(MySqlCrud sql, int supplierId)
    {
        Supplier row = sql.GetSupplier(supplierId);

        Console.WriteLine($"{row.id}: {row.prefix} {row.supplier} {row.desc_manual}");
    }

    private static void CreateNewSupplier(MySqlCrud sql)
    {
        Supplier supplier = new Supplier
        {
            //string sql = "insert into suppliers (supplier, prefix, partnum_col, avail, cost, markupthreshold, markupbelow, markupabove) values (@id, @supplier, @prefix, @partnum_col, @avail, @cost, @markupthreshold, @markupbelow, @markupabove);";

            supplier = "Test Sup2",
            prefix ="TT2",
            partnum_col = 1,
            avail = 2,
            cost = 5,
            markupthreshold = 100000,
            markupbelow = 0.3m,
            markupabove= 0.2m

        };

        sql.CreateSupplier(supplier);


    }

    private static void UpdateSupplier(MySqlCrud sql)
    {
        Supplier supplier = new Supplier
        {
            id = 36,
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

    private static void DeleteSupplier(MySqlCrud sql, int supplierId)
    {
        sql.RemoveSupplier(supplierId);


    }

}