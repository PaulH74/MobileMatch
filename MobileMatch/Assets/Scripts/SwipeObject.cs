using UnityEngine;
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
        InitialiseScreen();
    }

    #region Internal Methods
    /// <summary>
    /// Initialises the display, de-activating all objects in the upper and lower arrays, reseting indexes, and activating the first object in
    /// both arrays.
    /// </summary>
    private void InitialiseScreen()
    {
        HideObjects(ref upperObjects);
        HideObjects(ref lowerObjects);
        _IndexLwrObjs = 0;
        _IndexUprObjs = 0;
        upperObjects[0].SetActive(true);
        lowerObjects[0].SetActive(true);
    }

    /// <summary>
    /// Changes object according to swipe direction and position on screen.
    /// </summary>
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

    /// <summary>
    /// De-activates all game objects in the reference array.
    /// </summary>
    private void HideObjects(ref GameObject[] objects)
    {
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].SetActive(false);
        }
    }

    /// <summary>
    /// Increments the upper or lower index according to the position of the swipe (top / bottom) on screen.
    /// </summary>
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

    /// <summary>
    /// Decrements the upper or lower index according to the position of the swipe (top / bottom) on screen.
    /// </summary>
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

    /// <summary>
    /// Gets an update of the screen centreline, used to determine top / bottom swipeable regions.
    /// </summary>
    private int GetScreenCentreline()
    {
        return Mathf.RoundToInt(Screen.height / 2f);
    }
    #endregion

    #region Swipe Control Methods
    /// <summary>
    /// Called when screen is "swiped to the right", checks swipe screen position (top / bottom) and increments display object in array.
    /// </summary>
    public void SwipeRight()
    {
        for (int i = 0; i < LeanTouch.Fingers.Count; i++)
        {
            if (LeanTouch.Fingers[i].ScreenPosition.y >= GetScreenCentreline())
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
        }
    }

    /// <summary>
    /// Called when screen is "swiped to the left", checks swipe screen position (top / bottom) and decrements display object in array.
    /// </summary>
    public void SwipeLeft()
    {
        for (int i = 0; i < LeanTouch.Fingers.Count; i++)
        {
            if (LeanTouch.Fingers[i].ScreenPosition.y >= GetScreenCentreline())
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
        }
    }
    #endregion

    #region Enums
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
    #endregion
}
