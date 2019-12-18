using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class button : MonoBehaviour, IVirtualButtonEventHandler
{
    // Start is called before the first frame update
    public Animator ani;
    public VirtualButtonBehaviour[] vbs;
    void Start()
    {
        vbs = GetComponentsInChildren<VirtualButtonBehaviour>();
        //VirtualButtonBehaviour vbb = vb.GetComponent<VirtualButtonBehaviour>();
        for (int i = 0; i < vbs.Length; i++)
        {
            vbs[i].RegisterEventHandler(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnButtonPressed(VirtualButtonBehaviour button)
    {
        if (button.VirtualButtonName == "button1")
        {
            ani.gameObject.transform.position += new Vector3(-1, 0, 0);
        }
        else {
            ani.gameObject.transform.position += new Vector3(1, 0, 0);
        }
        Debug.Log(button.VirtualButtonName);
    }

    public void OnButtonReleased(VirtualButtonBehaviour vb)
    {

    }
}
