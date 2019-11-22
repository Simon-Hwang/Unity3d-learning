using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class panelMove : MonoBehaviour
{
    public Vector2 range = new Vector2(4f, 2f);
    Transform trans;
    Quaternion startRot;
    Vector2 mRot = Vector2.zero;
    void Start()
    {
        trans = transform;
        startRot = trans.localRotation;
    }
    
    void Update()
    {
        Vector3 pos = Input.mousePosition; //get the mouse information
        float halfWidth = Screen.width * 0.5f;
        float halfHeight = Screen.height * 0.5f; // get the center of screen
        float x = Mathf.Clamp((pos.x - halfWidth) / halfWidth, -0.4f, 0.4f);
        float y = Mathf.Clamp((pos.y - halfHeight) / halfHeight, -0.4f, 0.4f);
        mRot = Vector2.Lerp(mRot, new Vector2(x, y), Time.deltaTime * 2f); // set the position
        trans.localRotation = startRot * Quaternion.Euler(-mRot.y * range.y, -mRot.x * range.x, 0f);
    }
}
