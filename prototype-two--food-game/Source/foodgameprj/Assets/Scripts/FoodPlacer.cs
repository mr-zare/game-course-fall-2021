using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodPlacer : MonoBehaviour
{
    public GameObject[] prefabs;

    public float minX;
    public float maxX;

    public float timerMaxTime;
    private float currentTimerValue;

    private void Start()
    {
        currentTimerValue = timerMaxTime;
    }

    void Update()
    {
        if (currentTimerValue > 0)
        {
            currentTimerValue -= Time.deltaTime;
        }
        else
        {
            GameObject go = Instantiate(prefabs[GetRandomPrefabType()]);
            go.transform.position = new Vector3(GetRandomPrefabInitialX(), transform.position.y, transform.position.z);

            // reset timer
            currentTimerValue = timerMaxTime;
        }
    }
    int GetRandomPrefabType()
    {
        return Random.Range(0, prefabs.Length);
    }

    float GetRandomPrefabInitialX()
    {
        return Random.Range(minX,maxX);

    }
}
