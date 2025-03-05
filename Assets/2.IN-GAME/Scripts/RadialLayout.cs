using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class RadialLayout : MonoBehaviour
{
    public float radius = 200f; // Radius of the circle (in pixels for UI)
    public bool halfCircle = false; // Toggle between full circle and half circle
    public float offsetAngle = 0f; // Offset angle in degrees
    public float animationDuration = 0.5f; // Duration of the transition animation
    public float childChangeanimationDuration = 0.5f; // Duration of the transition animation
    public Vector2 childSize = new Vector2(100f, 100f); // Size of each child (width, height)
    public float middleChildScale = 1.2f; // Scale factor for the middle child in half-circle mode

    private bool isAnimating = false;
    private bool isChildAnimating = false;
    private float animationProgress = 0f;
    private float childAnimationProgress = 0f;
    private bool targetHalfCircleState;
    public Animator animator;

    private Transform rotatingChild;
    private int rotatingChildIndex;
    private Vector2 startPosition;
    private Vector2 endPosition;
    private float startAngle;
    private float endAngle;


private Dictionary<Button, UnityEngine.Events.UnityAction> buttonListeners = new Dictionary<Button, UnityEngine.Events.UnityAction>();
    public void Start()
    {
        AddListenerTOButton();
    }

    private void AddListenerTOButton()
    {
    for (int i = 0; i < transform.childCount; i++)
    {
        Button button = transform.GetChild(i).GetComponent<Button>();
        if (button == null) continue; 

        // Remove the previous listener if it exists
        if (buttonListeners.ContainsKey(button))
        {
            button.onClick.RemoveListener(buttonListeners[button]);
            buttonListeners.Remove(button);
        } 

        int temp = i;
        UnityEngine.Events.UnityAction action = () => checkMoveType(temp); 
        
        // Store the listener reference
        buttonListeners[button] = action;
        // Add the listener
        button.onClick.AddListener(action);
    }
     }

    void Update()
    {
        if (isAnimating)
        {
            AnimateLayout();
        }
        else if (isChildAnimating)
        {
            AnimateChildLayout();
        }
        else
        {
            ArrangeChildren();
        }
    }

    void ArrangeChildren()
    {
        int childCount = transform.childCount;
        if (childCount == 0) return;

        float angleStep = (halfCircle ? 180f : 360f) / childCount;
        float currentAngle = offsetAngle;

        for (int i = 0; i < childCount; i++)
        {
            RectTransform child = transform.GetChild(i) as RectTransform;
            if (child == null) continue;

            // Set the size of the child
            child.sizeDelta = childSize;

            float angle = currentAngle * Mathf.Deg2Rad; // Convert to radians

            // Calculate the position on the circumference
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;

            child.anchoredPosition = new Vector2(x, y);

            // Rotate the child to face outward
            if (halfCircle)
            {
                child.localRotation = Quaternion.Euler(0f, 0f, currentAngle - 90f);
            }
            else
            {
                child.localRotation = Quaternion.Euler(0f, 0f, currentAngle);
            }

            // Scale the middle child in half-circle mode
            if (halfCircle && i == childCount / 2)
            {
                child.localScale = Vector3.one * middleChildScale;
            }
            else
            {
                child.localScale = Vector3.one;
            }

            currentAngle += angleStep;
            child.rotation = Quaternion.identity;
        }
    }

    void AnimateLayout()
    {
        animationProgress += Time.deltaTime / animationDuration;
        if (animationProgress >= 1f)
        {
            animationProgress = 1f;
            isAnimating = false;
            halfCircle = targetHalfCircleState;
        }

        int childCount = transform.childCount;
        if (childCount == 0) return;

        float startAngleStep = (halfCircle ? 180f : 360f) / childCount;
        float targetAngleStep = (targetHalfCircleState ? 180f : 360f) / childCount;
        float currentAngleStep = Mathf.Lerp(startAngleStep, targetAngleStep, animationProgress);

        float currentAngle = offsetAngle;

        for (int i = 0; i < childCount; i++)
        {
            if(rotatingChild != null && rotatingChildIndex == i)
            {
                continue;
            }
            RectTransform child = transform.GetChild(i) as RectTransform;
            if (child == null) continue;

            // Set the size of the child
            child.sizeDelta = childSize;

            float angle = currentAngle * Mathf.Deg2Rad; // Convert to radians

            // Calculate the position on the circumference
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;

            child.anchoredPosition = Vector2.Lerp(child.anchoredPosition, new Vector2(x, y), animationProgress);

            // Rotate the child to face outward
            if (targetHalfCircleState)
            {
                Quaternion targetRotation = Quaternion.Euler(0f, 0f, currentAngle - 90f);
                child.localRotation = Quaternion.Lerp(child.localRotation, targetRotation, animationProgress);
            }
            else
            {
                Quaternion targetRotation = Quaternion.Euler(0f, 0f, currentAngle);
                child.localRotation = Quaternion.Lerp(child.localRotation, targetRotation, animationProgress);
            }

            // Scale the middle child in half-circle mode
            if (targetHalfCircleState && i == childCount / 2)
            {
                child.localScale = Vector3.Lerp(child.localScale, Vector3.one * middleChildScale, animationProgress);
            }
            else
            {
                child.localScale = Vector3.Lerp(child.localScale, Vector3.one, animationProgress);
            }

            currentAngle += currentAngleStep;
            child.rotation = Quaternion.identity;
        }
    }

    void AnimateChildLayout()
    {
        childAnimationProgress += Time.deltaTime / childChangeanimationDuration;
        if (childAnimationProgress >= 1f)
        {
            childAnimationProgress = 1f;

            // Move the rotating child to its new position in the hierarchy
            if (rotatingChild != null)
            {
                if (startAngle < endAngle)
                {
                    // Move first child to last position
                    rotatingChild.SetAsLastSibling();
                }
                else
                {
                    // Move last child to first position
                    rotatingChild.SetAsFirstSibling();
                }
            }

            // Rearrange the children
            ArrangeChildren();
            if (isChildAnimating)
            {
                AddListenerTOButton();
            }
            rotatingChild=null;//So that while closing button is should Animate Prorely
            isChildAnimating = false;
            return;
        }

        // Interpolate the angle for rotation
        float angle = Mathf.Lerp(startAngle, endAngle, childAnimationProgress) * Mathf.Deg2Rad;
        float x = -Mathf.Cos(angle) * radius;
        float y = Mathf.Sin(angle) * radius;

        rotatingChild.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);
        AnimateLayout();
    }

    public void ToggleCircleState()
    {
        if (isAnimating) return; // Prevent toggling during animation

        if (!halfCircle)
        {
            animator.Play("Open");
            transform.GetChild(transform.childCount/2).GetComponent<Button>().onClick.Invoke();
        }
        else
        {
            animator.Play("Close");
        }
        targetHalfCircleState = !halfCircle;
        isAnimating = true;
        animationProgress = 0f;
        childAnimationProgress= 0f;
    }

    public void delayToggleCircleState()
    {
        Invoke("ToggleCircleState", 0.25f);
    }

    public void RotateChildren(bool moveFirstToLast)
    {
        if (!halfCircle) return; // Only rotate in half-circle mode

        int childCount = transform.childCount;
        if (childCount == 0) return;

        // Determine which child to rotate
        if (moveFirstToLast)
        {
            // Move first child to last position
            rotatingChild = transform.GetChild(0);
            rotatingChildIndex = 0;
            startAngle = offsetAngle; // Start at the first child's position
            endAngle = startAngle + 180f; // Move clockwise by 180 degrees
        }
        else
        {
            // Move last child to first position
            rotatingChild = transform.GetChild(childCount - 1);
            rotatingChildIndex = childCount - 1;
            startAngle = offsetAngle + (childCount - 1) * (180f / childCount); // Start at the last child's position
            endAngle = startAngle - 180f; // Move anti-clockwise by 180 degrees
        }

        // Start the animation
        isChildAnimating = true;
        childAnimationProgress = 0f;
        animationProgress = 0f;
    }

    private void checkMoveType(int childIndex)
    {
        print("childIndex: " + childIndex);
        if (isChildAnimating || transform.childCount / 2 == childIndex)
        {
            return;
        }
        
        int middleChildIndex = transform.childCount / 2;
        int difference = Mathf.Abs(childIndex - middleChildIndex);

        if (difference == 1)
        {
            // If the child is next to the middle child, move it once
            RotateChildren(childIndex > middleChildIndex);
        }
        else
        {
            // If the child is not next to the middle child, move it multiple times
            StartCoroutine(MoveChildMultipleTimes(childIndex, difference));
        }
    }

    private System.Collections.IEnumerator MoveChildMultipleTimes(int childIndex, int difference)
    {
        for (int i = 0; i < difference; i++)
        {
            RotateChildren(childIndex > transform.childCount / 2);
            yield return new WaitUntil(() => !isChildAnimating); // Wait for the current animation to finish
        }
    }
}