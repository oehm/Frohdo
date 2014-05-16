using UnityEngine;
using System.Collections;

public interface Editor_State
{
    void init();
    void update();
    void leftMouseDown();
    void leftMouseUp();
    void mouseMove(Vector2 pos);
    void updateColor(string color);
    void updateObject(GameObject obj);
}
