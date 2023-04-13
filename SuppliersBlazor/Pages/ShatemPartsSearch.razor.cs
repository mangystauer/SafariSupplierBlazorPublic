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
using DataAccessLibrary.DataService.Shatem;
using DataAccessLibrary.Models.Shatem.DataAccess;
using DataAccessLibrary.Models.Shatem.Models;
using SuppliersBlazor.Models.Search_Models;

namespace SuppliersBlazor.Pages
{
    public partial class ShatemPartsSearch
    {
        [Inject] protected NavigationManager NavigationManager { get; set; }


        private ShatemPartsSearchModel searchModel = new ShatemPartsSearchModel();
        private List<ShatemFoundArticle> parts;
        private List<ShatemArticlePrice> availableParts;
        private bool searching;
        private async Task SearchParts()
        {
            searching = true;
            parts = await ShatemDataService.SearchArticlesAsync(searchModel.PartNumber, searchModel.TradeMarkName);
            if (parts is not null)
            {
                string searchPart = parts.FirstOrDefault().Id.ToString();
                List<ShatemArticlePrice> availableQtyParts = await ShatemDataService.SearchAvailableQuantityAsync(searchPart, searchModel.IncludeAnalogs);
                if (availableQtyParts is not null)
                {
                    availableParts = availableQtyParts;
                    foreach (var part in availableParts)
                    {
                        ShatemFoundArticle shf = new();
                        shf = await ShatemDataService.ArticleInfoAsync(part.articleId.ToString());
                        shf.AvailableQty = part.quantity.available;
                        shf.Price = part.price.value;
                        shf.stock = part.locationCodeReal;
                        parts.Add(shf);
                    }

                    parts.Remove(parts[0]);
                }
            }

            searching = false;
        }
    }
}