using UnityEngine;
using System.Collections;

public class EditorHelper {

	public static Vector2 localMouseToLocalSnapped (Vector2 mousePos,GameObject layer)
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x,mousePos.y,GlobalVars.Instance.layerZPos[GlobalVars.Instance.playLayer] - GlobalVars.Instance.mainCamerZ));
        pos -= new Vector2(layer.transform.position.x, layer.transform.position.x);
        pos.x = (int)pos.x;
        pos.y = (int)pos.y;
        return pos;
    }

    public static Vector2 getMatIndex(Vector2 localTransform, Vector2 planeSize)
    {
        return new Vector2((int)(localTransform.x + planeSize.x / 2), (int)(localTransform.y + planeSize.y / 2));
    }
}
