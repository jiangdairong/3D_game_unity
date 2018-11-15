using UnityEngine;
using System.Collections;

public class bombMovement : MonoBehaviour {

    Vector3 originalPos;
    int shakeFrames;
    int shakeCounter;
    float shakeAmp;
    int randFrames;
    int counter;
    GameObject bombEff;
	// Use this for initialization
	void Start () {
        originalPos = transform.position;
        counter = 0;
        shakeFrames = 60;
        shakeCounter = 0;
        shakeAmp = 0.1f;
        randFrames = Random.Range(60,150);
        bombEff = GameObject.Find( "Eff_Burst_1_oneShot");
    }
	
	// Update is called once per frame
	void Update () {
        if (counter < randFrames)
        {
            counter++;
        }
        else {
            shake();
        }
	}
    void shake() {
        if (shakeCounter < shakeFrames)
        {
            shakeCounter++;
            transform.position = originalPos + new Vector3(Random.Range(-shakeAmp, shakeAmp), Random.Range(-shakeAmp, shakeAmp), Random.Range(-shakeAmp, shakeAmp));
        }
        else {
            GameObject eff = Instantiate(bombEff, transform.position, Quaternion.identity) as GameObject;
            eff.AddComponent<Rigidbody>();
            eff.GetComponent<Rigidbody>().useGravity = false;
            eff.AddComponent<SphereCollider>();
            eff.GetComponent<SphereCollider>().radius = 2f;
            eff.GetComponent<SphereCollider>().isTrigger = true;
            eff.tag = "bombEff";
            eff.GetComponent<AudioSource>().enabled = true;
            Destroy(eff, 0.7f);

            Destroy(gameObject);
        }
    }
}
