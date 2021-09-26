using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneMove : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Rigidbody2D rb;

    public void Move(Vector3 vec)
    {
        transform.position += vec;
    }
}
