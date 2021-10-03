using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventSystemCustom : MonoBehaviour
{
    public UnityEvent OnCloneStickyPlatformEnter;

    void Awake()
    {
        OnCloneStickyPlatformEnter = new UnityEvent();
    }
}
