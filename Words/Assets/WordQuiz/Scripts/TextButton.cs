using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TextButton : MonoBehaviour, IPointerClickHandler
{
    // add callbacks in the inspector like for buttons
    public UnityEvent onClick;
    //[SerializeField]
    //private static Text soundsBtn;
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        //Output to console the clicked GameObject's name and the following message. You can replace this with your own actions for when clicking the GameObject.
        Debug.Log(name + " Game Object Clicked!", this);

        // invoke your event
        onClick.Invoke();
    }
        public void e()
        {
        QuizManager.quit();   
    }
    public void r()
    {
       // resume();
    }
    public static void sounds()
    {
        if (AudioListener.pause == true)
        {
            //soundsBtn.text = "(sounds-on)";
            AudioListener.pause = false;
        }else
        {
           // soundsBtn.text = "(sounds-off)";
            AudioListener.pause = true;
        }
    }
}