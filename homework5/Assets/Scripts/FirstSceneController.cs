using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.Engine;
public class FirstSceneController : MonoBehaviour, ISceneController, UserAction
{
    public bool pyhsics = false;
    bool manager = false; // true->physics
    int score = 0;
    int round = 1;
    int trail = 0;
    bool start = false;
    IActionManager Manager;
    DiskFactory DF;

    void Awake()
    {
        SSDirector director = SSDirector.getInstance();
        director.currentScenceController = this;
        DF = DiskFactory.DF;
    }

    // Use this for initialization
    void Start () {
        
    }

    // Update is called once per frame
    int count = 0;
	void Update () {
        if(start == true)
        {
            count++;
            if (count >= 120)  // 120s over
            {
                count = 0;
                trail++;
                Disk d = DF.GetDisk(round);
                Manager.MoveDisk(d);
                if (trail == 10)
                {
                    round++;
                    trail = 0;
                }
            }
        }
	}

    public void LoadResources() // awake physics mode or man-made mode
    {
        manager = pyhsics;
        if (pyhsics)
        {
            Manager = this.gameObject.AddComponent<CCPhysisActionManager>() as IActionManager;
        }
        else
        {
            Manager = this.gameObject.AddComponent<CCActionManager>() as IActionManager;
        }
    }

    public void Hit(Vector3 pos) // hit then judge score add  
    {
        Ray ray = Camera.main.ScreenPointToRay(pos);

        RaycastHit[] hits;
        hits = Physics.RaycastAll(ray);
        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];

            if (hit.collider.gameObject.GetComponent<Disk>() != null)
            {
                Color c = hit.collider.gameObject.GetComponent<Renderer>().material.color;
                if(c == Color.yellow) 
                    score += 1 + round / 3;
                else if(c == Color.red)
                    score += 2  + round / 3;
                else 
                    score += 3 + round / 3 ;
                hit.collider.gameObject.transform.position = new Vector3(0, -5, 0);//not destroy, just to manager to free disk 
            }
        }
    }

    public int GetScore()
    {
        return score;
    }

    public void Restart()
    {
        score = 0;
        round = 1;
        start = true;
    }
    public bool RoundStop()
    {
        if (round > 10) // ten round or 120s
        {
            start = false;
            return Manager.IsAllFinished();
        }
        else return false;
    }
    public int GetRound()
    {
        return round;
    }
}