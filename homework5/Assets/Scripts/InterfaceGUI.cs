using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.Engine;
using UnityEngine.UI;

public class InterfaceGUI : MonoBehaviour {
    UserAction UserActionController;
    public bool physics = false;//choice betwneen physics & man-made
    ISceneController controller;
    public GameObject t;
    bool ss = false;
    float S;
    float Now;
    int round = 1;
    // Use this for initialization
    void Start () {
        UserActionController = SSDirector.getInstance().currentScenceController as UserAction;
        controller = SSDirector.getInstance().currentScenceController as ISceneController;
        S = Time.time;
    }

    private void OnGUI()
    {
        if(!ss) 
            S = Time.time;
        
        if (!ss && GUI.Button(new Rect(Screen.width / 2 - 30, Screen.height / 2 - 30, 100, 50), "Start"))
        {
            S = Time.time;
            ss = true;
            UserActionController.Restart();
            controller.LoadResources();
        }
        if (ss)
        {
            GUI.Button(new Rect(0, 0, 100, 50), "Round: " + round );
            GUI.Button(new Rect(0, 55, 100, 50), "Score: " + UserActionController.GetScore().ToString());
            GUI.Button(new Rect(0, 110, 100, 50), "Time: " + ((int)(Time.time - S)).ToString());
            if(GUI.Button(new Rect(0, 165, 100, 50), "Reset")){
                S = Time.time;
                UserActionController.Restart();
            }
            round = UserActionController.GetRound();
            if (Input.GetButtonDown("Fire1"))
            {

                Vector3 pos = Input.mousePosition;
                UserActionController.Hit(pos);

            }
            if (round > 10)
            {
                round = 10;
                if (UserActionController.RoundStop())
                {
                    ss = false;
                }
            }
        }
    }
}