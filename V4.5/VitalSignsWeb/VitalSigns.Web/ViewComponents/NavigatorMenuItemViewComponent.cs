using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VitalSigns.Web.Models;

namespace VitalSigns.Web.ViewComponents
{

    public class NavigatorMenuItemViewComponent : ViewComponent
    {
        public NavigatorMenuItemViewComponent()
        {
            
        }
        
        public async Task<IViewComponentResult> InvokeAsync(ICollection<SiteMapNode> nodes)
        {
            return View(nodes);
        }
    }
}
