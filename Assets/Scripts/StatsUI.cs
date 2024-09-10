using TMPro;
using UnityEngine;

public class StatsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyTextMesh;
    [SerializeField] private TextMeshProUGUI peopleTextMesh;
    [SerializeField] private RectTransform happinessSliderRectTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start() {
        CityBuilder.Instance.OnPeopleAmountChanged += CityBuilder_OnPeopleAmountChanged;
        CityBuilder.Instance.OnMoneyAmountChanged += CityBuilder_OnMoneyAmountChanged;
        CityBuilder.Instance.OnHappinessChanged += CityBuilder_OnHappinessChanged;
        UpdateStats();
    }

    private void CityBuilder_OnPeopleAmountChanged(object sender, System.EventArgs e) {
        UpdateStats();
    }


    private void CityBuilder_OnHappinessChanged(object sender, System.EventArgs e) {
        UpdateStats();
    }

    private void CityBuilder_OnMoneyAmountChanged(object sender, System.EventArgs e) {
        UpdateStats();
    }
    private void UpdateStats() {
        CityBuilder.Instance.GetStats(out int moneyAmount, out int peopleAmount, out float happiness);
        moneyTextMesh.text = moneyAmount.ToString();
        peopleTextMesh.text = peopleAmount.ToString();
        float happinessSliderMax = 260;
        happinessSliderRectTransform.anchoredPosition = new Vector2(happiness * happinessSliderMax, 0f);
    }
}
