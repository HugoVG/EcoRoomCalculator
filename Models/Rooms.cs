using System.Runtime.InteropServices;
using static System.Reflection.Metadata.BlobBuilder;

namespace EcoRoomCalculator.Models;

public class PredefinedRooms
{
    public Room room25_3;
}

public class Room
{
    public double Roomtier { get; set; } = 0;
    public Tile[,,] Blocks { get; set; } // 3 Dimensions
    public int length => Blocks == null ? 0 : Blocks.GetLength(0);

    public int width => Blocks == null ? 0 : Blocks.GetLength(1);

    public int height => Blocks == null ? 0 : Blocks.GetLength(2);
    public int[,] hmap => heightMap(Blocks);
    public int[,] decimated => Decimate(hmap);

    public void fiddleAround(int roomsize)
    {
        Blocks = Atme3(roomsize);
    }

    public int total
    {
        get
        {
            var totalblock = 1;
            for (int i = 0; i < Blocks.Rank; i++)
            {
                totalblock *= Blocks.GetLength(i);
            }

            return totalblock;
        }
    }

    public static int[,] heightMap(Tile[,,] Blocks)
    {
        if (Blocks == null)
        {
            return null;
        }

        int[,] map = new int[Blocks.GetLength(0), Blocks.GetLength(1)];
        for (int i = 0; i < Blocks.GetLength(0); i++)
        {
            for (int j = 0; j < Blocks.GetLength(1); j++)
            {
                int bamount = 0;
                for (int k = 0; k < Blocks.GetLength(2); k++)
                {
                    var hb = Blocks[i, j, k];
                    if (hb.tier != -1)
                    {
                        bamount++;
                    }
                }
                map[i, j] = bamount;
            }
        }
        return map;
        
    }
    public int filled
    {
        get
        {
            int bamount = 0;
            if (Blocks == null)
            {
                return 0;
            }
            int q = 0;
            int w = 0;
            int e = 0;
            try
            {
                for (int i = 0; i < Blocks.GetLength(0); i++)
                {
                    q = i;
                    for (int j = 0; j < Blocks.GetLength(1); j++)
                    {
                        w = j;

                        for (int k = 0; k < Blocks.GetLength(2); k++)
                        {
                            e = k;
                            var hb = Blocks[i, j, k];
                            if (hb.tier != -1)
                            {
                                bamount++;
                            }
                        }

                        //Console.WriteLine($"[{i} {j}] {bamount}");

                    }

                    //Console.WriteLine($"\n");
                }

                //Console.WriteLine(bamount);
                return bamount;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[{q} {w} {e}] in [{Blocks.GetLength(0)} {Blocks.GetLength(1)} {Blocks.GetLength(2)}]");
                throw;
            }

        }
    }

    public static int Tilesfilled(Tile[,,] map)
    {
        int bamount = 0;
        if (map == null)
        {
            return 0;
        }

        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {


                for (int k = 0; k < map.GetLength(2); k++)
                {
                    var hb = map[i, j, k];
                    if (hb.tier != -1)
                    {
                        bamount++;
                    }
                }
            }
        }

