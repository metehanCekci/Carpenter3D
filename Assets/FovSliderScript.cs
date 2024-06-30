using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FovSliderScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    public Text textComponent;
    public Slider slider;
    Camera cam;

    void Start()
    {
        //textComponent = GetComponent<Text>();
        cam=Camera.main;
    }

    public void SetSliderValue()
    {
        textComponent.text = slider.value.ToString();
        cam.fieldOfView=slider.value;

    }
}
