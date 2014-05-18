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
            _rect = new Rect(_pos.x + ForceAspectRatio.xOffset, _pos.y + ForceAspectRatio.yOffset, _size.x, _size.y);
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


    public virtual bool mouseOnGui(Vector2 pos)
    {
        if(!active) return false;
        return _rect.Contains(pos);
    }
    public abstract void Draw();

    public virtual void resize(Rect screenRect)
    {
        if(_rect.x < screenRect.x)
        {
            _rect.x = screenRect.x;
        }
        if(_rect.x + _rect.width > screenRect.x + screenRect.width)
        {
            _rect.x = screenRect.x + screenRect.width - _rect.width;
        }

        if (_rect.y < screenRect.y)
        {
            _rect.y = screenRect.y;
        }
        if (_rect.y + _rect.height > screenRect.y + screenRect.height)
        {
            _rect.y = screenRect.y + screenRect.height - _rect.height;
        }
    }
}
