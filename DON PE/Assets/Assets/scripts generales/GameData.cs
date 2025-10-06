/*using UnityEngine;
using System.IO;

[System.Serializable]
public class GameData
{
    public float playerPosX;
    public float playerPosY;
    public float playerPosZ;
    public int playerScore;
}

public class GameSaveLoad : MonoBehaviour
{
    private string filePath; // Ruta del archivo de guardado

    private void Start()
    {
        filePath = Application.persistentDataPath + "/savefile.json"; // Ruta donde se guarda el archivo JSON
    }

    // Guardar los datos en un archivo JSON
    public void SaveGame(Vector3 playerPosition, int playerScore)
    {
        GameData data = new GameData
        {
            playerPosX = playerPosition.x,
            playerPosY = playerPosition.y,
            playerPosZ = playerPosition.z,
            playerScore = playerScore
        };

        string json = JsonUtility.ToJson(data); // Convertir los datos a JSON
        File.WriteAllText(filePath, json); // Guardar el JSON en el archivo

        Debug.Log("Juego guardado.");
    }

    // Cargar los datos desde el archivo JSON
    public void LoadGame()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath); // Leer el archivo JSON
            GameData data = JsonUtility.FromJson<GameData>(json); // Convertir el JSON a los datos

            // Aplicar los datos al juego
            Vector3 playerPosition = new Vector3(data.playerPosX, data.playerPosY, data.playerPosZ);
            int playerScore = data.playerScore;

            // Ejemplo: aplicar al jugador (puedes adaptarlo a tu propia lógica)
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                player.transform.position = playerPosition;
            }

            Debug.Log("Juego cargado. Puntuación: " + playerScore);
        }
        else
        {
            Debug.LogWarning("No se encontró un archivo de guardado.");
        }
    }
}
*/
using UnityEngine;
using System.IO;

[System.Serializable]
public class GameData
{
    public float playerPosX;
    public float playerPosY;
    public float playerPosZ;
    public int playerScore;
}

public class GameSaveLoad : MonoBehaviour
{
    private string filePath; // Ruta del archivo de guardado

    private void Start()
    {
        filePath = Application.persistentDataPath + "/savefile.json"; // Ruta donde se guarda el archivo JSON
    }

    // Guardar los datos en un archivo JSON
    public void SaveGame(Vector3 playerPosition, int playerScore)
    {
        GameData data = new GameData
        {
            playerPosX = playerPosition.x,
            playerPosY = playerPosition.y,
            playerPosZ = playerPosition.z,
            playerScore = playerScore
        };

        string json = JsonUtility.ToJson(data); // Convertir los datos a JSON
        File.WriteAllText(filePath, json); // Guardar el JSON en el archivo

        Debug.Log("Juego guardado.");
    }

    // Cargar los datos desde el archivo JSON
    public void LoadGame()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath); // Leer el archivo JSON
            GameData data = JsonUtility.FromJson<GameData>(json); // Convertir el JSON a los datos

            // Aplicar los datos al juego
            Vector3 playerPosition = new Vector3(data.playerPosX, data.playerPosY, data.playerPosZ);
            int playerScore = data.playerScore;

            // Ejemplo: aplicar al jugador (puedes adaptarlo a tu propia lógica)
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                player.transform.position = playerPosition;
            }

            Debug.Log("Juego cargado. Puntuación: " + playerScore);
        }
        else
        {
            Debug.LogWarning("No se encontró un archivo de guardado.");
        }
    }
}
