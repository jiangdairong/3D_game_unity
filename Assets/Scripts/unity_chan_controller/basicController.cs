using UnityEngine;
using System.Collections;
public class basicController : MonoBehaviour {

    public GameObject l_atk_particles;
    public itemGenerator ItemGenerator;
    public string up_button, down_button, right_button, left_button, attack_button, magic_button, defend_button;

    public float inputV;
    public float inputH;
    public float tempV;
    public float tempH;

    public gameSystem gameSys;

    public Animator anim;


    public GameObject opponent;
    public Animator opponent_anim;

    public bool canWASD;

    protected Vector3 moveDir;
    protected int animState;

    public attributes Attributes;

    public string character;

    protected string opponent_tag;

    protected float deltaTime;

    //protected bool h_atk, l_atk, m_atk;
    protected int atk_state;

    protected const int c_atk_state = 0;
    protected const int h_atk_state = 1;
    protected const int d_atk_state = 2;
    protected const int l_atk_state = 3;


    protected bool first_time_in_Attack_if_state;

    protected int combo;

    public GameObject R_hand_Cube;
    public GameObject L_hand_Cube;
    public GameObject R_lag_Cube;
    public GameObject L_lag_Cube;

    public bool OTtime;
    public bool opponent_OTtime;
    protected int OTstate;

    protected float canInAtkSysEndTime;
    protected float canInAtkSysCurTime;

    public GameObject getOTboxParticle;
    public GameObject toOpponentOTboxParticle;

    public GameObject cameraPlane;
    private Material cameraPlaneMaterial;
    private bool cameraPlaneIsDark;
    // Use this for initialization
    protected PlayerHealth playerHealth;
    protected PlayerHealth opponetPlayerHealth;
    protected bool def_b;
    protected bool atk_b;


    public bool blockGame;

    bool thereIsAWinner;

    virtual protected void setCubeTrigger(string str) {
        R_hand_Cube.GetComponent<Collider>().isTrigger = false;
        L_hand_Cube.GetComponent<Collider>().isTrigger = false;

        if (str == "r_hand")
            R_hand_Cube.GetComponent<Collider>().isTrigger = true;
        if (str == "l_hand")
            L_hand_Cube.GetComponent<Collider>().isTrigger = true;
    }
    virtual protected void setHeavyAtk(string str) {

    }
    virtual protected void setCTandHA(string str, bool isHA) {
        setCubeTrigger(str);
        if (isHA)
            setHeavyAtk(str);
    }

    //protected string last_WASD_state;
    //protected string now_WASD_state;



    void Start()
    {
        initialization();

    }
    public void initialization()
    {
        //if (choosechar.cp2 != null && choosechar.cp1 != null)
        //{
        //    print(choosechar.cp2);
        //    print(choosechar.cp1);
        //    print(name);
        //    if (name == "PQchan")
        //        tag = choosechar.cp2;
        //    else
        //        tag = choosechar.cp1;
        //}

        ItemGenerator = GameObject.Find("Main Camera").GetComponent<itemGenerator>();
        thereIsAWinner = false;
        def_b = false;
        atk_b = false;
        gameSys = GameObject.Find("Main Camera").GetComponent<gameSystem>();
        playerHealth = GetComponent<PlayerHealth>();
        OTstate = 0;
        cameraPlane = FindWithTag("camera_plane");
        cameraPlaneMaterial = cameraPlane.GetComponent<Renderer>().material;
        cameraPlaneIsDark = false;
        //gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        if (this.gameObject.tag == "p1")
        {
            /*
            up_button = "w";
            down_button = "s";
            right_button = "d";
            left_button = "a";
            
            attack_button = "f";
            //magic_button = "g";
            defend_button = "g";
            */
            opponent_tag = "p2";
        }
        else {
            /*
            up_button = "up";
            down_button = "down";
            right_button = "right";
            left_button = "left";
            
            attack_button = "/";
            //magic_button = ".";
            defend_button = ".";
            */
            opponent_tag = "p1";
        }
        opponent = FindWithTag(opponent_tag);
        opponetPlayerHealth = opponent.GetComponent<PlayerHealth>();
        opponent_anim = opponent.GetComponent<Animator>();
        //print(opponent.GetComponent<attributes>().canBeAtk);
        Attributes = GetComponent<attributes>();

        anim = GetComponent<Animator>();
        //anim.speed = 20;
        first_time_in_Attack_if_state = true;
        canWASD = true;
        atk_state = c_atk_state;
        combo = 0;
        //last_WASD_state = up_button;
        //now_WASD_state = up_button;

        canInAtkSysEndTime = 0;
        canInAtkSysCurTime = 0;
        OTtime = false;
        opponent_OTtime = false;
    }

