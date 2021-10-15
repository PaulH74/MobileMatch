using System.Collections;
using Lean.Touch;
using UnityEngine;

public class PageInteractions : MonoBehaviour
{
    // Public Attributes
    public GameObject[] upperObjects;
    public GameObject[] lowerObjects;

    // Private Attributes
    private int _IndexLwrObjs;
    private int _IndexUprObjs;
    private AudioSource _PageTurnAudio;
    private bool _CanTouch;


    // Start is called before the first frame update
    private void Start()
    {
        _PageTurnAudio = GetComponent<AudioSource>();
        _IndexLwrObjs = 0;
        _IndexUprObjs = 0;
        _CanTouch = true;
        ShowCurrentPage();
    }

    #region Internal Methods
    /// <summary>
    /// Changes object according to swipe direction and position on screen.
    /// </summary>
    private void ChangePage(SCREENPOSITION position, SWIPEDIRECTION direction)
    {
        switch (direction)
        {
            case SWIPEDIRECTION.RIGHT:
                DecrementCount(position);
                break;
            case SWIPEDIRECTION.LEFT:
                IncrementCount(position);
                break;
        }

        ShowCurrentPage();
        _PageTurnAudio.Play();
    }

    private void ShowCurrentPage()
    {
        HideObjects(ref upperObjects);
        HideObjects(ref lowerObjects);
        upperObjects[_IndexUprObjs].SetActive(true);
        lowerObjects[_IndexLwrObjs].SetActive(true);
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
                    // End Of Book, reset to front
                    _IndexUprObjs = 0;
                    _IndexLwrObjs = 0;
                }
                else if (_IndexUprObjs == upperObjects.Length - 1)
                {
                    // Reached back cover, match lower
                    _IndexLwrObjs = lowerObjects.Length - 1;
                }
                else if (_IndexUprObjs == 1)
                {
                    // First Page (set lower index to match)
                    _IndexLwrObjs = 1;
                }
                break;
            case SCREENPOSITION.LOWER:
                _IndexLwrObjs++;

                if (_IndexLwrObjs == lowerObjects.Length)
                {
                    // End Of Book, reset to front
                    _IndexUprObjs = 0;
                    _IndexLwrObjs = 0;
                }
                else if (_IndexLwrObjs == lowerObjects.Length - 1)
                {
                    // Reached back cover, match lower
                    _IndexUprObjs = upperObjects.Length - 1;
                }
                else if (_IndexLwrObjs == 1)
                {
                    // First Page (set upper index to match)
                    _IndexUprObjs = 1;
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
                if (_IndexUprObjs <= 0)
                {
                    // Reached front cover
                    _IndexUprObjs = 0;
                    _IndexLwrObjs = 0;
                }
                else if (_IndexUprObjs == upperObjects.Length - 2)
                {
                    // Going to last page from back cover, update lower index to match
                    _IndexLwrObjs = lowerObjects.Length - 2;
                }
                break;
            case SCREENPOSITION.LOWER:
                _IndexLwrObjs--;
                if (_IndexLwrObjs <= 0)
                {
                    // Reached front cover
                    _IndexUprObjs = 0;
                    _IndexLwrObjs = 0;
                }
                else if (_IndexLwrObjs == lowerObjects.Length - 2)
                {
                    // Going to last page from back cover, update lower index to match
                    _IndexUprObjs = upperObjects.Length - 2;
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
        if (_CanTouch)
        {
            _CanTouch = false;

            for (int i = 0; i < LeanTouch.Fingers.Count; i++)
            {
                if (LeanTouch.Fingers[i].ScreenPosition.y >= GetScreenCentreline())
                {
                    // Swipe Upper
                    ChangePage(SCREENPOSITION.UPPER, SWIPEDIRECTION.RIGHT);
                    break;
                }
                else
                {
                    // Swipe Lower
                    ChangePage(SCREENPOSITION.LOWER, SWIPEDIRECTION.RIGHT);
                    break;
                }
            }

            StartCoroutine(DelayTouch());
        }
    }

    /// <summary>
    /// Called when screen is "swiped to the left", checks swipe screen position (top / bottom) and decrements display object in array.
    /// </summary>
    public void SwipeLeft()
    {
        if (_CanTouch)
        {
            _CanTouch = false;

            for (int i = 0; i < LeanTouch.Fingers.Count; i++)
            {
                if (LeanTouch.Fingers[i].ScreenPosition.y >= GetScreenCentreline())
                {
                    // Swipe Upper
                    ChangePage(SCREENPOSITION.UPPER, SWIPEDIRECTION.LEFT);
                    break;
                }
                else
                {
                    // Swipe Lower
                    ChangePage(SCREENPOSITION.LOWER, SWIPEDIRECTION.LEFT);
                    break;
                }
            }

            StartCoroutine(DelayTouch());
        }
    }

    public void OnTouch()
    {
        if (_CanTouch)
        {
            _CanTouch = false;

            for (int i = 0; i < LeanTouch.Fingers.Count; i++)
            {
                if (LeanTouch.Fingers[i].ScreenPosition.y >= GetScreenCentreline())
                {
                    // Touch Upper
                    ChangePage(SCREENPOSITION.UPPER, SWIPEDIRECTION.LEFT);
                    Debug.Log("TOUCHED UPPER");
                    break;
                }
                else
                {
                    // Swipe Lower
                    ChangePage(SCREENPOSITION.LOWER, SWIPEDIRECTION.LEFT);
                    Debug.Log("TOUCHED LOWER");
                    break;
                }
            }

            StartCoroutine(DelayTouch());
        }
    }
    #endregion

    /// <summary>
    /// This co-routine adds a time delay between interactions (to avoid skipping on WebGL app / Simmer.IO site).
    /// </summary>
    private IEnumerator DelayTouch()
    {
        const float DELAY = 1f;

        yield return new WaitForSeconds(DELAY);

        _CanTouch = true;
    }

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