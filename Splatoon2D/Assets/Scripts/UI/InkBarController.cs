using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InkBarController : MonoBehaviour
{
    //ÑªÌõ
    public static InkBarController InkBar { get; private set; } 

    public Image Mask;
    float OriginLength;
    // Start is called before the first frame update
    protected void Start()
    {
        OriginLength = Mask.rectTransform.rect.width;
    }

    private void Awake() {
        InkBar = this;
    }

    public void setValue(float Value) {
        Mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, OriginLength * Value);
    }
}
