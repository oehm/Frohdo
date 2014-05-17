using UnityEngine;
using System.Collections;

public interface SCondition
{
    bool isFullfilled { get; set; }
    void checkCondition();
}
