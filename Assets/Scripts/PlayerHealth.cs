using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour {

    attributes atb;

    public float max_Health;// = 100f;
    //public float cur_Health = 0.0f;

    public GameObject healthBar;
	// Use this for initialization
	void Start () {
        //cur_Health = max_Health;
        //InvokeRepeating("decreaseHealth",1.0f,1.0f);
        max_Health = 100;
        atb = gameObject.GetComponent<attributes>();
        atb.HP = max_Health;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    public void decreaseHealth(float dmg)
    {
        atb.HP -= dmg;
        if (atb.HP < 0)
            atb.HP = 0;
        float calc_Health = (float)atb.HP / max_Health;
        SetHealthBar(calc_Health);
    }


    public void SetHealthBar(float myHealth)
    {
        healthBar.transform.localScale = new Vector3(myHealth,healthBar.transform.localScale.y, healthBar.transform.localScale.z);
        
    }
}
