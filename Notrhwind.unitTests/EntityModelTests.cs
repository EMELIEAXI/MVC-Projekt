using PraktiskaAppar;

namespace Notrhwind.unitTests
{
    public class EntityModelTests
    {
        [Fact]
        public void DatabaseConnectTest()
        {
            using NorthwindDatabaseContext db = new();

            Assert.True(db.Database.CanConnect());
        }

        [Fact]
        public void CategoryCountTest()
        {
            using NorthwindDatabaseContext db = new();
            int count = db.Categories.Count();

            Assert.Equal(8, count);
        }

        [Fact]
        public void ProductId48isChocoladeTest()
        {
            using NorthwindDatabaseContext db = new();
            Product? productId48 = db.Products.Find(48);
            Assert.Equal("Chocolade", productId48?.ProductName);

            //using NorthwindDatabaseContext db = new();
            //string expecter = "Chocolade";
            //Product? productId48 = db.Products.Find(48);
            //string acutal = productId48.ProductName ?? string.Empty;

            //Assert.Equal(expecter, acutal);
        }
        [Fact]
        public void ProductIdIsLauraTest()
        {
            using NorthwindDatabaseContext db = new();

            string expected = "Laura";
            Employee? employeemedId1 = db.Employees.Find(8);
            string actual = employeemedId1.FirstName ?? string.Empty;

            Assert.Equal(expected, actual);
        }
    }
}