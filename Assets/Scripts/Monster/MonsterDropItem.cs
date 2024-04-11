using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDropItem : MonoBehaviour
{
    public GameObject Item1;
    [SerializeField] float Item1SpawnProbability = 1;

    public void DropItem() {
        float RandomValue = Random.value;
        if (RandomValue <= Item1SpawnProbability) {
            GameObject Item = Instantiate(Item1, Vector3.zero, Quaternion.identity);
        }
    }
}
