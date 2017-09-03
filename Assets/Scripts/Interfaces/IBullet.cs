using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBullet
{
    bool enabled { get; set; }

    void SetTravelProperties(Vector3 direction, float speed);
    void SetTravelProperties(Vector3 direction);
    void OnHit(GameObject other);
}
