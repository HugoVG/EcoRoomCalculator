using EcoRoomCalculator;
using EcoRoomCalculator.Models;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Linq;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
//Room room = new Room();
//room.Blocks = Room.ComposeTiles(5, 5, 5, 1);
//Console.WriteLine(room.length);
//Console.WriteLine("Total: " + room.total);
//List<Tile> tiles = room.Blocks.Cast<Tile>().ToList();
//Console.WriteLine($"{Room.calculatefluid(room.Blocks)}m³");
//Console.WriteLine($"Used{room.filled}m³");
//Console.WriteLine("Fiddling");
//var h = Room.Atme3(25);
//Console.WriteLine("Best fit in blockss: " + Room.Tilesfilled(h));
//Console.WriteLine("Best fit in m³: " + Room.calculatefluid(h));
//Room.Decimate(Room.heightMap(h));

await builder.Build().RunAsync();

