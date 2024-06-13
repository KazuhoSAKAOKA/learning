using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JointController : MonoBehaviour
{
    const int JOINTS = 3;
    //robot
    private GameObject[] joint = new GameObject[JOINTS];
    private GameObject[] arm = new GameObject[JOINTS];
    private float[] armL = new float[JOINTS];
    private Vector3[] angle = new Vector3[JOINTS];


    //UI
    private GameObject[] slider = new GameObject[JOINTS];
    private float[] sliderValue = new float[JOINTS];
    private float[] prevSliderValue = new float[JOINTS];
    private GameObject[] angleText = new GameObject[JOINTS];
    private GameObject[] posText = new GameObject[JOINTS];


    // Start is called before the first frame update
    void Start()
    {
        for(var i = 0; i< joint.Length; i++)
        {
            joint[i] = GameObject.Find($"Joint_{i}");
            arm[i] = GameObject.Find($"Arm_{i}");
            if (i == 0) { armL[i] = arm[i].transform.localScale.y; }
            else { armL[i] = arm[i].transform.localScale.x; }
        }

        for (var i = 0; i < joint.Length; i++)
        {
            slider[i] = GameObject.Find($"Slider_{i}");
            angleText[i] = GameObject.Find($"Angle_{i}");
            sliderValue[i] = slider[i].GetComponent<Slider>().value;
            posText[i] = GameObject.Find($"Ref_{i}");
        }
    }

    float pow2(float x)
    {
        return x * x;
    }

    // Update is called once per frame
    void Update()
    {
        for (var i = 0; i < joint.Length; i++)
        {
            sliderValue[i] = slider[i].GetComponent<Slider>().value;
        }
        var x = sliderValue[0];
        var y = sliderValue[1];
        var z = sliderValue[2];
        angle[0].y = -Mathf.Atan2(z, x);
        var a = x / Mathf.Cos(angle[0].y);
        var b = y - armL[0];

        if (Mathf.Pow(pow2(a) + pow2(b), 0.5f) > armL[1] + armL[2])
        {
            for(var i =0; i< joint.Length; i++)
            {
                sliderValue[i] = prevSliderValue[i];
                slider[i].GetComponent<Slider>().value = sliderValue[i];
                //posText[i].GetComponent<TMP_Text>().text = sliderValue[i].ToString("f2");
            }
        }
        else
        {
            var alpha = Mathf.Acos((pow2(armL[1]) + pow2(armL[2]) - pow2(a) - pow2(b)) / (2f * armL[1] * armL[2]));
            angle[2].z = -Mathf.PI + alpha;
            var beta = Mathf.Acos((pow2(armL[1]) + pow2(a) + pow2(b) - pow2(armL[2])) / (2f * armL[1] * Mathf.Pow((pow2(a) + pow2(b)), 0.5f)));
            angle[1].z = Mathf.Atan2(b, a) + beta;


            for (var i = 0; i < joint.Length; i++)
            {
                joint[i].transform.localEulerAngles = angle[i] * Mathf.Rad2Deg;
                posText[i].GetComponent<TMP_Text>().text = sliderValue[i].ToString("f2");
                prevSliderValue[i] = sliderValue[i];
            }
            angleText[0].GetComponent<TMP_Text>().text = (angle[0].y * Mathf.Rad2Deg).ToString("f2");
            angleText[1].GetComponent<TMP_Text>().text = (angle[1].z * Mathf.Rad2Deg).ToString("f2");
            angleText[2].GetComponent<TMP_Text>().text = (angle[2].z * Mathf.Rad2Deg).ToString("f2");
        }   
    }
}
