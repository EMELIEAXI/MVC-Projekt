using PraktiskaAppar;
namespace Northwind.Mvc.Models
{
    public class HomeIndexViewModel
    (
        int VisitorCount,
        IList <Category> Categories,
        IList<Product> Products
    );
}
