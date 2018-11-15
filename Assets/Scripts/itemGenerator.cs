using UnityEngine;
using System.Collections;

public class itemGenerator : MonoBehaviour {
    public GameObject tornadoCenter;
    public GameObject diamond;
    public GameObject bomb;
    public GameObject bombEff;

    public int itemState;
    public int lastItemState;

    public bool canGen;
    public Transform boundaryMin;
    public Transform boundaryMax;

    public int curTime;


    float timer;

    // Use this for initialization
    void Start () {
        timer = 0;
        itemState = 0;
        lastItemState = 0;
        canGen = false;
        boundaryMin = GameObject.Find("battleField_0_cube_pos_min").transform;
        boundaryMax = GameObject.Find("battleField_0_cube_pos_max").transform;
        bombEff = GameObject.Find("Eff_Burst_2_oneShot");
        
        //generateBomb();
    }

    public void setStop() {
        canGen = false;
        tornadoCenter.GetComponent<tornadoMovement>().setStop();
        GameObject[] bombs = FindGameObjectsWithTag("bomb");
        for (int i = 0; i < bombs.Length; i++)
            Destroy(bombs[i]);
        diamond.SetActive(false);
    }
    public void setStart() {
        timer = 0;
        itemState = 0;
        lastItemState = 0;
        canGen = true;
    }
    void generateBomb() {
        for (int i = 0; i < 7; i++)
        {
            float minx = boundaryMin.position.x;
            float maxx = boundaryMax.position.x;
            float minz = boundaryMin.position.z;
            float maxz = boundaryMax.position.z;
            float randx = Random.Range(minx, maxx);
            float randy = -1;
            float randz = Random.Range(minz, maxz);
            GameObject b = Instantiate(bomb, new Vector3(randx, randy, randz), Quaternion.identity) as GameObject;
            b.GetComponent<bombMovement>().enabled = true;
        }
    }

    void generateDiamond() {
        float minx = boundaryMin.position.x;
        float maxx = boundaryMax.position.x;
        float minz = boundaryMin.position.z;
        float maxz = boundaryMax.position.z;
        float randx = Random.Range(minx, maxx);
        float randz = Random.Range(minz, maxz);
        diamond.transform.position = new Vector3(randx, diamond.transform.position.y, randz);
        diamond.SetActive(true);
    }

    void generateTornado() {
        tornadoCenter.GetComponent<tornadoMovement>().setMove();
    }
	// Update is called once per frame
	void Update () {
        if (canGen)
        {
            timer += Time.deltaTime;
            if (timer > 10f)
            {
                itemState = lastItemState + 1;
                if (itemState > 3)
                    itemState = 1;
                if (diamond.activeSelf == true)
                    diamond.SetActive(false);
                timer = 0;
            }

            checkItemState();
        }
    }
    void checkItemState() {
        if (itemState != 0) {
            if (itemState == 1)
                generateBomb();
            else if (itemState == 2)
                generateTornado();
            else if (itemState == 3)
                generateDiamond();
            lastItemState = itemState;
            itemState = 0;
        }
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
}
