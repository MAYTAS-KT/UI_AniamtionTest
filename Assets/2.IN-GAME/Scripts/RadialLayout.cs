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

    private bool isAnimating = false;
    private float animationProgress = 0f;
    private bool targetHalfCircleState;

    void Update()
    {
        if (isAnimating)
        {
            AnimateLayout();
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

            currentAngle += angleStep;
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

            currentAngle += currentAngleStep;
        }
    }

    public void ToggleCircleState()
    {
        if (isAnimating) return; // Prevent toggling during animation

        targetHalfCircleState = !halfCircle;
        isAnimating = true;
        animationProgress = 0f;
    }
}