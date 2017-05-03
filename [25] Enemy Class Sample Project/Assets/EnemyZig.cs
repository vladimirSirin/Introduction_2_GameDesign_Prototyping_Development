using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyZig : Enemy {

    public override void Move()
    {
        Vector3 tempPos = pos;
        tempPos.x = Mathf.Sin(Time.time * Mathf.PI * 2) * 4;
        pos = tempPos;
        base.Move();
    }

}
