using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parabola_2 : MonoBehaviour
{
    public float init_speed;
    public float y_speed;
    public float x_speed;
    private float gravity = 0.98f;//0.98运动更好被捕捉
    public float angle = 0; //平抛
    // Start is called before the first frame update
    void Start()
    {
        init_speed = 1f;
        x_speed = init_speed * Mathf.Cos(angle);
        y_speed = init_speed * Mathf.Sin(angle);//初始速度
    }

    // Update is called once per frame
    void Update()
    {   
        y_speed -= gravity * Time.deltaTime;
        this.transform.position += new Vector3(Time.deltaTime * x_speed, Time.deltaTime * y_speed, 0);
    }
}
