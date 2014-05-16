using UnityEngine;
using System.Collections;

public class State_Default : Editor_State {

    public StateManager manager { get; set; }

    private Vector2 mousePos;

    public void init()
    {
        manager.colorSelection.active = true;
        manager.selected.active = false;
        manager.commands.active = true;

        mousePos = new Vector2(0, 0);
    }

    public void update()
    {

    }

    public void leftMouseDown()
    {
        Vector2 matIndex = EditorHelper.getMatIndex(EditorHelper.localMouseToLocalSnapped(mousePos, GameObject.Find("SceneObjects").GetComponentsInChildren<Layer>()[manager.currentLayer].gameObject),Editor_Grid.Instance.planeSizes[manager.currentLayer]);
        if(matIndex.x > 0 && matIndex.y >0 && matIndex.x < Editor_Grid.Instance.planeSizes[manager.currentLayer].x &&  matIndex.y < Editor_Grid.Instance.planeSizes[manager.currentLayer].y)
        {
            GameObject select = Editor_Grid.Instance.levelGrid[manager.currentLayer][(int)matIndex.x][(int)matIndex.y];
            if(select!= null)
            {
                manager.changeState(new State_SetObject());
            }
        }
    }

    public void leftMouseUp()
    {
     
    }

    public void mouseMove(Vector2 pos)
    {
        mousePos = pos;
    }
}
