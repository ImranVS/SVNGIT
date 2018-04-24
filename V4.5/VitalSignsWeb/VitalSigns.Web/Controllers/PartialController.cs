using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using VitalSigns.API.Models;
using MongoDB.Bson;
using VitalSigns.Web.Models;
using System.Linq;
using MongoDB.Driver;

namespace VitalSigns.Web.Controllers
{

    [Route("[controller]/[action]")]
    public class PartialController : Controller
    {

        public IActionResult SiteMap()
        {
            var dataContext = new DataContext();

            var menu = dataContext.SiteMap.Find(x => x.Id == "left").SingleOrDefault();
            var serverTypes = dataContext.ServerTypesUsed;
            menu = filterMenu(menu, serverTypes);
            
            return PartialView(menu);
        }

        public IActionResult Navigator()
        {
            var dataContext = new DataContext();

            var menu = dataContext.SiteMap.Find(x => x.Id == "navigator").SingleOrDefault();

            return PartialView(menu);
        }

        public SiteMap filterMenu(SiteMap siteMap, List<string> serverTypes)
        {
            siteMap.Nodes = 
                siteMap.Nodes != null && siteMap.Nodes.Count() > 0 ? 
                siteMap.Nodes.Where(x => (x.ServerTypes == null) || x.ServerTypes.Count() == 0 || (x.ServerTypes.Count() > 0 && x.ServerTypes.Intersect(serverTypes).Count() > 0)).ToList() : 
                null;
            if (siteMap.Nodes != null)
                foreach (var x in siteMap.Nodes)
                    filterMenu(x, serverTypes);
            return siteMap;
        }

        public SiteMapNode filterMenu(SiteMapNode siteMapNode, List<string> serverTypes)
        {
            siteMapNode.Nodes = siteMapNode.Nodes != null && siteMapNode.Nodes.Count() > 0 ? siteMapNode.Nodes.Where(x => (x.ServerTypes == null) || x.ServerTypes.Count() == 0 || (x.ServerTypes.Count() > 0 && x.ServerTypes.Intersect(serverTypes).Count() > 0)).ToList() : null;
            if(siteMapNode.Nodes != null)
                foreach (var x in siteMapNode.Nodes)
                    filterMenu(x, serverTypes);
            return siteMapNode.ServerTypes == null || siteMapNode.ServerTypes.Intersect(serverTypes).Count() > 0 ? siteMapNode : null;
        }
    }
}
