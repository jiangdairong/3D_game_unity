using UnityEngine;
using System.Collections;

public class wordEff : MonoBehaviour {

    // Use this for initialization
    public bool canShake;
    public bool canScaleDown;

    
    private float canInAtkSysCurTime;
    private float canInAtkSysEndTime;
    private Vector3 originalPos;
    private Vector3 originalSca;
    private Vector3 pushVector;
    private float shakeAmp;
    public float maxShakeAmp;
    private float scaleAmp;
    private int moveAwayTimer;
    void Start () {
        //canShake = false;
        canScaleDown = false;
        canInAtkSysCurTime = 0f;
        canInAtkSysEndTime = 0f;
        originalPos = transform.position;
        maxShakeAmp = 0.3f;
        shakeAmp = 0.3f;
        scaleAmp = 1f;
        moveAwayTimer = 0;
        pushVector = new Vector3(0, 0, 0);


    }
    public void setMaxShakeAmp(float amp) {
        maxShakeAmp = amp;
        shakeAmp = amp;
    }
    // Update is called once per frame
    void Update () {
        if (canShake) {
            shake();
        }
        if (canScaleDown) {
            scaleDown();
        }
	}

    private void scaleDown() {
        transform.position = originalPos + new Vector3(Random.Range(-shakeAmp, shakeAmp), Random.Range(-shakeAmp, shakeAmp), Random.Range(-shakeAmp, shakeAmp));
        scaleAmp -= 0.01f;

        if(scaleAmp<0.5f)
            setSca(new Vector3(scaleAmp / 0.5f, scaleAmp / 0.5f, scaleAmp / 0.5f));

        if (scaleAmp < 0)
        {
            canScaleDown = false;
            scaleAmp = 1f;
            transform.localPosition = new Vector3(0, 0, 0);
            //Destroy(this.gameObject);
        }
    }


    void shake() {
        transform.position = originalPos + new Vector3(Random.Range(-shakeAmp, shakeAmp), Random.Range(-shakeAmp, shakeAmp), Random.Range(-shakeAmp, shakeAmp));
        shakeAmp -= 0.01f;
        if (shakeAmp < 0) {
            canShake = false;
            shakeAmp = maxShakeAmp;
        }
    }

    public void setPos(Vector3 v) {
        GetComponent<AudioSource>().Play();
        transform.position = v;
        originalPos = v;
    }
    public void setSca(Vector3 v)
    {
        transform.localScale = v;
    }
    void setCanActTime(float ct, float et)
    {
        canInAtkSysCurTime = ct;
        canInAtkSysEndTime = et;
    }
    bool canAct()
    {
        if (canInAtkSysCurTime >= canInAtkSysEndTime)
            return true;

        canInAtkSysCurTime += Time.deltaTime;
        return false;
    }
}
