using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdConstruccion : MonoBehaviour
{
    public ConstructManager construct;
    public int construccion;

    public void selecionar()
    {
        construct.SeleccionarConstruccion(construccion);
    }
}
