using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Mask))]
[RequireComponent(typeof(ScrollRect))]
public class ScrollSnapRect : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler {

    [Tooltip("Set starting page index - starting from 0")]
    public int startingPage = 0;
    [Tooltip("Threshold time for fast swipe in seconds")]
    public float fastSwipeThresholdTime = 0.3f;
    [Tooltip("Threshold time for fast swipe in (unscaled) pixels")]
    public int fastSwipeThresholdDistance = 100;
    [Tooltip("How fast will page lerp to target position")]
    public float decelerationRate = 10f;
    [Tooltip("Button to go to the previous page (optional)")]
    public GameObject prevButton;
    [Tooltip("Button to go to the next page (optional)")]
    public GameObject nextButton;
    [Tooltip("Sprite for unselected page (optional)")]
    public Sprite unselectedPage;
    [Tooltip("Sprite for selected page (optional)")]
    public Sprite selectedPage;
    [Tooltip("Container with page images (optional)")]
    public Transform pageSelectionIcons;

    private ScrollRect scrollRectComponent;
    private RectTransform scrollRectRect;
    private RectTransform container;
    
    // number of pages in container
    private int pageCount;
    private int currentPage;

    // whether lerping is in progress and target lerp position
    private bool lerp;
    private Vector2 lerpTo;

    // target position of every page
    private List<Vector2> pagePositions = new List<Vector2>();

    // in draggging, when dragging started and where it started
    private bool dragging;
    private float timeStamp;
    private Vector2 startPosition;

    // for showing small page icons
    private bool showPageSelection;
    private int previousPageSelectionIndex;
    // container with Image components - one Image for each page
    private List<Image> pageSelectionImages;

    //------------------------------------------------------------------------
    void Start() {
        scrollRectComponent = GetComponent<ScrollRect>();
        scrollRectRect = GetComponent<RectTransform>();
        container = scrollRectComponent.content;
        pageCount = container.childCount;
        lerp = false;

        // init
        SetPagePositions();
        SetPage(startingPage);
        //InitPageSelection();
        //SetPageSelection(startingPage);


        if (nextButton)
            nextButton.GetComponent<Button>().onClick.AddListener(() => { NextScreen(); });

        if (prevButton)
            prevButton.GetComponent<Button>().onClick.AddListener(() => { PreviousScreen(); });
	}

    //------------------------------------------------------------------------
    void Update() {
        if (lerp) {
            float decelerate = Mathf.Min(decelerationRate * Time.deltaTime, 1f);
            container.anchoredPosition = Vector2.Lerp(container.anchoredPosition, lerpTo, decelerate); 
            
            if (Vector2.SqrMagnitude(container.anchoredPosition - lerpTo) < 0.25f) {

                container.anchoredPosition = lerpTo;
                lerp = false;
                scrollRectComponent.velocity = Vector2.zero;
            }

            /* switches selection icon exactly to correct page
            if (_showPageSelection) {
                SetPageSelection(GetNearestPage());
            }
            */
        }
    }

    //------------------------------------------------------------------------
    private void SetPagePositions() {
        int width = 0;
        int offsetX = 0;
        int containerWidth = 0;

        width = (int)scrollRectRect.rect.width; // ancho del scrollrect
        offsetX = width / 2; // posicion central de las paginas
        containerWidth = width * pageCount; // ancho total

        // set width of container
        Vector2 newSize = new Vector2(containerWidth, 0);
        container.sizeDelta = newSize;
        Vector2 newPosition = new Vector2(containerWidth / 2, 0);
        container.anchoredPosition = newPosition;

        pagePositions.Clear();

        for (int i = 0; i < pageCount; i++) {
            RectTransform child = container.GetChild(i).GetComponent<RectTransform>();
            Vector2 childPosition = new Vector2(i * width - containerWidth / 2 + offsetX, 0f);
            child.anchoredPosition = childPosition;
            pagePositions.Add(-childPosition);
        }
    }

    //------------------------------------------------------------------------
    private void SetPage(int position) {
        position = Mathf.Clamp(position, 0, pageCount - 1);
        container.anchoredPosition = pagePositions[position];
        currentPage = position;
    }

    //------------------------------------------------------------------------
    private void GoToPage(int position) {
        position = Mathf.Clamp(position, 0, pageCount - 1);
        lerpTo = pagePositions[position];
        lerp = true;
        currentPage = position;
    }

    private void NextScreen() {
        GoToPage(currentPage + 1);
    }

    private void PreviousScreen() {
        GoToPage(currentPage - 1);
    }

    //------------------------------------------------------------------------
    public void OnBeginDrag(PointerEventData aEventData) {
        // if currently lerping, then stop it as user is draging
        lerp = false;
        // not dragging yet
        dragging = false;
    }

    //------------------------------------------------------------------------
    public void OnEndDrag(PointerEventData aEventData) {
        // how much was container's content dragged
        float difference = startPosition.x - container.anchoredPosition.x; 
        if (difference > 0) {
                NextScreen();
            } else {
                PreviousScreen();
            }
        dragging = false;
    }

    //------------------------------------------------------------------------
    public void OnDrag(PointerEventData aEventData) {
        if (!dragging) {
            // dragging started
            dragging = true;
            // save time - unscaled so pausing with Time.scale should not affect it
            timeStamp = Time.unscaledTime;
            // save current position of cointainer
            startPosition = container.anchoredPosition;
        } else {
            /*
            if (_showPageSelection) {
                SetPageSelection(GetNearestPage());
            }
            */
        }
    }


    /*
    //------------------------------------------------------------------------
    private void InitPageSelection() {
        // page selection - only if defined sprites for selection icons
        _showPageSelection = unselectedPage != null && selectedPage != null;
        if (_showPageSelection) {
            // also container with selection images must be defined and must have exatly the same amount of items as pages container
            if (pageSelectionIcons == null || pageSelectionIcons.childCount != _pageCount) {
                Debug.LogWarning("Different count of pages and selection icons - will not show page selection");
                _showPageSelection = false;
            } else {
                _previousPageSelectionIndex = -1;
                _pageSelectionImages = new List<Image>();

                // cache all Image components into list
                for (int i = 0; i < pageSelectionIcons.childCount; i++) {
                    Image image = pageSelectionIcons.GetChild(i).GetComponent<Image>();
                    if (image == null) {
                        Debug.LogWarning("Page selection icon at position " + i + " is missing Image component");
                    }
                    _pageSelectionImages.Add(image);
                }
            }
        }
    }

    //------------------------------------------------------------------------
    private void SetPageSelection(int aPageIndex) {
        // nothing to change
        if (_previousPageSelectionIndex == aPageIndex) {
            return;
        }
        
        // unselect old
        if (_previousPageSelectionIndex >= 0) {
            _pageSelectionImages[_previousPageSelectionIndex].sprite = unselectedPage;
            _pageSelectionImages[_previousPageSelectionIndex].SetNativeSize();
        }

        // select new
        _pageSelectionImages[aPageIndex].sprite = selectedPage;
        _pageSelectionImages[aPageIndex].SetNativeSize();

        _previousPageSelectionIndex = aPageIndex;
    }

    //------------------------------------------------------------------------

        private int GetNearestPage() {
        // based on distance from current position, find nearest page
        Vector2 currentPosition = _container.anchoredPosition;

        float distance = float.MaxValue;
        int nearestPage = _currentPage;

        for (int i = 0; i < _pagePositions.Count; i++) {
            float testDist = Vector2.SqrMagnitude(currentPosition - _pagePositions[i]);
            if (testDist < distance) {
                distance = testDist;
                nearestPage = i;
            }
        }

        return nearestPage;
    }
    */
}