    // Update is called once per frame
    void Update()
    {
        update_controller();
    }

    void getButton() {
        def_b = Input.GetButton("def_button_" + tag) || Input.GetButtonDown("def_button_" + tag);
        atk_b = Input.GetButtonDown("atk_button_" + tag);
    }
    void checkWin() {
        if (Attributes.HP <= 0) {
            gameSys.setWinner(int.Parse(opponent.tag.Substring(1,1)));
            thereIsAWinner = true;
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("DamageDown"))
                anim.Play("DamageDown");
        }
    }
    virtual protected void update_controller() {
        //print(tag + Input.GetAxis("Horizontal_p1"));
        if (thereIsAWinner)
            return;
        if(!OTtime)
            checkWin();

        if (blockGame)
            return;

        getButton();

        if (OTtime)
        {
            if (OTstate == 0)
                cameraPlaneGetDaker();
            else if (OTstate == 1)
                cameraPlaneGetLighter();
            else if (OTstate >= 2)
                OT_animation();
        }
        else if (opponent_OTtime) {
        }
        else {
            if (Attributes.getDown)
            {
                Attributes.canBeAtk = false;
                getDownHandling();
            }
            if (!Attributes.getDown)
            {
                if (canAct())
                {
                    Attributes.canBeAtk = true;

                    atkSystem();
                    //defense();
                    if (canWASD)
                    {
                        wasdKeyHandler();
                        walkControl();
                    }
                }
                //if(anim)
                if(anim.GetCurrentAnimatorStateInfo(0).IsName("WAIT00") || anim.GetCurrentAnimatorStateInfo(0).IsName("MOVE_F"))
                this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, -1.5f, this.gameObject.transform.position.z);
            }

            collisionHandling();
        }
    }
    virtual protected bool canAct()
    {

        if (canInAtkSysCurTime >= canInAtkSysEndTime)
            return true;

        canInAtkSysCurTime += Time.deltaTime;

        return false;

    }
    virtual protected void setCanActTime(float ct, float et)
    {
        canInAtkSysCurTime = ct;
        canInAtkSysEndTime = et;
    }
    virtual protected void collisionHandling() {
        if (gameObject.tag == "p1")
        {
            Vector3 v = gameObject.transform.position - opponent.transform.position;
            float dis = v.magnitude;
            if (dis < 0.5)
            {
                v.Normalize();
                float temp = 0.5f - dis;
                v *= temp / 2;

                gameObject.transform.position += v;
                opponent.transform.position -= v;

            }
        }
    }

    virtual protected void wasdKeyHandler() {

        tempV = inputV;
        tempH = inputH;
        inputH = Input.GetAxis("Horizontal_" + tag);
        //print(inputH+tag);
        inputV = Input.GetAxis("Vertical_" + tag);
        //print(inputV + tag);
         

        anim.SetFloat("inputH", inputH);
        anim.SetFloat("inputV", inputV);

        //anim.SetFloat("HGap",Mathf.Abs( inputH - tempH));
        //anim.SetFloat("VGap", Mathf.Abs(inputV - tempV));
    }
    virtual protected void walkControl() {

        float maxInput = Mathf.Abs(inputH);
        if (maxInput < Mathf.Abs(inputV))
            maxInput = Mathf.Abs(inputV);


        
        //if (Input.GetKey(up_button) || Input.GetKey(left_button) || Input.GetKey(down_button) || Input.GetKey(right_button))
        if (inputH > 0.1f || inputH < -0.1f || inputV > 0.1f || inputV < -0.1f)
        //if (inputH != 0 || inputV != 0)
        {

            anim.Play("MOVE_F");// make suer that the character will move
            if (canWASD)
            {
                Vector3 relativePos = new Vector3(inputH, 0, inputV);

                //relativePos.is

                Quaternion rotation = Quaternion.LookRotation(relativePos);
                transform.rotation = rotation;

            }
            else {
                clearIHIV();
            }
            moveDir = new Vector3(0, 0, 1).normalized;

            transform.Translate(moveDir * Time.deltaTime * 10 * maxInput);
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("MOVE_F"))
        {
            transform.Translate(moveDir * Time.deltaTime * 10 * maxInput);
        }
        



    }
    virtual protected void getDownHandling() {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("DamageDown"))
        {
            deltaTime += Time.deltaTime;

            if (deltaTime < 1)
                return;
            this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, -1.5f, this.gameObject.transform.position.z);
            anim.Play("Headspring");
            //Attributes.getDown = false;
            deltaTime = 0;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Headspring"))
        {
            deltaTime += Time.deltaTime;

            if (deltaTime < 0.8)
                return;
            
            this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, -1.5f, this.gameObject.transform.position.z);
            anim.Play("WAIT00");
            deltaTime = 0;
            Attributes.getDown = false;

        }
    }
    
    virtual protected void atkSystem() {
        
        if (atk_state == h_atk_state || atk_state == d_atk_state)
            comboAtk();

        getAtkKey();
    }
    virtual protected void getAtkKey() {

    }

    virtual protected void comboAtk()
    {
        anim.SetBool("isDEFFENSE", def_b);

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Jab_R") || anim.GetCurrentAnimatorStateInfo(0).IsName("Jab_L") || anim.GetCurrentAnimatorStateInfo(0).IsName("Punch_h_R") || anim.GetCurrentAnimatorStateInfo(0).IsName("DEFENSE") || anim.GetCurrentAnimatorStateInfo(0).IsName("Spinkick"))
        {
            if (!def_b && anim.GetCurrentAnimatorStateInfo(0).IsName("DEFENSE"))
            {
                canWASD = true;
                atk_state = c_atk_state;
                combo = 0;
                setCTandHA("nothing", true);
            }
        }

        else {
            canWASD = true;
            atk_state = c_atk_state;
            combo = 0;
            setCTandHA("nothing", true);
        }
    }

    virtual protected void defense()
    {
    }

    public void faceToOpponent(float y_offset) {

        Vector3 relativePos = opponent.transform.position - this.gameObject.transform.position;
        relativePos = new Vector3(relativePos.x, 0, relativePos.z);
        relativePos = Quaternion.AngleAxis(y_offset, Vector3.up) * relativePos;
        Quaternion rotation = Quaternion.LookRotation(relativePos);
        transform.rotation = rotation;
    }
    public void faceTo(Transform trans)
    {
        Vector3 relativePos = trans.position - this.gameObject.transform.position;
        relativePos = new Vector3(relativePos.x, 0, relativePos.z);
        Quaternion rotation = Quaternion.LookRotation(relativePos);
        transform.rotation = rotation;
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (Attributes.canBeAtk)
        {
            itemHit(other);
            if (other.gameObject.tag == "atk_" + opponent_tag)
            {
                other.gameObject.GetComponent<Collider>().isTrigger = false;
                GameObject eff;
                eff = Instantiate(opponent.GetComponent<basicController>().l_atk_particles, other.gameObject.transform.position, Quaternion.identity) as GameObject;
                eff.GetComponent<AudioSource>().enabled = true;
                Destroy(eff, 1);

                if (anim.GetCurrentAnimatorStateInfo(0).IsName("DEFENSE")) {
                    //transform.position = new Vector3(transform.position.x, -1.5f, transform.position.z);
                    faceToOpponent(0);
                    playerHealth.decreaseHealth(0.5f);
                }
                else if (!other.GetComponent<disableRigibodyVelocity>().isHeavyAtk) {
                    //Attributes.l_atk_accumulator++;
                    faceToOpponent(0);
                    setCanActTime(0, 0.3f);
                    anim.Play("DAMAGED",-1,0f);
                    playerHealth.decreaseHealth(3);
                    clearIHIV();

                }
                else 
                {
                    Attributes.getDown = true;
                    faceToOpponent(0);
                    {
                        anim.Play("DamageDown");
                        playerHealth.decreaseHealth(10);
                        deltaTime = 0;
                    }
                    clearIHIV();
                }
            }
        }
        if (other.gameObject.tag == "OT_BOX") { //OT!!!!!
            //if (tag == "p2")
            //    return;
            OTtime = true;
            Attributes.getDown = false;

            clearIHIV();

            transform.position = new Vector3(transform.position.x,-1.5f, transform.position.z);
            anim.Play("Land");

            opponent.transform.position = new Vector3(opponent.transform.position.x, -1.5f, opponent.transform.position.z);
            opponent.GetComponent<Animator>().Play("Land");
            opponent.GetComponent<basicController>().opponent_OTtime = true;
            opponent.GetComponent<basicController>().clearIHIV();
            //opponent.

            //Destroy(other.gameObject);
            other.gameObject.SetActive(false);
            ItemGenerator.setStop();
        }

    }
    void itemHit(Collider coll) {
        if (coll.tag == "tornado" || coll.tag == "bombEff") {
            Attributes.getDown = true;
            faceTo(coll.transform);
            {
                anim.Play("DamageDown");
                playerHealth.decreaseHealth(10);
                deltaTime = 0;
            }
            clearIHIV();
        }
    }

    virtual protected void cameraPlaneGetDaker() {
        float alpha = cameraPlaneMaterial.color.a + 0.01f;
        if (alpha > 1)
        {
            alpha = 1;
            //cameraPlaneIsDark = true;
            OTstate++;
        }
        cameraPlaneMaterial.color = new Color(0, 0, 0, alpha);

    }
    virtual protected void cameraPlaneGetLighter()
    {
        cameraPlaneMaterial.color = new Color(0, 0, 0, 0);

        float alpha = cameraPlaneMaterial.color.a - 0.01f;
        if (alpha < 0) {
            alpha = 0;
            //cameraPlaneIsDark = true;
            OTstate++;
        }

        cameraPlaneMaterial.color = new Color(0, 0, 0, alpha);
        
    }

    virtual protected void OT_animation(){
        if (OTstate == 2)
        {
            transform.position = new Vector3(-0.25f, -1.5f, -1f);
            
            anim.Play("WAIT00");
            
            opponent.transform.position = new Vector3(0.35f, -1.5f, -1f);
            opponent.GetComponent<Animator>().Play("WAIT00");

            opponent.GetComponent<basicController>().faceToOpponent(0);
            faceToOpponent(0);
            OTstate++;
        }
        if (OTstate == 3) {

            anim.Play("Punch_b_R_OT");


            //if (canAct()) {
            //    GameObject OTele = Instantiate(opponent.GetComponent<basicController>().l_atk_particles, other.gameObject.transform.position, Quaternion.identity) as GameObject; ;

            //}
        }
    }

    virtual protected void clearIHIV()
    {
        inputH = 0;
        inputV = 0;
        anim.SetFloat("inputH", inputH);
        anim.SetFloat("inputV", inputV);
    }

    public static GameObject[] FindGameObjectsWithTag(string tag)
    {
        ArrayList resultList = new ArrayList();
        foreach (GameObject result in GameObject.FindGameObjectsWithTag(tag))
        {
            if (!result.name.Contains("(Clone)"))
            {
                resultList.Add(result);
            }
        }

        GameObject[] resultArray = new GameObject[resultList.Count];
        resultList.CopyTo(resultArray);
        return resultArray;
    }
    public static GameObject FindWithTag(string tag)
    {
        try
        {
            return FindGameObjectsWithTag(tag)[0];
        }
        catch
        {
            return null;
        }
    }
   
}
