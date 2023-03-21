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

namespace SuppliersBlazor.Pages
{
    public partial class SupplierEntry
    {
        [Parameter]
        public int Value { get; set; }

        private ISupplier model = new DisplaySupplierModel();
        private int supplierId;
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            supplierId = Value;
            model = await sql.GetSupplierAsync(supplierId);
        }

        private async Task HandleValidSubmit()
        {
            await sql.UpdateSupplierAsync(model);
        }
    }
}