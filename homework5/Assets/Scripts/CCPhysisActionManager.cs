using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.Engine;

public class CCPhysisActionManager : SSActionManager, SSActionCallback, IActionManager  // server for physice move
{
    int count = 0;
    public SSActionEventType Complete = SSActionEventType.Completed;

    public void MoveDisk(Disk disk)
    {
        count++;
        Complete = SSActionEventType.Started;
        CCPhysisAction action = CCPhysisAction.getAction(disk.speed); 
        addAction(disk.gameObject, action, this);
    }

    public void SSActionCallback(SSAction source) 
    {
        count--;
        Complete = SSActionEventType.Completed;
        source.gameObject.SetActive(false);
    }

    public bool IsAllFinished() 
    {
        if (count == 0) return true;
        else return false;
    }
}