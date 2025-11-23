using UnityEngine;
using System.Collections.Generic;

public class ConstructManager : MonoBehaviour
{
    [Header("Prefabs")]
    public List<GameObject> prefabs;

    [Header("Jugador / Cámara")]
    public Transform playerCamera;
    public InvYcons jugador;

    [Header("Opciones")]
    public float shortRayDistance = 3f;   // Distancia máxima del preview
    public LayerMask Suelo;
    public LayerMask Estructura;
    public float rotationStep = 90f;

    // Estado interno
    private int selectedIndex = -1;
    private GameObject previewObject;
    private Quaternion previewRotation = Quaternion.identity;
    private bool canBuild = false;
    private int costoActual = 0;

    void Update()
    {
        if (selectedIndex == -1 || selectedIndex >= prefabs.Count)
            return;

        HandleRotation();
        UpdatePreviewPosition();

        if (Input.GetMouseButtonDown(0))
            PlaceObject();
    }

    // -------------------------------------------------------
    //  SELECCIONAR PREFAB
    // -------------------------------------------------------
    public void SeleccionarConstruccion(int index, int Ctroncos)
    {
        if (jugador.troncos >= Ctroncos)
        {
            jugador.ModoConstruccion();
            if (index < 0 || index >= prefabs.Count)
                return;

            selectedIndex = index;
            costoActual = Ctroncos;   // Guardamos el costo
            CreatePreview();
        }
        else
        {
            Debug.Log("Faltan troncos para construir");
        }
    }

    // -------------------------------------------------------
    //  CREAR PREVIEW
    // -------------------------------------------------------
    void CreatePreview()
    {
        if (previewObject != null)
            Destroy(previewObject);

        previewObject = Instantiate(prefabs[selectedIndex]);

        previewRotation = Quaternion.identity;
        previewObject.transform.rotation = previewRotation;

        SetTransparent(previewObject, 0.4f);

        foreach (Transform t in previewObject.GetComponentsInChildren<Transform>())
            t.gameObject.layer = LayerMask.NameToLayer("Preview");
    }

    // -------------------------------------------------------
    //  ACTUALIZAR POSICIÓN DEL PREVIEW
    // -------------------------------------------------------
    void UpdatePreviewPosition()
    {
        if (previewObject == null) return;

        RaycastHit hit;
        bool hitFloor = false;

        // Ray corto desde la cámara hacia adelante
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);

        if (Physics.Raycast(ray, out hit, shortRayDistance, Suelo))
        {
            hitFloor = true;
        }

        if (hitFloor)
        {
            previewObject.SetActive(true);

            Renderer r = previewObject.GetComponentInChildren<Renderer>();

            // 💡 Ajustar para que NO se hunda en el suelo
            if (r != null)
            {
                previewObject.transform.position =
                    hit.point + Vector3.up * r.bounds.extents.y;
            }
            else
            {
                previewObject.transform.position = hit.point;
            }

            previewObject.transform.rotation = previewRotation;

            // Comprobación de solape
            if (r != null)
            {
                Vector3 extents = r.bounds.extents * 0.9f;

                canBuild = !Physics.CheckBox(
                    previewObject.transform.position,
                    extents,
                    previewRotation,
                    Estructura
                );
            }

            UpdatePreviewColor(canBuild);
        }
        else
        {
            // Siempre visible aunque no haya suelo
            previewObject.SetActive(true);
            previewObject.transform.position =
                playerCamera.position + playerCamera.forward * shortRayDistance;

            previewObject.transform.rotation = previewRotation;

            canBuild = false;
            UpdatePreviewColor(false);
        }
    }

    // -------------------------------------------------------
    //  ROTAR CON RUEDA DEL MOUSE
    // -------------------------------------------------------
    void HandleRotation()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll > 0f)
            previewRotation *= Quaternion.Euler(0, rotationStep, 0);
        else if (scroll < 0f)
            previewRotation *= Quaternion.Euler(0, -rotationStep, 0);
    }

    // -------------------------------------------------------
    //  COLOCAR OBJETO REAL
    // -------------------------------------------------------
    void PlaceObject()
    {
        if (!previewObject.activeSelf || !canBuild)
            return;

        GameObject realObj = Instantiate(
            prefabs[selectedIndex],
            previewObject.transform.position,
            previewRotation
        );

        foreach (Transform t in realObj.GetComponentsInChildren<Transform>())
            t.gameObject.layer = LayerMask.NameToLayer("Estructura");

        // 🔥 RESTAR RECURSOS
        jugador.troncos -= costoActual;

        // 🔥 BORRAR PREVIEW
        Destroy(previewObject);
        previewObject = null;

        // 🔥 SALIR DEL MODO CONSTRUCCIÓN
        selectedIndex = -1;
        costoActual = 0;
    }

    // -------------------------------------------------------
    //  CAMBIAR COLOR ROJO / VERDE
    // -------------------------------------------------------
    void UpdatePreviewColor(bool valid)
    {
        Renderer[] renderers = previewObject.GetComponentsInChildren<Renderer>();

        foreach (Renderer r in renderers)
        {
            foreach (Material m in r.materials)
            {
                Color c = m.color;
                c = valid ?
                    new Color(0f, 1f, 0f, c.a) :
                    new Color(1f, 0f, 0f, c.a);

                m.color = c;
            }
        }
    }

    // -------------------------------------------------------
    //  HACER TRANSPARENTE
    // -------------------------------------------------------
    void SetTransparent(GameObject obj, float alpha)
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();

        foreach (Renderer r in renderers)
        {
            foreach (Material m in r.materials)
            {
                Color c = m.color;
                c.a = alpha;
                m.color = c;
            }
        }
    }
}
