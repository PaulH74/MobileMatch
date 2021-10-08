using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Lean.Touch;

public class SwipeObject : MonoBehaviour
{
    public GameObject[] upperObjects;
    public GameObject[] lowerObjects;

    private int _IndexLwrObjs;
    private int _IndexUprObjs;


    // Start is called before the first frame update
    private void Start()
    {
        HideObjects(ref upperObjects);
        HideObjects(ref lowerObjects);
        _IndexLwrObjs = 0;
        _IndexUprObjs = 0;
        upperObjects[0].SetActive(true);
        lowerObjects[0].SetActive(true);
    }

    private void ChangeObject(SCREENPOSITION position, SWIPEDIRECTION direction)
    {
        switch (direction)
        {
            case SWIPEDIRECTION.RIGHT:
                IncrementCount(position);
                break;
            case SWIPEDIRECTION.LEFT:
                DecrementCount(position);
                break;
        }

        switch (position)
        {
            case SCREENPOSITION.UPPER:
                HideObjects(ref upperObjects);
                upperObjects[_IndexUprObjs].SetActive(true);
                break;
            case SCREENPOSITION.LOWER:
                HideObjects(ref lowerObjects);
                lowerObjects[_IndexLwrObjs].SetActive(true);
                break;
        }
    }

    private void HideObjects(ref GameObject[] objects)
    {
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].SetActive(false);
        }
    }

    private void IncrementCount(SCREENPOSITION position)
    {
        switch (position)
        {
            case SCREENPOSITION.UPPER:
                _IndexUprObjs++;
                if (_IndexUprObjs == upperObjects.Length)
                {
                    _IndexUprObjs = 0;
                }
                break;
            case SCREENPOSITION.LOWER:
                _IndexLwrObjs++;
                if (_IndexLwrObjs == lowerObjects.Length)
                {
                    _IndexLwrObjs = 0;
                }
                break;
        }
    }

    private void DecrementCount(SCREENPOSITION position)
    {
        switch (position)
        {
            case SCREENPOSITION.UPPER:
                _IndexUprObjs--;
                if (_IndexUprObjs < 0)
                {
                    _IndexUprObjs = upperObjects.Length - 1;
                }
                break;
            case SCREENPOSITION.LOWER:
                _IndexLwrObjs--;
                if (_IndexLwrObjs < 0)
                {
                    _IndexLwrObjs = lowerObjects.Length - 1;
                }
                break;
        }
    }


    public void SwipeRight()
    {
        for (int i = 0; i < LeanTouch.Fingers.Count; i++)
        {
            if (LeanTouch.Fingers[i].ScreenPosition.y >= CheckDeviceOrientation())
            {
                // Swipe Upper
                ChangeObject(SCREENPOSITION.UPPER, SWIPEDIRECTION.RIGHT);
                break;
            }
            else
            {
                // Swipe Lower
                ChangeObject(SCREENPOSITION.LOWER, SWIPEDIRECTION.RIGHT);
                break;
            }
            
            //Debug.Log(string.Format("Current Finger Position {0}: {1},{2}", i, LeanTouch.Fingers[i].ScreenPosition.x, LeanTouch.Fingers[i].ScreenPosition.y));
        }
    }

    public void SwipeLeft()
    {
        for (int i = 0; i < LeanTouch.Fingers.Count; i++)
        {
            if (LeanTouch.Fingers[i].ScreenPosition.y >= CheckDeviceOrientation())
            {
                // Swipe Upper
                ChangeObject(SCREENPOSITION.UPPER, SWIPEDIRECTION.LEFT);
                break;
            }
            else
            {
                // Swipe Lower
                ChangeObject(SCREENPOSITION.LOWER, SWIPEDIRECTION.LEFT);
                break;
            }

            //Debug.Log(string.Format("Current Finger Position {0}: {1},{2}", i, LeanTouch.Fingers[i].ScreenPosition.x, LeanTouch.Fingers[i].ScreenPosition.y));
        }
    }

    private int CheckDeviceOrientation()
    {
        //int centre = 0;

        //if (Input.deviceOrientation == DeviceOrientation.Portrait || Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown)
        //{
        //    centre = Mathf.RoundToInt(Screen.height / 2f);
        //}
        //else //if (Input.deviceOrientation == DeviceOrientation.LandscapeLeft || Input.deviceOrientation == DeviceOrientation.LandscapeRight)
        //{
        //    centre = Mathf.RoundToInt(Screen.height / 2f);
        //}

        //return centre;

        return Mathf.RoundToInt(Screen.height / 2f);
    }

    private enum SCREENPOSITION
    {
        UPPER,
        LOWER
    }

    private enum SWIPEDIRECTION
    { 
        LEFT,
        RIGHT
    }
}
