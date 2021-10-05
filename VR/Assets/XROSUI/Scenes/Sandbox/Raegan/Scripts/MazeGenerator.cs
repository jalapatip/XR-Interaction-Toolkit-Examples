using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Flags]
public enum WallStates
{
    // state of maze walls 
    LEFT = 1, //0001
    RIGHT = 2,//0010
    UP = 4,//0100
    DOWN = 8,
    VISITED = 128, //1000 1000


}
public struct Position
{
    //keep track of coordinate in a maze
    public int X;
    public int Y;
   
}
//containt the position of neighbor that we are breaking the wall with and the wall that is being shared.
public struct NeighborWall
{
    public Position Position;
    public WallStates SharedWall;
}



public  class MazeGenerator : MonoBehaviour
{


    public Vector3 initial;
    //takes wall state and returns opposite side
  
    private static WallStates GetOppositeWall(WallStates wall)
    {
        switch(wall)
        {
            case WallStates.RIGHT: return WallStates.LEFT;
            case WallStates.LEFT: return WallStates.RIGHT;
            case WallStates.UP: return WallStates.DOWN;
            case WallStates.DOWN: return WallStates.UP;
            default: return WallStates.LEFT;
        }
    }
    //recursive backtracker
    public static WallStates[,] ApplyRecursiveBacktracker(WallStates[,] maze, int w, int h)
    {  
        var randomnum = new System.Random();
        var positionStack = new Stack<Position>();
        Position position = new Position { X = randomnum.Next(0, w), Y = randomnum.Next(0, h) };
        Vector3 initial = new Vector3(position.X, position.Y, 0);
       
        maze[position.X, position.Y] |= WallStates.VISITED;

        positionStack.Push(position);
        while (positionStack.Count > 0)
        {
            var current = positionStack.Pop();
            var near = GetUnvisitedNeighbor(current, maze, w, h);
            if(near.Count >0)
            {
                positionStack.Push(current);
                var index = randomnum.Next(0, near.Count);
                var randomNeighbor = near[index];

                var nPosition = randomNeighbor.Position;
                maze[current.X, current.Y] &= ~randomNeighbor.SharedWall;
                maze[nPosition.X, nPosition.Y] &= ~GetOppositeWall(randomNeighbor.SharedWall);
                maze[nPosition.X, nPosition.Y] |= WallStates.VISITED;
                positionStack.Push(nPosition);


            }
        }

        return maze;
    }
    //return all the neighbors positions that havent been visited
    public static List<NeighborWall> GetUnvisitedNeighbor(Position p, WallStates[,] maze, int w, int h)
    {
        var list = new List<NeighborWall>();
        if (p.X > 0) //left
        {
            if(!maze[p.X -1, p.Y].HasFlag(WallStates.VISITED))
            { list.Add(new NeighborWall {
                    Position = new Position
                    {
                        X = p.X - 1,
                        Y = p.Y
                    },
                    SharedWall = WallStates.LEFT
                });
            }
        }
        if (p.Y > 0) //bottom
        {
            if (!maze[p.X , p.Y-1].HasFlag(WallStates.VISITED))
            {
                list.Add(new NeighborWall {
                    Position = new Position
                    {
                        X = p.X ,
                        Y = p.Y-1
                    },
                    SharedWall = WallStates.DOWN

                  });
            }
        }
        if (p.Y < h -1) //left
        {
            if (!maze[p.X , p.Y +1].HasFlag(WallStates.VISITED))
            {
                list.Add(new NeighborWall {
                    Position = new Position
                    {
                        X = p.X ,
                        Y = p.Y +1
                    },
                    SharedWall = WallStates.UP
                  });
            }
        }
        if (p.X < w -1 ) //right
        {
            if (!maze[p.X + 1, p.Y].HasFlag(WallStates.VISITED))
            {
                list.Add(new NeighborWall {
                    Position = new Position
                    {
                        X = p.X + 1,
                        Y = p.Y
                    },
                    SharedWall = WallStates.RIGHT
                  });
            }
        }
        return list;

    }
    public static WallStates[,] Generate(int w, int h)
    {
        WallStates[,] maze = new WallStates[w, h];
        WallStates start = WallStates.RIGHT | WallStates.LEFT | WallStates.UP | WallStates.DOWN;
        for(int i = 0; i < w; ++i)
        {
            for (int j = 0; j < h; ++j)
            {
                maze[i, j] = start; 
            }
        }
        return ApplyRecursiveBacktracker(maze, w, h);
    }

 
}
