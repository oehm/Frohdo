using UnityEngine;
using System.Collections;

public class EditorHelper {

	public static Vector2 localMouseToLocalLayer (Vector2 mousePos,GameObject layer, bool snaped)
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x,mousePos.y,GlobalVars.Instance.layerZPos[GlobalVars.Instance.playLayer] - Camera.main.transform.position.z));
        pos -= new Vector2(layer.transform.position.x, layer.transform.position.y);
        if (snaped)
        {
            pos.x = Mathf.Floor(pos.x);
            pos.y = Mathf.Floor(pos.y);
        }
        return pos;
    }

    public static Vector2 getMatIndex(Vector2 localTransform, Vector2 planeSize)
    {
        return new Vector2((int)(localTransform.x + planeSize.x / 2), (int)(localTransform.y + planeSize.y / 2));
    }

    public static Vector2 getLocalObjectPosition(Vector2 mousePos, GameObject layer, Gridable grid)
    {
        Vector2 localPos = localMouseToLocalLayer(mousePos, layer, true);
        localPos.x += (float)grid.width/2.0f;
        localPos.y += (float)grid.height/2.0f;
        return localPos;
    }
}
