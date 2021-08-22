using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugPersistentData : MonoBehaviour
{
    private void Start()
    {
        Debug();
    }
    [ContextMenu("Debug Persistent Data")]public void Debug()
    {
        PersistentData.Debug();
    }
}
