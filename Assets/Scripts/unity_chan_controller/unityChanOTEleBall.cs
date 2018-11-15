using UnityEngine;
using System.Collections;

public class unityChanOTEleBall : MonoBehaviour {
    public GameObject target;
    private Vector3 target_v;
    private bool attach;
    private Vector3 eleBornPos;
	// Use this for initialization
	void Start () {

        eleBornPos = new Vector3(Random.Range(-2F, 2F), Random.Range(-2F, 2F), Random.Range(-2F, 2F));
        GetComponent<AudioSource>().enabled = true;


        eleBornPos += target.transform.position;

        transform.position = eleBornPos;
        //transform.position
        attach = false;
    }
	
	// Update is called once per frame
	void Update () {
        target_v = target.transform.position;
        if ((target_v - transform.position).magnitude < 0.15f)
        {
            attach = true;
        }

        if(attach)
            transform.position = target_v;
        else
            transform.position = Vector3.MoveTowards(transform.position, target_v, 0.1f);
        
    }
}
