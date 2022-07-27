using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridItem : MonoBehaviour
{
    [HideInInspector] public int Grid_I,Grid_J;
    public bool IsWall, IsRoad, IsPainted;
    public SphereCollider Collider;
   
    
}
