using System;

public interface IHealth
{
    int MaxHP { get; }
    int CurrentHP { get; }
    event Action<int, int> OnHealthChanged;
    event Action OnDied;
}