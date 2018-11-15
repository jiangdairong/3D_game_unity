using UnityEngine;
using System.Collections;



public class PQchanController : basicController
{
    public GameObject fireBall;
    private GameObject[] OTwords;

    private int OTtimer;
    public GameObject[] OTfb;

    public AudioSource audioPlayer;

    void Start()
    {
        OTwords = new GameObject[8];
        for (int i = 0; i < 8; i++)
        {
            OTwords[i] = GameObject.Find("word_white_fire" + (i + 1));
            OTwords[i].AddComponent<wordEff>();
        }


        OTfb = new GameObject[20];
        OTtimer = 0;
        initialization();

        audioPlayer = GetComponent<AudioSource>();

    }
    void Update()
    {
        update_controller();
    }


    override protected void setCubeTrigger(string str)
    {
        R_hand_Cube.GetComponent<Collider>().isTrigger = false;
        L_hand_Cube.GetComponent<Collider>().isTrigger = false;
        R_lag_Cube.GetComponent<Collider>().isTrigger = false;
        if (str == "r_lag")
            R_lag_Cube.GetComponent<Collider>().isTrigger = true;
        if (str == "r_hand")
            R_hand_Cube.GetComponent<Collider>().isTrigger = true;
        if (str == "l_hand")
            L_hand_Cube.GetComponent<Collider>().isTrigger = true;
    }

