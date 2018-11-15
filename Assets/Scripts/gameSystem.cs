using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
public class gameSystem : MonoBehaviour {


    public GameObject gameBeginText;
    public int roundNo;
    public int winner; // 1 for p1      2 for p2       0 for not yet;

    public int blockFrames;
    public int frameCounter;

    public GameObject menuButton;

    public GameObject p1;
    public GameObject p2;

    public Image p1win1;
    public Image p1win2;

    public Image p2win1;
    public Image p2win2;

    public Sprite circle;
    public Sprite circlewin;

    public int win1;
    public int win2;

    public GameObject cameraPlane;


    private bool canShowBegin;
    private bool canShowWin;
    
    Transform trans1;
    Transform trans2;

    // Use this for initialization
    void Start () {

        p1win1.sprite = circle;
        p1win2.sprite = circle;
        p2win1.sprite = circle;
        p2win2.sprite = circle;

        trans1 = new GameObject().transform;
        trans2 = new GameObject().transform;
        trans1.position = new Vector3(-6.92f,-1.5f,-2.73f);
        trans2.position = new Vector3(6.49f, -1.5f, -2.73f);
        trans1.Rotate(0, 90, 0);
        trans2.Rotate(0, 270, 0);


        win1 = 0;
        win2 = 0;
        canShowBegin = true;
        blockFrames = 0;
        frameCounter = 0;
        winner = 0;
        roundNo = 1;
        gameBeginText = GameObject.Find("gameBeginText");
        //gameBeginText.GetComponent<Text>().text = "Round " + roundNo;
        p1 = FindWithTag("p1");
        p1.transform.position = trans1.position;
        p1.transform.rotation= trans1.rotation;
        p2 = FindWithTag("p2");
        p2.transform.position = trans2.position;
        p2.transform.rotation = trans2.rotation;

        menuButton.SetActive(false);

        setBlockGame(true);

    }

    void setBlockGame(bool b) {
        p1.GetComponent<basicController>().blockGame = b;
        p2.GetComponent<basicController>().blockGame = b;
    }
    // Update is called once per frame
    void Update () {
        if (block())
            return;
        showBegin();
        showWin();
    }


    void showBegin() {
        if (canShowBegin) {
            if (gameBeginText.GetComponent<Text>().text == "")
            {
                gameBeginText.GetComponent<Text>().text = "Round " + roundNo;
                setBlockFrame(50);
            }
            else if (gameBeginText.GetComponent<Text>().text.Length>5 && gameBeginText.GetComponent<Text>().text.Substring(0,5) == "Round")
            {
                gameBeginText.GetComponent<Text>().text = "FIGHT";
                setBlockFrame(50);
            }
            else if(gameBeginText.GetComponent<Text>().text == "FIGHT") {
                gameBeginText.GetComponent<Text>().text = "";
                GetComponent<itemGenerator>().setStart();
                canShowBegin = false;
                setBlockGame(false);
            }
        }
    }

    void showWin() {
        if (canShowWin) {
            GetComponent<itemGenerator>().setStop();
            if (gameBeginText.GetComponent<Text>().text == "")
            {
                gameBeginText.GetComponent<Text>().text = "p" + winner + " win!";
                setBlockFrame(150);
            }
            else if (gameBeginText.GetComponent<Text>().text == "p" + winner + " win!") {
                if (win1 + win2 == 3 || win1==2 || win2==2) {
                    if (win1 > win2)
                        gameBeginText.GetComponent<Text>().text = "p1 win this game!";
                    else
                        gameBeginText.GetComponent<Text>().text = "p2 win this game!";
                    
                    menuButton.SetActive(true);
                }

                Color c = new Vector4(0,0,0,0.02f);
                cameraPlane.GetComponent<Renderer>().material.color += c;
                if (cameraPlane.GetComponent<Renderer>().material.color.a >= 1) {
                    gameBeginText.GetComponent<Text>().text = "";
                    canShowWin = false;
                    resetPlayer();
                    cameraPlane.GetComponent<Renderer>().material.color = new Vector4(0, 0, 0, 0);
                    roundNo++;
                    canShowBegin = true;
                }
            }
        }
    }

    public void loadScene() {
        SceneManager.LoadScene("menu_scene"); 
    }

    void setBlockFrame(int f) {
        frameCounter = 0;
        blockFrames = f;
    }
    bool block() {
        if (frameCounter < blockFrames)
        {
            frameCounter++;
            return true;
        }
        return false;
    }
    public void setWinner(int win) {
        if (win == 1)
        {
            win1++;
            if (win1 == 1)
                p1win1.sprite = circlewin;
            else if (win1 == 2)
                p1win2.sprite = circlewin;
        }
        else {
            win2++;
            if (win2 == 1)
                p2win1.sprite = circlewin;
            else if (win2 == 2)
                p2win2.sprite = circlewin;
        }
        winner = win;               //set the winner
        setBlockGame(true);         //cannot move
        setBlockFrame(30);          // block
        canShowWin = true;          // show win text
    }

    void resetPlayer() {
        p1.GetComponent<basicController>().initialization();
        p1.GetComponent<attributes>().HP = p1.GetComponent<PlayerHealth>().max_Health;
        p1.GetComponent<attributes>().canBeAtk = true;
        p1.GetComponent<attributes>().getDown = false;
        p1.GetComponent<Animator>().Play("WAIT00");
        p1.transform.position = new Vector3(p1.transform.position.x, -1.5f, p1.transform.position.z);
        p1.GetComponent<PlayerHealth>().SetHealthBar(1);
        p1.transform.position = trans1.position;
        p1.transform.rotation = trans1.rotation;

        p2.GetComponent<basicController>().initialization();
        p2.GetComponent<Animator>().Play("WAIT00");
        p2.GetComponent<attributes>().canBeAtk = true;
        p2.GetComponent<attributes>().getDown = false;
        p2.transform.position = new Vector3(p2.transform.position.x, -1.5f, p2.transform.position.z);
        p2.GetComponent<attributes>().HP = p2.GetComponent<PlayerHealth>().max_Health;
        p2.GetComponent<PlayerHealth>().SetHealthBar(1);
        p2.transform.position = trans2.position;
        p2.transform.rotation = trans2.rotation;
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
