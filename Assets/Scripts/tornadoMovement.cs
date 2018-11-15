using UnityEngine;
using System.Collections;

public class tornadoMovement : MonoBehaviour {
    public GameObject tornado;
    Vector3 startPos;
    ParticleSystem windEff;
    float degree;

    public bool canMove;
	// Use this for initialization
	void Start () {
        windEff = tornado.GetComponent<ParticleSystem>();

        degree = 0;
        startPos = new Vector3(0,-0.73f,0);
        canMove = true;
    }
    public void setMove() {
        canMove = true;
        tornado.SetActive(true);
        transform.position = startPos;
        transform.rotation = Quaternion.identity;
    }
    public void setStop() {
        tornado.SetActive(false);
        canMove = false;
    }
    // Update is called once per frame
    void Update () {
        if (canMove) {
            
            transform.Rotate(0, 1f, 0);
            if (transform.rotation.eulerAngles.y > 350)
            {
                tornado.SetActive(false);
                canMove = false;
            }
            //transform.position = new Vector3(0, transform.position.y-0.007f, 0);
            //transform
        }
    }

}
