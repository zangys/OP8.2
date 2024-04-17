using System;
using System.Linq;

// Интерфейс для объектов, содержащих координаты
public interface ICoordinate
{
    double Latitude { get; }
    double Longitude { get; }
}

// Класс для хранения координат
public class Coordinate : ICoordinate
{
    public double Latitude { get; }
    public double Longitude { get; }

    public Coordinate(double latitude, double longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
    }
}

// Класс корабля
public class Ship
{
    public int ShipNumber { get; }
    public ICoordinate Coordinates { get; }

    public Ship(int shipNumber, ICoordinate coordinates)
    {
        ShipNumber = shipNumber;
        Coordinates = coordinates;
    }

    // Метод для ввода данных о корабле с обработкой ошибок
    public static Ship InputShip()
    {
        int shipNumber;
        double latitude, longitude;

        try
        {
            Console.WriteLine("Введите данные о корабле:");
            Console.Write("Учетный номер: ");
            shipNumber = int.Parse(Console.ReadLine());

            Console.Write("Введите широту (в градусах): ");
            latitude = double.Parse(Console.ReadLine());

            Console.Write("Введите долготу (в градусах): ");
            longitude = double.Parse(Console.ReadLine());
        }
        catch (FormatException)
        {
            Console.WriteLine("Ошибка: Некорректный формат ввода данных.");
            return null;
        }

        return new Ship(shipNumber, new Coordinate(latitude, longitude));
    }

    // Метод для вывода координат корабля
    public void PrintShipCoordinates()
    {
        Console.WriteLine($"Координаты корабля с учетным номером {ShipNumber}: " +
                          $"Широта: {Coordinates.Latitude}°, Долгота: {Coordinates.Longitude}°");
    }

    // Метод для вычисления расстояния между двумя кораблями
    public double DistanceTo(Ship otherShip)
    {
        // Формула Гаверсинуса для расчета расстояния между точками на сфере
        const double EarthRadiusKm = 6371;
        double lat1 = Coordinates.Latitude * Math.PI / 180;
        double lat2 = otherShip.Coordinates.Latitude * Math.PI / 180;
        double lon1 = Coordinates.Longitude * Math.PI / 180;
        double lon2 = otherShip.Coordinates.Longitude * Math.PI / 180;

        double dLat = lat2 - lat1;
        double dLon = lon2 - lon1;

        double a = Math.Pow(Math.Sin(dLat / 2), 2) +
                   Math.Cos(lat1) * Math.Cos(lat2) * Math.Pow(Math.Sin(dLon / 2), 2);
        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        return EarthRadiusKm * c;
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Ввод данных о кораблях
        var ships = Enumerable.Range(1, 2).Select(_ =>
        {
            Ship ship = null;
            while (ship == null)
            {
                ship = Ship.InputShip();
            }
            return ship;
        }).ToList();

        // Вывод координат кораблей
        foreach (var ship in ships)
            ship.PrintShipCoordinates();

        // Вычисление и вывод расстояния между кораблями
        double distance = ships[0].DistanceTo(ships[1]);
        Console.WriteLine($"Расстояние между кораблями: {distance} км");
    }
}
