using UnityEngine;

public abstract class BaseObj : MonoBehaviour
{
    protected abstract void MoveTo(Vector3 go, float dis);

    protected abstract void Damage(int damage);
}
