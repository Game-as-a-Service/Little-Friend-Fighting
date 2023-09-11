using UnityEngine;

public class PlayerAttackCollider : MonoBehaviour
{
    public int knockBackForce;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        var parentPosition = transform.parent.position;
        var direction = other.transform.position - parentPosition;

        var component = other.GetComponent<Rigidbody2D>();
        component.AddForce(direction * knockBackForce);
        other.SendMessage("OnDamage",5f);
    }
}
