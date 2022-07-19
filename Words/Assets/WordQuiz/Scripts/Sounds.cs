using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Sounds : MonoBehaviour
{
    public UnityEvent onClick;
    public  Text soundsBtn;
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        //Output to console the clicked GameObject's name and the following message. You can replace this with your own actions for when clicking the GameObject.
        // Debug.Log(name + " Game Object Clicked!", this);
      
        
        // invoke your event
        onClick.Invoke();
    }

    public void s2()
    {
        Debug.Log("1122220");
        if (soundsBtn.text == "(sounds-off)")
        {
            soundsBtn.text = "(sounds-on)";
            Debug.Log("22220");
        }
        else
        {
            soundsBtn.text = "(sounds-off)";

        }
    }
}
