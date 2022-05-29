using UnityEngine;
using Pixelplacement;
public class Tile : MonoBehaviour
{
    //Tile behaviour


    private TileLayout parent;
    private GameManager gameManager;

    [System.NonSerialized] public GameObject Rock, Chest, End, Immutable, Steps;

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
        if (type == Type.cable && gameManager.HasCableStar == false) { Destroy(Steps); type = Type.none; }
    }

    public void SpawnNothing()
    {
        Destroy(Rock);
        Rock = null;

        Destroy(Chest);
        Chest = null;

        Destroy(End);
        End = null;

        Destroy(Immutable);
        Immutable = null;
    }

    public void SpawnRock()
    {
        Rock = Instantiate(parent.Rock);
        Rock.SetActive(true);
        Rock.transform.parent = transform;
        Rock.transform.localPosition = parent.RockPos;
    }
    public void SpawnChest()
    {
        Chest = Instantiate(parent.Chest);
        Chest.SetActive(true);
        Chest.transform.parent = transform;
        Chest.transform.localPosition = parent.StarPos;
    }
    public void SpawnEnd()
    {
        End = Instantiate(parent.End);
        End.SetActive(true);
        End.transform.parent = transform;
        End.transform.localPosition = parent.EndPos;
    }
    public void SpawnImmutable()
    {
        Immutable = Instantiate(parent.Immutable);
        Immutable.SetActive(true);
        Immutable.transform.parent = transform;
        Immutable.transform.localPosition = parent.ImmutablePos;
    }
    public void SpawnSteps(float angle)
    {
        Steps = Instantiate(parent.Steps);
        Steps.SetActive(true);
        Steps.transform.parent = transform;
        Steps.transform.localPosition = new Vector3(0, 0.5f, 0);
        Steps.transform.rotation = Quaternion.Euler(90, 0, angle);
    }



    public void MoveRock(int tileTo, float time)
    {
        AudioManager.Play("RockMove");
        Rock.transform.SetParent(TileLayout.tiles[tileTo]);
        //LeanTween.moveLocal(Rock, new Vector3(0, 13, 0), time);
        Tween.LocalPosition(Rock.transform, parent.RockPos, time, 0, Tween.EaseInOutStrong, Tween.LoopType.None, null, null, true);
        TileLayout.tiles[tileTo].GetComponent<Tile>().Rock = Rock;
        Rock = null;
        Invoke(nameof(StopRockSound), time);
    }
    private void StopRockSound() => AudioManager.Stop("RockMove");
}
