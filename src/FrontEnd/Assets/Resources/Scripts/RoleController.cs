using UnityEngine;
using UnityEngine.UI;

public class RoleController : MonoBehaviour
{
    public int PlayerId;
    public KeyCode AttackKeyCode;
    public float speed = 500f;
    [SerializeField] private Rigidbody2D rb;
    private Camera _mainCamera;
    private Animator _animator;
    private AudioManager _audioManager;
    private float _objectWidth;
    private float _objectHeight;
    private static readonly int IsWalk = Animator.StringToHash("isWalk");
    private static readonly int Attacked = Animator.StringToHash("attacked");
    public string PlayerHp; 
    private float _hp;
    private float _maxHp;
    private Image _hpBar;


    private Vector2 _moveInput;
    private Transform _transform;

    void Start()
    {
        _mainCamera = Camera.main;

        // _rb = GetComponent<Rigidbody2D>();
        _objectWidth = GetComponent<SpriteRenderer>().bounds.size.x;
        _objectHeight = GetComponent<SpriteRenderer>().bounds.size.y;
        _animator = GetComponentInChildren<Animator>();
        _audioManager = GetComponentInChildren<AudioManager>();
        _transform = GetComponent<Transform>();

        _maxHp = 100f;
        _hp = _maxHp;
        _hpBar = GameObject.Find(PlayerHp).GetComponent<Image>();
    }

    void Update()
    {
        RestrictMovementInCameraView();
        Attack();
        Move();
    }

    /// <summary>
    /// 可以用 Cine Machine
    /// </summary>
    private void RestrictMovementInCameraView()
    {
        // 取得攝影機的可見範圍
        var cameraLeftEdge = _mainCamera.ViewportToWorldPoint(new Vector3(0, 0, _mainCamera.nearClipPlane)).x;
        var cameraRightEdge = _mainCamera.ViewportToWorldPoint(new Vector3(1, 0, _mainCamera.nearClipPlane)).x;
        var cameraBottomEdge = _mainCamera.ViewportToWorldPoint(new Vector3(0, 0, _mainCamera.nearClipPlane)).y;
        var cameraTopEdge = _mainCamera.ViewportToWorldPoint(new Vector3(0, 1, _mainCamera.nearClipPlane)).y;

        // 計算ui
        var uiLeftEdge = -400f /*UI左边缘的X坐标*/;
        var uiRightEdge = 400f /*UI右边缘的X坐标*/;
        var uiBottomEdge = -38.78078f /*UI底边缘的Y坐标*/;
        var uiTopEdge = 46.74922f /*UI顶边缘的Y坐标*/;

        // 限制角色在摄像机的可见范围内移动，同时考虑UI元素的边界
        var currentPosition = transform.position;
        currentPosition.x = Mathf.Clamp(currentPosition.x, cameraLeftEdge + _objectWidth / 2,
            cameraRightEdge - _objectWidth / 2);
        currentPosition.y = Mathf.Clamp(currentPosition.y, cameraBottomEdge + _objectHeight / 2,
            cameraTopEdge - _objectHeight / 2);

        // 限制角色在攝影機的可見範圍內移動
        currentPosition.x =
            Mathf.Clamp(currentPosition.x, uiLeftEdge + _objectWidth / 2, uiRightEdge - _objectWidth / 2);
        currentPosition.y = Mathf.Clamp(currentPosition.y, uiBottomEdge + _objectHeight / 2,
            uiTopEdge - _objectHeight / 2);

        transform.position = currentPosition;
    }


    private void Attack()
    {
        if (Input.GetKeyDown(AttackKeyCode))
        {
            // TODO: 要切檔案，集中管理
            _animator.SetTrigger(Attacked);
        }
    }

    public void OnDamage(float damage)
    {
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

    private void Move()
    {
        _moveInput = new Vector2(Input.GetAxis("PlayerHorizontal" + PlayerId), Input.GetAxis("PlayerVertical" + PlayerId));
        if (_moveInput == Vector2.zero)
        {
            _animator.SetBool(IsWalk, false);
        }
        else
        {
            _animator.SetBool(IsWalk, true);
            var localScale = _transform.localScale;
            _transform.localScale = _moveInput.x > 0
                ? new Vector3(1, localScale.y, localScale.z)
                : new Vector3(-1, localScale.y, localScale.z);
        }
    }

    private void FixedUpdate()
    {
        rb.AddForce(_moveInput * speed);
    }
}