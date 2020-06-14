using System;
using System.Collections.Generic;

namespace Iterator
{
     internal static class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Tablica z nazwami miast wojewódzkich:");
            var arrayCollection = new[]
            {
                "Rzeszów", "Gdańsk", "Warszawa", "Kraków", "Katowice", "Poznań"
            };
            Iterate(new ArrayIterableCollection<string>(arrayCollection));
            
            Console.WriteLine();

            Console.WriteLine("Lista powiązana z nazwami województw:");
            var listCollection = new List<string>
            {
                "podkarpackie", "pomorskie", "mazowieckie", "małopolskie", "śląskie", "wielkopolskie"
            };
            Iterate(new ListIterableCollection<string>(listCollection));
        }

        private static void Iterate<T>(IIterableCollection<T> collection)
        {
            var iterator = collection.CreateIterator();
            while (iterator.HasMore())
            {
                Console.WriteLine(iterator.GetNext());
            }
        }
    }
    
    public interface IIterableCollection<out T>
    {
        public IIterator<T> CreateIterator();
    }

    public class ArrayIterableCollection<T> : IIterableCollection<T>
    {
        private readonly T[] _array;

        public int Length => _array.Length;
        public T GetItem(int index) => _array[index];

        public ArrayIterableCollection(T[] array) => _array = array;

        public IIterator<T> CreateIterator() => new ArrayIterator<T>(this);
    }
    
    public class ListIterableCollection<T> : IIterableCollection<T>
    {
        private readonly List<T> _list;

        public int Length => _list.Count;
        public T GetItem(int index) => _list[index];

        public ListIterableCollection(List<T> list) => _list = list;

        public IIterator<T> CreateIterator() => new ListIterator<T>(this);
    }

    public interface IIterator<out T>
    {
         T GetNext();
         bool HasMore();
    }
    
    public class ArrayIterator<T> : IIterator<T>
    {
        private readonly ArrayIterableCollection<T> _collection;
        private int _currentPosition = 0;
        
        public ArrayIterator(ArrayIterableCollection<T> collection)
        {
            _collection = collection;
        }

        public T GetNext() => _collection.GetItem(_currentPosition++);
        public bool HasMore() => _collection.Length > _currentPosition;
    }

    public class ListIterator<T> : IIterator<T>
    {
        private readonly ListIterableCollection<T> _collection;
        private int _currentPosition = 0;
        
        public ListIterator(ListIterableCollection<T> collection)
        {
            _collection = collection;
        }

        public T GetNext() => _collection.GetItem(_currentPosition++);
        public bool HasMore() => _collection.Length > _currentPosition;
    }
}