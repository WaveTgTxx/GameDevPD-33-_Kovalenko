using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCrabController : EnemyControllerBase
{
    protected override void ChangeState(EnemyState state)
    {
        base.ChangeState(state);
        switch (currState)
        {
            case EnemyState.Idle:
                enemyRb.velocity = Vector2.zero;
                break;
            case EnemyState.Move:
                startPoint = transform.position;
                break;
        }
    }
}
