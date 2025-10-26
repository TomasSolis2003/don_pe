/*using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHambre : MonoBehaviour
{
    [Header("Hambre")]
    [Tooltip("Nivel máximo de hambre (100 = lleno, 0 = muriendo).")]
    public int hambreMax = 100;
    public int hambreActual;

    [Header("Desgaste")]
    [Tooltip("Cuánto se reduce el hambre cada cierto tiempo.")]
    public int perdidaPorTick = 1;

    [Tooltip("Cada cuántos segundos se pierde hambre.")]
    public float intervaloPerdida = 5f;

    [Tooltip("Daño por inanición cuando el hambre llega a 0.")]
    public int dañoPorHambre = 2;

    [Header("Referencias UI")]
    public Slider barraHambre;            // Slider o barra visual
    public TextMeshProUGUI textoHambre;   // Texto opcional ("75 / 100")

    private PlayerHealth salud;           // Referencia al script de vida

    private float timer = 0f;

    void Start()
    {
        hambreActual = hambreMax;
        salud = GetComponent<PlayerHealth>();
        RefrescarUI();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= intervaloPerdida)
        {
            timer = 0f;
            CambiarHambre(-perdidaPorTick);
        }
    }

    /// <summary>
    /// Cambia el nivel de hambre (positivo = comer, negativo = perder hambre)
    /// </summary>
    public void CambiarHambre(int cantidad)
    {
        hambreActual = Mathf.Clamp(hambreActual + cantidad, 0, hambreMax);
        RefrescarUI();

        if (hambreActual <= 0)
        {
            if (salud != null)
                salud.RecibirDaño(dañoPorHambre);
        }
    }

    private void RefrescarUI()
    {
        if (barraHambre)
            barraHambre.value = (float)hambreActual / hambreMax;

        if (textoHambre)
            textoHambre.text = $"{hambreActual} / {hambreMax}";
    }
}
*/
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHambre : MonoBehaviour
{
    [Header("Hambre")]
    [Tooltip("Nivel máximo de hambre (100 = lleno, 0 = muriendo).")]
    public int hambreMax = 100;
    public int hambreActual;

    [Header("Desgaste")]
    [Tooltip("Cuánto se reduce el hambre cada cierto tiempo.")]
    public int perdidaPorTick = 1;

    [Tooltip("Cada cuántos segundos se pierde hambre.")]
    public float intervaloPerdida = 5f;

    [Tooltip("Daño por inanición cuando el hambre llega a 0.")]
    public int dañoPorHambre = 2;

    [Header("Referencias UI")]
    public Slider barraHambre;
    public Image rellenoHambre;            // La parte “Fill” del slider
    public TextMeshProUGUI textoHambre;

    private PlayerHealth salud;
    private float timer = 0f;

    void Start()
    {
        hambreActual = hambreMax;
        salud = GetComponent<PlayerHealth>();
        RefrescarUI();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= intervaloPerdida)
        {
            timer = 0f;
            CambiarHambre(-perdidaPorTick);
        }
    }

    /// <summary>
    /// Cambia el nivel de hambre (positivo = comer, negativo = perder hambre)
    /// </summary>
    public void CambiarHambre(int cantidad)
    {
        hambreActual = Mathf.Clamp(hambreActual + cantidad, 0, hambreMax);
        RefrescarUI();

        if (hambreActual <= 0)
        {
            if (salud != null)
                salud.RecibirDaño(dañoPorHambre);
        }
    }

    private void RefrescarUI()
    {
        float porcentaje = (float)hambreActual / hambreMax;

        if (barraHambre)
            barraHambre.value = porcentaje;

        if (textoHambre)
            textoHambre.text = $"{hambreActual} / {hambreMax}";

        // 🌈 Cambiar color del relleno (verde → amarillo → rojo)
        if (rellenoHambre)
        {
            Color colorHambre = Color.Lerp(Color.red, Color.yellow, porcentaje * 1.5f);
            colorHambre = Color.Lerp(colorHambre, Color.green, porcentaje);
            rellenoHambre.color = colorHambre;
        }
    }
}
