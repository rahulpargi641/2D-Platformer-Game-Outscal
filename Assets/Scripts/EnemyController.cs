using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    void PatrolSound() // in the animation
    {
        SoundManager.Instance.PlayEnemyPatrolSound(ESounds.ChomperWalk);
    }
}
