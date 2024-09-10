using System;
using UnityEngine;

public class OfficeBuilding : MonoBehaviour,IPlaceBuilding
{
    public static event EventHandler<OnGeneratedMoneyEventArgs> OnAnyGeneratedMoney;
    public class OnGeneratedMoneyEventArgs : EventArgs {
        public int amount;
    }
    [SerializeField] private Transform visualTransform;

    private GridXZ<CityBuilder.GridObject> grid;
    private CityBuilder.GridObject gridObject;
    private float moneyTimer;

    public void Setup(GridXZ<CityBuilder.GridObject> grid, CityBuilder.GridObject gridObject) {
        this.grid = grid;
        this.gridObject = gridObject;
    }
    public void Update() {
        moneyTimer -= Time.deltaTime;
        if (moneyTimer <= 0f) {
            float moneyTimerMax = UnityEngine.Random.Range(.6f, 2f);
            moneyTimer = moneyTimerMax;
            int moneyAmount = UnityEngine.Random.Range(10, 300);
            OnAnyGeneratedMoney?.Invoke(this, new OnGeneratedMoneyEventArgs {
                amount = moneyAmount,
            });
        }
    }
}
