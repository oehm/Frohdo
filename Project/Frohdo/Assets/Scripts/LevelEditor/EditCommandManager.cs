using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EditCommandManager : MonoBehaviour
{
    private List<Command> history;

    private int histIndex;
    private int lastModifiedIndex;
    // Use this for initialization
    void Start()
    {
        history = new List<Command>();
        histIndex = -1;
        lastModifiedIndex = -1;
    }

    public void executeCommand(Command c)
    {
        c.exectute();
        histIndex++;
        lastModifiedIndex = histIndex;
        history.RemoveRange(lastModifiedIndex, history.Count - lastModifiedIndex);
        history.Add(c);
        Debug.Log("Histrory is " + history.Count.ToString() + " long!");
    }

    public void undo()
    {
        Debug.Log(histIndex);

        if (histIndex >= 0 && histIndex < history.Count)
        {
            history[histIndex].undo();
            histIndex--;
        }
        else
        {
            Debug.Log("Can't undo!");
        }
    }

    public void redo()
    {
        Debug.Log(histIndex);
        if (histIndex < lastModifiedIndex && histIndex >= -1)
        {
            histIndex++;
            history[histIndex].redo();
        }
        else
        {
            Debug.Log("Can't Redo");
        }
    }
}
