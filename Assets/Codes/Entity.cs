using System;
using UnityEngine;

public class Entity
{
    public GameObject obj;

    public Vector2 spawnPosition;
    public Vector2 targetPosition;




    public Entity(Vector2 spawnPosition)
    {
        this.spawnPosition = spawnPosition;
        this.targetPosition = spawnPosition;
    }
    public Vector2 Position
    {
        get
        {
            if (this.obj == null)
            {
                return Vector2.zero;
            }

            return this.obj.transform.position;
        }
    }
}
