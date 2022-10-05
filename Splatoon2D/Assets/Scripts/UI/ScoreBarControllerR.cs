using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBarControllerR : MonoBehaviour
{
    //·ÖÊýÌõ
    public static ScoreBarControllerR ScoreBarR { get; private set; } 

    public Image Mask;
    float OriginLength;
    // Start is called before the first frame update
    protected void Start()
    {
        OriginLength = Mask.rectTransform.rect.width;
    }

    private void Awake() {
        ScoreBarR = this;
    }

    public void setValue(float Value) {
        Mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, OriginLength * Value);
    }
}
