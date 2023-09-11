using UnityEngine;
using UnityEngine.UI;

public class Demo : MonoBehaviour
{
    private float _hp;
    private float _maxHp;
    private Image _hpBar;

    void Start()
    {
        _maxHp = 100f;
        _hp = _maxHp;
        _hpBar = GameObject.Find("P2Hp").GetComponent<Image>();
    }

    public void OnDamage(float damage)
    {
        var spriteRenderer = GetComponent<SpriteRenderer>();
        
        if (spriteRenderer != null)
        {
            var randomColor = Random.ColorHSV();
            spriteRenderer.color = randomColor;
        }

        Debug.Log(damage);
        if (_hp <= 0)
        {
            Debug.Log("dead.");
            return;
        }

        _hp -= damage;

        var fillAmount = _hp / _maxHp;
        fillAmount = Mathf.Clamp01(fillAmount);
        _hpBar.fillAmount = fillAmount;
        Debug.Log($"血量剩餘{_hp}");
    }
}