    override protected void setHeavyAtk(string str)
    {
        R_hand_Cube.GetComponent<disableRigibodyVelocity>().isHeavyAtk = false;
        L_hand_Cube.GetComponent<disableRigibodyVelocity>().isHeavyAtk = false;
        R_lag_Cube.GetComponent<disableRigibodyVelocity>().isHeavyAtk = false;
        if (str == "r_lag")
            R_lag_Cube.GetComponent<disableRigibodyVelocity>().isHeavyAtk = true;
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
            if (def_b && (atk_state == d_atk_state || atk_state == c_atk_state))
            { // priority higher than combo atk
                atk_state = d_atk_state;
                anim.Play("DEFENSE");
                anim.SetBool("isDEFFENSE", true);
                //anim.Play("Jab_L", -1, 0);
            }
            else if (atk_b && (atk_state == h_atk_state || atk_state == c_atk_state))
            {
                atk_state = h_atk_state;
                if (combo < 3)
                {
                    anim.Play("Jab_L", -1, 0f);
                    combo++;

                    faceToOpponent(20);
                    setCTandHA("l_hand", false);
                }
                else if (combo == 3)
                {
                    anim.Play("Spinkick");
                    combo++;
                    faceToOpponent(30);
                    setCTandHA("r_lag", true);
                }
            }

        }
    }



    override protected void OT_animation()
    {
        if (OTstate == 2)
        {
            transform.position = new Vector3(-2.75f, -1.5f, -1f);

            anim.Play("WAIT00");

            opponent.transform.position = new Vector3(2.7f, -1.5f, -1f);
            opponent.GetComponent<Animator>().Play("WAIT00");
            opponent.GetComponent<Animator>().Play("Tired");

            opponent.GetComponent<basicController>().faceToOpponent(0);
            faceToOpponent(0);
            OTstate++;

            anim.Play("hadoken_OT_slow");
        }
        if (OTstate == 3)
        {
            if (canAct())
            {
                OTfb[OTtimer] = Instantiate(fireBall, fireBall.transform.position, Quaternion.identity) as GameObject;
                OTfb[OTtimer].AddComponent<PQchanOTFireBall>();
                OTfb[OTtimer].GetComponent<PQchanOTFireBall>().target = R_hand_Cube;
                OTfb[OTtimer].transform.position = R_hand_Cube.transform.position;
                OTfb[OTtimer].GetComponent<PQchanOTFireBall>().flashEff = GameObject.Find("light_flash_eff");
                OTfb[OTtimer].GetComponent<PQchanOTFireBall>().spark();

                OTfb[OTtimer].GetComponent<PQchanOTFireBall>().target_v = R_hand_Cube.transform.position;

                OTfb[OTtimer].GetComponent<PQchanOTFireBall>().opponent = opponent;

                OTfb[OTtimer].GetComponent<AudioSource>().enabled = true;

                OTtimer++;
                if (OTtimer < 20)
                    setCanActTime(0, 0.1f);
                else {
                    anim.Play("hadoken_OT_fast", -1, 0.15f);
                    anim.speed = 0;
                    opponent.GetComponent<Animator>().speed = 0;

                    //setCanActTime(0, 0.15f);
                    //OTfb[OTtimer].GetComponent<PQchanOTFireBall>()
                    OTstate++;
                    OTtimer = 0;

                }
            }
        }


        if (OTstate == 4)
        {
            if (canAct())
            {
                OTwords[OTtimer].GetComponent<wordEff>().canShake = true;
                OTwords[OTtimer].GetComponent<wordEff>().setSca(new Vector3(0.8f, 0.8f, 0.8f));

                if (OTtimer == 0)
                    OTwords[OTtimer].GetComponent<wordEff>().setPos(transform.position + new Vector3(-3.88f, 5.51f, 2f));
                if (OTtimer == 1)
                    OTwords[OTtimer].GetComponent<wordEff>().setPos(transform.position + new Vector3(-3.45f, 2.5f, 2f));
                if (OTtimer == 2)
                    OTwords[OTtimer].GetComponent<wordEff>().setPos(transform.position + new Vector3(-0.32f, 5.56f, 2f));
                if (OTtimer == 3)
                    OTwords[OTtimer].GetComponent<wordEff>().setPos(transform.position + new Vector3(0.75f, 2.31f, 2f));
                if (OTtimer == 4)
                    OTwords[OTtimer].GetComponent<wordEff>().setPos(transform.position + new Vector3(3.01f, 5.65f, 2f));
                if (OTtimer == 5)
                    OTwords[OTtimer].GetComponent<wordEff>().setPos(transform.position + new Vector3(4.75f, 2.25f, 2f));
                if (OTtimer == 6)
                    OTwords[OTtimer].GetComponent<wordEff>().setPos(transform.position + new Vector3(7.58f, 5.46f, 2f));
                if (OTtimer == 7)
                    OTwords[OTtimer].GetComponent<wordEff>().setPos(transform.position + new Vector3(8.94f, 2.11f, 2f));

                setCanActTime(0, 0.07f);
                OTtimer++;
            }

            if (OTtimer == 8)
            {
                OTstate++;
                OTtimer = 0;
                //anim.speed = 2f;
                setCanActTime(0, 0.3f);
            }
        }

        if (OTstate == 5)
        {
            if (canAct())
            {
                anim.speed = 2f;
                setCanActTime(0, 0.1f);
                OTstate++;
            }
        }
        if (OTstate == 6)
        {
            if (canAct())
            {
                anim.speed = 0.1f;
                opponent.GetComponent<Animator>().speed = 1f;
                OTstate++;

                for (int i = 0; i < 8; i++)
                {
                    OTwords[i].GetComponent<wordEff>().canScaleDown = true;
                }

            }
        }

        if (OTstate == 7)
        {
            if (canAct())
            {
                //opponent.GetComponent<Animator>().speed = 0.1f;
                OTfb[OTtimer].GetComponent<PQchanOTFireBall>().canShake = false;
                OTfb[OTtimer].GetComponent<PQchanOTFireBall>().canShoot = true;
                OTtimer++;
                if (OTtimer < 20)
                {
                    setCanActTime(0, 0.05f);
                    OTfb[OTtimer].GetComponent<PQchanOTFireBall>().canFly = false;
                    if (OTtimer == 19)
                        OTfb[OTtimer].GetComponent<PQchanOTFireBall>().canFly = true;
                }
                else {
                    anim.speed = 1;
                    OTstate++;
                    OTtimer = 0;
                }
            }
        }


        if (OTstate == 8)
        {
            if(opponent.GetComponent<attributes>().HP < 0.3f)
                opponent.GetComponent<PlayerHealth>().decreaseHealth(1.5f);

            opponent.GetComponent<basicController>().Attributes.getDown = true;
            opponent.GetComponent<Animator>().Play("DamageDown", -1, 0);
            opponent.GetComponent<basicController>().opponent_OTtime = false;

            OTtime = false;
            OTstate = 0;
            OTtimer = 0;
            ItemGenerator.setStart();
        }
    }

    public void playSound(AudioClip sound)
    {
        AudioPlay.playSound(sound, audioPlayer);

    }
}