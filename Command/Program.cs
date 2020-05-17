using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Command
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var file = new MyFile("myFile.txt");
            var application = new Application(file);
            
            Console.WriteLine("-- Executing save values command...");
            application.ExecuteCommand(new SaveValuesCommand(file, new []{1, 2, 3, 4, 5}));
            
            Console.WriteLine("-- Executing read values command...");
            application.ExecuteCommand(new ReadValuesCommand(file));
            
            Console.WriteLine("-- Executing add arrays command...");
            application.ExecuteCommand(new AddArraysCommand(file, new []{5, 4, 3, 2, 1}));
            
            Console.WriteLine("-- Executing multiply command...");
            application.ExecuteCommand(new MultiplyCommand(file, 2));
            
            Console.WriteLine("-- Executing rollback command...");
            application.ExecuteCommand(new RollbackCommand(application));
            
            Console.WriteLine("-- Executing rollback command once again...");
            application.ExecuteCommand(new RollbackCommand(application));
            
            Console.ReadKey();
        }
    }

    public abstract class CommandBase
    {
        public bool ShouldBackup { get; }

        protected CommandBase(bool shouldBackup)
        {
            ShouldBackup = shouldBackup;
        }

        public abstract void Execute();
    }

    public class SaveValuesCommand : CommandBase
    {
        private readonly MyFile _file;
        private readonly int[] _values;

        public SaveValuesCommand(MyFile file, int[] values) : base(true)
        {
            _file = file;
            _values = values;
        }

        public override void Execute() => _file.Save(_values);
    }
    
    public class ReadValuesCommand : CommandBase
    {
        private readonly MyFile _file;

        public ReadValuesCommand(MyFile file) : base(false) => _file = file;

        public override void Execute() => Console.WriteLine($"Odczytuję dane: {_file.GetText()}");
    }
    
    public class AddArraysCommand : CommandBase
    {
        private readonly MyFile _file;
        private readonly int[] _values;

        public AddArraysCommand(MyFile file, int[] values) : base(true)
        {
            _file = file;
            _values = values;
        }

        public override void Execute()
        {
            var current = _file.GetValues();
            for (var i = 0; i < current.Length; i++)
            {
                current[i] += _values[i];
            }
            
            _file.Save(current);
        }
    }
    
    public class MultiplyCommand : CommandBase
    {
        private readonly MyFile _file;
        private readonly int _multiply;

        public MultiplyCommand(MyFile file, int multiply) : base(true)
        {
            _file = file;
            _multiply = multiply;
        } 

        public override void Execute()
        {
            var current = _file.GetValues();
            for (var i = 0; i < current.Length; i++)
            {
                current[i] *= _multiply;
            }
            
            _file.Save(current);
        }
    }
    
    public class RollbackCommand : CommandBase
    {
        private readonly Application _application;

        public RollbackCommand(Application application) : base(false) => _application = application;

        public override void Execute() => _application.RestoreLastChanges();
    }

    public class MyFile
    {
        private readonly string _fileName;

        public MyFile(string fileName)
        {
            _fileName = fileName;
            using (File.Create(_fileName))
            {
            }
        }

        public string GetText() => File.ReadAllText(_fileName);
        
        public int[] GetValues()
        {
            var text = GetText();
            if (string.IsNullOrEmpty(text))
            {
                return new int[] { };
            }
            
            return text
                .Split(",")
                .Select(x => Convert.ToInt32(x))
                .ToArray();
        }
        
        public void Save(int[] values)
        {
            var text = string.Join(",", values);
            File.WriteAllText(_fileName, text);
        }
    }

    public class Application
    {
        private readonly MyFile _file;
        private readonly Stack<int[]> _backups;

        public Application(MyFile file)
        {
            _file = file;
            _backups = new Stack<int[]>();
        }

        public void ExecuteCommand(CommandBase command)
        {
            if (command.ShouldBackup)
            {
                SaveBackup();
            }
            
            Console.WriteLine($"-- Before command execution: {_file.GetText()}");
            command.Execute();
            Console.WriteLine($"-- After command execution: {_file.GetText()}");
            Console.WriteLine();
        }

        public void RestoreLastChanges()
        {
            var backup = _backups.Pop();
            _file.Save(backup);
        }

        private void SaveBackup()
        {
            var backup = _file.GetValues();
            _backups.Push(backup);
        }
    }
}