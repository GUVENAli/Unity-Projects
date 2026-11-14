using System.Collections;
using UnityEngine;

public class DamageEffect : MonoBehaviour
{
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Color _damageColor = Color.red;
    [SerializeField] private float _effectDuration = 0.15f;

    private Color _originalColor;
    private Coroutine _effectCoroutine;

    private void Awake()
    {
        if (_renderer == null) _renderer = GetComponentInChildren<Renderer>();

        if (_renderer != null) _originalColor = _renderer.material.color;
    }

    public void ShowDamageEffect()
    {
        if (_renderer == null) return;

        if(_effectCoroutine != null) StopCoroutine(_effectCoroutine);

        _effectCoroutine = StartCoroutine(DamageFlash());
    }

    private IEnumerator DamageFlash()
    {
        _renderer.material.color = _damageColor;

        yield return new WaitForSeconds(_effectDuration);

        _renderer.material.color = _originalColor;
        _effectCoroutine = null;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
