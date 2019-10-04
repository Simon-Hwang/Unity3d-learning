using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.Engine;
public class CCActionManager : SSActionManager, SSActionCallback
{
    int count = 0;
    public SSActionEventType Complete = SSActionEventType.Completed;

    public void MoveDisk(Disk Disk)
    {
        count++;
        Complete = SSActionEventType.Started;
        CCMoveToAction action = CCMoveToAction.getAction(Disk.speed);
        addAction(Disk.gameObject, action, this); // SSActionManage is executed by system auto, not by user itself
    }

    public void SSActionCallback(SSAction source) //callback when the program edd
    {
        count--;
        Complete = SSActionEventType.Completed;
        source.gameObject.SetActive(false);
    }

    public bool IsAllFinished() //check 
    {
        if (count == 0) return true;
        else return false;
    }
}