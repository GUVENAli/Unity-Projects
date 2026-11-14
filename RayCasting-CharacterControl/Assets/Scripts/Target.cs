using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private int _health = 100;

    public void TakeDamage(int damage)
    {
        _health -= damage;
        
        if (_health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
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
