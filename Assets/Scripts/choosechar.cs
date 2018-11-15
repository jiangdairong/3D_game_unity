using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
public class choosechar : MonoBehaviour {

    float time_t;
    void Start () {
        time_t = 0;
    }
	
	// Update is called once per frame
	void Update () {
        if (time_t < 5.0) {
            time_t += Time.deltaTime;
            return;
        }

        SceneManager.LoadScene("fighting_mode");


    }

       
   
}
