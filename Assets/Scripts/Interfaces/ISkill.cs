using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface ISkill : IAttack
{
    float CastTime { get; }
    float GetCooldown();
    float GetRemainingCooldown();
    bool IsReady();
    bool IsUsable();


    void DecrementCooldown(float decrement);
}

