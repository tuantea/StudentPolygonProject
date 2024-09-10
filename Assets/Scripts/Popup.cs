using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    private static Transform popupTemplate;

    public static void Create(Vector3 postiton, Sprite sprite, string text) {
        Transform popupTransform = Instantiate(popupTemplate, postiton, Quaternion.identity);
        popupTransform.gameObject.SetActive(true);
        popupTransform.GetComponent<Popup>().Setup(sprite, text);
    }
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI textMesh;

    private CanvasGroup canvasGroup;
    private float aliveTimer = 1.5f;

    private void Awake() {
        if (popupTemplate == null) {
            popupTemplate = transform;
            gameObject.SetActive(false);
            return;
        }
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void Update()
    {
        float moveSpeed = 3f;
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;
        if (aliveTimer < .5f) {
            float alpha = aliveTimer / .5f;
            canvasGroup.alpha = alpha;
        }
        aliveTimer -= Time.deltaTime;
        if (aliveTimer <= 0f) {
            Destroy(gameObject);
        }
    }
    public void Setup(Sprite sprite, string text) {
        image.sprite = sprite;
        textMesh.text = text;
    }
}
