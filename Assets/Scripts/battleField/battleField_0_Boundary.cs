using UnityEngine;
using System.Collections;

public class battleField_0_Boundary : MonoBehaviour {
    private GameObject p1;
    private GameObject p2;
    private GameObject battleField00;
    private GameObject cube_pos_min;
    private GameObject cube_pos_max;
    private Vector3 cube_pos_min_v;
    private Vector3 cube_pos_max_v;

    // Use this for initialization
    void Start () {
        p1 = FindWithTag("p1");
        p2 = FindWithTag("p2");
        battleField00 = GameObject.Find("battleField_0_0");

        cube_pos_min = GameObject.Find("battleField_0_cube_pos_min");
        cube_pos_min_v = cube_pos_min.transform.position;

        cube_pos_max = GameObject.Find("battleField_0_cube_pos_max");
        cube_pos_max_v = cube_pos_max.transform.position;


        cube_pos_min.gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;
        cube_pos_max.gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;

    }

    // Update is called once per frame
    void Update () {
        boundary_checking();

    }

    void boundary_checking() {
        if (p1.transform.position.x < cube_pos_min_v.x)
            p1.transform.position = new Vector3(cube_pos_min_v.x, p1.transform.position.y, p1.transform.position.z);

        if (p1.transform.position.x > cube_pos_max_v.x)
            p1.transform.position = new Vector3(cube_pos_max_v.x, p1.transform.position.y, p1.transform.position.z);

        if (p1.transform.position.z < cube_pos_min_v.z)
            p1.transform.position = new Vector3(p1.transform.position.x, p1.transform.position.y, cube_pos_min_v.z);

        if (p1.transform.position.z > cube_pos_max_v.z)
            p1.transform.position = new Vector3(p1.transform.position.x, p1.transform.position.y, cube_pos_max_v.z);

        if (p2.transform.position.x < cube_pos_min_v.x)
            p2.transform.position = new Vector3(cube_pos_min_v.x, p2.transform.position.y, p2.transform.position.z);

        if (p2.transform.position.x > cube_pos_max_v.x)
            p2.transform.position = new Vector3(cube_pos_max_v.x, p2.transform.position.y, p2.transform.position.z);

        if (p2.transform.position.z < cube_pos_min_v.z)
            p2.transform.position = new Vector3(p2.transform.position.x, p2.transform.position.y, cube_pos_min_v.z);

        if (p2.transform.position.z > cube_pos_max_v.z)
            p2.transform.position = new Vector3(p2.transform.position.x, p2.transform.position.y, cube_pos_max_v.z);
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
