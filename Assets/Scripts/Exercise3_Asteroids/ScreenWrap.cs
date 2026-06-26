using UnityEngine;

public class ScreenWrap : MonoBehaviour
{
    void Update()
    {
        WrapAround();
    }

    /// <summary>
    /// Wraps object out of bound to the other side
    /// </summary>
    private void WrapAround()
    {
        Vector3 wrappedTransformPosition = transform.position;

        if (wrappedTransformPosition.x > ScreenBounds.ScreenRight)
        {
            wrappedTransformPosition.x = ScreenBounds.ScreenLeft;
        }
        else if (wrappedTransformPosition.x < ScreenBounds.ScreenLeft)
        {
            wrappedTransformPosition.x = ScreenBounds.ScreenRight;
        }

        if (wrappedTransformPosition.y > ScreenBounds.ScreenTop)
        {
            wrappedTransformPosition.y = ScreenBounds.ScreenBottom;
        }
        else if (wrappedTransformPosition.y < ScreenBounds.ScreenBottom)
        {
            wrappedTransformPosition.y = ScreenBounds.ScreenTop;
        }

        transform.position = wrappedTransformPosition;
    }
}
