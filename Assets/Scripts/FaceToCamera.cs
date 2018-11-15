using UnityEngine;
using System.Collections;

public class FaceToCamera : MonoBehaviour {
    public Camera my_camera;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(transform.position + my_camera.transform.rotation * Vector3.back,
             my_camera.transform.rotation * Vector3.down);
    }
}
