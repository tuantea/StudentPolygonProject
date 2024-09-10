using UnityEngine;

public class CityBuilderGameAssets : MonoBehaviour
{
    public static CityBuilderGameAssets Instance { get; private set; }

    private void Awake() {
        Instance = this;
    }

    public BuildingTypeListSO buildingTypeListSO;
    public Transform gridObjectDebugPrefab;
    public Transform roadPrefab;
    public Transform npcPrefab;
    public Transform carPrefab;
    public Sprite dollarSprite;

    // Update is called once per frame
    void Update()
    {
        
    }
}
