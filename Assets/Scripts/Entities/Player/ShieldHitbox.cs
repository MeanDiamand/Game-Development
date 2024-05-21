using UnityEngine;

public class ShieldHitbox : ObjectHitbox
{
    protected Vector3 shieldFaceUp = new Vector3(-5.82f, 1f, 0);
    protected Vector3 shieldUpSize = new Vector3(5f, 0.12f, 0);

    protected Vector3 shieldFaceDown = new Vector3(-5.82f, -2f, 0);
    protected Vector3 shieldDownSize = new Vector3(5f, 0.12f, 0);

    protected Vector3 shieldFaceLeft = new Vector3(-3f, 0, 0);
    protected Vector3 shieldLeftSize = new Vector3(1f, 1f, 0);

    protected Vector3 shieldFaceRight = new Vector3(0.1f, 0, 0);
    protected Vector3 shieldRightSize = new Vector3(1f, 1f, 0);
    public override void TurnLeft(bool left)
    {

        if (left)
        {
            gameObject.transform.localPosition = shieldFaceLeft;
            gameObject.transform.localScale = shieldLeftSize;
        }
    }
    public override void TurnRight(bool right)
    {
        if (right)
        {
            gameObject.transform.localPosition = shieldFaceRight;
            gameObject.transform.localScale = shieldRightSize;
        }
    }
    public override void TurnUp(bool up)
    {
        if (up)
        {
            gameObject.transform.localPosition = shieldFaceUp;
            gameObject.transform.localScale = shieldUpSize;
        }
    }
    public override void TurnDown(bool down)
    {
        if (down)
        {
            gameObject.transform.localPosition = shieldFaceDown;
            gameObject.transform.localScale = shieldDownSize;
        }
    }
}
