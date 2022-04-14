using UnityEngine;
using Pixelplacement;
public class Tile : MonoBehaviour
{
    //Tile behaviour


    private TileLayout parent;
    private GameManager gameManager;

    public GameObject Rock, Chest, End, Immutable;

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
        if (isChest) SpawnChest();
        if (isEnd) SpawnEnd();
        if(isImmutable) SpawnImmutable();
    }



    private void Update()
    {
        switch (type)
        {
            case Type.none: gameObject.GetComponent<MeshRenderer>().sharedMaterial.color = new Color(1, 1, 1, 0.25f); break;

            case Type.astronaut: gameObject.GetComponent<MeshRenderer>().sharedMaterial.color = Color.blue; break;

            case Type.cable: gameObject.GetComponent<MeshRenderer>().sharedMaterial.color = Color.cyan; break;

        }


        if (type == Type.cable && gameManager.HasCableStar == false)
            type = Type.none;
    }

    private void SpawnRock()
    {
        Rock = Instantiate(parent.Rock);
        Rock.SetActive(true);
        Rock.transform.parent = transform;
        Rock.transform.localPosition = parent.RockPos;
    }
    private void SpawnChest()
    {
        Chest = Instantiate(parent.Chest);
        Chest.SetActive(true);
        Chest.transform.parent = transform;
        Chest.transform.localPosition = parent.StarPos;
    }
    private void SpawnEnd()
    {
        End = Instantiate(parent.End);
        End.SetActive(true);
        End.transform.parent = transform;
        End.transform.localPosition = parent.EndPos;
    }
    private void SpawnImmutable()
    {
        Immutable = Instantiate(parent.Immutable);
        Immutable.SetActive(true);
        Immutable.transform.parent = transform;
        Immutable.transform.localPosition = parent.ImmutablePos;
    }

    public void MoveRock(int tileTo, float time)
    {
        Rock.transform.SetParent(TileLayout.tiles[tileTo]);
        //LeanTween.moveLocal(Rock, new Vector3(0, 13, 0), time);
        Tween.LocalPosition(Rock.transform, parent.RockPos, time, 0, Tween.EaseInOutStrong, Tween.LoopType.None, null, null, true);
        TileLayout.tiles[tileTo].GetComponent<Tile>().Rock = Rock;
        Rock = null;
    }
}
