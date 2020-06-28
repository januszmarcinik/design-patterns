using System;
using System.Collections.Generic;

namespace Composite
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var administrationUnits = new List<IAdministrationUnit>
            {
                new Voivodeship("Podkarpackie", new[]
                {
                    new County("Leski", new[]
                    {
                        new Community("Baligród"),
                        new Community("Lesko"),
                        new Community("Solina"),
                        new Community("Cisna")
                    }),
                    new County("Bieszczadzki", new[]
                    {
                        new Community("Olszanica"),
                        new Community("Ustrzyki Dolne"),
                        new Community("Czarna"),
                        new Community("Lutowiska")
                    })
                }),
                new Voivodeship("Lubelskie", new[]
                {
                    new County("Biłgorajski", new[]
                    {
                        new Community("Biłgoraj"),
                        new Community("Frampol"),
                        new Community("Tarnogród")
                    }),
                    new County("Hrubieszowski", new[]
                    {
                        new Community("Poturzyn"),
                        new Community("Telatyn")
                    })
                }),
            };

            foreach (var unit in administrationUnits)
            {
                unit.ShowDetails();
            }
        }
    }

    public interface IAdministrationUnit
    {
        void ShowDetails();
    }
    
    public class Voivodeship : IAdministrationUnit
    {
        private readonly string _name;
        private readonly IEnumerable<IAdministrationUnit> _dependentUnits;

        public Voivodeship(string name, IEnumerable<IAdministrationUnit> dependentUnits)
        {
            _name = name;
            _dependentUnits = dependentUnits;
        }
        
        public void ShowDetails()
        {
            Console.WriteLine($"- Województwo: {_name}");
            foreach (var unit in _dependentUnits)
            {
                unit.ShowDetails();
            }
        }
    }

    public class County : IAdministrationUnit
    {
        private readonly string _name;
        private readonly IEnumerable<IAdministrationUnit> _dependentUnits;

        public County(string name, IEnumerable<IAdministrationUnit> dependentUnits)
        {
            _name = name;
            _dependentUnits = dependentUnits;
        }
        
        public void ShowDetails()
        {
            Console.WriteLine($"--- Powiat: {_name}");
            foreach (var unit in _dependentUnits)
            {
                unit.ShowDetails();
            }
        }
    }

    public class Community : IAdministrationUnit
    {
        private readonly string _name;

        public Community(string name) => _name = name;

        public void ShowDetails() => Console.WriteLine($"----- Gmina: {_name}");
    }
}