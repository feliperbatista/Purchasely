using Microsoft.EntityFrameworkCore;
using Purchasely.Domain.Entities;
using Bogus;

namespace Purchasely.Infrastructure.Persistence.Seeders;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        if (await context.Users.AnyAsync()) return;

        var passwordHash = (string password) => BCrypt.Net.BCrypt.HashPassword(password);

        var users = new List<User>
        {
            User.Create("Admin User", "admin@purchasely.com", passwordHash("admin"), Domain.Enums.UserRole.Admin),
            User.Create("Alice Buyer", "buyer@purchasely.com", passwordHash("buyer"), Domain.Enums.UserRole.Buyer),
            User.Create("Bob Manager", "manager@purchasely.com", passwordHash("manager"), Domain.Enums.UserRole.Manager),
            User.Create("Carol Requester", "requester@purchasely.com", passwordHash("requester"), Domain.Enums.UserRole.Requester),
        };

        await context.Users.AddRangeAsync(users);
        await context.SaveChangesAsync();

        var productFaker = new Faker<Product>()
            .CustomInstantiator(f => Product.Create(
                f.Commerce.Ean8(),
                f.Commerce.ProductName(),
                f.Commerce.ProductDescription(),
                f.Commerce.Categories(1)[0]
            ));

        var products = productFaker.Generate(30);
        await context.Products.AddRangeAsync(products);
        await context.SaveChangesAsync();

        var supplierFaker = new Faker<Supplier>()
            .CustomInstantiator(f => Supplier.Create(
                f.Company.CompanyName(),
                f.Internet.Email(),
                f.Phone.PhoneNumber("(##) ####-####"),
                f.Address.FullAddress(),
                f.Random.String2(14, "0123456789")
            ));

        var suppliers = supplierFaker.Generate(20);
        await context.Suppliers.AddRangeAsync(suppliers);
        await context.SaveChangesAsync();

        var random = new Random();
        var supplierProducts = new List<SupplierProduct>();

        foreach(var supplier in suppliers)
        {
            var randomProducts = products
                .OrderBy(_ => random.Next())
                .Take(random.Next(3, 8))
                .ToList();

            foreach (var product in randomProducts)
            {
                supplierProducts.Add(SupplierProduct.Create(
                    supplier.Id,
                    product.Id,
                    Math.Round((decimal)random.NextDouble() * 10 * (decimal)(0.85 + random.NextDouble() * 0.3), 2)
                ));
            }
        }

        await context.SupplierProducts.AddRangeAsync(supplierProducts);
        await context.SaveChangesAsync();
    }
}