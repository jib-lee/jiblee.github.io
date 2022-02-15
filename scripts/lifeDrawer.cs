using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lifeDrawer : MonoBehaviour
{
    //get from lifemanager
    public lifeManager lifeManager;
    public int width, height, size;

    Color[] array_Main; //pixels to draw
    Texture2D textureLife; //the texture itself
    MeshRenderer meshrend; //where we put the texture

    public Gradient lifegradient;

    //This is like a manual start function, we call it from LifeManager
    public void Initialize(int w, int h, int s, lifeManager _life)
    {
        //initialize variables:
        width = w;
        height = h;
        size = s;
        lifeManager = _life;
        array_Main = new Color[size];
        meshrend = GetComponent<MeshRenderer>();

        //and initialize the array with blank values:
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                array_Main[j * width + i] = new Color(1f, 0f, 0f);

            }
        }
        //create the texture:
        textureLife = new Texture2D(width, height);
        textureLife.filterMode = FilterMode.Point; //turn off anti-aliasing
        meshrend.material.SetTexture("_MainTex", textureLife); //set it onto the material.
    }

    //This function passes into the world of cells, and converts it into pixels
    public void DrawLife(lifeManager.Cell[,] cells)
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                //read if the cell is alive or dead, and assign a color.
                //Right now they're just alive or dead. If there were more to these cells, we could assign other colors based on their state too.
                if (cells[x, y].alive == 1 && cells[x, y].type == lifeManager.Cell.Type.GOL)
                {
                    //array_Main[x + (y * width)] = Color.Lerp(aliveCol, aliveCol2, Mathf.PingPong(Time.time, 1));
                    float agepercent = (float)cells[x, y].age / 5f;
                    array_Main[x + (y * width)] = lifegradient.Evaluate(agepercent);
                }
                else if (cells[x, y].alive == 0)
                {
                    array_Main[x + (y * width)] = deadCol; //black
                }

            }

        }

        textureLife.SetPixels(array_Main); //pass the pixels to the texture
        textureLife.Apply(); //and apply it. You have to do this otherwise the image doesn't update.

    }

    //public Color aliveCol;
    //public Color aliveCol2;
    public Color deadCol;
}
