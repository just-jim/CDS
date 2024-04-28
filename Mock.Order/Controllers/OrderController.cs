using System.Text.Json;
using Mock.Order.Models;

namespace Mock.Order.Controllers;

internal class OrderController {
    readonly List<string> _names = [
        "Luke Skywalker",
        "Leia Organa",
        "Han Solo",
        "Obi-Wan Kenobi",
        "Darth Vader",
        "Chewbacca",
        "C-3PO",
        "R2-D2",
        "Yoda",
        "Palpatine",
        "Padmé Amidala",
        "Anakin Skywalker",
        "Boba Fett",
        "Mace Windu",
        "Rey",
        "Kylo Ren",
        "Finn",
        "Poe Dameron",
        "Ahsoka Tano",
        "Jyn Erso"
    ];
    readonly Random _random = new Random();

    static readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions {
        PropertyNameCaseInsensitive = true
    };

    static Models.Order ParseJson() {
        const string jsonFilePath = "Resources/OrderListMetadata.json";
        string jsonString = File.ReadAllText(jsonFilePath);
        return JsonSerializer.Deserialize<Models.Order>(jsonString, JsonSerializerOptions)!;
    }

    public static Models.Order Original() {
        return ParseJson();
    }

    public Models.Order Random() {
        var order = ParseJson();
        order.OrderNumber = Guid.NewGuid().ToString();
        order.CustomerName = Any(_names);
        order.OrderDate = DateTime.Parse(order.OrderDate).AddDays(-_random.Next(1000)).ToString("yyyy-MM-dd");
        List<Asset> assetsToRemove = [];
        foreach (var asset in order.Assets) {
            if (_random.Next(100) <= 20) {
                assetsToRemove.Add(asset);
            }
            else {
                asset.Quantity = _random.Next(100);
            }
        }
        foreach (var asset in assetsToRemove) {
            order.Assets.Remove(asset);
        }

        return order;
    }

    T Any<T>(IReadOnlyList<T> list) {
        int randomIndex = _random.Next(list.Count);
        return list[randomIndex];
    }
}