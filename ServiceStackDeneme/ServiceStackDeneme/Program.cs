using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using static ProductService;

namespace ServiceStackDeneme
{
    class Program
    {
        static void Main(string[] args)
        {
            var hostAddress = "http://*:4568/";
            var appHost = new AppHost().Init().Start(hostAddress);

            Console.WriteLine("Host is running at {0}", hostAddress);
            Console.ReadLine();
        }
    }
}

public class Product
{
    public int PartNumber { get; set; }
    public string Name { get; set; }
    public decimal ListPrice { get; set; }
}

[Route("/Products", "GET")]
[Route("/Products/{NameLike}", "GET")]
public class ProductSelectRequest : IReturn<ProductSelectResponse>

{
    public string NameLike { get; set; }
}
public class ProductSelectResponse
{
    public List<Product> Products { get; set; }
}
[Route("/products", "POST")]
public class CreateProduct : IReturn<ProductSelectResponse>
{
    public int PartNumber { get; set; }
    public string Name { get; set; }
    public decimal ListPrice { get; set; }

}
public class ProductService : Service
{
    public static List<Product> products = new List<Product>
    {
            new Product{ PartNumber=1001, Name="AC-210",ListPrice=1000},
            new Product{ PartNumber=1002, Name="AC-215",ListPrice=960},
            new Product{ PartNumber=1003, Name="KC-210",ListPrice=850.50M},
            new Product{ PartNumber=1004, Name="BC-210",ListPrice=750},
            new Product{ PartNumber=1005, Name="BD-123",ListPrice=450},
            new Product{ PartNumber=1006, Name="BD-400",ListPrice=900},
            new Product{ PartNumber=1007, Name="ZD-405",ListPrice=250},
            new Product{ PartNumber=1008, Name="CD-505",ListPrice=385}
    };
    public object Get(ProductSelectRequest request)
    {
        if (request.NameLike != default(string))
        {
            return new ProductSelectResponse
            {
                Products = (from p in products
                            where p.Name.StartsWith(request.NameLike)
                            select p).ToList()
            };

        }
        else
        {
            return new ProductSelectResponse
            {
                Products = products
            };
        }
    }

    public object Post(CreateProduct request)
    {
        Product newProduct = new Product
        {
            PartNumber = request.PartNumber,
            Name = request.Name,
            ListPrice = request.ListPrice
        };
        products.Add(newProduct);
        return newProduct;
    }

    public class AppHost
       : AppSelfHostBase
    {
        public AppHost()
            : base("HttpListener Self-Host", typeof(ProductService).Assembly)
        {
        }
        public override void Configure(Funq.Container container)
        {
        }
    }
}


