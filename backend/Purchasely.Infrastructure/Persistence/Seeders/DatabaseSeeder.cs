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
            User.Create("Felipe Rodrigues Batista", "admin@purchasely.com", passwordHash("admin"), Domain.Enums.UserRole.Admin)
        };

        await context.Users.AddRangeAsync(users);
        await context.SaveChangesAsync();

        var productsList = new List<Product>
        {
            Product.Create("NB-0001", "Notebook Dell Latitude 5450", "Notebook corporativo Intel Core i5, 16GB RAM e SSD de 512GB.", "Informática"),
            Product.Create("MON-0002", "Monitor LG 24\" Full HD", "Monitor IPS de 24 polegadas com resolução Full HD.", "Informática"),
            Product.Create("TEC-0003", "Teclado Mecânico Logitech", "Teclado mecânico ABNT2 com iluminação LED.", "Periféricos"),
            Product.Create("MOU-0004", "Mouse Sem Fio Logitech M720", "Mouse ergonômico sem fio com conexão Bluetooth.", "Periféricos"),
            Product.Create("CAD-0005", "Cadeira Ergonômica Office Pro", "Cadeira ergonômica com apoio lombar e braços ajustáveis.", "Móveis"),
            Product.Create("MES-0006", "Mesa Escritório 140cm", "Mesa em MDF para escritório com acabamento amadeirado.", "Móveis"),
            Product.Create("IMP-0007", "Impressora HP LaserJet Pro", "Impressora laser monocromática para uso corporativo.", "Impressão"),
            Product.Create("PAP-0008", "Papel Sulfite A4 500 Folhas", "Resma de papel A4 branco 75g.", "Papelaria"),
            Product.Create("CAN-0009", "Caneta Esferográfica Azul", "Caixa com 50 canetas esferográficas azuis.", "Papelaria"),
            Product.Create("GRA-0010", "Grampeador Médio", "Grampeador metálico para até 30 folhas.", "Papelaria"),
            Product.Create("CAB-0011", "Cabo HDMI 2m", "Cabo HDMI 2.0 de alta velocidade com 2 metros.", "Informática"),
            Product.Create("NET-0012", "Switch TP-Link 24 Portas", "Switch Gigabit Ethernet com 24 portas RJ45.", "Redes"),
            Product.Create("NOB-0013", "Nobreak 1500VA", "Nobreak senoidal com autonomia para equipamentos críticos.", "Energia"),
            Product.Create("SSD-0014", "SSD Kingston 1TB", "SSD SATA III de 1TB para armazenamento.", "Hardware"),
            Product.Create("RAM-0015", "Memória RAM DDR5 16GB", "Módulo de memória DDR5 5600MHz.", "Hardware"),
            Product.Create("AR-0016", "Ar Condicionado Split 12000 BTUs", "Ar-condicionado inverter quente e frio.", "Climatização"),
            Product.Create("CAF-0017", "Cafeteira Elétrica 1,5L", "Cafeteira elétrica com capacidade para 15 xícaras.", "Copa"),
            Product.Create("FIL-0018", "Filtro de Linha 6 Tomadas", "Filtro de linha bivolt com proteção contra surtos.", "Elétrica"),
            Product.Create("WEB-0019", "Webcam Logitech C920", "Webcam Full HD com microfone embutido.", "Periféricos"),
            Product.Create("HDS-0020", "Headset Jabra Evolve 20", "Headset USB para videoconferências e atendimento.", "Áudio")
        };

        var suppliersList = new List<Supplier>
        {
            Supplier.Create("Alpha Tecnologia Ltda", "contato@alphatec.com.br", "1132541001", "Av. Paulista, 1000, Bela Vista, São Paulo - SP", "12784567000190"),
            Supplier.Create("Beta Informática Ltda", "vendas@betainfo.com.br", "2121344455", "Rua da Assembleia, 250, Centro, Rio de Janeiro - RJ", "45879654000112"),
            Supplier.Create("Gamma Office Solutions", "comercial@gammaoffice.com.br", "3133456677", "Av. Afonso Pena, 1450, Centro, Belo Horizonte - MG", "30987456000108"),
            Supplier.Create("Delta Suprimentos Ltda", "atendimento@deltasuprimentos.com.br", "4130258844", "Rua XV de Novembro, 890, Centro, Curitiba - PR", "56987412000177"),
            Supplier.Create("Epsilon Equipamentos Ltda", "contato@epsilon.com.br", "5132239911", "Av. Ipiranga, 3200, Partenon, Porto Alegre - RS", "71894563000129"),
            Supplier.Create("Omega Distribuidora", "vendas@omegadist.com.br", "6234127788", "Av. Goiás, 1800, Setor Central, Goiânia - GO", "84231657000146"),
            Supplier.Create("Prime Hardware Ltda", "comercial@primehardware.com.br", "7133342200", "Av. Tancredo Neves, 1500, Caminho das Árvores, Salvador - BA", "93754168000153"),
            Supplier.Create("Nexus Soluções Empresariais", "contato@nexussolucoes.com.br", "8540021234", "Av. Dom Luís, 800, Meireles, Fortaleza - CE", "65412789000135"),
            Supplier.Create("Vision Office Comércio Ltda", "vendas@visionoffice.com.br", "4730284511", "Rua Blumenau, 500, América, Joinville - SC", "21548976000164"),
            Supplier.Create("Atlas Business Supply", "atendimento@atlasbusiness.com.br", "1937569900", "Av. José de Souza Campos, 1200, Cambuí, Campinas - SP", "78321594000181")
        };

        await context.Products.AddRangeAsync(productsList);
        await context.SaveChangesAsync();

        await context.Suppliers.AddRangeAsync(suppliersList);
        await context.SaveChangesAsync();

        var supplierProductsList = new List<SupplierProduct>
        {
            SupplierProduct.Create(suppliersList[0].Id, productsList[0].Id, 5899.90m),
            SupplierProduct.Create(suppliersList[0].Id, productsList[1].Id, 899.90m),
            SupplierProduct.Create(suppliersList[0].Id, productsList[13].Id, 449.90m),

            SupplierProduct.Create(suppliersList[1].Id, productsList[2].Id, 349.90m),
            SupplierProduct.Create(suppliersList[1].Id, productsList[3].Id, 179.90m),
            SupplierProduct.Create(suppliersList[1].Id, productsList[18].Id, 479.90m),

            SupplierProduct.Create(suppliersList[2].Id, productsList[4].Id, 1399.90m),
            SupplierProduct.Create(suppliersList[2].Id, productsList[5].Id, 899.90m),
            SupplierProduct.Create(suppliersList[2].Id, productsList[9].Id, 49.90m),

            SupplierProduct.Create(suppliersList[3].Id, productsList[7].Id, 29.90m),
            SupplierProduct.Create(suppliersList[3].Id, productsList[8].Id, 54.90m),
            SupplierProduct.Create(suppliersList[3].Id, productsList[17].Id, 89.90m),

            SupplierProduct.Create(suppliersList[4].Id, productsList[6].Id, 1299.90m),
            SupplierProduct.Create(suppliersList[4].Id, productsList[12].Id, 1099.90m),
            SupplierProduct.Create(suppliersList[4].Id, productsList[10].Id, 39.90m),

            SupplierProduct.Create(suppliersList[5].Id, productsList[11].Id, 1599.90m),
            SupplierProduct.Create(suppliersList[5].Id, productsList[13].Id, 429.90m),
            SupplierProduct.Create(suppliersList[5].Id, productsList[14].Id, 529.90m),

            SupplierProduct.Create(suppliersList[6].Id, productsList[0].Id, 6099.90m),
            SupplierProduct.Create(suppliersList[6].Id, productsList[14].Id, 549.90m),
            SupplierProduct.Create(suppliersList[6].Id, productsList[18].Id, 499.90m),

            SupplierProduct.Create(suppliersList[7].Id, productsList[15].Id, 2599.90m),
            SupplierProduct.Create(suppliersList[7].Id, productsList[16].Id, 179.90m),
            SupplierProduct.Create(suppliersList[7].Id, productsList[17].Id, 79.90m),

            SupplierProduct.Create(suppliersList[8].Id, productsList[5].Id, 949.90m),
            SupplierProduct.Create(suppliersList[8].Id, productsList[7].Id, 31.90m),
            SupplierProduct.Create(suppliersList[8].Id, productsList[8].Id, 52.90m),

            SupplierProduct.Create(suppliersList[9].Id, productsList[1].Id, 879.90m),
            SupplierProduct.Create(suppliersList[9].Id, productsList[6].Id, 1279.90m),
            SupplierProduct.Create(suppliersList[9].Id, productsList[10].Id, 42.90m),
        };

        await context.SupplierProducts.AddRangeAsync(supplierProductsList);
        await context.SaveChangesAsync();
    }
}