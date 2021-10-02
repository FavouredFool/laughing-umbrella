using UnityEngine;

public interface IMeleeAttackerActions
{
    public void StartAttack();
    void EndAttack();
    void AttackerMoving();
    void AttackerSearching();
    float GetAttackDowntime();
}

