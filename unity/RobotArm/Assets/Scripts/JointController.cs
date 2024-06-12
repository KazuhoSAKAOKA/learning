using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JointController : MonoBehaviour
{
    const int JOINTS = 2;
    //robot
    private GameObject[] joint = new GameObject[JOINTS];
    private GameObject[] arm = new GameObject[JOINTS];
    private float[] armL = new float[JOINTS];
    private Vector3[] angle = new Vector3[JOINTS];


    //UI
    private GameObject[] slider = new GameObject[JOINTS];
    private float[] sliderValue = new float[JOINTS];
    private GameObject[] angleText = new GameObject[JOINTS];
    private GameObject[] posText = new GameObject[JOINTS];


    // Start is called before the first frame update
    void Start()
    {
        for(var i = 0; i< joint.Length; i++)
        {
            joint[i] = GameObject.Find($"Joint_{i}");
            arm[i] = GameObject.Find($"Arm_{i}");
            armL[i] = arm[i].transform.localScale.x;
        }

        for (var i = 0; i < joint.Length; i++)
        {
            slider[i] = GameObject.Find($"Slider_{i}");
            angleText[i] = GameObject.Find($"Angle_{i}");
            sliderValue[i] = slider[i].GetComponent<Slider>().value;
        }
        posText[0] = GameObject.Find("Pos_X");
        posText[1] = GameObject.Find("Pos_Y");
    }

    // Update is called once per frame
    void Update()
    {
        for(var i =0; i< joint.Length; i++)
        {
            sliderValue[i] = slider[i].GetComponent<Slider>().value;
            angleText[i].GetComponent<TMP_Text>().text = $"{sliderValue[i]}";
            angle[i].z = sliderValue[i];
            joint[i].transform.localEulerAngles = angle[i];
        }

        var px = armL[0] + Mathf.Cos(angle[0].z * Mathf.Deg2Rad) + armL[1] * Mathf.Cos((angle[0].z + angle[1].z) * Mathf.Deg2Rad);
        var py = armL[0] + Mathf.Sin(angle[0].z + Mathf.Deg2Rad) + armL[1] * Mathf.Sin((angle[0].z + angle[1].z) * Mathf.Deg2Rad);

        posText[0].GetComponent<TMP_Text>().text = $"{px}";
        posText[1].GetComponent<TMP_Text>().text = $"{py}";
    }
}
