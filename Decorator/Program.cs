using System;
using System.Linq;

namespace Decorator
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            IComponent component = new SumComponent();
            
            // --------------------------------------------------------------------------
            Console.WriteLine("-- Bez dekoratorów:");
            MyCode(component);
            Console.WriteLine();
            
            // --------------------------------------------------------------------------
            Console.WriteLine("-- Dodany dekorator sumujcy wartości bezwzględne:");
            
            component = new AbsoluteSumDecorator(component);
            
            MyCode(component);
            Console.WriteLine();
            
            // --------------------------------------------------------------------------
            Console.WriteLine("-- Dodany dekorator sumujący liczby dodatnie:");
            
            component = new PositiveSumDecorator(component);
            
            MyCode(component);
            Console.WriteLine();
        }

        private static void MyCode(IComponent component)
        {
            // My code is not aware of the attached decorators. 
            var values = new[] {10, 5, 20, 35, -15, -10, -5};
            component.Count(values);
        }
    }
    
    // Abstract decorator
    public abstract class SumComponentDecoratorBase : IComponent
    {
        private readonly IComponent _component;

        protected SumComponentDecoratorBase(IComponent component)
        {
            _component = component;
        }

        public virtual void Count(int[] values)
        {
            _component.Count(values);
        }
    }
    
    // Absolute sum decorator
    public class AbsoluteSumDecorator : SumComponentDecoratorBase
    {
        public AbsoluteSumDecorator(IComponent component) 
            : base(component)
        {
        }
        
        public override void Count(int[] values)
        {
            base.Count(values);
            
            var value = values == null || values.Length == 0
                ? 0
                : values
                    .Select(Math.Abs)
                    .Sum(x => x);

            Console.WriteLine($"Suma wartości bezzwględnych elementów tablicy: {value}");
        }
    }

    // Positive numbers sum decorator
    public class PositiveSumDecorator : SumComponentDecoratorBase
    {
        public PositiveSumDecorator(IComponent component) 
            : base(component)
        {
        }
        
        public override void Count(int[] values)
        {
            base.Count(values);
            
            var value = values == null || values.Length == 0
                ? 0
                : values
                    .Where(x => x > 0)
                    .Sum(x => x);

            Console.WriteLine($"Suma wartości dodatnich elementów tablicy: {value}");
        }
    }

    // Component abstraction
    public interface IComponent
    {
        void Count(int[] values);
    }
    
    // Concrete component
    public class SumComponent : IComponent
    {
        public void Count(int[] values)
        {
            var value = values == null || values.Length == 0
                ? 0
                : values.Sum(x => x);

            Console.WriteLine($"Wartość sumy elementów tablicy: {value}");
        }
    }
}