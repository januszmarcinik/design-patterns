using System;

namespace State
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var robot = new Robot();
            var key = Console.ReadKey();
            do
            {
                switch (key.KeyChar)
                {
                    case 'w': robot.MoveUp();
                        break;
                    case 's': robot.MoveDown();
                        break;
                    case 'a': robot.MoveLeft();
                        break;
                    case 'd': robot.MoveRight();
                        break;
                }

                key = Console.ReadKey();
            } while (key.KeyChar != 'e');
        }
    }

    public class Robot
    {
        private MovingState _movingState;
        private int _positionX;
        private int _positionY;

        public Robot()
        {
            _movingState = new RestingState(this);
            _positionX = 50;
            _positionY = 50;
        }

        public void MoveUp()
        {
            var (x, y) = _movingState.MoveUp();
            Console.WriteLine($"Turned up: {MakeMove(x, y)}");
        }

        public void MoveDown()
        {
            var (x, y) = _movingState.MoveDown();
            Console.WriteLine($"Turned down: {MakeMove(x, y)}");
        }

        public void MoveLeft()
        {
            var (x, y) = _movingState.MoveLeft();
            Console.WriteLine($"Turned left: {MakeMove(x, y)}");
        }

        public void MoveRight()
        {
            var (x, y) = _movingState.MoveRight();
            Console.WriteLine($"Turned right: {MakeMove(x, y)}");
        }

        public void SetMovingState(MovingState movingState) => _movingState = movingState;

        private string MakeMove(int x, int y)
        {
            _positionX += x;
            _positionY += y;
            return $"(X:{_positionX}, Y:{_positionY})";
        }
    }
    
    public abstract class MovingState
    {
        public abstract (int x, int y) MoveUp();
        public abstract (int x, int y) MoveDown();
        public abstract (int x, int y) MoveLeft();
        public abstract (int x, int y) MoveRight();
        protected static (int x, int y) NoMove() => (0, 0);
    }

    public class MovingUpState : MovingState
    {
        private readonly Robot _robot;
        public MovingUpState(Robot robot) => _robot = robot;

        public override (int x, int y) MoveUp() => (0, 1);
        public override (int x, int y) MoveDown()
        {
            _robot.SetMovingState(new RestingState(_robot));
            return NoMove();
        }

        public override (int x, int y) MoveLeft() => NoMove();
        public override (int x, int y) MoveRight() => NoMove();
    }

    public class RestingState : MovingState
    {
        private readonly Robot _robot;
        public RestingState(Robot robot)
        {
            Console.WriteLine("-- Robot centered --");
            _robot = robot;
        }

        public override (int x, int y) MoveUp()
        {
            var state = new MovingUpState(_robot);
            _robot.SetMovingState(state);
            return state.MoveUp();
        }
        public override (int x, int y) MoveDown()
        {
            var state = new MovingDownState(_robot);
            _robot.SetMovingState(state);
            return state.MoveDown();
        }
        public override (int x, int y) MoveLeft()
        {
            var state = new MovingLeftState(_robot);
            _robot.SetMovingState(state);
            return state.MoveLeft();
        }
        public override (int x, int y) MoveRight()
        {
            var state = new MovingRightState(_robot);
            _robot.SetMovingState(state);
            return state.MoveRight();
        }
    }
    
    public class MovingDownState : MovingState
    {
        private readonly Robot _robot;
        public MovingDownState(Robot robot) => _robot = robot;
        
        public override (int x, int y) MoveUp()
        {
            _robot.SetMovingState(new RestingState(_robot));
            return NoMove();
        }
        public override (int x, int y) MoveDown() => (0, -1);
        public override (int x, int y) MoveLeft() => NoMove();
        public override (int x, int y) MoveRight() => NoMove();
    }
    
    public class MovingLeftState : MovingState
    {
        private readonly Robot _robot;
        public MovingLeftState(Robot robot) => _robot = robot;

        public override (int x, int y) MoveUp() => NoMove();
        public override (int x, int y) MoveDown() => NoMove();
        public override (int x, int y) MoveLeft() => (-1, 0);
        public override (int x, int y) MoveRight()
        {
            _robot.SetMovingState(new RestingState(_robot));
            return NoMove();
        }
    }
    
    public class MovingRightState : MovingState
    {
        private readonly Robot _robot;
        public MovingRightState(Robot robot) => _robot = robot;

        public override (int x, int y) MoveUp() => NoMove();
        public override (int x, int y) MoveDown() => NoMove();
        public override (int x, int y) MoveLeft()
        {
            _robot.SetMovingState(new RestingState(_robot));
            return NoMove();
        }
        public override (int x, int y) MoveRight() => (1, 0); 
    }
}