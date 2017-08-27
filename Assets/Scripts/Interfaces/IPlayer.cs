using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IPlayer
{
    bool IsFacingRight { get; }
    bool Grounded { get; }
    bool Moving { get; }
    bool Attacking { get; }
}
