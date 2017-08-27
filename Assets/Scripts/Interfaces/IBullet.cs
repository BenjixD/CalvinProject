using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IBullet
{
    void SetTravelProperties(Vector3 direction, float speed);
    void SetTravelProperties(Vector3 direction);
    void OnHit(GameObject other);
}
