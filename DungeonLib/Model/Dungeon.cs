using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonLib.Model
{
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
            Map[currentx, currenty] = 17;

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

                while (!corridorCreation | corridorCreationAttempts < 3)
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
            Direction direction = Direction.North;
            bool result = false;
            if (Map[currentx, currenty] > 1) //Breaks -- Fixed?
            {
                return false;
            }


            for (int i = 0; i < 4; i++)
            {
                direction = (Direction)i;
                result = IsClearForRoom(direction, size);
                if (result)
                {
                    break;
                }
            }

            if (result)
            {
                AddRoom(size, direction);
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
                    for (int i = 0; i < size; i++)
                    {
                        distanceOut = (size / 2) + 1;
                        if (x - i < 0)
                        {
                            return false;
                        }
                        else if (Map[x - i, y] != 0)
                        {
                            return false;
                        }
                        else if (y + distanceOut >= Height)
                        {
                            return false;
                        }
                        else if (y - distanceOut < 0)
                        {
                            return false;
                        }
                    }
                    break;
                case Direction.East:
                    if (Map[x, y++] != 0)
                    {
                        return false;
                    }
                    for (int i = 0; i < size; i++)
                    {
                        distanceOut = (size / 2) + 1;
                        if (y + i < 0)
                        {
                            return false;
                        }
                        else if (Map[x, y + i] != 0)
                        {
                            return false;
                        }
                        else if (currentx + distanceOut >= Width)
                        {
                            return false;
                        }
                        else if (currentx - distanceOut < 0)
                        {
                            return false;
                        }
                    }
                    break;
                case Direction.South:
                    for (int i = 0; i < size; i++)
                    {
                        distanceOut = (size / 2) + 1;
                        if (x + i >= Width)
                        {
                            return false;
                        }
                        else if (Map[x + i, y] != 0)
                        {
                            return false;
                        }
                        else if (y + distanceOut >= Height)
                        {
                            return false;
                        }
                        else if (y - distanceOut < 0)
                        {
                            return false;
                        }
                    }
                    break;
                case Direction.West:
                    if (y - 1 < 0)
                    {
                        return false;
                    }
                    else if (Map[x, --y] != 0)
                    {
                        return false;
                    }
                    for (int i = 0; i < size; i++)
                    {
                        distanceOut = (size / 2) + 1;
                        if (y - i < 0)
                        {
                            return false;
                        }
                        else if (Map[x, y - i] != 0)
                        {
                            return false;
                        }
                        else if (currentx + distanceOut >= Width)
                        {
                            return false;
                        }
                        else if (currentx - distanceOut < 0)
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

                        Map[currentx - i, currenty] = currentRoomNum;
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
                    Map[currentx, currenty++] = 1;
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

                    Map[currentx, currenty--] = 1;
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

            List<Direction> directions = GetDirectionsPossible(length);

            if (directions.Count > 0)
            {
                result = true;
                AddCorridor(currentx, currenty, length, directions.ElementAt(GetRandom(0, directions.Count - 1)));
            }
            return result;
        }

        //Checks if the area in the given direction is clear for a corridor of the given length
        private bool IsClearForCorridor(Direction direction, int length)//, int x, int y)
        {
            bool result = true;
            switch (direction)
            {
                case Direction.North:
                    if (currentx - length < 0)
                    {
                        return false;
                    }
                    else
                    {
                        for (int i = 0; i < length; i++)
                        {
                            if (currentx - i < 0)
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
                            else if (Map[currentx - i, currenty] != 0)
                            {
                                return false;
                            }
                            else if (Map[currentx - i, currenty - 1] != 0)
                            {
                                return false;
                            }
                            else if (Map[currentx - i, currenty + 1] != 0)
                            {
                                return false;
                            }
                        }
                    }
                    break;
                case Direction.East:
                    if (currenty + length >= Height)
                    {
                        return false;
                    }
                    else
                    {
                        for (int i = 0; i < length; i++)
                        {
                            if (currenty + i >= Height)
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
                            else if (Map[currentx, currenty + i] != 0)
                            {
                                return false;
                            }
                            else if (Map[currentx - 1, currenty + i] != 0)
                            {
                                return false;
                            }
                            else if (Map[currentx + 1, currenty + i] != 0)
                            {
                                return false;
                            }
                        }
                    }
                    break;
                case Direction.South:
                    if ((currentx + length >= Width))
                    {
                        return false;
                    }
                    else
                    {
                        for (int i = 0; i < length; i++)
                        {
                            if (currentx + i >= Width)
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
                            else if (Map[currentx + i, currenty] != 0)
                            {
                                return false;
                            }
                            else if (Map[currentx + i, currenty - 1] != 0)
                            {
                                return false;
                            }
                            else if (Map[currentx + i, currenty + 1] != 0)//Breaks -- Fixed?
                            {
                                return false;
                            }
                        }
                    }
                    break;
                case Direction.West:
                    if (currenty - length < 0)
                    {
                        return false;
                    }
                    else
                    {
                        for (int i = 0; i < length; i++)
                        {
                            if (currenty - i < 0)
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
                            else if (Map[currentx, currenty - i] != 0)
                            {
                                return false;
                            }
                            else if (Map[currentx - 1, currenty - i] != 0)
                            {
                                return false;
                            }
                            else if (Map[currentx + 1, currenty - i] != 0)
                            {
                                return false;
                            }
                        }
                    }
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
            if (direction == Direction.North)
            {
                Console.WriteLine("North");
                for (int i = 0; i < length; i++)
                {
                    Map[x - i, y] = 1;
                }
                currentx -= length;
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


        //Returns a list of valid directions for creation -- Make this work for both rooms and corridors
        private List<Direction> GetDirectionsPossible(int length)
        {
            List<Direction> directions = new List<Direction>();
            Direction direction = Direction.North;
            int x = currentx;
            int y = currenty;


            bool result = false;
            for (int i = 0; i < 4; i++)//Note: I think I have flawed logic here with the x and y
            {
                result = false;
                direction = (Direction)i;
                GoToRoomsEdge(direction);
                result = IsClearForCorridor(direction, length);
                if (result)
                {
                    directions.Add(direction);
                }
            }
            return directions;
        }

        //Returns a list of valid directions for creation -- Make this work for both rooms and corridors
        private List<Direction> GetDirectionsPossibleRoom(int length)
        {
            List<Direction> directions = new List<Direction>();
            Direction direction = Direction.North;

            bool result = false;
            for (int i = 0; i < 4; i++)
            {
                result = false;
                direction = (Direction)i;
                result = IsClearForRoom(direction, length);
                if (result)
                {
                    directions.Add(direction);
                }
            }
            return directions;
        }

        //Puts current coordinates
        private void GoToRoomsEdge(Direction direction)
        {
            if (Map[currentx, currenty] > 1)//Breaks (IndexOutOfRangeException)
            {
                switch (direction)
                {
                    case Direction.North:
                        bool OnWall = false;
                        do
                        {
                            if (currenty - 1 < 0)
                            {
                                break;
                            }
                            //OnWall = (Map[currentx, --currenty] > 1);
                        } while (Map[currentx, --currenty] > 1);//Breaks -- Fixed?
                        break;
                    case Direction.East:
                        do
                        {
                            if (currentx + 1 >= Width)
                            {
                                break;
                            }
                        } while (Map[++currentx, currenty] > 1);//Breaks -- Fixed? -- Still Breaks -- Fixed?
                        break;
                    case Direction.South:
                        do
                        {
                            if (currenty + 1 > Height)
                            {
                                break;
                            }
                        } while (Map[currentx, ++currenty] > 1);//Breaks -- Fixed?
                        break;
                    case Direction.West:
                        do
                        {
                            if (currentx - 1 < 0)
                            {
                                break;
                            }
                        } while (Map[--currentx, currenty] > 1);
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
            for (int i = 0; i < Width - 1; i++)
            {
                string s = "";
                for (int j = 0; j < Height - 1; j++)
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
                    else if (Map[i, j] == 17)
                    {
                        s += " S ";
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
