using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

[RequireComponent(typeof(NavMeshSurface))]
[DefaultExecutionOrder(-103)]
public class BuildNavmesh : MonoBehaviour
{
    void Awake()
    {
        GetComponent<NavMeshSurface>().BuildNavMesh();

        StartCoroutine(BuildNavmeshCoroutine());
    }

    IEnumerator BuildNavmeshCoroutine()
    {
        while(true)
        {
            GetComponent<NavMeshSurface>().BuildNavMesh();
            yield return new WaitForSeconds(1.0f);
        }
    }
}