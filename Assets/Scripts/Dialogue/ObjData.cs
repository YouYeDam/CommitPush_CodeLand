using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjData : MonoBehaviour
{
    public int ID;
    public bool IsNPC;
 
    public static ObjData Instance;
 
 
    private void Awake()
    {
        Instance = this;
    }
}