        return bamount;
    }

    public static int Tilesfilled(int[,] map)
        {
            int bamount = 0;
            if (map == null)
            {
                return 0;
            }

            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    var hb = map[i, j];
                    bamount += hb;
                }
            }

            Console.WriteLine(bamount);
            return bamount;
    }
    

    public void debug()
    {
        foreach (var VARIABLE in heightMap(Blocks))
        {
            Console.WriteLine(VARIABLE);
        }
    }

    public static Tile[,,] ComposeTiles(int length, int  width, int height, int tier)
    {
        Tile[,,] rTiles = new Tile[ length, width, height];

        int q = 0;
        int w = 0;
        int e = 0;
        try
        {
            for (int i = 0; i < length; i++)
            {
                q = i;
                for (int j = 0; j < width; j++)
                {
                    w = j;
                    for (int k = 0; k < height; k++)
                    {
                        e = k;
                        if (i == 0 || i == length - 1 || j == 0 || j == width - 1 || k == 0 || k == height - 1)
                        {
                            rTiles[i, j, k] = new Tile(tier);
                        }
                        else
                        {
                            rTiles[i, j, k] = new Tile(-1); //air
                        }

                    }
                }
            }
            return rTiles;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[{q}{w}{e}]");
            Console.WriteLine(ex);
            throw;
        }

    }

    public static Tile[,,] Atme3(int i)
    {
        Tile[,,] map;
        map = ComposeTiles(5, 5, 5, 1);
        if (i == 25)
        {
            map = ComposeTiles(5, 5, 5, 1);
        }
        else if (i == 45)
        {
            map = ComposeTiles(5, 7, 5, 1);
        }
        else
        {
            map = tryFiddle(i);
        }

        return map;
    }

    private static Tile[,,] tryFiddle(int to)
    {
        var attemptvar = Tuple.Create(5, 5, 5);
        //length, width, height, m³, blocks
        List<Tuple<int, int, int, int, int>> source = new();
        
        while (!source.Any(x=>x.Item4 >= to))
        {
            Tile[,,] attempt = ComposeTiles(attemptvar.Item1, attemptvar.Item2, attemptvar.Item3, 1);
            int fluid = calculatefluid(attempt);
            int filled = Tilesfilled(attempt);
            source.Add(new Tuple<int, int, int, int, int>(attemptvar.Item1, attemptvar.Item2, attemptvar.Item3, fluid, filled ));
            if (fluid >= to)
            {
                continue;
            }
            if (attemptvar.Item1 > attemptvar.Item2)
            {
                attemptvar = attemptvar.Item2 > attemptvar.Item3 ? new Tuple<int, int, int>(attemptvar.Item1, attemptvar.Item2, attemptvar.Item3 + 1) : new Tuple<int, int, int>(attemptvar.Item1, attemptvar.Item2+1, attemptvar.Item3);
            }
            else
            {
                attemptvar = attemptvar.Item1 > attemptvar.Item3 ? new Tuple<int, int, int>(attemptvar.Item1, attemptvar.Item2, attemptvar.Item3 + 1) : new Tuple<int, int, int>(attemptvar.Item1+1, attemptvar.Item2, attemptvar.Item3);
            }
        }
        var chose = source.First(x => x.Item4 >= to);
        Tile[,,] lastattempts = ComposeTiles(chose.Item2, chose.Item1, chose.Item3, 1);
        int lastfluid = calculatefluid(lastattempts);
        int lastfilled = Tilesfilled(lastattempts);
        if (lastfilled > to && lastfilled < chose.Item5)
        {
            chose = new Tuple<int, int, int, int, int>(chose.Item2, chose.Item1, chose.Item3, lastfluid, lastfilled);
        }
        Tile[,,] map = ComposeTiles(chose.Item1, chose.Item2, chose.Item3, 1);
        return map;
    }

    public static int[,] Decimate(int[,] map)
    {
        int[,]? attemp1 = map.Clone() as int[,];
        int[,]? attemp2 = map.Clone() as int[,];
        for (int i = 0; i < attemp1.GetLength(0); i++)
        {
            for (int j = 0; j < attemp1.GetLength(1); j++)
            {
                //Only outline
                if (i == 0 || i == attemp1.GetLength(0) - 1 || j == 0 || j == attemp1.GetLength(1) - 1)
                {
                    if ((i + j) % 2 == 0)
                    {
                        attemp1[i, j] = attemp1[i, j] - 2;
                    }
                }
            }
        }
        for (int i = 0; i < attemp2.GetLength(0); i++)
        {
            for (int j = 0; j < attemp2.GetLength(1); j++)
            {
                if (i != 0 && i != attemp2.GetLength(0) - 1 && j != 0 && j != attemp2.GetLength(1) - 1)
                {
                    continue;
                }
                if ((i + j) % 2 == 1)
                {
                    attemp2[i, j] = attemp2[i, j] - 2;
                }
            }
        }
        int attemp1total = attemp1.Cast<int>().ToArray().Sum();
        int attemp2total = attemp2.Cast<int>().ToArray().Sum();

        Console.WriteLine(attemp1total);
        Console.WriteLine(attemp2total);
        return attemp1total < attemp2total ? attemp1 : attemp2 ;
    }

    public static int calculatefluid(Tile[,,] tilemap)
    {

        int length = tilemap.GetLength(0);
        int width = tilemap.GetLength(1);
        int height = tilemap.GetLength(2);
        int count = 0;
        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < width; j++)
            {
                for (int k = 0; k < height; k++)
                {
                    if (i == 0 || i == length - 1 || j == 0 || j == width - 1 || k == 0 || k == height - 1)
                    {

                    }
                    else
                    {
                        count++;
                    }

                }
            }
        }

        return count;
    }
}

public class Tile
{
    public int tier { get; set; }

    public Tile(int blocktier)
    {
        tier = blocktier;
    }
    public Tile()
    {
        tier = 1;
    }
}