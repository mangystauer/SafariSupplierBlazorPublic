using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.JSInterop;
using SuppliersBlazor;
using SuppliersBlazor.Shared;
using SuppliersBlazor.Models;
using DataAccessLibrary.Models;
using DataAccessLibrary;
using System.Globalization;
using Google.Protobuf.WellKnownTypes;

namespace SuppliersBlazor.Pages
{
    public partial class SupplierEntry
    {


        [Parameter]
        public int Value { get; set; }

        private ISupplier model = new DisplaySupplierModel();
        private int supplierId;
        private string MarkupAboveString = "";
        private string MarkupBelowString = "";
        private int MarkupAboveInt = 0;
        private int MarkupBelowInt = 0;
        private decimal MarkupAboveDecimal = 0;
        private decimal MarkupBelowDecimal = 0;

        private bool showSuccessMessage;



        NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            supplierId = Value;
            model = await sql.GetSupplierAsync(supplierId);
            //MarkupAboveString = model.markupabove.ToString("00%", nfi);
            //MarkupBelowString = model.markupbelow.ToString("00%", nfi);

            MarkupBelowDecimal = model.markupbelow * 100;
            MarkupAboveDecimal = model.markupabove * 100;

            MarkupBelowInt = Decimal.ToInt32(MarkupBelowDecimal);
            MarkupAboveInt = Decimal.ToInt32(MarkupAboveDecimal);
        }




        private async Task HandleBelowMarkup(ChangeEventArgs args)
        {
            decimal result;
            int.TryParse(args.Value?.ToString(), out MarkupBelowInt);
            decimal.TryParse(args.Value?.ToString(), out result);
            result = result / 100;

            await Task.Run(() => model.markupbelow = result);

        }

        private async Task HandleAboveMarkup(ChangeEventArgs args)
        {
            decimal result;
            decimal.TryParse(args.Value?.ToString(), out result);
            result = result / 100;

            await Task.Run(() => model.markupabove = result);

        }





        private async Task HandleValidSubmit()
        {
            await sql.UpdateSupplierAsync(model);
            showSuccessMessage = true;
            await Task.Delay(5000);
            showSuccessMessage = false;

        }
    }
}