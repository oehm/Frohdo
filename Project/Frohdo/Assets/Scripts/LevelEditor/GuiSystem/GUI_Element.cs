using UnityEngine;
using System.Collections;

abstract public class GUI_Element
{
    private Vector2 _pos;
    private Vector2 _size;
    protected Rect _rect;

    protected GUISkin skin;

    public bool active { get; set; }

    public Vector2 position
    {
        get { return _pos; }
        set
        {
            _pos = value;
            _rect = new Rect(_pos.x, _pos.y, _size.x, _size.y);
        }
    }
    public Vector2 size
    {
        get { return _size; }
        set
        {
            _size = value;
            _rect = new Rect(_pos.x, _pos.y, _size.x, _size.y);
        }
    }

    public bool mouseOnGui(Vector2 pos)
    {
        if(!active) return false;
        return _rect.Contains(pos);
    }
    public abstract void Draw();
}
