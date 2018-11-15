using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DimondMovement : MonoBehaviour {
    private float movement;

    // Use this for initialization
    void Start () {
        movement = 0;
    }
	
	// Update is called once per frame
	void Update () {
        movement += 3f*Time.deltaTime;
        transform.Translate(0,0.001f*Mathf.Sin(movement),0);
        transform.Rotate(0, 50f*Time.deltaTime, 0);
        if (movement > 6.28318530718f)
            movement -= 6.28318530718f;
    }

}
