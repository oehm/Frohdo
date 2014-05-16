using UnityEngine;
using System.Collections;

public class GUI_ContentFunction {
    public  delegate void Del(params object[] parameter);
    public GUIContent content { get; set; }
    public Del func { get; set; }
}
