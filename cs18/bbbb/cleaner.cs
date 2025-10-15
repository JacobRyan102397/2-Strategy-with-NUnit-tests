using System;
namespace RobotCleaner
{
  public class Map
  {
    private enum CellType { Empty, Dirt, Obstacle, Cleaned };
    private CellType[,] _grid;
    public int Width {get; private set;}
    public int Height {get; private set;}

    public Map(int width, int height)
    {
      this.Width = width;
      this.Height = height;
      _grid = new CellType[width, height];
      for (int x = 0; x < width; x++)
      {
        for (int y = 0; y < height; y++ )
        {
          _grid[x,y] = CellType.Empty;
        }
      }
    }

    public bool IsInBounds(int x, int y)
    {
      return x >= 0 && x < this.Width && y >= 0 && y < this.Height;
    }

    public bool IsDirt(int x, int y){
      return IsInBounds(x,y) && _grid[x,y] == CellType.Dirt;
    }

    public bool IsObstacle(int x, int y){
      return IsInBounds(x,y) && _grid[x,y] == CellType.Obstacle;
    }

    public void AddObstacle(int x, int y)
    {
      _grid[x, y] = CellType.Obstacle;
    }
    public void AddDirt(int x, int y)
    {
      _grid[x, y] = CellType.Dirt;
    }

    public void Clean(int x, int y)
    {
      if( IsInBounds(x,y))
      {
        _grid[x, y] = CellType.Cleaned;
      }
    }
    public void Display(int robotX, int robotY)
    {
      // display the 2d grid, it accepts the location of the robot in x and y
      Console.Clear();
      Console.WriteLine("Vacuum cleaner robot simulation");
      Console.WriteLine("--------------------------------");
      Console.WriteLine("Legends: #=Obstacles, D=Dirt, .=Empty, R=Robot, C=Cleaned");

      //display the grid using loop
      for (int y = 0; y < this.Height; y++)
      {
        for (int x = 0; x < this.Width; x++)
        {
          if( x==robotX && y == robotY)
          {
            Console.Write("R ");
          }
          else
          {
            switch(_grid[x,y])
            {
              case CellType.Empty: Console.Write(". "); break;
              case CellType.Dirt: Console.Write("D "); break;
              case CellType.Obstacle: Console.Write("# "); break;
              case CellType.Cleaned: Console.Write("C "); break;
            }
          }
        }
        Console.WriteLine();
      } //outer for loop
      // add delay
      Thread.Sleep(200);
    } // display method
  }//class map
  public interface IStrategy
  {
    void Clean(Robot robot);
  }

  public class Robot
  {
    private readonly Map _map;
    private readonly IStrategy _strategy;

    public int X {get; set;}
    public int Y {get; set;}

    public Map Map { get { return _map;}}

    public Robot(Map map, IStrategy strategy)
    {
      _map = map;
      _strategy = strategy;
      X = 0;
      Y = 0;
    }

    public bool Move(int newX, int newY)
    {
      if( _map.IsInBounds(newX, newY) && !_map.IsObstacle(newX, newY) )
      {
        // set the new location
        X = newX;
        Y = newY;
        // display the map with the robot in its location in the grid
        _map.Display(X, Y);
          return true;
      }
      // it cannot move
      return false;
    }// Move method

    public void CleanCurrentSpot()
    {
      if(_map.IsDirt(X, Y))
      {
        _map.Clean(X, Y);
        _map.Display(X, Y);
      }
    }

    public void StartCleaning()
    {
      _strategy.Clean(this);
    }
  }

 public class SomeStrategy : IStrategy
  {
    public void Clean(Robot robot)
    {
        int direction = 1; // 1 = right, -1 = left
        for (int y = 0; y < robot.Map.Height; y++)
        {
            int startX = (direction == 1) ? 0 : robot.Map.Width - 1;
            int endX = (direction == 1) ? robot.Map.Width : -1;
            
            for (int x = startX; x != endX; x += direction)
            {
                robot.Move(x, y);
                robot.CleanCurrentSpot();
            }
            direction *= -1; // Reverse direction for the next row
        }
    }
  }

<<<<<<< Updated upstream
  public class Program
=======
    public class PerimeterHuggerStrategy : IStrategy
    {
        public void Clean(Map map, Robot robot)
        {
            // Move Right
            while (robot.Move(robot.X + 1, robot.Y))
            {
                robot.CleanCurrentSpot();
            }

            // Move Down
            while (robot.Move(robot.X, robot.Y + 1))
            {
                robot.CleanCurrentSpot();
            }

            // Move Left
            while (robot.Move(robot.X - 1, robot.Y))
            {
                robot.CleanCurrentSpot();
            }

            // Move Up (back to start)
            while (robot.Move(robot.X, robot.Y - 1))
            {
                robot.CleanCurrentSpot();
            }
        }
    }

    public class SpiralStrategy : IStrategy
    {
        public void Clean(Map map, Robot robot)
        {
            // Directions: Right → Down → Left → Up (clockwise spiral)
            int[,] directions = new int[,] { { 1, 0 }, { 0, 1 }, { -1, 0 }, { 0, -1 } };
            int dirIndex = 0; // start moving right

            int segmentLength = 1; // how far to move before turning
            int stepsTaken = 0;    
            int turns = 0;         

            bool canMove = true;

            robot.CleanCurrentSpot(); 

            while (canMove)
            {
                for (int i = 0; i < segmentLength; i++)
                {
                    int nextX = robot.X + directions[dirIndex, 0];
                    int nextY = robot.Y + directions[dirIndex, 1];

                    if (!robot.Move(nextX, nextY))
                    {
                        canMove = false; // stop if robot hits boundary
                        break;
                    }

                    robot.CleanCurrentSpot();
                    stepsTaken++;
                }

                if (!canMove)
                    break;

                dirIndex = (dirIndex + 1) % 4;
                turns++;
                stepsTaken = 0;

                // Every two turns, increase spiral size
                if (turns % 2 == 0)
                {
                    segmentLength++;
                }
            }
        }
    }

    public class Program
>>>>>>> Stashed changes
  {

    public static void Main(string[] args){
      Console.WriteLine("Initialize robot");


<<<<<<< Updated upstream
      IStrategy some_strategy = new SomeStrategy();
=======
            //IStrategy some_strategy = new SomeStrategy();
            //IStrategy hugger_strategy = new PerimeterHuggerStrategy();
            IStrategy strategy = new SpiralStrategy();
>>>>>>> Stashed changes

      Map map = new Map(20, 10);
      // map.Display( 10,10);

      map.AddDirt(5,3);
      map.AddDirt(10, 8);
      map.AddObstacle(1,1);
      map.AddObstacle(2,5);
      map.Display(12,1);

      Robot robot = new Robot(map,strategy);

      robot.StartCleaning();

      Console.WriteLine("Done.");
    }
  }
}

