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

        public Dungeon(string dungeonName, int width, int height, int numOfEntrances)
        {
            DungeonName = dungeonName;
            Width = width;
            Height = height;
            NumOfEntrances = numOfEntrances;

            GenerateMap(width, height);
        }

        private void GenerateMap(int width, int height)
        {
            int[,] map = new int[width, height];

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    map[i, j] = 0;

                    //if (i % 2 == 0)
                    //{
                    //    map[i, j] = 0;
                    //}
                    //else
                    //{
                    //    if (j % 2 == 0)
                    //    {
                    //        map[i, j] = 0;
                    //    }
                    //    else
                    //    {
                    //        map[i, j] = 3;
                    //    }
                    //}
                }
            }

            map = Gen(map, width, height);

            Map = map;
        }

        private int[,] Gen(int[,] map, int width, int height)
        {
            List<int[,]> visited = new List<int[,]>();
            List<int[,]> backtrack = new List<int[,]>();

            #region Get starting position
            int x = 0;
            int y = 0;
            //while (map[x, y] != 3)
            //{
            x = GetRandom(0, width - 1);
            y = GetRandom(0, height - 1);
            //}
            map[x, y] = 1;
            visited.Add(new int[,] { { x, y } });
            backtrack.Add(new int[,] { { x, y } });
            #endregion

            int count = 0;

            while (backtrack.Count != 0)
            {
                TryMakeRoom(map, width, height, x, y, GetRandom(1, 3));
                TryMakeCorridor(map, width, height, x, y, GetRandom(4, 7));
            }
            return map;
        }


        private bool TryMakeCorridor(int[,] map, int width, int height, int x, int y, int length)
        {
            Direction direction = Direction.North;
            //Check every direction
            //corridor width is 1
            //find the wall of the room that goes with the given direction
            bool result = true;
            for (int i = 0; i < length; i++)
            {
                //Within width bounds
                if (x + i > width || x - i < 0)
                {
                    return false;
                }
                //Within height bounds
                else if (y + i > height || y - i < 0)
                {
                    return false;
                }
            }
            GenerateCorridor(map, x, y, length, direction);
            return result;
        }

        private bool TryMakeRoom(int[,] map, int width, int height, int x, int y, int distanceOut)
        {
            bool result = true;
            for (int i = 0; i < distanceOut; i++)
            {
                //Within width bounds
                if (x + i > width || x - i < 0)
                {
                    return false;
                }
                //Within height bounds
                else if (y + i > height || y - i < 0)
                {
                    return false;
                }
                #region 
                //Doesn't run into another room
                //else if (map[x - i, y] == 2)
                //{
                //    return false;
                //}
                //else if (map[x, y - i] == 2)
                //{
                //    return false;
                //}
                //else if (map[x + i, y] == 2)
                //{
                //    return false;
                //}
                //else if (map[x, y + i] == 2)
                //{
                //    return false;
                //}
                //else if (map[x - i, y - i] == 2)
                //{
                //    return false;
                //}
                //else if (map[x + i, y + i] == 2)
                //{
                //    return false;
                //} 
                #endregion
            }
            GenerateRoom(map, x, y, distanceOut);
            return result;
        }

        /// <summary>
        /// Addes 
        /// </summary>
        /// <param name="map"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="length"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        private int[,] GenerateCorridor(int[,] map, int x, int y, int length, Direction direction)
        {
            if (direction == Direction.North)
            {
                for (int i = 1; i < length; i++)
                {
                    map[x, y - i] = 1;
                }
            }
            else if (direction == Direction.East)
            {
                for (int i = 1; i < length; i++)
                {
                    map[x + i, y] = 1;
                }
            }
            else if (direction == Direction.South)
            {
                for (int i = 1; i < length; i++)
                {
                    map[x, y + i] = 1;
                }
            }
            else if (direction == Direction.West)
            {
                for (int i = 1; i < length; i++)
                {
                    map[x - i, y] = 1;
                }
            }
            return map;
        }

        private void GenerateRoom(int[,] map, int x, int y, int distanceOut)
        {
            for (int i = 0; i < distanceOut; i++)
            {
                map[x - i, y] = currentRoomNum;
                map[x, y - i] = currentRoomNum;
                map[x + i, y] = currentRoomNum;
                map[x, y + i] = currentRoomNum;
                map[x - i, y - i] = currentRoomNum;
                map[x + i, y + i] = currentRoomNum;
                map[x - i, y + i] = currentRoomNum;
                map[x + i, y - i] = currentRoomNum;
            }
            currentRoomNum++;
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
                        s += " = ";
                    }
                    else
                    {
                        s += " # ";
                    }
                    sb.Append(s);
                }
                sb.Append("\n");
            }
            return sb.ToString();
        }

        /// <summary>
        /// Takes in an inclusive upper and lower bound and returns a random int between between the x and y parameters
        /// </summary>
        /// <param name="x">inclusive lower bound</param>
        /// <param name="y">inclusive upper bound</param>
        /// <returns>Random int between x and y </returns>
        public int GetRandom(int x, int y)
        {
            return rand.Next(x, y + 1);
        }
    }
}
