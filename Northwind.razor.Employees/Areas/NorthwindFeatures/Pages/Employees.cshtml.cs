using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PraktiskaAppar;

namespace Northwind.razor.Employees.MyFeature.Pages
{
    public class EmployeesModel : PageModel
    {
        private NorthwindDatabaseContext db;

        public EmployeesModel(NorthwindDatabaseContext injectedContext)
        {
            db = injectedContext;
        }
        public Employee[] Employees { get; set; } = null!;

        public void OnGet()
        {
            ViewData["Titel"] = "Northwind App - Employees";
            Employees = db.Employees.OrderBy(e => e.LastName)
                .ThenBy(e => e.FirstName).ToArray();

        }
    }
}
