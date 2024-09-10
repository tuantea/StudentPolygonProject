
using System.Collections.Generic;
using UnityEngine;

public class CityNPC : MonoBehaviour {
    [SerializeField] private SkinnedMeshRenderer[] skinnedMeshRendererArray;
    [SerializeField] private Material[] materialArray;
    [SerializeField] private Animator animator;

    private List<SidewalkPosition> path;
    private int currentPathIndex;

    private void Awake() {
        foreach (SkinnedMeshRenderer skinnedMeshRenderer in skinnedMeshRendererArray) {
            skinnedMeshRenderer.gameObject.SetActive(false);
        }
        SkinnedMeshRenderer selectedSkinnedMeshRenderer = skinnedMeshRendererArray[Random.Range(0, skinnedMeshRendererArray.Length)];
        selectedSkinnedMeshRenderer.gameObject.SetActive(true);
        selectedSkinnedMeshRenderer.material = materialArray[Random.Range(0, materialArray.Length)];
    }
    public void Setup(List<SidewalkPosition> path) {
        this.path = path;
        currentPathIndex = 0;
    }

    void Update()
    {
        animator.SetFloat("Speed", 2f);
        if (path[currentPathIndex] == null) {
            Destroy(gameObject);
            return;
        }

        Vector3 targetPosition = path[currentPathIndex].transform.position;
        Vector3 moveDir = (targetPosition - transform.position).normalized;
        float moveSpeed = 3f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
        float rotationSpeed = 12f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, rotationSpeed*Time.deltaTime);
        float reachedTargetDistance = .1f;
        if (Vector3.Distance(transform.forward, targetPosition) < reachedTargetDistance) {
            currentPathIndex++;
            if (currentPathIndex > path.Count) {
                Destroy(gameObject);
            }
        }

    }
}
