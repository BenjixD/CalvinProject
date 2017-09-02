using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface IAttack
{
    void InitAttack(Action<bool> action = null);
}
