using Lean.Touch;
using UnityEngine;

public class BookInteractions : MonoBehaviour
{
    // Public Attributes
    public GameObject[] upperObjects;
    public GameObject[] lowerObjects;
    public GameObject frontCover;
    public GameObject backCover;

    // Private Attributes
    private int _IndexLwrObjs;
    private int _IndexUprObjs;
    private AudioSource _PageTurnAudio;
    private bool _IsFrontCover;
    private bool _IsBackCover;


    // Start is called before the first frame update
    private void Start()
    {
        _PageTurnAudio = GetComponent<AudioSource>();
        ActivateFrontCover();
    }

    #region Internal Methods
    private void DeActivateCovers()
    {
        frontCover.SetActive(false);
        backCover.SetActive(false);
        _IsFrontCover = frontCover.activeSelf;
        _IsBackCover = backCover.activeSelf;
    }

    private void ActivateFrontCover()
    {
        frontCover.SetActive(true);
        backCover.SetActive(false);
        _IsFrontCover = frontCover.activeSelf;
        _IsBackCover = backCover.activeSelf;
        _IndexLwrObjs = -1;
        _IndexUprObjs = -1;
        HideObjects(ref upperObjects);
        HideObjects(ref lowerObjects);
    }

    private void ActivateBackCover()
    {
        backCover.SetActive(true);
        frontCover.SetActive(false);
        _IsFrontCover = frontCover.activeSelf;
        _IsBackCover = backCover.activeSelf;
        _IndexUprObjs = upperObjects.Length;
        _IndexLwrObjs = lowerObjects.Length;
        HideObjects(ref upperObjects);
        HideObjects(ref lowerObjects);
    }


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

        if (!_IsFrontCover && !_IsBackCover)
        {
            ShowCurrentPage();
        }
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
                if (_IndexUprObjs == 0)
                {
                    _IndexLwrObjs = 0;
                    DeActivateCovers();
                }
                else if (_IndexUprObjs >= upperObjects.Length)
                {
                    ActivateBackCover();
                }
                break;
            case SCREENPOSITION.LOWER:
                _IndexLwrObjs++;
                if (_IndexLwrObjs == 0)
                {
                    _IndexUprObjs = 0;
                    DeActivateCovers();
                }
                if (_IndexLwrObjs >= lowerObjects.Length)
                {
                    ActivateBackCover();
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
                    // Activate Cover Page
                    ActivateFrontCover();
                }
                else if (_IndexUprObjs == upperObjects.Length - 1)
                {
                    // Last page before cover
                    _IndexLwrObjs = _IndexUprObjs;
                    DeActivateCovers();
                }
                break;
            case SCREENPOSITION.LOWER:
                _IndexLwrObjs--;
                if (_IndexLwrObjs < 0)
                {
                    // Activate Cover Page
                    ActivateFrontCover();
                }
                else if (_IndexLwrObjs == lowerObjects.Length - 1)
                {
                    // Last page before cover
                    _IndexUprObjs = _IndexLwrObjs;
                    DeActivateCovers();
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
        Debug.Log("UpperIndex: " + _IndexUprObjs);
        Debug.Log("LowerIndex: " + _IndexLwrObjs);
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
        Debug.Log("UpperIndex: " + _IndexUprObjs);
        Debug.Log("LowerIndex: " + _IndexLwrObjs);
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
