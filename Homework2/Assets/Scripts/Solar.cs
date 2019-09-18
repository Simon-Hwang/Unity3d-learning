using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solar : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform moon;
    public Transform mercury;
    public Transform venus;
    public Transform earth;
    public Transform mars;
    public Transform jupiter;
    public Transform saturn;
    public Transform uranus;
    public Transform neptune;
    private float[] pos_planets = {2.11f, 3.23f,0.99f,4.34f,1.02f,0.98f,0.97f,0.96f};
    private int[] ang_planets = {47, 35,30,24,13,9,6,5};
    private int[] spe_planets = {300,280,250,220,180,160,150,140};
    void Start()
    {
        this.transform.position = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        mercury.Rotate (Vector3.up * spe_planets[0] * Time.deltaTime);
        venus.Rotate (Vector3.up * spe_planets[1] * Time.deltaTime);
        earth.Rotate (Vector3.up * spe_planets[2] * Time.deltaTime);
        mars.Rotate (Vector3.up * spe_planets[3] * Time.deltaTime);
        jupiter.Rotate (Vector3.up * spe_planets[4] * Time.deltaTime);
        saturn.Rotate (Vector3.up * spe_planets[5] * Time.deltaTime);
        uranus.Rotate (Vector3.up * spe_planets[6] * Time.deltaTime);
        neptune.Rotate (Vector3.up * spe_planets[7] * Time.deltaTime);

        mercury.RotateAround (this.transform.position, new Vector3(0, pos_planets[0], 1), ang_planets[0] * Time.deltaTime);
        venus.RotateAround (this.transform.position, new Vector3(0, pos_planets[1], 1), ang_planets[1] * Time.deltaTime);
        earth.RotateAround (this.transform.position, new Vector3(0, pos_planets[2], 2), ang_planets[2] * Time.deltaTime);
        mars.RotateAround (this.transform.position, new Vector3(0, pos_planets[3], 5), ang_planets[3] * Time.deltaTime);
        jupiter.RotateAround (this.transform.position, new Vector3(0, pos_planets[4], 1), ang_planets[4] * Time.deltaTime);
        saturn.RotateAround (this.transform.position, new Vector3(0, pos_planets[5], 3), ang_planets[5] * Time.deltaTime);
        uranus.RotateAround (this.transform.position, new Vector3(0, pos_planets[6], 1), ang_planets[6] * Time.deltaTime);
        neptune.RotateAround (this.transform.position, new Vector3(0, pos_planets[7], 1), ang_planets[7] * Time.deltaTime);

        moon.transform.RotateAround (earth.position, Vector3.up, 300 * Time.deltaTime);
    }
}
