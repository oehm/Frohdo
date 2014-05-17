using UnityEngine;
using System.Collections;

public class CommandHelper
{
    public static bool setMatrix(ref GameObject[][] mat, Vector2 pos, Gridable grid, GameObject value)
    {
        int pW = mat.Length;
        int pH = mat[0].Length;
        Vector2 objPos = new Vector2((int)(pos.x + (pW + 1) / 2), (int)(pos.y + (pH + 1) / 2));
        int w = (grid.width + 1) / 2;
        int h = (grid.height + 1) / 2;
        int w2 = grid.width / 2;
        int h2 = grid.height / 2;

        //Test if obejct in in layerBounds
        if ((int)objPos.x - w2 < 0 || (int)objPos.x + w > pW || (int)objPos.y - h2 < 0 || (int)objPos.y + h > pH) return false;

        //test if theres is an object
        for (int x = (int)objPos.x - w2, xm = 0; x < (int)objPos.x + w; x++, xm++)
        {
            for (int y = (int)objPos.y - h2, ym = 0; y < (int)objPos.y + h; y++, ym++)
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
        int pW = mat.Length;
        int pH = mat[0].Length;
        Vector2 objPos = new Vector2((int)(pos.x + (pW + 1) / 2), (int)(pos.y + (pH + 1) / 2));
        int w = (grid.width + 1) / 2;
        int h = (grid.height + 1) / 2;
        int w2 = grid.width / 2;
        int h2 = grid.height / 2;

        for (int x = (int)objPos.x - w2, xm = 0; x < (int)objPos.x + w; x++, xm++)
        {
            for (int y = (int)objPos.y - h2, ym = 0; y < (int)objPos.y + h; y++, ym++)
            {
                if (grid.hitMat[xm][ym])
                {
                    mat[x][y] = value;
                }
            }
        }
    }
}
