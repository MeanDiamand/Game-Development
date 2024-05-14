using UnityEngine;

public class ObjectHitbox : MonoBehaviour
{
    public float weaponDamage = 1f;

    public Collider2D objectCollider;

    protected Vector3 faceUp = new Vector3(-0.59f, 0.99f, 0);
    protected Vector3 faceUpSize = new Vector3(2.5f, 0.5f, 0);

    protected Vector3 faceDown = new Vector3(-0.67f, -1.53f, 0);
    protected Vector3 faceDownSize = new Vector3(2.5f, 0.5f, 0);

    protected Vector3 faceLeft = new Vector3(-1.34f, 0.14f, 0);
    protected Vector3 faceLeftSize = new Vector3(1f, 1f, 0);

    protected Vector3 faceRight = new Vector3(0.96f, 0.16f, 0);
    protected Vector3 faceRightSize = new Vector3(1f, 1f, 0);


    private void Start()
    {
        if (objectCollider == null)
        {
            Debug.LogWarning("Object collider is not set");
        }
    }

    public virtual void TurnLeft(bool left)
    {

        if (left)
        {
            gameObject.transform.localPosition = faceLeft;
            gameObject.transform.localScale = faceLeftSize;
        }
    }
    public virtual void TurnRight(bool right)
    {
        if (right)
        {
            gameObject.transform.localPosition = faceRight;
            gameObject.transform.localScale = faceRightSize;
        }
    }
    public virtual void TurnUp(bool up)
    {
        if (up)
        {
            gameObject.transform.localPosition = faceUp;
            gameObject.transform.localScale = faceUpSize;
        }
    }
    public virtual void TurnDown(bool down)
    {
        if (down)
        {
            gameObject.transform.localPosition = faceDown;
            gameObject.transform.localScale = faceDownSize;
        }
    }
}
