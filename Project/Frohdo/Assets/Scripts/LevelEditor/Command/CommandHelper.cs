using UnityEngine;
using System.Collections;

public class CommandHelper
{
    public static bool setMatrix(int matlayer, Vector2 pos, Gridable grid, GameObject value)
    {
        int pW = Editor_Grid.Instance.levelGrid[matlayer].Length + 1;
        int pH = Editor_Grid.Instance.levelGrid[matlayer][0].Length + 1;
        Vector2 objPos = new Vector2(pos.x + pW / 2.0f - (float)grid.width/ 2.0f, pos.y + pH / 2.0f - (float)grid.height/2.0f);

        //Test if obejct in in layerBounds
        if (objPos.x  < 0 || objPos.x + grid.width > pW || objPos.y  < 0 || objPos.y + grid.height > pH) return false;
        //test if theres is an object
        for (int x = (int)objPos.x, xm = 0; x < (int)objPos.x + grid.width; x++, xm++)
        {
            for (int y = (int)objPos.y, ym = 0; y < (int)objPos.y + grid.height; y++, ym++)
            {
                //Debug.Log("Check mat at: " + x.ToString() + " " + y.ToString());                                    
                if (grid.hitMat[xm][ym] && Editor_Grid.Instance.levelGrid[matlayer][x][y] != null)
                {
                    return false;
                }
            }
        }
        //if not set the values
        //Debug.Log("set value of gameobject");
        setMatrixForceOverride(matlayer, pos, grid, value);
        return true;
    }

    public static void setMatrixForceOverride(int matlayer, Vector2 pos, Gridable grid, GameObject value)
    {
        
        int pW = Editor_Grid.Instance.levelGrid[matlayer].Length + 1;
        int pH = Editor_Grid.Instance.levelGrid[matlayer][0].Length + 1;
        Vector2 objPos = new Vector2(pos.x + pW / 2.0f - (float)grid.width / 2.0f, pos.y + pH / 2.0f - (float)grid.height / 2.0f);

        for (int x = (int)objPos.x, xm = 0; x < (int)objPos.x + grid.width; x++, xm++)
        {
            for (int y = (int)objPos.y, ym = 0; y < (int)objPos.y + grid.height; y++, ym++)
            {
                //if (grid.hitMat[xm][ym])
                //{

                //Debug.Log(value);
                //Debug.Log("mat at: " + x.ToString() + " " + y.ToString());
                Editor_Grid.Instance.levelGrid[matlayer][x][y] = value;
                //}
            }
        }
    }
}
