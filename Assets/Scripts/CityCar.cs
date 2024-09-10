using System.Collections.Generic;
using UnityEngine;

public class CityCar : MonoBehaviour
{

    [SerializeField] private Material[] materialArray;
    [SerializeField] private MeshRenderer[] meshRendererArray;
    
    private List<DrivingPosition> path;
    private int currentPathIndex;
    public void Setup(List<DrivingPosition> path) {
        this.path = path;
        currentPathIndex = 0;
        foreach (MeshRenderer meshRenderer in meshRendererArray) {
            meshRenderer.gameObject.SetActive(false);
        }
        MeshRenderer selectedMeshRender = meshRendererArray[Random.Range(0, meshRendererArray.Length)];
        selectedMeshRender.gameObject.SetActive(true);
        selectedMeshRender.material = materialArray[Random.Range(0, materialArray.Length)];
    }

    private void Update()
    {
        if (path[currentPathIndex] == null || path[currentPathIndex].transform == null) {
            Destroy(gameObject);
            return;
        }
        Vector3 targetPosition = path[currentPathIndex].transform.position;
        Vector3 moveDir = (targetPosition - transform.position).normalized;
        float moveSpeed = 12f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;

        float rotationSpeed = 12f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotationSpeed);
        float reachedTargetDistance = .1f;
        if (Vector3.Distance(transform.forward, targetPosition) < reachedTargetDistance) {
            currentPathIndex++;
            if (currentPathIndex >= path.Count) {
                Destroy(gameObject);
            }
        }
    }
}
