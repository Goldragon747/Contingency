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
            //List<int[,]> visited = new List<int[,]>();
            //List<int[,]> backtrack = new List<int[,]>();

            currentx = GetRandom(0, width - 1);
            currenty = GetRandom(0, height - 1);
            //Map[currentx, currenty] = 1;
            //visited.Add(new int[,] { { currentx, currenty } });
            //backtrack.Add(new int[,] { { currentx, currenty } });

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
                    roomCreation = TryMakeRoom(sizes[roomCreationAttempts++]);
                    if (roomCreationAttempts == 3)
                        break;
                }
                while (!corridorCreation | corridorCreationAttempts < 3)
                {
                    corridorCreation = TryMakeCorridor(sizes[corridorCreationAttempts++]);
                    if (corridorCreationAttempts == 3)
                        break;
                }
            }
            while (roomCreation || corridorCreation);
        }

        //Checks if a corridor can be made and makes it if it's possible
        private bool TryMakeCorridor(int length)
        {
            bool result = false;
            Direction direction = Direction.North;
            bool inRoom = false;

            int x = currentx;
            int y = currenty;

            if (Map[currentx, currenty] > 1)
            {
                inRoom = true;
            }

            for (int i = 0; i < 4; i++)//Note: I think I have flawed logic here with the x and y
            {
                direction = (Direction)i;
                if (inRoom)
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
                            } while (Map[currentx, --currenty ] > 1);//Breaks -- Fixed?
                            break;
                        case Direction.East:
                            do
                            {
                                if(currentx + 1 >= Width)
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
                                currentx--;
                            } while (Map[currentx - 1, currenty] > 1);
                            break;
                        default:
                            break;
                    }
                }

                result = IsClearForCorridor(direction, length);
                if (result)
                {
                    break;
                }
            }

            if (result)
            {
                AddCorridor(currentx, currenty, length, direction);
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
                    for (int i = 0; i < length; i++)
                    {
                        if(currenty - i < 0)
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
                    break;
                case Direction.East:
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
                        else if (Map[currentx + i, currenty + 2] != 0)
                        {
                            return false;
                        }
                    }
                    break;
                case Direction.South:
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
                    break;
                case Direction.West:
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
                    break;
                default:
                    break;
            }
            return result;
        }

        // Adds a corridor to the given map 
        private void AddCorridor(int x, int y, int length, Direction direction)
        {
            if (direction == Direction.North)
            {
                for (int i = 1; i < length; i++)
                {
                    Map[x, y - i] = 1;
                    currenty--;
                }
            }
            else if (direction == Direction.East)
            {
                for (int i = 1; i < length; i++)
                {
                    Map[x + i, y] = 1;
                    currentx++;
                }
            }
            else if (direction == Direction.South)
            {
                for (int i = 1; i < length; i++)
                {
                    Map[x, y + i] = 1;
                    currenty++;
                }
            }
            else if (direction == Direction.West)
            {
                for (int i = 1; i < length; i++)
                {
                    Map[x - i, y] = 1;
                    currentx--;
                }
            }
        }

        //Checks if a room can be made and makes it if it's possible
        private bool TryMakeRoom(int size)
        {
            Direction direction = Direction.North;
            bool result = false;
            if (Map[currentx, currenty] > 1)
            {
                return false;
            }


            for (int i = 0; i < 4; i++)//Note: I think I have flawed logic here with the x and y
            {
                direction = (Direction)i;
                result = IsClearForCorridor(direction, size);
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
                    if (Map[x, --y] != 0)
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
                        else if (currentx + distanceOut > Width)
                        {
                            return false;
                        }
                        else if (currentx - distanceOut < 0)
                        {
                            return false;
                        }
                        else
                        {
                            while (distanceOut > 0)
                            {
                                if (Map[x + distanceOut, y - i] != 0)
                                {
                                    return false;
                                }
                                else if (Map[x - distanceOut, y - i] != 0)
                                {
                                    return false;
                                }
                                distanceOut--;
                            }
                        }
                    }
                    break;
                case Direction.East:
                    for (int i = 0; i < size; i++)
                    {
                        distanceOut = (size / 2) + 1;
                        if (x + i > Width)
                        {
                            return false;
                        }
                        else if (Map[x + i, y] != 0)
                        {
                            return false;
                        }
                        else if (y + distanceOut > Height)
                        {
                            return false;
                        }
                        else if (y - distanceOut < 0)
                        {
                            return false;
                        }
                        else
                        {
                            while (distanceOut > 0)
                            {
                                if (Map[x + i, y - distanceOut] != 0)
                                {
                                    return false;
                                }
                                else if (Map[x + i, y + distanceOut] != 0)
                                {
                                    return false;
                                }
                                distanceOut--;
                            }
                        }
                    }
                    break;
                case Direction.South:
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
                        else if (currentx + distanceOut > Width)
                        {
                            return false;
                        }
                        else if (currentx - distanceOut < 0)
                        {
                            return false;
                        }
                        else
                        {
                            while (distanceOut > 0)
                            {
                                if (Map[x + distanceOut, y + i] != 0)
                                {
                                    return false;
                                }
                                else if (Map[x - distanceOut, y + i] != 0)
                                {
                                    return false;
                                }
                                distanceOut--;
                            }
                        }
                    }
                    break;
                case Direction.West:
                    for (int i = 0; i < size; i++)
                    {
                        distanceOut = (size / 2) + 1;
                        if (x + i > Width)
                        {
                            return false;
                        }
                        else if (Map[x - i, y] != 0)
                        {
                            return false;
                        }
                        else if (y + distanceOut > Height)
                        {
                            return false;
                        }
                        else if (y - distanceOut < 0)
                        {
                            return false;
                        }
                        else
                        {
                            while (distanceOut > 0)
                            {
                                if (Map[x - i, y - distanceOut] != 0)
                                {
                                    return false;
                                }
                                else if (Map[x - i, y + distanceOut] != 0)
                                {
                                    return false;
                                }
                                distanceOut--;
                            }
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
            int distanceOut = (size / 2);

            if (direction == Direction.North)
            {
                Map[currentx, --currenty] = 1;

                for (int i = 0; i < size; i++)
                {
                    distanceOut = (size / 2);

                    Map[currentx, currenty - i] = currentRoomNum;//? (Don't know if this actully breaks..)
                    while (distanceOut > 0)
                    {
                        Map[currentx + distanceOut, currenty - i] = currentRoomNum;//Breaks
                        Map[currentx - distanceOut, currenty - i] = currentRoomNum;
                        distanceOut--;
                    }
                }
            }
            else if (direction == Direction.East)
            {
                for (int i = 0; i < size; i++)
                {
                    distanceOut = (size / 2);

                    Map[currentx + i, currenty] = currentRoomNum;
                    while (distanceOut > 0)
                    {
                        Map[currentx + i, currenty + distanceOut] = currentRoomNum;
                        Map[currentx + i, currenty - distanceOut] = currentRoomNum;//Breaks
                        distanceOut--;
                    }
                }
            }
            else if (direction == Direction.South)
            {
                Map[currentx, ++currenty] = 1;

                for (int i = 0; i < size; i++)
                {
                    distanceOut = (size / 2);

                    Map[currentx, currenty + i] = currentRoomNum;
                    while (distanceOut > 0)
                    {
                        Map[currentx + distanceOut, currenty + i] = currentRoomNum;//Breaks
                        Map[currentx - distanceOut, currenty + i] = currentRoomNum;
                        distanceOut--;
                    }
                }
            }
            else if (direction == Direction.West)
            {
                for (int i = 0; i < size; i++)
                {
                    distanceOut = (size / 2);

                    Map[currentx - i, currenty] = currentRoomNum;
                    while (distanceOut > 0)
                    {
                        Map[currentx - i, currenty + distanceOut] = currentRoomNum;
                        Map[currentx - i, currenty - distanceOut] = currentRoomNum;//Breaks
                        distanceOut--;
                    }
                }
            }

            currentRoomNum++;
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
