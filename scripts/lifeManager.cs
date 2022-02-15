using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lifeManager : MonoBehaviour
{
    public lifeDrawer[] lifedraw; //what visualizes the system.

    //life variables
    public int width, height;
    Cell[,] worldCells; //where we store the world of cells
    Cell[,] worldCellsNext; //the world of cells for next turn
    private int sx, sy; // the internal x and y size of the world
    private int size;

    public float randomFillLevel;

    public Cell.Type celltype;

    // Start is called before the first frame update
    void Start()
    {
        //set up initial variables
        worldCells = new Cell[width, height];
        worldCellsNext = new Cell[width, height];
        sx = width;
        sy = height;
        size = width * height;

        //initialize the world
        for (int x = 0; x < sx; x++)
        {
            for (int y = 0; y < sy; y++)
            {
                //worldCells[x, y] = new Cell(x, y, Cell.Type.GOL);
                //worldCellsNext[x, y] = new Cell(x, y, Cell.Type.GOL);
                //worldCells[x, y] = new Cell(x, y, Cell.Type.sand);
                //worldCellsNext[x, y] = new Cell(x, y, Cell.Type.sand);
                worldCells[x, y] = new Cell(x, y, celltype);
                worldCellsNext[x, y] = new Cell(x, y, celltype);

            }
        }

        //put some random cells in. This way there will be some initial state for us to see.
        RandomFill(randomFillLevel);

        //Initialize the life drawer
        for (int i = 0; i < lifedraw.Length; i++)
        {
            lifedraw[i].Initialize(width, height, size, this);
        }
        
    }

    //Fill a random percentage of cells. 
    void RandomFill(float fillAmt)
    {
        for (int x = 0; x < sx; x++)
        {
            for (int y = 0; y < sy; y++)
            {
                float rand = Random.value;
                if (rand < fillAmt)
                {
                    worldCells[x, y].type = Cell.Type.GOL;
                    worldCells[x, y].alive = 1;
                }

            }
        }
    }

    //Every so many seconds, update life. This way it doesn't update every frame and go too fast.
    public float lifeUpdateTimer = 0.5f;
    private float timer = 0;
    void Update()
    {
        if (timer <= 0)
        {
            timer = lifeUpdateTimer;
            MainLifeLogic();
            for (int i = 0; i < lifedraw.Length; i++)
            {
                lifedraw[i].DrawLife(worldCells);
            }
        }
        else
        {
            timer -= Time.deltaTime;
        }

    }


    void MainLifeLogic()
    {

        //go throgh all cells
        for (int x = 0; x < sx; x++)
        {
            for (int y = 0; y < sy; y++)
            {
                //life logic
                if (worldCells[x, y].type == Cell.Type.GOL) //check if neighbors are GOL!!!
                {
                    //count the alive neighbors
                    int count = 0;
                    count = neighbors(x, y);

                    if (worldCells[x, y].alive == 0)
                    {
                        //check if something can be born
                        //if (count == 1 || count == 2 || count == 4 || count == 6)
                        if (count == 1 || count == 4)
                        { //am dead, how many neighbors trigger a birth
                            worldCellsNext[x, y].alive = 1; //come alive
                            worldCellsNext[x, y].type = Cell.Type.GOL;

                        }
                        else
                        {
                            //am dead and stayed dead
                        }

                    }
                    else if (worldCells[x, y].alive == 1)
                    {
                        //check if the cell should die
                        // if (count == 3 || count == 5 || count == 7)
                        if (count == 2 || count == 3 || count == 5 || count <= 7)
                        { //am alive, how many neighbors trigger me dying
                            worldCellsNext[x, y].alive = 0; //die
                            worldCellsNext[x, y].age = 0;

                        }
                        else
                        {
                            //am alive and stayed alive
                            worldCellsNext[x, y].age += 1;

                            // if (worldCellsNext[x, y].age >= 20)
                            if (worldCellsNext[x, y].age >= 12)
                            {
                                worldCellsNext[x, y].alive = 0; //die
                                worldCellsNext[x, y].age = 0;
                            }
                        }

                    }
                }



            }
        }

        //set current life equal to the next one;
        for (int x = 0; x < sx; x++)
        {
            for (int y = 0; y < sy; y++)
            {
                worldCells[x, y].alive = worldCellsNext[x, y].alive;
                worldCells[x, y].age = worldCellsNext[x, y].age;
                worldCells[x, y].type = worldCellsNext[x, y].type;
                //worldCellsNext
            }
        }

    }


    int neighbors(int x, int y)
    {
        //this wraps around the edges
        return worldCells[(x + 1) % sx, y].alive +
        worldCells[x, (y + 1) % sy].alive +
        worldCells[(x + sx - 1) % sx, y].alive +
        worldCells[x, (y + sy - 1) % sy].alive + //
        worldCells[(x + 1) % sx, (y + 1) % sy].alive + //move top right
        worldCells[(x + sx - 1) % sx, (y + 1) % sy].alive + //move top left
        worldCells[(x + sx - 1) % sx, (y + sy - 1) % sy].alive + //down left
        worldCells[(x + 1) % sx, (y + sy - 1) % sy].alive; //down right
    }

    public void DrawLife(int x, int y, Cell.Type _type)
    {
        worldCellsNext[x, y].alive = 1;
        worldCellsNext[x, y].age = 0;
        worldCellsNext[x, y].type = celltype;
    }


    public class Cell
    {
        public enum Type { GOL }//, sand,}

        public Type type;

        public int x, y; //coordinates of the cell
        public int alive; //is the cell alive?
                          //public int age???
                          //other properties??
        public int age; //track how many turns its been alive

        public Cell(int _x, int _y, Type celltype)
        {
            x = _x;
            y = _y;
            alive = 0;
            age = 0;
            //if we have other properties, initialize them here
            type = celltype;
        }

    }
}
