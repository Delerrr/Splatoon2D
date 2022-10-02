using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class iconcontroller : MonoBehaviour
{
    public Texture texture;
    void Start()
    {
        Cursor.visible = false;
    }
    void OnGUI()
    {
        Vector3 vector3 = Input.mousePosition;
        GUI.DrawTexture(new Rect(vector3.x - texture.width/2, (Screen.height - vector3.y) - texture.height/2, texture.width, texture.height), texture);
    }
}