using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PraktiskaAppar;

namespace Northwind.razor.Employees.Areas.NorthwindFeatures.Pages
{
    public class CustomersModel : PageModel
    {
        private NorthwindDatabaseContext db;

        public CustomersModel(NorthwindDatabaseContext injectedContext)
        {
            db = injectedContext;
        }

        public Customer[] Customers { get; set; } = null!;
        public void OnGet()
        {
            ViewData["Titel"] = "Northwind App - Customers";
            Customers = db.Customers.OrderBy(c => c.Country)
                .ThenBy(c => c.ContactName).ToArray();
        }
    }
}
