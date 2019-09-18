using UnityEngine;
using System.Collections;
using Com.MyGame;
 
public class UserInterface : MonoBehaviour {
    IUserActions myActions;
    float btnWidth = (float)Screen.width / 8.0f;
    float btnHeight = (float)Screen.height / 8.0f;
 
    void Start () {
        myActions = mainSceneController.getInstance() as IUserActions;
    }
	
	void Update () {
	
	}
 
    void OnGUI() {
        if (GUI.Button(new Rect(100, 350, btnWidth, btnHeight), "Priests GetOn")) {
            myActions.priestsGetOn();
        }
        if (GUI.Button(new Rect(225, 350, btnWidth, btnHeight), "Priests GetOff")) {
            myActions.priestsGetOff();
        }
        if (GUI.Button(new Rect(375, 350, btnWidth, btnHeight), "Go!")) {
            myActions.boatMove();
        }
        if (GUI.Button(new Rect(525, 350, btnWidth, btnHeight), "Devils GetOn")) {
            myActions.devilsGetOn();
        }
        if (GUI.Button(new Rect(675, 350, btnWidth, btnHeight), "Devils GetOff")) {
            myActions.devilsGetOff();
        }
 
    }
}
