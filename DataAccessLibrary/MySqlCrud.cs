﻿using DataAccessLibrary.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary
{
    public class MySqlCrud : ICrud
    {

        //ToDo пересадить процессы на Stored Procedures

        private readonly IDataAccess _db;

        //Application Data
        private const string connectionStringName = "Default";


        //User and Auth database accesss
        private const string connectionStringAuthDb = "AuthDb";

        public MySqlCrud(IDataAccess db)
        {
            _db = db;
        }


        public async Task<List<ISupplier>> GetAllSuppliersAsync()
        {


            //string sql = "select * from suppliers";
            string sql = "sp_GetAllSuppliers";

            var rows = await _db.LoadData<Supplier, dynamic>(sql, new { }, connectionStringName, true);


            return rows.ToList<ISupplier>();

        }


        public async Task<ISupplier> GetSupplierAsync(int supplierId)
        {

            //string sql = "select * from suppliers where id = @Id";
            string sql = "sp_GetSupplierById";
            
            //var output = new Supplier();

            var output = (await _db.LoadData<Supplier, dynamic>(sql, new { Id = supplierId }, connectionStringName, true)).ToList<ISupplier>().First();

            //if (output.supplier == null)
            //{
            //    // do something to tell the user that the record was not found
            //    return null;
            //}
            return output;
        }





        public async Task<ISupplier> CreateSupplierAsync(ISupplier supplier)
        {
            // Save the supplier
            //string sql = "insert into suppliers (supplier, prefix, partnum_col, avail, cost, markupthreshold, markupbelow, markupabove) values (@supplier, @prefix, @partnum_col, @avail, @cost, @markupthreshold, @markupbelow, @markupabove);";
            string sql = "sp_CreateNewSupplier";

            await _db.SaveData(sql,
                        new { supplier.supplier, supplier.prefix, supplier.partnum_col, supplier.avail, supplier.cost, supplier.markupthreshold, supplier.markupbelow, supplier.markupabove },
                        connectionStringName, true);


            // Get the ID number of the supplier

            //sql = "select id from suppliers where supplier = @supplier and prefix = @prefix;";

            sql = "sp_SearchSupplierByPrefix";

            //Supplier output = new Supplier();

            var rows = await _db.LoadData<Supplier, dynamic>(sql, new { supplier = supplier.supplier, prefix = supplier.prefix }, connectionStringName, true);

            return rows.ToList<ISupplier>().First();

            //if (output.supplier == null)
            //{
            //    // do something to tell the user that the record was not found
            //    return null;
            //}

            //int supplierId = _db.LoadData<IdLookupModel, dynamic>(
            //    sql,
            //    new { supplier.supplier, supplier.prefix },
            //    connectionStringName, true ).First().Id;

            //return Task.FromResult((ISupplier)(await _db.LoadData<ISupplier, dynamic>(sql, new { supplier = supplier.supplier, prefix = supplier.prefix }, connectionStringName, true));

            //return output;

        }

        //Найти Automapper? если понадобится после stored procedure
        public async Task UpdateSupplier(ISupplier supplier)
        {
            //string sql = "update suppliers set supplier = @supplier," +
            //    " prefix = @prefix," +
            //    " partnum_col = @partnum_col," +
            //    " avail = @avail," +
            //    " cost = @cost," +
            //    " markupthreshold = @markupthreshold," +
            //    " markupbelow = @markupbelow," +
            //    " markupabove = @markupabove," +
            //    " p_time = @p_time," +
            //    " massUpload = @massUpload," +
            //    " hasnobrand = @hasnobrand," +
            //    " brand = @brand," +
            //    " brand_col = @brand_col," +
            //    " partnum_col = @partnum_col" +
            //    "," +
            //    // Uncomment when Razor is wired up and all the fields are populated
            //    " manual_description = @manual_description," +
            //    " descr = @descr," +
            //    " desc_manual = @desc_manual," +
            //    " hasnoqty = @hasnoqty," +
            //    " qty = @qty," +
            //    " hasnomodels = @hasnomodels," +
            //    " models = @models," +
            //    " p_altnum1 = @p_altnum1," +
            //    " avail1 = @avail1," +
            //    " avail2 = @avail2," +
            //    " avail3 = @avail3," +
            //    " avail4 = @avail4," +
            //    " avail5 = @avail5," +
            //    " avail6 = @avail6," +
            //    " avail2t = @avail2t," +
            //    " avail3t = @avail3t," +
            //    " avail4t = @avail4t," +
            //    " avail5t = @avail5t," +
            //    " avail6t = @avail6t," +
            //    " not_round_to_200 = @not_round_to_200," +
            //    " cross1t = @cross1t," +
            //    " cross2t = @cross2t," +
            //    " cross3t = @cross3t," +
            //    " cross4t = @cross4t," +
            //    " cross5t = @cross5t," +
            //    " cross6t = @cross6t," +
            //    " cross1col = @cross1col," +
            //    " cross2col = @cross2col," +
            //    " cross3col = @cross3col," +
            //    " cross4col = @cross4col," +
            //    " cross5col = @cross5col," +
            //    " cross6col = @cross6col" +
            //    " where id = @id";
            //            db.SaveData(sql, new {supplier.supplier, supplier.prefix, supplier.partnum_col, supplier.avail, supplier.cost, supplier.markupthreshold, supplier.markupbelow, supplier.markupabove, supplier.id }, _connectionString);


            supplier = (Supplier)supplier;

            string sql = "sp_UpdateSupplierSettingsBlazor";


                _db.SaveData(sql, supplier, connectionStringName, true);

            


        }

        public void RemoveSupplier(int supplierId)
        {

            string sql = "delete from suppliers where id = @id";
            _db.SaveData(sql, new { id = supplierId }, connectionStringName);

        }

    }
}
