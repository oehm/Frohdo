using UnityEngine;
using System.Collections;

public class CommandHelper
{
    public static bool setMatrix(ref GameObject[][] mat, Vector2 pos, Gridable grid, GameObject value)
    {
        int pW = mat.Length + 1;
        int pH = mat[0].Length + 1;
        Vector2 objPos = new Vector2(pos.x + pW / 2.0f - (float)grid.width/ 2.0f, pos.y + pH / 2.0f - (float)grid.height/2.0f);
        Debug.Log(objPos);

        //Test if obejct in in layerBounds
        if (objPos.x  < 0 || objPos.x + grid.width > pW || objPos.y  < 0 || objPos.y + grid.height > pH) return false;

        //test if theres is an object
        for (int x = (int)objPos.x, xm = 0; x < (int)objPos.x + grid.width; x++, xm++)
        {
            for (int y = (int)objPos.y, ym = 0; y < (int)objPos.y + grid.height; y++, ym++)
            {
                if (grid.hitMat[xm][ym] && mat[x][y] != null)
                {
                    return false;
                }
            }
        }
        //if not set the values
        setMatrixForceOverride(ref mat, pos, grid, value);
        return true;
    }

    public static void setMatrixForceOverride(ref GameObject[][] mat, Vector2 pos, Gridable grid, GameObject value)
    {
        int pW = mat.Length + 1;
        int pH = mat[0].Length + 1;
        Vector2 objPos = new Vector2(pos.x + pW / 2.0f - (float)grid.width / 2.0f, pos.y + pH / 2.0f - (float)grid.height / 2.0f);

        for (int x = (int)objPos.x, xm = 0; x < (int)objPos.x + grid.width; x++, xm++)
        {
            for (int y = (int)objPos.y, ym = 0; y < (int)objPos.y + grid.height; y++, ym++)
            {
                if (grid.hitMat[xm][ym])
                {
                    mat[x][y] = value;
                }
            }
        }
    }
}
