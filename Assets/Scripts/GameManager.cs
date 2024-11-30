using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public Transform player;

    // Prefab do inimigo a ser spawnado
    public GameObject enemyPrefab;
    public GameObject[] BoomerangPrefabs;

    // Intervalo em segundos entre os spawns
    public float spawnIntervalEnemy = 2f;
    public float spawnIntervalBoomerang = 15f;
    // Distância para spawnar fora da tela (opcional)
    public float spawnOffset = 1f;
    private float minX, maxX, minY, maxY;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Calcular os limites da câmera principal
        Camera mainCamera = Camera.main;
        minX = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
        maxX = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;
        minY = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y;
        maxY = mainCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y;

        // Iniciar o spawn periódico
        InvokeRepeating(nameof(SpawnEnemy), 0f, spawnIntervalEnemy);
        InvokeRepeating(nameof(SpawnBoomerang), 0f, spawnIntervalBoomerang);
    }

    void SpawnEnemy()
    {
        // Escolher uma borda aleatória
        int edge = Random.Range(0, 4); // 0: Esquerda, 1: Direita, 2: Topo, 3: Base
        Vector3 spawnPosition = Vector3.zero;

        switch (edge)
        {
            case 0: // Esquerda
                spawnPosition = new Vector3(minX - spawnOffset, Random.Range(minY, maxY), 0);
                break;
            case 1: // Direita
                spawnPosition = new Vector3(maxX + spawnOffset, Random.Range(minY, maxY), 0);
                break;
            case 2: // Topo
                spawnPosition = new Vector3(Random.Range(minX, maxX), maxY + spawnOffset, 0);
                break;
            case 3: // Base
                spawnPosition = new Vector3(Random.Range(minX, maxX), minY - spawnOffset, 0);
                break;
        }

        if (GameObject.FindGameObjectsWithTag("Player").Length >= 1)
        {
            // Instanciar o inimigo
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }
    }

    void SpawnBoomerang()
    {
        // Gerar uma posição aleatória dentro da tela
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);

        Vector3 spawnPosition = new Vector3(randomX, randomY, 0);

        if (GameObject.FindGameObjectsWithTag("Boomerang").Length < 4)
        {
            GameObject selectedBoomerang = BoomerangPrefabs[Random.Range(0, BoomerangPrefabs.Length)];
            Instantiate(selectedBoomerang, spawnPosition, Quaternion.identity);
        }
    }
}
