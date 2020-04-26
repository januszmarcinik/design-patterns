using System;
using System.Linq;

namespace FactoryMethod
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Obliczanie sumy elementów tablicy liczb zmiennoprzecinkowych.");
            MyCode(new SumCounterCreator());
            
            Console.WriteLine();

            Console.WriteLine("Obliczanie sumy wartości bezwzględnych elementów tablicy liczb zmiennoprzecinkowych.");
            MyCode(new AbsoluteCounterCreator());
        }

        private static void MyCode(CounterCreator creator)
        {
            // My code is not aware of the creator's class. 
            var values = new[] {11.2, 0.01, 1.23, -4, 5.5, -9.01};
            creator.DoCount(values);
        }
    }
    
    // Abstract creator
    public abstract class CounterCreator
    {
        protected abstract ICounter GetCounter();
        
        public void DoCount(double[] values)
        {
            var counter = GetCounter();
            counter.Count(values);
        }
    }

    // First creator
    public class SumCounterCreator : CounterCreator
    {
        protected override ICounter GetCounter() => new SumCounter();
    }
    
    // Second creator
    public class AbsoluteCounterCreator : CounterCreator
    {
        protected override ICounter GetCounter() => new AbsoluteCounter();
    }

    // Product abstraction
    public interface ICounter
    {
        void Count(double[] values);
    }
    
    // First product
    public class SumCounter : ICounter
    {
        public void Count(double[] values)
        {
            var value = values == null || values.Length == 0
                ? 0
                : values.Sum(x => x);

            Console.WriteLine($"Wartość sumy elementów tablicy: {value}");
        }
    }

    // Second product counter
    public class AbsoluteCounter : ICounter
    {
        public void Count(double[] values)
        {
            var value = values == null || values.Length == 0
                ? 0
                : values
                    .Select(Math.Abs)
                    .Sum(x => x);

            Console.WriteLine($"Wartość sumy elementów tablicy: {value}");
        }
    }
}