/*using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BotonEstructuraUI : MonoBehaviour
{
    public Button button;
    public Image icono;
    public TextMeshProUGUI etiqueta;

    public void Set(Sprite sprite, string texto)
    {
        if (icono) icono.sprite = sprite;
        if (etiqueta) etiqueta.text = texto;
    }
}
*/
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BotonEstructuraUI : MonoBehaviour
{
    [Header("Config")]
    public string nombre;
    public GameObject prefab;
    public Sprite icono;
    public int costoTroncos;

    [Header("Refs UI")]
    public Button button;
    public Image imagen;
    public TextMeshProUGUI etiqueta;

    private PanelConstruccion panel;

    public void Inicializar(PanelConstruccion p)
    {
        panel = p;

        if (imagen) imagen.sprite = icono;
        if (etiqueta) etiqueta.text = $"{nombre} ({costoTroncos})";

        if (button)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OnClick);
        }
    }

    void OnClick()
    {
        if (panel && prefab != null)
            panel.SeleccionarEstructura(this);
    }
}
