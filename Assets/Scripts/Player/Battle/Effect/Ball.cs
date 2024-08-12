using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private BattleBehaviourComponent target;

    public void SetTarget(BattleBehaviourComponent target)
    {
        this.target = target;
    }

    void Update()
    {
        if (target != null)
        {
            MoveTowardsTarget(target);
        }
    }

    private void MoveTowardsTarget(BattleBehaviourComponent t)
    {
        transform.position = Vector3.MoveTowards(transform.position, t.transform.position + new Vector3(0f, 1.2f, 0f), Time.deltaTime * 30);

        // 타겟에 도달했는지 확인
        if (Vector3.Distance(transform.position, t.transform.position) < 0.1f)
        {
            // 몬스터에게 닿았을 때의 로직
            Destroy(gameObject, 0.5f);
        }
        else
        {
            Destroy(gameObject, 1f);
        }
    }
}
