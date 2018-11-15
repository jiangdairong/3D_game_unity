using UnityEngine;
using System.Collections;

public class PQchanOTFireBall : MonoBehaviour
{
    public GameObject target;
    public GameObject opponent;
    public GameObject flashEff;
    //private PlayerHealth opponentPlayerHealth;

    public Vector3 target_v;

    public GameObject eff1;
    public GameObject eff2;
    public GameObject eff3;


    public bool canFly;

    public bool canShake;
    public bool canShoot;
    public Vector3 shoot_v;
    private float shakeAmp;
    // Use this for initialization
    void Start()
    {
        //flashEff = GameObject.Find("light_flash_eff");
        //GameObject fe = Instantiate(flashEff, flashEff.transform.position,Quaternion.identity) as GameObject;
        //Destroy(fe, 2f);

        eff1 = GameObject.Find("Eff_Burst_1_oneShot");
        eff2 = GameObject.Find("Eff_Burst_2_oneShot");
        eff3 = GameObject.Find("light_flash_eff");

        canShake = true;
        canShoot = false;
        shakeAmp = 0.1f;
        shoot_v = new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));

        while ((shoot_v.x > 0.01f || shoot_v.x < -0.01f) && (shoot_v.z > 0.01f || shoot_v.z < -0.01f))
        {
            shoot_v = new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));
        }
        shoot_v.Normalize();
    }

    // Update is called once per frame
    void Update()
    {
        if (canShake)
        {
            shake();
        }
        if (canShoot)
        {
            shoot();
        }
    }

    public void spark() {
        GameObject fe = Instantiate(flashEff, transform.position, Quaternion.identity) as GameObject;
        Destroy(fe, 2f);
    }

    void shake()
    {
        target_v = target.transform.position;
        transform.position = target_v + new Vector3(Random.Range(-shakeAmp, shakeAmp), Random.Range(-shakeAmp, shakeAmp), Random.Range(-shakeAmp, shakeAmp));
    }

    void shoot()
    {
        Vector3 opponent_pos = opponent.transform.position;
        //opponent_pos = new Vector3(opponent_pos.x, 0, opponent_pos.z);
        Vector3 v = opponent_pos - transform.position;



        v = new Vector3(v.x, 0, v.z).normalized;
        v.Normalize();

        //shoot_v = v*0.2f + shoot_v*0.8f;
        //shoot_v.Normalize();
        transform.position += v * 0.2f;
    }
    protected void OnTriggerEnter(Collider other)
    {
        if (other.tag == opponent.tag) {
            float r = Random.Range(-1.0f, 1.0f);
            if (r > 0.3f)
            {
                GameObject eff = Instantiate(eff1, transform.position, Quaternion.identity) as GameObject;
                eff.GetComponent<AudioSource>().enabled = true;
                Destroy(eff, 5f);
            }
            else if(r<-0.3f) {
                GameObject eff = Instantiate(eff2, transform.position, Quaternion.identity) as GameObject;
                eff.GetComponent<AudioSource>().enabled = true;
                Destroy(eff, 5f);
            }
            else{
                GameObject eff = Instantiate(eff3, transform.position, Quaternion.identity) as GameObject;
                Destroy(eff, 5f);
            }

            other.GetComponent<Animator>().Play("DAMAGED",-1,0);
            if(canFly)
                other.GetComponent<Animator>().Play("DamageDown", -1, 0);
            if (opponent.GetComponent<attributes>().HP > 1.6f)
                opponent.GetComponent<PlayerHealth>().decreaseHealth(1.5f);
            else
                opponent.GetComponent<attributes>().HP = 0.1f;

            Destroy(this.gameObject);
        }
    }
}


