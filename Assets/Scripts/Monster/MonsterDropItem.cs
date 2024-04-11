using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDropItem : MonoBehaviour
{
    public GameObject Item1;
    Transform MyTransform;
    Vector3 MyPosition;
    [SerializeField] float Item1SpawnProbability = 1;

    void Update() {
    MyPosition = transform.position;
    }

    public void DropItem() {
        float RandomValue = Random.value;
        if (RandomValue <= Item1SpawnProbability) {
            GameObject Item = Instantiate(Item1, MyPosition, Quaternion.identity);
        }
    }
}
