using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface ISkill : IAttack
{
    float GetCooldown();
    float GetRemainingCooldown();
    bool IsReady();
    bool IsUsable();
    void UseSkill();
}

