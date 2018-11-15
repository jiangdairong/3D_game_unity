using UnityEngine;
using System.Collections;

public class cameraControl : MonoBehaviour {
    public GameObject player1;
    public GameObject player2;
    private float midPointOfTwoPlayers_x;
    private float nearest_z;
    private float offset_z;
    public bool cameraControlEnable=true;

   

    /*
    X = 0
    Y = 3
    Z = -20
    */

    //private float distance;
    // Use this for initialization
    void Start () {
        if(cameraControlEnable)
            this.gameObject.transform.position = new Vector3(0,3,-20);
        offset_z = -10;
        //cameraControlEnable = true;
    }
	
	// Update is called once per frame
	void Update () {
        if (cameraControlEnable == false)
            return;
        //Vector3 relativePos = (player1.transform.position + player2.transform.position)/2.0f - transform.position;
        //Quaternion rotation = Quaternion.LookRotation(relativePos);
        //transform.rotation = rotation;

        //distance = (player1.transform.position - player2.transform.position).sqrMagnitude;

        midPointOfTwoPlayers_x = (player1.transform.position.x + player2.transform.position.x) / 2.0f;
        if (player1.transform.position.z < player2.transform.position.z)
            nearest_z = player1.transform.position.z;
        else
            nearest_z = player2.transform.position.z;
        this.gameObject.transform.position = new Vector3(midPointOfTwoPlayers_x, this.gameObject.transform.position.y, nearest_z + offset_z );



    }
}
