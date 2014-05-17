using UnityEngine;
using System.Collections;

public interface Command
{
    bool exectute();
    void undo();
    void redo();
    void freeResources();
}

