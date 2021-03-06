﻿using UnityEngine;
using System.Collections;

abstract public class GUI_Element
{
    private Vector2 _pos;
    private Vector2 _size;
    protected Rect _rect;
    protected Rect _Parentrect;

    protected GUISkin skin;

    public bool active { get; set; }
    public virtual Rect parentRect
    {
        get
        {
            return _Parentrect;
        }
        set
        {
            {
                _Parentrect = value;
                _rect = new Rect(_pos.x * parentRect.width, _pos.y * parentRect.height, _size.x * parentRect.width, _size.y * parentRect.height);
            }
        }
    }

    public Vector2 position
    {
        get { return _pos; }
        set
        {
            _pos = value;
            _rect = new Rect(_pos.x * parentRect.width, _pos.y * parentRect.height, _size.x * parentRect.width, _size.y * parentRect.height);
        }
    }
    public Vector2 size
    {
        get { return _size; }
        set
        {
            _size = value;
            _rect = new Rect(_pos.x * parentRect.width, _pos.y * parentRect.height, _size.x * parentRect.width, _size.y * parentRect.height);
        }
    }


    public virtual bool mouseOnGui(Vector2 pos)
    {
        if (!active) return false;
        return _rect.Contains(pos);
    }
    public abstract void Draw();

}
