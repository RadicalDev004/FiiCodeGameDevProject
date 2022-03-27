using UnityEngine;

public class Tile : MonoBehaviour
{
    private TileLayout parent;
    private GameManager gameManager;

    public GameObject Rock, Chest, End;

    public int number;
    public Type type;

    public bool isEnd = false;
    public bool isChest = false;
    public bool isRock = false;
    public bool isImmutable = false;
    public enum Type { none, astronaut, cable };

    private void Awake()
    {
        parent = transform.parent.GetComponent<TileLayout>();
        gameManager = FindObjectOfType<GameManager>();

        if (isRock) SpawnRock();
    }

    private void Update()
    {
        switch (type)
        {
            case Type.none: gameObject.GetComponent<MeshRenderer>().sharedMaterial.color = Color.white; break;

            case Type.astronaut: gameObject.GetComponent<MeshRenderer>().sharedMaterial.color = Color.blue; break;

            case Type.cable: gameObject.GetComponent<MeshRenderer>().sharedMaterial.color = Color.cyan; break;

        }

        if (isEnd)
            gameObject.GetComponent<MeshRenderer>().sharedMaterial.color = Color.black;

        if (isChest)
            gameObject.GetComponent<MeshRenderer>().sharedMaterial.color = Color.yellow;

        if (isRock)
            gameObject.GetComponent<MeshRenderer>().sharedMaterial.color = Color.gray;

        if (isImmutable)
            gameObject.GetComponent<MeshRenderer>().sharedMaterial.color = Color.red;

        if (gameManager.HasChestStar && isChest)
            isChest = false;


        if (type == Type.cable && gameManager.HasCableStar == false)
            type = Type.none;
    }

    private void SpawnRock()
    {
        Rock = Instantiate(parent.Rock);
        Rock.SetActive(true);
        Rock.transform.parent = transform;
        Rock.transform.localPosition = new Vector3(0, 13, 0);
    }

    public void MoveRock(int tileTo, float time)
    {
        Rock.transform.SetParent(TileLayout.tiles[tileTo]);
        LeanTween.moveLocal(Rock, new Vector3(0, 13, 0), time);
        TileLayout.tiles[tileTo].GetComponent<Tile>().Rock = Rock;
        Rock = null;
    }
}
