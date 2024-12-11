using System;
using System.Collections.Generic;
using System.Linq;

namespace OrderApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var products = new List<Product>
            {
                new Product("Laptop", 2500),
                new Product("Klawiatura", 120),
                new Product("Mysz", 90),
                new Product("Monitor", 1000),
                new Product("Kaczka debuggująca", 66)
            };

            var order = new Order();
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("\nWybierz opcję:");
                Console.WriteLine("1. Dodaj produkt");
                Console.WriteLine("2. Usuń produkt");
                Console.WriteLine("3. Wyświetl wartość zamówienia");
                Console.WriteLine("4. Wyjdź z aplikacji");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        while(true)
                        {
                            Console.WriteLine("\nDostępne produkty:");
                            for (int i = 0; i < products.Count; i++)
                            {
                                Console.WriteLine($"{i + 1}. {products[i].Name} - {products[i].Price} PLN");
                            }
                            Console.WriteLine("0. Powrót do głównego menu");
                            Console.Write("Wybierz produkt (numer): ");
                            if (int.TryParse(Console.ReadLine(), out int productIndex))
                            {
                                if (productIndex == 0)
                                {
                                    Console.WriteLine("\nPowrót do głównego menu...");
                                    break;
                                }
                                else if (productIndex > 0 && productIndex <= products.Count)
                                {
                                    order.AddItem(products[productIndex - 1]);
                                    Console.WriteLine("\nProdukt dodany.");
                                }
                                else
                                {
                                    Console.WriteLine("\nNiepoprawny wybór. Wpisz numer produktu lub '0', aby wrócić.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("\nNiepoprawny wybór. Wpisz numer produktu lub '0', aby wrócić.");
                            }
                        }
                        break;
                    case "2":
                        while (true)
                        {
                            Console.WriteLine("Produkty w zamówieniu:");
                            if (order.Items.Count == 0)
                            {
                                Console.WriteLine("\nBrak produktów w zamówieniu. Powrót do głównego menu...");
                                break; // Back to main menu if list is empty
                            }

                            for (int i = 0; i < order.Items.Count; i++)
                            {
                                Console.WriteLine($"{i + 1}. {order.Items[i].Product.Name} - {order.Items[i].Product.Price} PLN");
                            }
                                Console.WriteLine("0. Powrót do głównego menu");
                            Console.Write("Wybierz produkt do usunięcia (numer): ");
                            if (int.TryParse(Console.ReadLine(), out int itemIndex))
                            {
                                if (itemIndex == 0)
                                {
                                    Console.WriteLine("\nPowrót do głównego menu...");
                                    break; // Back to main menu
                                }
                                else if (itemIndex > 0 && itemIndex <= order.Items.Count)
                                {
                                    order.RemoveItem(itemIndex - 1);
                                    Console.WriteLine("\nProdukt usunięty.");
                                }
                                else
                                {
                                    Console.WriteLine($"\nNiepoprawny wybór. Wybierz numer numer produktu do usunięcia lub '0', aby wrócić.");
                                }
                            }
                            else
                            {
                                Console.WriteLine($"\nNiepoprawny wybór. Wybierz numer od 1 do {order.Items.Count}.");
                            }
                        }
                        break;
                    case "3":
                        if (order.Items.Count == 0) 
                        { 
                            Console.WriteLine("Brak produktów w zamówieniu."); 
                        }
                        else
                        {
                            Console.WriteLine($"\nCałkowita wartość zamówienia: {order.CalculateTotal()} PLN");
                        }
                        break;
                    case "4":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Niepoprawny wybór. Wybierz numer od 1 do 4.");
                        break;
                }
            }
        }
    }

    public class Product
    {
        public string Name { get; set; }
        public decimal Price { get; set; }

        public Product(string name, decimal price)
        {
            Name = name;
            Price = price;
        }
    }

    public class OrderItem
    {
        public Product Product { get; set; }

        public OrderItem(Product product)
        {
            Product = product;
        }
    }

    public class Order
    {
        public List<OrderItem> Items { get; private set; } = new List<OrderItem>();

        public void AddItem(Product product)
        {
            Items.Add(new OrderItem(product));
        }

        public void RemoveItem(int index)
        {
            Items.RemoveAt(index);
        }

        public decimal CalculateTotal()
        {
            var prices = Items.Select(item => item.Product.Price).OrderBy(price => price).ToList();
            decimal total = prices.Sum();
            decimal discount = 0;

            // Apply discount for second cheapest product
            if (prices.Count == 2)
            {
                discount += prices[prices.Count - 2] * 0.10m;
            }

            // Apply discount for third cheapest product
            if (prices.Count >= 3)
            {
                discount += prices[prices.Count - 3] * 0.20m;
            }

            // Subtract discount from the total cost
            total -= discount;

            // Apply 5% discount for orders over 5000 PLN
            if (total > 5000)
            {
                total *= 0.95m;
            }

            return Math.Round(total, 2);
        }
    }
}
