using UnityEngine;
using System.Collections;

public class Command_ChangePosition : Command
{

    private Vector2 oldPos_;
    private Vector2 newPos_;
    private int layerIndex_;
    private GameObject objToEdit_;

    public void setUpCommand(Vector2 oldPos, Vector2 newPos, int layerIndex, GameObject objToEdit)
    {
        oldPos_ = oldPos;
        newPos = newPos_;
        layerIndex_ = layerIndex;
        objToEdit_ = objToEdit;
    }

    public bool exectute()
    {
        Gridable grid = objToEdit_.GetComponentInChildren<Gridable>();
        if (grid == null)
        {
            return false;
        }
        CommandHelper.setMatrixForceOverride(layerIndex_, oldPos_, grid, null);
        if (CommandHelper.setMatrix(layerIndex_, newPos_, grid, objToEdit_))
        {
            return true;
        }
        else
        {
            CommandHelper.setMatrixForceOverride(layerIndex_, oldPos_, grid, objToEdit_);
            return false;
        }
    }

    public void undo()
    {
        Gridable grid = objToEdit_.GetComponentInChildren<Gridable>();
        CommandHelper.setMatrixForceOverride(layerIndex_, newPos_, grid, null);
        CommandHelper.setMatrixForceOverride(layerIndex_, oldPos_, grid, objToEdit_);
    }

    public void redo()
    {
        exectute();
    }

    public void freeResources()
    {

    }
}
