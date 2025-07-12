using System;
using UnityEngine;

public class Health : MonoBehaviour, IHealth, IDamageable
{
    [SerializeField] private int maxHP = 30;
    private int currentHP;

    public int MaxHP => maxHP;
    public int CurrentHP => currentHP;

    public event Action<int, int> OnHealthChanged = delegate { };
    public event Action OnDied = delegate { };

    private void OnEnable()
    {
        currentHP = maxHP;
        OnHealthChanged(currentHP, maxHP);
    }

    public void ApplyDamage(int damage)
    {
        if (damage <= 0) return;

        currentHP = Mathf.Max(currentHP - damage, 0);
        OnHealthChanged(currentHP, maxHP);

        if (currentHP == 0)
            Die();
    }

    private void Die()
    {
        OnDied();
    }
}
