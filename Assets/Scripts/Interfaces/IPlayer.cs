using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayer
{
    bool IsFacingRight { get; }
    bool Grounded { get; }
    bool Moving { get; }
    bool Attacking { get; }

    GameObject GetObject { get; }

    void StunPlayer(float stunLength);
    void InvinciblePlayer(float invincibleLength);
    void KnockbackPlayer(Vector3 knockback);
}
