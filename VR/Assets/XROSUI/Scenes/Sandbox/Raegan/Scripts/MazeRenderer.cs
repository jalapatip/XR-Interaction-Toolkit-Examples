using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeRenderer : MonoBehaviour
{
    [SerializeField]
    [Range(1, 50)]
    private int w = 10;
    [SerializeField]
    [Range(1, 50)]
    private int h =10;
    //wall size
    [SerializeField]
    private float size = 1f;
    [SerializeField] // Link wall prefab;
    private Transform wall = null;
     
    // Start is called before the first frame update
    public void Start()
    {
        var maze = MazeGenerator.Generate(w, h);
        Draw(maze);
        
      
        
      
        
    }

    private void Draw(WallStates[,] maze) {
       
        for(int i = 0; i < w;++i){
            for (int j = 0; j < h; ++j)
            {
                //each cell position is offset 
                var cell = maze[i, j];
                 var position = new Vector3(-w / 2 + i,0, -h/2 + j);
               
                if (cell.HasFlag(WallStates.UP))
                {
                    var topWall = Instantiate(wall, transform) as Transform;
                    topWall.position = position + new Vector3(0, 0, size/2);
                    topWall.localScale = new Vector3(size ,topWall.localScale.y, topWall.localScale.z);
                }
                if (cell.HasFlag(WallStates.LEFT))
                {
                    var leftWall = Instantiate(wall, transform) as Transform;
                    leftWall.position = position + new Vector3(-size/2, 0, 0);
                   leftWall.localScale = new Vector3(size, leftWall.localScale.y, leftWall.localScale.z);
                    leftWall.eulerAngles = new Vector3(0,90,0);
 
                }
                if ( i == w -1)
                {
                    if (cell.HasFlag(WallStates.RIGHT))
                    {
                        var rightWall = Instantiate(wall, transform) as Transform;
                        rightWall.position = position + new Vector3(+size / 2, 0, 0);
                       rightWall.localScale = new Vector3(size, rightWall.localScale.y, rightWall.localScale.z);
                        rightWall.eulerAngles = new Vector3(0, 90, 0);
                    }
                }
                if (j == 0)
                {
                    if (cell.HasFlag(WallStates.DOWN))
                    {
                        var bottomWall = Instantiate(wall, transform) as Transform;
                        bottomWall.position = position + new Vector3(0, 0, -size / 2);
                       bottomWall.localScale = new Vector3(size, bottomWall.localScale.y, bottomWall.localScale.z);


                    }
                }
            }
        }
 
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
