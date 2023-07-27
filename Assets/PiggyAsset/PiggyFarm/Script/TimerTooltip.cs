using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimerTooltip : MonoBehaviour
{
    //static timer tooltip
    private static TimerTooltip instance;

    //timer to display
    private Timer timer;

    //UI fields
    [SerializeField] private Camera uiCamera;
    [SerializeField] private TextMeshProUGUI callerNameText;
    [SerializeField] private TextMeshProUGUI timeLeftText;
    [SerializeField] private Slider progressSlider;

    //if the timer tooltip is visible and text (2h 34min) has to be displayed 
    private bool countdown;

    private void Awake()
    {
        //initialize static instance
        instance = this;
        //disable timer tooltip (it has a fullscreen panel as a parent)
        transform.parent.gameObject.SetActive(false);
    }

    /*
     * Show timer tooltip
     * @caller object which calls this method
     */
    private void ShowTimer(GameObject caller)
    {
        //get the timer from the object
        timer = caller.GetComponent<Timer>();

        //if there is no timer attached
        if (timer == null)
        {
            return;
        }

        //initialize the name of the process 
        callerNameText.text = timer.Name;
        //enable skip button (it might be disabled after skipping)

        //calculate object position
        Vector3 position = caller.transform.position - uiCamera.transform.position;
        //TransformPoint - convert from local space to world
        //WorldToScreenPoint - convert from world to screen
        position = uiCamera.WorldToScreenPoint(uiCamera.transform.TransformPoint(position));
        //position the tooltip under the object
        transform.position = position;
        

        //countdown text has to be visible
        countdown = true;
        //ensure the text and slider initialized
        FixedUpdate();
        
        //enable tooltip panel
        transform.parent.gameObject.SetActive(true);
        StartCoroutine(ActivateObjectCoroutine());
    }

    private void FixedUpdate()
    {
        //if there has to be a countdown display
        if (countdown)
        {
            //set the slider value
            progressSlider.value = (float) (1.0 - timer.secondsLeft / timer.timeToFinish.TotalSeconds);
            //display countdown text
            timeLeftText.text = timer.DisplayTime();
        }
    }

    /*
     * Hide the timer
     */
    public void HideTimer()
    {
        //disable timer tooltip panel
        transform.parent.gameObject.SetActive(false);
        //remove timer
        timer = null;
        //do not display the countdown text
        countdown = false;
    }

    /*
     * Static method to show timer tooltip
     * @caller object which calls this method
     */
    public static void ShowTimer_Static(GameObject caller)
    {
        //call ShowTimer method on the static instance
        instance.ShowTimer(caller);
    }

    public static void HideTimer_Static()
    {
        //call HideTimer method on the static instance
        instance.HideTimer();
    }

    public IEnumerator ActivateObjectCoroutine()
    {
        yield return new WaitForSeconds(2f);

        HideTimer_Static();
        StopCoroutine(ActivateObjectCoroutine());
    }
}
