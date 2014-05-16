using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EditCommandManager
{
    private static EditCommandManager instance = null;
    public static EditCommandManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new EditCommandManager();
                instance.Awake();
            }
            return instance;
        }
    }
    
    private List<Command> history;
    private int histIndex;
    
    void Awake()
    {
        history = new List<Command>();
        histIndex = -1;
    }

    public bool executeCommand(Command c)
    {
        if(!c.exectute()) return false;
        if (histIndex < history.Count - 1)
        {
            for (int i = histIndex+1; i < history.Count; i++)
            {
                history[i].freeResources();
            }
            //Debug.Log("Remove Elements :" + (histIndex+1).ToString()+ " to "+);
            history.RemoveRange(histIndex+1, history.Count - histIndex-1);
        }
        histIndex++;
        history.Add(c);
        return true;
    }

    public void undo()
    {
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
        if (histIndex < history.Count-1 && histIndex >= -1)
        {
            histIndex++;
            history[histIndex].redo();
        }
        else
        {
            Debug.Log("Can't Redo");
        }
    }

    public void resetHistory()
    {
        history.RemoveRange(0, history.Count);        
        histIndex = -1;
    }
}
