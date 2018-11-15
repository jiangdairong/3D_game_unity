using UnityEngine;
using System.Collections;

public class unitychanController : basicController
{
    //public GameObject[] magicBall;
    //public GameObject[] shooter;
    public GameObject eleBall;
    public GameObject eleSpark;
    public GameObject ele_eff;
    public GameObject otSpark;

    //private float []speed_of_ball;
    //private Vector3 []vector_of_ball;

    //private bool []ball_shooted;

    private bool Jab_L_isPlaying;
    private bool Jab_R_isPlaying;
    private bool Punch_h_R_isPlaying;

    private GameObject[] OTele;
    private GameObject[] OTspark;
    private GameObject[] OTwords;
    private GameObject[] OTls;

    private int timeForOT;
    //public GameObject R_hand_Cube;
    //public GameObject L_hand_Cube;

    // Use this for initialization
    void Start()
    {
        OTele = new GameObject[10];
        OTspark = new GameObject[10];
        OTls = new GameObject[5];
        OTwords = new GameObject[6];
        OTwords[0] = GameObject.Find("word_white_ruo");
        OTwords[1] = GameObject.Find("word_white_niu");
        OTwords[2] = GameObject.Find("word_white_tz");
        OTwords[3] = GameObject.Find("word_white_shi");
        OTwords[4] = GameObject.Find("word_white_li");
        OTwords[5] = GameObject.Find("word_white_chian");

        for (int i = 0; i < 6; i++)
            OTwords[i].AddComponent<wordEff>();

        //magicBall[0].transform.position = new Vector3(0, -3, 0);
        //magicBall[1].transform.position = new Vector3(0, -3, 0);

        initialization();
        /*ball_shooted = new bool[2];
        speed_of_ball = new float[2];
        vector_of_ball = new Vector3[2];
        ball_shooted[0] = false;
        ball_shooted[1] = false;
        */
        Jab_L_isPlaying = false;
        Jab_R_isPlaying = false;
        Punch_h_R_isPlaying = false;
        
    }
    void Update() {
        update_controller();
    }
    override protected void setCubeTrigger(string str)
    {
        R_hand_Cube.GetComponent<Collider>().isTrigger = false;
        L_hand_Cube.GetComponent<Collider>().isTrigger = false;

        if (str == "r_hand")
            R_hand_Cube.GetComponent<Collider>().isTrigger = true;
        if (str == "l_hand")
            L_hand_Cube.GetComponent<Collider>().isTrigger = true;
    }
    override protected void setHeavyAtk(string str)
    {
        R_hand_Cube.GetComponent<disableRigibodyVelocity>().isHeavyAtk = false;
        L_hand_Cube.GetComponent<disableRigibodyVelocity>().isHeavyAtk = false;

        if (str == "r_hand")
            R_hand_Cube.GetComponent<disableRigibodyVelocity>().isHeavyAtk = true;
        if (str == "l_hand")
            L_hand_Cube.GetComponent<disableRigibodyVelocity>().isHeavyAtk = true;
    }

    override protected void getAtkKey()
    {

        if (atk_b || def_b)
        {
            if (atk_state == c_atk_state)
            {
                clearIHIV();
                canWASD = false;
            }
            if (def_b && (atk_state == d_atk_state || atk_state == c_atk_state)) { // priority higher than combo atk
                atk_state = d_atk_state;
                anim.Play("DEFENSE");
                anim.SetBool("isDEFFENSE", true);
                //anim.Play("Jab_L", -1, 0);
            }
            else if (atk_b && (atk_state == h_atk_state || atk_state == c_atk_state))
            {

                atk_state = h_atk_state;

                if (combo < 3 )
                {
                    anim.Play("Jab_L",-1,0);
                    combo++;

                    faceToOpponent(20);
                    setCTandHA("l_hand",false);

                }
                else if (combo == 3)
                {
                    anim.Play("Punch_h_R");
                    combo++;
                    faceToOpponent(0);
                    setCTandHA("r_hand",true);

                }
            }
        }

    }


