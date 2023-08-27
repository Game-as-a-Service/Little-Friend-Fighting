using UnityEngine;

public class RoleHit : MonoBehaviour
{
    private Transform _attackRangeObject;
    private readonly float _attackRange = 0.05f;
    public LayerMask testMask;

    
    private void AttackIsHit()
    {
        var parentTransform = transform.parent;

        var attackRangeObject = parentTransform.Find("AttackRangeObject");
        
        var hitObjects = Physics2D.OverlapCircleAll(attackRangeObject.position, _attackRange, testMask);
        foreach (var hitObj in hitObjects)
        {
            Debug.Log(hitObj.gameObject.name);
            
            var parent = hitObj.gameObject.transform.parent;
            var spriteRenderer = parent.GetComponent<SpriteRenderer>();
        
            if (spriteRenderer != null)
            {
                var randomColor = Random.ColorHSV();
                spriteRenderer.color = randomColor;
            }
            
            parent.SendMessage("OnDamage", 5f);
        }
    }
}