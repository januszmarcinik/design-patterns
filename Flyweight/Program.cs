using System;
using System.Collections.Generic;
using System.Linq;

namespace Flyweight
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var registry = new MeasurementsRegistry(new List<Measurement>());

            var random = new Random();
            var date = new DateTime(2020, 5, 9);
            var places = new[] {"Podkarpackie", "Mazowieckie"};

            Console.WriteLine("-- Generowane pomiarów...");
            foreach (var place in places)
            {
                for (var stationNr = 1; stationNr <= 100; stationNr++)
                {
                    var pollution = random.Next(0, 1000);
                    registry.AddMeasurement(stationNr, pollution, place, date);
                }
            }
            
            registry.ShowMeasurementsLog();
            
            Console.WriteLine();
            var cachedObjectsCount = MeasurementPlaceFactory.GetNumberOfObjects();
            Console.WriteLine($"-- Liczba powtarzalnych obiektów: {cachedObjectsCount}");
        }
    }

    public class MeasurementsRegistry
    {
        private readonly List<Measurement> _measurements;

        public MeasurementsRegistry(List<Measurement> measurements)
        {
            _measurements = measurements;
        }

        public void AddMeasurement(int stationNr, double pollution, string place, DateTime date)
        {
            var measurement = new Measurement(
                stationNr,
                pollution,
                MeasurementPlaceFactory.Get(place, date));
            
            _measurements.Add(measurement);
        }

        public void ShowMeasurementsLog()
        {
            foreach (var m in _measurements)
            {
                Console.WriteLine($"{m.Place.Date.ToShortDateString()}: ({m.Place.Name}) [{m.StationNr}]: {m.Pollution}");
            }
        }
    }
    
    public class Measurement
    {
        public Measurement(int stationNr, double pollution, MeasurementPlace place)
        {
            StationNr = stationNr;
            Pollution = pollution;
            Place = place;
        }

        public int StationNr { get; }
        public double Pollution { get; }
        public MeasurementPlace Place { get; }
    }
    
    public class MeasurementPlace
    {
        public MeasurementPlace(string name, DateTime date)
        {
            Name = name;
            Date = date;
        }

        public string Name { get; }
        public DateTime Date { get; }
    }

    public static class MeasurementPlaceFactory
    {
        private static readonly List<MeasurementPlace> Places;

        static MeasurementPlaceFactory()
        {
            Places = new List<MeasurementPlace>();
        }

        public static MeasurementPlace Get(string name, DateTime date)
        {
            var place = Places.SingleOrDefault(x => x.Name == name && x.Date == date);
            if (place == null)
            {
                place = new MeasurementPlace(name, date);
                Places.Add(place);
            }

            return place;
        }

        public static int GetNumberOfObjects() => Places.Count;
    }
}