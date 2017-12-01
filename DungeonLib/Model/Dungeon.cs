using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonLib.Model
{
    /*
     *  PROBLEMS LEFT TO FIX:
     *   - Room check fails on corners. Rooms overlap at corners.
     *   - Super lazer beam corridors.
     *   - Stops before finishing
     *   - Paul Fox still hasn't given us a pizza party yet :(
     */


    public class Dungeon
    {
        Random rand = new Random();
        public string DungeonName { get; private set; }
        public int[,] Map { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int NumOfEntrances { get; private set; }
        public int NumOfLootChests { get; private set; }
        public List<Monster> Monsters { get; private set; }
        public bool TrapDoors { get; private set; }

        private int currentRoomNum = 2;
        private int currentx;
        private int currenty;

        public Dungeon(string dungeonName, int width, int height, int numOfEntrances)
        {
            DungeonName = dungeonName;
            Width = width;
            Height = height;
            NumOfEntrances = numOfEntrances;

            GenerateMap(width, height);
        }

        public Dungeon(string dungeonName, int[,] map)
        {
            Map = map;
        }

        private void GenerateMap(int width, int height)
        {
            Map = new int[width, height];

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Map[i, j] = 0;
                }
            }

            Gen(width, height);
        }

        private void Gen(int width, int height)
        {
            currentx = GetRandom(0, width - 1);
            currenty = GetRandom(0, height - 1);
            Console.WriteLine($"{currentx}, {currenty}");
            //Map[currentx, currenty] = -50;

            int[] sizes = { 7, 3, 5 };

            bool roomCreation = false;
            bool corridorCreation = false;
            do
            {
                roomCreation = false;
                corridorCreation = false;

                int roomCreationAttempts = 0;
                int corridorCreationAttempts = 0;

                while (!roomCreation)
                {
                    roomCreation = TryMakeRoom(sizes[GetRandom(0, 2)]);
                    if (roomCreationAttempts == 3)
                    {
                        break;
                    }
                    roomCreationAttempts++;
                }

                while (!corridorCreation)
                {
                    corridorCreation = TryMakeCorridor(sizes[GetRandom(0, 2)]);
                    if (corridorCreationAttempts == 3)
                    {
                        break;
                    }
                    corridorCreationAttempts++;
                }
            }
            while (roomCreation || corridorCreation);
        }


        //There's a lot of repeat logic in my checks

        #region Room creation methods
        //Checks if a room can be made and makes it if it's possible
        private bool TryMakeRoom(int size)
        {
            bool result = false;
            if (Map[currentx, currenty] > 1) //Breaks -- Fixed?
            {
                return false;
            }

            List<Direction> directions = GetDirectionsPossible(size, CreationType.Room);

            if (directions.Count > 0)
            {
                result = true;
                AddRoom(size, directions.ElementAt(GetRandom(0, directions.Count - 1)));
            }
            return result;
        }

        //Checks if the area in the given direction is clear for a room of the given size
        private bool IsClearForRoom(Direction direction, int size)
        {
            bool result = true;
            int distanceOut = (size / 2);
            int x = currentx;
            int y = currenty;
            switch (direction)
            {
                case Direction.North:
                    if (y - distanceOut < 0 || y + distanceOut >= Height)
                    {
                        return false;
                    }
                    //CheckForRoom(size, currentx, currenty, -1, direction, distanceOut);
                    for (int i = 0; i < size; i++)
                    {
                        distanceOut = (size / 2);
                        if (x - i < 0)
                        {
                            return false;
                        }
                        else if (Map[x - i, y] != 0)
                        {
                            return false;
                        }
                        else if (Map[x - i, y - distanceOut] != 0 || Map[x - i, y + distanceOut] != 0)
                        {
                            return false;
                        }
                        else if (y - (distanceOut + 1) >= 0 && Map[x - i, y - (distanceOut + 1)] != 0)
                        {
                            return false;
                        }
                        else if (y + (distanceOut + 1) > Height && Map[x - i, y + (distanceOut + 1)] != 0)
                        {
                            return false;
                        }
                    }
                    break;
                case Direction.East:
                    if (x - distanceOut < 0 || x + distanceOut >= Width)
                    {
                        return false;
                    }
                    //CheckForRoom(size, currentx, currenty, 1, direction, distanceOut);

                    for (int i = 0; i < size; i++)
                    {
                        distanceOut = (size / 2);
                        if (y + i < 0)
                        {
                            return false;
                        }
                        else if (Map[x, y + i] != 0) //Breaks -- Fixed?
                        {
                            return false;
                        }
                        else if (Map[x - distanceOut, y + i] != 0 || Map[x + distanceOut, y + i] != 0)
                        {
                            return false;
                        }
                        else if (x - (distanceOut + 1) >= 0 && Map[x - (distanceOut + 1), y + i] != 0)
                        {
                            return false;
                        }
                        else if (x + (distanceOut + 1) > Width && Map[x + (distanceOut + 1), y + i] != 0)
                        {
                            return false;
                        }
                    }
                    break;
                case Direction.South:
                    if (y - distanceOut < 0 || y + distanceOut >= Height)
                    {
                        return false;
                    }
                    //CheckForRoom(size, currentx, currenty, 1, direction, distanceOut);
                    for (int i = 0; i < size; i++)
                    {
                        distanceOut = (size / 2);
                        if (x + i >= Width)
                        {
                            return false;
                        }
                        else if (Map[x + i, y] != 0)
                        {
                            return false;
                        }

                        else if (y - (distanceOut + 1) >= 0 && Map[x + i, y - (distanceOut + 1)] != 0)
                        {
                            return false;
                        }
                        else if (y + (distanceOut + 1) > Height && Map[x + i, y + (distanceOut + 1)] != 0)
                        {
                            return false;
                        }
                    }

                    break;
                case Direction.West:
                    if (x - distanceOut < 0 || x + distanceOut >= Width)
                    {
                        return false;
                    }
                    //CheckForRoom(size, currentx, currenty, -1, direction, distanceOut);
                    for (int i = 0; i < size; i++)
                    {
                        distanceOut = (size / 2);
                        if (y - i < 0)
                        {
                            return false;
                        }
                        else if (Map[x, y - i] != 0)
                        {
                            return false;
                        }
                        else if (Map[x - distanceOut, y - i] != 0 || Map[x + distanceOut, y - i] != 0)
                        {
                            return false;
                        }
                        else if (x - (distanceOut + 1) >= 0 && Map[x - (distanceOut + 1), y - i] != 0)
                        {
                            return false;
                        }
                        else if (x + (distanceOut + 1) > Width && Map[x + (distanceOut + 1), y - i] != 0)
                        {
                            return false;
                        }
                    }
                    break;
                default:
                    break;
            }
            return result;
        }

        // Adds a room to the given map 
        private void AddRoom(int size, Direction direction)
        {
            Console.WriteLine($"Room Size = {size}");
            int distanceOut = (size / 2);
            switch (direction)
            {
                case Direction.North:
                    Console.WriteLine("North");
                    for (int i = 0; i < size; i++)
                    {
                        distanceOut = (size / 2);

                        Map[currentx - i, currenty] = currentRoomNum;//Breaks
                        while (distanceOut > 0)
                        {
                            Map[currentx - i, currenty + distanceOut] = currentRoomNum;
                            Map[currentx - i,
                                currenty - distanceOut]
                                = currentRoomNum;//Breaks -- Fixed? -- Nope -- Fixed? -- Nope
                            distanceOut--;
                        }
                    }
                    break;
                case Direction.East:
                    Console.WriteLine("East");
                    //Map[currentx, currenty++] = 1;
                    for (int i = 0; i < size; i++)
                    {
                        distanceOut = (size / 2);
                        Map[currentx, currenty + i] = currentRoomNum;
                        while (distanceOut > 0)
                        {
                            Map[currentx + distanceOut, currenty + i] = currentRoomNum;//Breaks -- Fixed?
                            Map[currentx - distanceOut, currenty + i] = currentRoomNum;
                            distanceOut--;
                        }
                    }
                    break;
                case Direction.South:
                    Console.WriteLine("South");
                    for (int i = 0; i < size; i++)
                    {
                        distanceOut = (size / 2);
                        Map[currentx + i, currenty] = currentRoomNum;
                        while (distanceOut > 0)
                        {
                            Map[currentx + i, currenty + distanceOut] = currentRoomNum;
                            Map[currentx + i, currenty - distanceOut] = currentRoomNum;//Breaks -- Fixed? -- Nope -- Fixed? -- Nope
                            distanceOut--;
                        }
                    }
                    break;
                case Direction.West:
                    Console.WriteLine("West");

                    //Map[currentx, currenty--] = 1;
                    for (int i = 0; i < size; i++)
                    {
                        distanceOut = (size / 2);
                        Map[currentx, currenty - i] = currentRoomNum;//? (Don't know if this actully breaks..) -- Maybe? -- Yes it does break
                        while (distanceOut > 0)
                        {
                            Map[currentx + distanceOut, currenty - i] = currentRoomNum;//Breaks -- Fixed? -- Yes!! -- Nope....
                            Map[currentx - distanceOut, currenty - i] = currentRoomNum;//Breaks
                            distanceOut--;
                        }
                    }
                    break;
                default:
                    break;
            }
            currentRoomNum++;
        }
        #endregion


        #region Corridor creation methods
        //Checks if a corridor can be made and makes it if it's possible
        private bool TryMakeCorridor(int length)
        {
            bool result = false;

            List<Direction> directions = GetDirectionsPossible(length, CreationType.Corridor);

            if (directions.Count > 0)
            {
                result = true;
                AddCorridor(currentx, currenty, length, directions.ElementAt(GetRandom(0, directions.Count - 1)));
            }
            return result;
        }

        //Checks if the area in the given direction is clear for a corridor of the given length
        private bool IsClearForCorridor(Direction direction, int length)
        {
            bool result = true;
            switch (direction)
            {
                case Direction.North:
                    if (currentx - length < 0)
                    {
                        return false;
                    }
                    else if (currenty - 1 < 0)
                    {
                        return false;
                    }
                    else if (currenty + 1 >= Height)
                    {
                        return false;
                    }
                    result = CheckForCorridor(length, currentx, currenty, -1, Direction.North);
                    //for (int i = 0; i < length; i++)
                    //{
                    //    if (currentx - i < 0)
                    //    {
                    //        return false;
                    //    }
                    //    else if (Map[currentx - i, currenty] != 0)
                    //    {
                    //        return false;
                    //    }
                    //    else if (Map[currentx - i, currenty - 1] != 0)
                    //    {
                    //        return false;
                    //    }
                    //    else if (Map[currentx - i, currenty + 1] != 0)
                    //    {
                    //        return false;
                    //    }
                    //}
                    break;
                case Direction.East:
                    if (currenty + length >= Height)
                    {
                        return false;
                    }
                    else if (currentx - 1 < 0)
                    {
                        return false;
                    }
                    else if (currentx + 1 >= Width)
                    {
                        return false;
                    }
                    result = CheckForCorridor(length, currentx, currenty, 1, Direction.East);
                    //for (int i = 0; i < length; i++)
                    //{
                    //    if (currenty + i >= Height)
                    //    {
                    //        return false;
                    //    }
                    //    else if (Map[currentx, currenty + i] != 0)
                    //    {
                    //        return false;
                    //    }
                    //    else if (Map[currentx - 1, currenty + i] != 0)
                    //    {
                    //        return false;
                    //    }
                    //    else if (Map[currentx + 1, currenty + i] != 0)
                    //    {
                    //        return false;
                    //    }
                    //}
                    break;
                case Direction.South:
                    if ((currentx + length >= Width))
                    {
                        return false;
                    }
                    else if (currenty - 1 < 0)
                    {
                        return false;
                    }
                    else if (currenty + 1 >= Height)
                    {
                        return false;
                    }
                    result = CheckForCorridor(length, currentx, currenty, 1, Direction.South);
                    //for (int i = 0; i < length; i++)
                    //{
                    //    if (currentx + i >= Width)
                    //    {
                    //        return false;
                    //    }
                    //    else if (Map[currentx + i, currenty] != 0)
                    //    {
                    //        return false;
                    //    }
                    //    else if (Map[currentx + i, currenty - 1] != 0)
                    //    {
                    //        return false;
                    //    }
                    //    else if (Map[currentx + i, currenty + 1] != 0)//Breaks -- Fixed?
                    //    {
                    //        return false;
                    //    }
                    //}
                    break;
                case Direction.West:
                    if (currenty - length < 0)
                    {
                        return false;
                    }
                    else if (currentx + 1 >= Width)
                    {
                        return false;
                    }
                    else if (currentx - 1 < 0)
                    {
                        return false;
                    }
                    result = CheckForCorridor(length, currentx, currenty, -1, Direction.West);
                    //for (int i = 0; i < length; i++)
                    //{
                    //    if (currenty - i < 0)
                    //    {
                    //        return false;
                    //    }
                    //    else if (Map[currentx, currenty - i] != 0)
                    //    {
                    //        return false;
                    //    }
                    //    else if (Map[currentx - 1, currenty - i] != 0)
                    //    {
                    //        return false;
                    //    }
                    //    else if (Map[currentx + 1, currenty - i] != 0)
                    //    {
                    //        return false;
                    //    }
                    //}

                    break;
                default:
                    break;
            }
            return result;
        }

        // Adds a corridor to the given map 
        private void AddCorridor(int x, int y, int length, Direction direction)
        {
            Console.WriteLine($"Corridor Length = {length}");
            GoToRoomsEdge(direction);
            if (direction == Direction.North)
            {
                Console.WriteLine("North");
                for (int i = 0; i < length; i++)
                {
                    Map[x - i, y] = 1;
                    currentx--;
                }
            }
            else if (direction == Direction.East)
            {
                Console.WriteLine("East");
                for (int i = 0; i < length; i++)
                {
                    Map[x, y + i] = 1;
                }
                currenty += length;
            }
            else if (direction == Direction.South)
            {
                Console.WriteLine("South");
                for (int i = 0; i < length; i++)
                {
                    Map[x + i, y] = 1;
                }
                currentx += length;
            }
            else if (direction == Direction.West)
            {
                Console.WriteLine("West");
                for (int i = 0; i < length; i++)
                {
                    Map[x, y - i] = 1;
                }
                currenty -= length;
            }
        }
        #endregion

        #region Checks
        private bool CheckForCorridor(int length, int x, int y, int Mod, Direction direction)
        {
            //Works for North and South
            if (direction == Direction.North || direction == Direction.South)
            {
                for (int i = 0; i < length; i++)
                {
                    if (x + (i * Mod) < 0 || x + (i * Mod) >= Width)
                    {
                        return false;
                    }
                    else if (Map[x + (i * Mod), y] != 0)
                    {
                        return false;
                    }
                    else if (Map[x + (i * Mod), y - 1] != 0 || Map[x + (i * Mod), y + 1] != 0)
                    {
                        return false;
                    }
                }
            }

            //Works for East and West
            if (direction == Direction.East || direction == Direction.West)
            {
                for (int i = 0; i < length; i++)
                {
                    if (y + (i * Mod) < 0 || y + (i * Mod) >= Height)
                    {
                        return false;
                    }
                    else if (Map[x, y + (i * Mod)] != 0)
                    {
                        return false;
                    }
                    else if (Map[x - 1, y + (i * Mod)] != 0 || Map[x + 1, y + (i * Mod)] != 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private bool Check(int length, int x, int y, int Mod, Direction direction, int distanceOut)
        {
            //Works for North and South
            if (direction == Direction.North || direction == Direction.South)
            {
                for (int i = 0; i < length; i++)
                {
                    if (x + (i * Mod) < 0 || x + (i * Mod) >= Width)
                    {
                        return false;
                    }
                    else if (Map[x + (i * Mod), y] != 0)
                    {
                        return false;
                    }
                    else if (Map[x + (i * Mod), y - distanceOut] != 0 || Map[x + (i * Mod), y + distanceOut] != 0)
                    {
                        return false;
                    }
                }
            }

            //Works for East and West
            if (direction == Direction.East || direction == Direction.West)
            {
                for (int i = 0; i < length; i++)
                {
                    if (y + (i * Mod) < 0 || y + (i * Mod) >= Height)
                    {
                        return false;
                    }
                    else if (Map[x, y + (i * Mod)] != 0)
                    {
                        return false;
                    }
                    else if (Map[x - 1, y + (i * Mod)] != 0)
                    {
                        return false;
                    }
                    else if (Map[x + 1, y + (i * Mod)] != 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private bool CheckForRoom(int size, int x, int y, int Mod, Direction direction, int distanceOut)
        {
            //Works for North and South
            if (direction == Direction.North || direction == Direction.South)
            {
                for (int i = 0; i < size; i++)
                {
                    distanceOut = (size / 2);
                    if (x + (i * Mod) < 0 || x + (i * Mod) >= Width)
                    {
                        return false;
                    }
                    if (Map[x + (i * Mod), y] != 0)
                    {
                        return false;
                    }
                    if (Map[x + (i * Mod), y - distanceOut] != 0 || Map[x + (i * Mod), y + distanceOut] != 0)
                    {
                        return false;
                    }
                    if (y - (distanceOut + 1) >= 0)
                    {
                        if (Map[x + (i * Mod), y - (distanceOut + 1)] != 0)
                        {
                            return false;
                        }
                    }
                    if (y + (distanceOut + 1) > Height)
                    {
                        if (Map[x + (i * Mod), y + (distanceOut + 1)] != 0)
                        {
                            return false;
                        }
                    }
                }
            }

            //Works for East and West
            if (direction == Direction.East || direction == Direction.West)
            {
                for (int i = 0; i < size; i++)
                {
                    distanceOut = (size / 2);
                    if (y + (i * Mod) < 0 || x + (i * Mod) >= Height)
                    {
                        return false;
                    }
                    if (Map[x, y + (i * Mod)] != 0) //Breaks -- Fixed?
                    {
                        return false;
                    }
                    if (Map[x - distanceOut, y + (i * Mod)] != 0 || Map[x + distanceOut, y + (i * Mod)] != 0) //Breaks -- Fixed?
                    {
                        return false;
                    }
                    if (x - (distanceOut + 1) >= 0)
                    {
                        if (Map[x - (distanceOut + 1), y + (i * Mod)] != 0)
                        {
                            return false;
                        }
                    }
                    if (x + (distanceOut + 1) > Width)
                    {
                        if (Map[x + (distanceOut + 1), y + (i * Mod)] != 0)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        #endregion

        //Returns a list of valid directions for creation
        private List<Direction> GetDirectionsPossible(int size, CreationType creationType)
        {
            List<Direction> directions = new List<Direction>();
            Direction direction = Direction.North;
            int x = currentx;
            int y = currenty;

            bool result = false;
            for (int i = 0; i < 4; i++)
            {
                result = false;
                direction = (Direction)i;

                if (creationType == CreationType.Corridor)
                {
                    currentx = x;
                    currenty = y;
                    GoToRoomsEdge(direction);
                    result = IsClearForCorridor(direction, size);
                }
                else
                {
                    result = IsClearForRoom(direction, size);
                }

                if (result)
                {
                    directions.Add(direction);
                }
            }
            return directions;
        }

        //Sets current coords to rooms edge
        private void GoToRoomsEdge(Direction direction)
        {
            if (Map[currentx, currenty] > 1)
            {
                switch (direction)
                {
                    case Direction.North:
                        do
                        {
                            if (currentx - 1 < 0)
                            {
                                break;
                            }
                        } while (Map[--currentx, currenty] > 1);
                        break;
                    case Direction.East:
                        do
                        {
                            if (currenty + 1 >= Height)
                            {
                                break;
                            }
                        } while (Map[currentx, ++currenty] > 1);//Breaks -- Fixed?
                        break;
                    case Direction.South:

                        do
                        {
                            if (currentx + 1 >= Width)
                            {
                                break;
                            }
                        } while (Map[++currentx, currenty] > 1);//Breaks -- Fixed? -- Still Breaks -- Fixed?
                        break;
                    case Direction.West:

                        do
                        {
                            if (currenty - 1 < 0)
                            {
                                break;
                            }
                        } while (Map[currentx, --currenty] > 1);//Breaks -- Fixed?
                        break;
                    default:
                        break;
                }
            }
        }


        // Takes in an inclusive upper and lower bound and returns a random int between between the x and y parameters
        public int GetRandom(int x, int y)
        {
            return rand.Next(x, y + 1);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Width; i++)
            {
                string s = "";
                for (int j = 0; j < Height; j++)
                {
                    s = "";
                    if (Map[i, j] == 0)
                    {
                        s += " . ";
                    }
                    else if (Map[i, j] == 1)
                    {
                        s += " 1 ";
                    }
                    else if (Map[i, j] > 9)
                    {
                        s += $" {(char)(87 + Map[i, j])} ";

                    }
                    else
                    {
                        s += $" {Map[i, j]} ";
                    }
                    sb.Append(s);
                }
                sb.Append("\n");
            }
            return sb.ToString();
        }
    }
}
