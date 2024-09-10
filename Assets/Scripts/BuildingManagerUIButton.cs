using UnityEngine;
using UnityEngine.UI;

public class BuildingManagerUIButton : MonoBehaviour
{
    [SerializeField] private Image selectedImage;
    [SerializeField] private BuildingTypeSO buildingTypeSO;

    private Button button;

    private void Awake() {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => {
            BuildingManager.Instance.SetActiveBuildingTypeSO(buildingTypeSO);
        });
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BuildingManager.Instance.OnSelectedBuildingTypeChanged += BuildingManager_OnSelectedBuildingTypeChanged;
        UpdateSelected();
    }

    private void BuildingManager_OnSelectedBuildingTypeChanged(object sender, System.EventArgs e) {
        UpdateSelected();
    }

    // Update is called once per frame
    void UpdateSelected()
    {
        selectedImage.enabled = BuildingManager.Instance.GetSelectedBuildingTypeSO()== buildingTypeSO;
    }
}
