using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class RadialLayout : MonoBehaviour
{
    public float radius = 200f; // Radius of the circle (in pixels for UI)
    public bool halfCircle = false; // Toggle between full circle and half circle
    public float offsetAngle = 0f; // Offset angle in degrees
    public float animationDuration = 0.5f; // Duration of the transition animation
    public Vector2 childSize = new Vector2(100f, 100f); // Size of each child (width, height)
    public float middleChildScale = 1.2f; // Scale factor for the middle child in half-circle mode

    private bool isAnimating = false;
    private bool isChildAnimating = false;
    private float animationProgress = 0f;
    private float childAnimationProgress = 0f;
    private bool targetHalfCircleState;
    public Animator animator;

    private Transform rotatingChild;
    private Vector2 startPosition;
    private Vector2 endPosition;
    private float startAngle;
    private float endAngle;

    public void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Button>().onClick.AddListener(RotateChildren);
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
        childAnimationProgress += Time.deltaTime / animationDuration;
        if (childAnimationProgress >= 1f)
        {
            childAnimationProgress = 1f;
            isChildAnimating = false;
            rotatingChild.SetAsFirstSibling();
            ArrangeChildren();
            return;
        }

        // Interpolate the angle for anti-clockwise rotation
        float angle = Mathf.Lerp(startAngle, endAngle, childAnimationProgress) * Mathf.Deg2Rad;
        float x = -Mathf.Cos(angle) * radius;
        float y = Mathf.Sin(angle) * radius;

        rotatingChild.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);
    }

    public void ToggleCircleState()
    {
        if (isAnimating) return; // Prevent toggling during animation

        if (!halfCircle)
        {
            animator.Play("Open");
        }
        else
        {
            animator.Play("Close");
        }
        targetHalfCircleState = !halfCircle;
        isAnimating = true;
        animationProgress = 0f;
    }

    public void delayToggleCircleState()
    {
        Invoke("ToggleCircleState", 0.25f);
    }

    public void RotateChildren()
    {
        if (!halfCircle) return; // Only rotate in half-circle mode

        int childCount = transform.childCount;
        if (childCount == 0) return;

        // Get the last child
        rotatingChild = transform.GetChild(childCount - 1);

        // Calculate start and end angles for anti-clockwise rotation
        startAngle = offsetAngle + (childCount - 1) * (180f / childCount); // Start at the last child's position
        endAngle = startAngle - 180f; // Move anti-clockwise by 180 degrees

        // Start the animation
        isChildAnimating = true;
        childAnimationProgress = 0f;
    }
}