    override protected void OT_animation()
    {
        if (OTstate == 2)
        {
            transform.position = new Vector3(-0.25f, -1.5f, -1f);

            anim.Play("WAIT00");

            opponent.transform.position = new Vector3(0.7f, -1.5f, -1f);
            opponent.GetComponent<Animator>().Play("WAIT00");
            opponent.GetComponent<Animator>().Play("Tired");

            opponent.GetComponent<basicController>().faceToOpponent(0);
            faceToOpponent(0);
            OTstate++;
            timeForOT = 0;
            anim.Play("Punch_b_R_OT_slow");
        }
        int numOfEleBall = 8;
        if (OTstate == 3)
        {

            if (canAct())
            {
                if (timeForOT < numOfEleBall)
                {
                    Vector3 pos = new Vector3(0, 0, 0);
                    OTele[timeForOT] = Instantiate(eleBall, pos, Quaternion.identity) as GameObject;
                    OTele[timeForOT].AddComponent<unityChanOTEleBall>();
                    OTele[timeForOT].GetComponent<unityChanOTEleBall>().target = R_hand_Cube;
                }

                if (timeForOT==2) {
                    OTwords[0].GetComponent<wordEff>().canShake = true;
                    OTwords[0].GetComponent<wordEff>().setSca(new Vector3(0.8f, 0.8f, 0.8f));
                    OTwords[0].GetComponent<wordEff>().setPos(transform.position + new Vector3(-4.5f,6f,2f));
                    //OTwords = Instantiate(eleSpark, OTele[i].transform.position, Quaternion.identity) as GameObject;
                }
                if (timeForOT == 4)
                {
                    OTwords[1].GetComponent<wordEff>().canShake = true;
                    OTwords[1].GetComponent<wordEff>().setSca(new Vector3(0.8f, 0.8f, 0.8f));
                    OTwords[1].GetComponent<wordEff>().setPos(transform.position + new Vector3(-5.7f, 3.75f, 2f));
                    //OTwords = Instantiate(eleSpark, OTele[i].transform.position, Quaternion.identity) as GameObject;
                }
                if (timeForOT == 6) {
                    OTwords[2].GetComponent<wordEff>().canShake = true;
                    OTwords[2].GetComponent<wordEff>().setSca(new Vector3(0.8f, 0.8f, 0.8f));
                    OTwords[2].GetComponent<wordEff>().setPos(transform.position + new Vector3(-4f, 1.8f, 2f));
                }

                timeForOT++;
                //anim.GetCurrentAnimatorStateInfo(0).IsName().
                
                if(timeForOT<10)
                    setCanActTime(0, 0.3f);
            }
            if (timeForOT >= 10) {
                anim.Play("Punch_b_R_OT_fast",-1,0.3f);
                setCanActTime(0, 0.15f);
                timeForOT = 0;
                OTstate++;
            }
        }
        if (OTstate == 4)
        {
            if (canAct()) { 
                opponent.GetComponent<Animator>().Play("DAMAGED",-1,0.2f);
                
                anim.speed = 0.0f;
                opponent.GetComponent<Animator>().speed = 0.0f;


                //setCanActTime(0, 0.15f);
                OTstate++;
            }
        }
        if (OTstate == 5) {

            if (canAct()) {
                if(timeForOT == 0)
                {
                    OTwords[3].GetComponent<wordEff>().canShake = true;
                    OTwords[3].GetComponent<wordEff>().setSca(new Vector3(1.2f, 1.2f, 1.2f));
                    //OTwords[3].GetComponent<wordEff>().setPos(transform.position + new Vector3(-1.5f, 5f, 2f));
                    OTwords[3].GetComponent<wordEff>().setPos(transform.position + new Vector3(-1.2f, 3.5f, 2f));

                }
                if (timeForOT == 1)
                {
                    OTwords[4].GetComponent<wordEff>().canShake = true;
                    OTwords[4].GetComponent<wordEff>().setSca(new Vector3(1.2f, 1.2f, 1.2f));
                    //OTwords[4].GetComponent<wordEff>().setPos(transform.position + new Vector3(2.5f, 3.5f, 2f));
                    OTwords[4].GetComponent<wordEff>().setPos(transform.position + new Vector3(3f, 5.2f, 2f));

                }
                if (timeForOT == 2)
                {
                    OTwords[5].GetComponent<wordEff>().canShake = true;
                    OTwords[5].GetComponent<wordEff>().setSca(new Vector3(1.2f, 1.2f, 1.2f));
                    OTwords[5].GetComponent<wordEff>().setPos(transform.position + new Vector3(6.5f, 3f, 2f));
                }

                timeForOT++;


                if (timeForOT < 6)
                    setCanActTime(0, 0.12f);
                else {
                    timeForOT = 0;
                    anim.speed = 1;
                    opponent.GetComponent<Animator>().speed = 1;
                    for (int i = 0; i < numOfEleBall; i++)
                    {
                        GameObject ls = Instantiate(otSpark, OTele[i].transform.position, Quaternion.identity) as GameObject;
                        OTspark[i] = Instantiate(eleSpark, OTele[i].transform.position, Quaternion.identity) as GameObject;
                        if (i == 0)
                            OTspark[i].GetComponent<AudioSource>().enabled = true;
                        Destroy(ls, 3.5f);
                        Destroy(OTspark[i], 3.5f);

                        Destroy(OTele[i]);
                        
                    }

                    for (int i = 0; i < 6; i++) {
                        OTwords[i].GetComponent<wordEff>().canScaleDown = true;
                    }

                    OTstate++;

                }
            }


            
        }
        if (OTstate == 6) {
            if (canAct()) {
                opponent.GetComponent<Animator>().Play("DAMAGED",-1,0);
                opponetPlayerHealth.decreaseHealth(3);
                setCanActTime(0, 0.15f);
                timeForOT++;
            }
            if (timeForOT >= 10) {
                OTstate++;
                setCanActTime(0, 0);
                timeForOT = 0;
            }

        }
        if (OTstate == 7) {
            opponent.GetComponent<basicController>().Attributes.getDown = true;
            opponetPlayerHealth.decreaseHealth(7);
            opponent.GetComponent<Animator>().Play("DamageDown", -1, 0);
            opponent.GetComponent<basicController>().opponent_OTtime = false;

            OTtime = false;
            OTstate = 0;
            timeForOT = 0;
            ItemGenerator.setStart();
        }


    }


}
