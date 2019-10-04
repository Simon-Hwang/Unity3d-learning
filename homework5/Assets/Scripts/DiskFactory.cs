using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.Engine;
public class DiskFactory { 
    public GameObject diskPrefab;
    public static DiskFactory DF = new DiskFactory();
    private Dictionary<int, Disk> used = new Dictionary<int, Disk>();
    private List<Disk> free = new List<Disk>();

    private DiskFactory()
    {
        diskPrefab = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/disk"));
        diskPrefab.AddComponent<Disk>();
        diskPrefab.SetActive(false);
    }

    public void FreeDisk()
    {
        foreach (Disk x in used.Values)
        {
            if (!x.gameObject.activeSelf)
            {
                free.Add(x);
                used.Remove(x.GetInstanceID());
                return;
            }
        }
    }

    public Disk GetDisk(int round)  
    {
        FreeDisk();
        GameObject newDisk = null;
        Disk diskdata;
        if (free.Count > 0)
        {
            newDisk = free[0].gameObject;
            free.Remove(free[0]);
        }
        else
        {
            newDisk = GameObject.Instantiate<GameObject>(diskPrefab, Vector3.zero, Quaternion.identity); // create disk
        }
        newDisk.SetActive(true);
        diskdata = newDisk.AddComponent<Disk>();// add detail
        int swith = Random.Range(round, round * 2);
        float s = Random.Range(round * 10, round * 20);
        float RanX = UnityEngine.Random.Range(-1f, 1f) < 0 ? -1 : 1; 
        diskdata.Direction = new Vector3(RanX, 1, 0);
        diskdata.StartPoint = new Vector3(Random.Range(-110, -130), Random.Range(30,90), Random.Range(110,140));
        diskdata.speed = round * 5 + s; 
        int choose = round % 3;  // change the disk color according to round
        switch (choose)   
        {  
            case 1:  
                {  
                    diskdata.color = Color.yellow;  
                    break;  
                }  
            case 2:  
                {  
                    diskdata.color = Color.red;  
                    break;  
                }  
            case 0:  
                {  
                    diskdata.color = Color.black;  
                    break;  
                }
        }
        used.Add(diskdata.GetInstanceID(), diskdata); //添加到使用中
        diskdata.name = diskdata.GetInstanceID().ToString();
        return diskdata;  
    }
}