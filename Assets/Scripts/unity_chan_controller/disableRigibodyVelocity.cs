using UnityEngine;
using System.Collections;

public class disableRigibodyVelocity : MonoBehaviour {
    public string player_tag;
    public GameObject player;

    public int atk_offset;

    public bool isHeavyAtk;

    private int gap;
    // Use this for initialization
    void Start () {
        isHeavyAtk = false;
        //gap = -10;

        //atk_offset = 1;
        gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        this.gameObject.tag = "atk_" + player.tag;
        gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;

        gameObject.GetComponentInChildren<Collider>().isTrigger = false;
        //this.transform.Translate(0, gap, 0);
    }


    //void set_atk_offset(int offset) {
    //    int off_tmp = offset - atk_offset;
    //    atk_offset += off_tmp;
    //    this.transform.Translate(0, gap * off_tmp, 0);
    //}
}
