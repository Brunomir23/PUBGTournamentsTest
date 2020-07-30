using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScalerUI : MonoBehaviour
{
    public float x,y;

    private CanvasScaler canScaler;
    public Text resolution;
    // Start is called before the first frame update
    void Start()
    {
        canScaler = GetComponent<CanvasScaler>();
        setXyValues();

        canScaler.referenceResolution = new Vector2(x, y);
    }

    void setXyValues()
    {
        x = (float)Screen.currentResolution.width;
        y = (float)Screen.currentResolution.height;
        resolution.text = "Resolución: " + x + "x" + y;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
