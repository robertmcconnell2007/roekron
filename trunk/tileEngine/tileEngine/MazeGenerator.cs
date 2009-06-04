using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
            // add this line to the load section to create a map, during compile time, the map
            // is created under tileEngine\bin\x86\debug and will be named whatever you put as
            // the first string plus the number that follows. The maze must have an even size
            // which is the first number. And you must also include the dungeontiles texture file
            // as that is the only working texture file. The last two strings are the connecting maps
            // the one in the first spot will lead to the top left corner, the other is the bottom right

            //maze.createMaze(10, "Dungeon", 1, "DungeonTiles", "Map002", "Map001");
namespace tileEngine
{
    class MazeGenerator
    {
        public void createMaze(int size, string name, int num, string textureType, string previousMapName, string nextMapName)
        {
            int h, w;
            h = w = size;
            h *= 3;
            w *= 3;
            int height, width;
            height = h+1;
            width = w+1;
            int[,] mazeOut = new int[height,width];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (i == 0 || j == 0)
                    {
                        mazeOut[i, j] = 0;
                    }
                    else if (i == h || j == w)
                    {
                        mazeOut[i, j] = 0;
                    }
                    else
                    {
                        mazeOut[i, j] = 1;
                    }
                }
            }
            h = height - 3;
            w = width - 3;
            mazeOut = proceed(mazeOut, h, w);
            mazeOut[1, 2] = 0;
            mazeOut[height - 3, width - 2] = 0;
            writeMaze(mazeOut, name, num, height, width, textureType, previousMapName, nextMapName);
        }
        protected int getNewDirection()
        {
            Random temp = new Random();
            return temp.Next(0,4);
        }
        protected int[,] proceed(int[,] maze, int h, int w)
        {
            maze[h, w] = 0;
            int north, south, east, west;
            north = maze[h - 2, w];
            south = maze[h + 2, w];
            east = maze[h, w + 2];
            west = maze[h, w - 2];
            int nDirection = 4;
            //for (int i = 0; i < 31; i++)
            //{
            //    for (int j = 0; j < 31; j++)
            //    {
            //        Console.Write(maze[i, j].ToString());
            //    }
            //    Console.WriteLine();
            //}
            System.Threading.Thread.Sleep(10);
            while (north != 0 || south != 0 || east != 0 || west != 0)
            {
                north = maze[h - 2, w];
                south = maze[h + 2, w];
                east = maze[h, w + 2];
                west = maze[h, w - 2];
                nDirection = getNewDirection();
                if (nDirection == 0 && north == 1)
                {
                    maze[h - 1, w] = 0;
                    maze = proceed(maze, h - 2, w);
                }
                if (nDirection == 1 && south == 1)
                {
                    maze[h + 1, w] = 0;
                    maze = proceed(maze, h + 2, w);
                }
                if (nDirection == 2 && east == 1)
                {
                    maze[h, w + 1] = 0;
                    maze = proceed(maze, h, w + 2);
                }
                if (nDirection == 3 && west == 1)
                {
                    maze[h, w - 1] = 0;
                    maze = proceed(maze, h, w - 2);
                }
            }
            return maze;
        }
        private int[,] drawMaze(int[,] maze, int height, int width)
        {
            int[,] gMaze = new int[height, width];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (i == 0 || j == 0)
                    {
                        gMaze[i, j] = 0;
                    }
                    else if (i == height - 1 || j == width - 1)
                    {
                        gMaze[i, j] = 0;
                    }
                    else if (maze[i, j] == 1)
                    {
                        gMaze[i, j] = 0;
                    }
                    else if (maze[i, j] == 0)
                    {
                        if (maze[i - 1, j] == 0 && maze[i + 1, j] == 0 && maze[i, j - 1] == 0 && maze[i, j + 1] == 0) // 4 way
                        {
                            gMaze[i, j] = 15;
                        }
                        if (maze[i - 1, j] == 1 && maze[i + 1, j] == 0 && maze[i, j - 1] == 0 && maze[i, j + 1] == 0) // 3 way ews
                        {
                            gMaze[i, j] = 9;
                        }
                        if (maze[i - 1, j] == 0 && maze[i + 1, j] == 1 && maze[i, j - 1] == 0 && maze[i, j + 1] == 0) // 3 way new
                        {
                            gMaze[i, j] = 7;
                        }
                        if (maze[i - 1, j] == 0 && maze[i + 1, j] == 0 && maze[i, j - 1] == 1 && maze[i, j + 1] == 0) // 3 way nes
                        {
                            gMaze[i, j] = 8;
                        }
                        if (maze[i - 1, j] == 0 && maze[i + 1, j] == 0 && maze[i, j - 1] == 0 && maze[i, j + 1] == 1) // 3 way nws
                        {
                            gMaze[i, j] = 10;
                        }
                        if (maze[i - 1, j] == 1 && maze[i + 1, j] == 1 && maze[i, j - 1] == 0 && maze[i, j + 1] == 0) // 2 way we
                        {
                            gMaze[i, j] = 5;
                        }
                        if (maze[i - 1, j] == 1 && maze[i + 1, j] == 0 && maze[i, j - 1] == 1 && maze[i, j + 1] == 0) // 2 way se
                        {
                            gMaze[i, j] = 13;
                        }
                        if (maze[i - 1, j] == 1 && maze[i + 1, j] == 0 && maze[i, j - 1] == 0 && maze[i, j + 1] == 1) // 2 way sw
                        {
                            gMaze[i, j] = 14;
                        }
                        if (maze[i - 1, j] == 0 && maze[i + 1, j] == 1 && maze[i, j - 1] == 1 && maze[i, j + 1] == 0) // 2 way ne
                        {
                            gMaze[i, j] = 12;
                        }
                        if (maze[i - 1, j] == 0 && maze[i + 1, j] == 1 && maze[i, j - 1] == 0 && maze[i, j + 1] == 1) // 2 way nw
                        {
                            gMaze[i, j] = 11;
                        }
                        if (maze[i - 1, j] == 0 && maze[i + 1, j] == 0 && maze[i, j - 1] == 1 && maze[i, j + 1] == 1) // 2 way ns
                        {
                            gMaze[i, j] = 6;
                        }
                        if (maze[i - 1, j] == 0 && maze[i + 1, j] == 1 && maze[i, j - 1] == 1 && maze[i, j + 1] == 1) // end n
                        {
                            gMaze[i, j] = 4;
                        }
                        if (maze[i - 1, j] == 1 && maze[i + 1, j] == 0 && maze[i, j - 1] == 1 && maze[i, j + 1] == 1) // end s
                        {
                            gMaze[i, j] = 2;
                        }
                        if (maze[i - 1, j] == 1 && maze[i + 1, j] == 1 && maze[i, j - 1] == 0 && maze[i, j + 1] == 1) // end w
                        {
                            gMaze[i, j] = 3;
                        }
                        if (maze[i - 1, j] == 1 && maze[i + 1, j] == 1 && maze[i, j - 1] == 1 && maze[i, j + 1] == 0) // end e
                        {
                            gMaze[i, j] = 1;
                        }
                    }
                }
            }
            return gMaze;
        }
        private void writeMaze(int[,] maze, string name, int num, int height, int width, string textureType, string previousMapName, string nextMapName)
        {
            int[,] gMaze = new int[height, width];
            gMaze = drawMaze(maze, height, width);
            using (StreamWriter sw = new StreamWriter(name + num.ToString() + ".xml"))
            {
                sw.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
                sw.WriteLine("<XnaContent>");
                sw.WriteLine("  <Asset Type=\"RolePlayingGameData.Map\">");
                sw.WriteLine("      <Name>" + name + num.ToString() + "</Name>");
                sw.WriteLine("      <MapDimensions>" + height.ToString() + " " + width.ToString() + "</MapDimensions>");
                sw.WriteLine("      <TileSize>64 64</TileSize>");
                sw.WriteLine("      <SpawnMapPosition>2 2</SpawnMapPosition>");
                sw.WriteLine("      <TextureName>"+ textureType +"</TextureName>");
                sw.WriteLine("      <BaseLayer>");
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        sw.Write(gMaze[i, j] + " ");
                    }
                    sw.WriteLine();
                }
                sw.WriteLine("      </BaseLayer>");
                sw.WriteLine("      <FringeLayer>");
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        sw.Write("-1 ");
                    }
                    sw.WriteLine();
                }
                sw.WriteLine("      </FringeLayer>");
                sw.WriteLine("      <ObjectLayer>");
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        if((i == 1 && j == 2))
                        {
                            sw.Write("17 ");
                        }
                        else if((i == height -3 && j == width -2))
                        {
                            sw.Write("17 ");
                        }
                        else
                        {
                            sw.Write("16 ");
                        }
                        
                    }
                    sw.WriteLine();
                }
                sw.WriteLine("      </ObjectLayer>");
                sw.WriteLine("      <CollisionLayer>");
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        sw.Write(maze[i, j] + " ");
                    }
                    sw.WriteLine();
                }
                sw.WriteLine("      </CollisionLayer>");
                sw.WriteLine("      <Portals>");
                sw.WriteLine("          <Item>");
                sw.WriteLine("			  <Name>P1</Name>");
                sw.WriteLine("			  <LandingMapPosition>2 2</LandingMapPosition>");
                sw.WriteLine("              <DestinationMapContentName>" + previousMapName + "</DestinationMapContentName>");
                sw.WriteLine("              <DestinationMapPortalName>To" + name + num.ToString() + "</DestinationMapPortalName>");
                sw.WriteLine("          </Item>");
                sw.WriteLine("          <Item>");
                sw.WriteLine("			  <Name>P2</Name>");
                sw.WriteLine("			  <LandingMapPosition>" + (height - 3).ToString() + " " + (width - 3).ToString() + "</LandingMapPosition>");
                sw.WriteLine("              <DestinationMapContentName>" + nextMapName + "</DestinationMapContentName>");
                sw.WriteLine("              <DestinationMapPortalName>To" + name + num.ToString() + "</DestinationMapPortalName>");
                sw.WriteLine("          </Item>");
                sw.WriteLine("      </Portals>");
                sw.WriteLine("	  <PortalEntries>");
                sw.WriteLine("		  <Item>");
                sw.WriteLine("			  <ContentName>P1</ContentName>");
                sw.WriteLine("			  <MapPosition>1 2</MapPosition>");
                sw.WriteLine("		  </Item>");
                sw.WriteLine("		  <Item>");
                sw.WriteLine("			  <ContentName>P2</ContentName>");
                sw.WriteLine("			  <MapPosition>" + (width - 2).ToString() + " " + (height - 3).ToString() + "</MapPosition>");
                sw.WriteLine("		  </Item>");
                sw.WriteLine("    </PortalEntries>");
                sw.WriteLine("  </Asset>");
                sw.WriteLine("</XnaContent>");
            }
        }

    }
}
