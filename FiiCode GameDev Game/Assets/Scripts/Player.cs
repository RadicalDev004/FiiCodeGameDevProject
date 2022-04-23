using Pixelplacement;
using System.Collections;
using UnityEngine;
public class Player : MonoBehaviour
{
    private GameManager gameManager;
    private AnimationManager animationManager;
    private GameObject Cam;

    public delegate void EndGame();
    public static event EndGame End;

    public ParticleSystem Fireworks;

    public int posX = 0, posY = 1;

    public float TimeToRotate = 0.25f;
    public float TimeToMove = 0.5f;

    public bool CanMove = true;

    public int CurrentTile { get { return (posX % 7) + posY * 7; } }

    public enum Directions { none = -1, up = 1, down = 3, right = 0, left = 2 }
    public Directions dir;

    private int Speed = 1;
    private void Awake()
    {

        Cam = Camera.main.gameObject;
        CanMove = true;
        gameManager = FindObjectOfType<GameManager>();
        animationManager = FindObjectOfType<AnimationManager>();
        dir = Directions.right;

        if (PlayerPrefs.GetInt("FastMode") == 1) Speed = 2;

        TimeToMove /= Speed;
        TimeToRotate /= Speed;
    }


    private void OnEnable()
    {
        DragHandler.OnMoveUp += Up;
        DragHandler.OnMoveDown += Down;
        DragHandler.OnMoveRight += Right;
        DragHandler.OnMoveLeft += Left;
    }

    private void OnDisable()
    {
        DragHandler.OnMoveUp -= Up;
        DragHandler.OnMoveDown -= Down;
        DragHandler.OnMoveRight -= Right;
        DragHandler.OnMoveLeft -= Left;
    }


    private void Up() => StartCoroutine(GenericMove(Directions.up));
    private void Down() => StartCoroutine(GenericMove(Directions.down));
    private void Right() => StartCoroutine(GenericMove(Directions.right));
    private void Left() => StartCoroutine(GenericMove(Directions.left));



    private IEnumerator GenericMove(Directions _dir)
    {
        //Player Move algorithm

        if (!CanMove) yield break;
        CanMove = false;

        Look(_dir, TimeToRotate);

        yield return new WaitForSeconds(TimeToRotate);
        dir = _dir;

        int prevTile = CurrentTile;
        int nextTile = 0;

        switch (_dir)
        {
            case Directions.up:
                if (posY == 0) { RecoverFromInadequateMove(); yield break; }
                nextTile = CurrentTile - 7;
                break;

            case Directions.down:
                if (posY == 3) { RecoverFromInadequateMove(); yield break; }
                nextTile = CurrentTile + 7;
                break;

            case Directions.right:
                if (posX == 6) { RecoverFromInadequateMove(); yield break; }
                nextTile = CurrentTile + 1;
                break;

            case Directions.left:
                if (posX == 0) { RecoverFromInadequateMove(); yield break; }
                nextTile = CurrentTile - 1;
                break;

            default:
                break;
        }
        int anim = 10;

        if (IsNextTileImmutable(nextTile))
        {
            RecoverFromInadequateMove();
            yield break;
        }

        if (IsNextTileRock(nextTile))
        {
            if (CanMoveRock(CurrentTile, nextTile, _dir))
            {
                MoveRock(CurrentTile, nextTile);
                anim = 11;
            }
            else { RecoverFromInadequateMove(); yield break; }
        }


        Move(nextTile, TimeToMove);


        SetAnimationState(anim);

        yield return new WaitForSeconds(TimeToMove);

        if (IsNextTileEnd(nextTile)) { Win(); End(); yield break; }

        if (IsNextTileStar(nextTile))
        {
            gameManager.HasChestStar = true;
            GetStarAnimation(nextTile);
        }

        SetAnimationState(0);

        SetSteps(prevTile, _dir);

        SetTilesAttributes(prevTile);
    }

    private void Look(Directions directions, float time)
    {
        float to = Mathf.Abs(360 - 90 * Mathf.Abs((float)directions));

        LeanTween.rotate(gameObject, new Vector3(0, to, 0), time);
    }
    private void Move(int tileTo, float time)
    {

        if (IsMoveOutOfBounds(tileTo)) { RecoverFromInadequateMove(); return; }

        CheckIfNextTileIsCable(tileTo);

        Tween.Position(transform, new Vector3(TileLayout.tiles[tileTo].position.x, transform.position.y, TileLayout.tiles[tileTo].position.z), time, 0, Tween.EaseInOut, Tween.LoopType.None, null, null, true);
        //LeanTween.move(gameObject, new Vector3(TileLayout.tiles[tileTo].position.x, transform.position.y, TileLayout.tiles[tileTo].position.z), time);

        posX = tileTo % 7;
        posY = tileTo / 7;

        AudioManager.Play("Steps");

        Invoke(nameof(CanMoveAgain), time);
    }

    private bool IsMoveOutOfBounds(int tileTo)
    {
        if (tileTo < 0 || tileTo > TileLayout.tiles.Length)
            return true;

        return false;
    }
    private void CanMoveAgain() { AudioManager.Stop("Steps"); CanMove = true; }

    private void SetTilesAttributes(int prevTile)
    {
        TileLayout.tiles[prevTile].GetComponent<Tile>().type = Tile.Type.cable;
        TileLayout.tiles[CurrentTile].GetComponent<Tile>().type = Tile.Type.astronaut;
    }

    private void CheckIfNextTileIsCable(int nextTile)
    {
        if (TileLayout.tiles[nextTile].GetComponent<Tile>().type == Tile.Type.cable)
        {
            gameManager.HasCableStar = false;
            AudioManager.Play("Error");
            ShakeCamera();
            if (PlayerPrefs.GetInt("Vibrations") == 1) Handheld.Vibrate();
        }

    }

    private bool IsNextTileEnd(int tileTo)
    {
        return TileLayout.tiles[tileTo].GetComponent<Tile>().isEnd;
    }
    private bool IsNextTileStar(int tileTo)
    {
        return TileLayout.tiles[tileTo].GetComponent<Tile>().isChest;

    }
    private bool IsNextTileImmutable(int nextTile)
    {
        return TileLayout.tiles[nextTile].GetComponent<Tile>().isImmutable;
    }
    private bool IsNextTileRock(int nextTile)
    {
        return TileLayout.tiles[nextTile].GetComponent<Tile>().isRock;
    }


    private bool CanMoveRock(int currentTile, int nextTile, Directions dir)
    {
        int tileToMoveRockTo = nextTile + nextTile - currentTile;

        int rockX = nextTile % 7;
        int rockY = nextTile / 7;

        if (IsMoveOutOfBounds(tileToMoveRockTo)) return false;

        if (dir == Directions.up && rockY == 0) return false;
        if (dir == Directions.down && rockY == 6) return false;
        if (dir == Directions.left && rockX == 0) return false;
        if (dir == Directions.right && rockX == 6) return false;

        if (TileLayout.tiles[tileToMoveRockTo].GetComponent<Tile>().isEnd == true) return false;
        if (TileLayout.tiles[tileToMoveRockTo].GetComponent<Tile>().isChest == true) return false;
        if (TileLayout.tiles[tileToMoveRockTo].GetComponent<Tile>().isRock == true) return false;
        if (TileLayout.tiles[tileToMoveRockTo].GetComponent<Tile>().isImmutable == true) return false;

        return true;
    }
    private void MoveRock(int currentTile, int nextTile)
    {
        int nextnextTile = nextTile + nextTile - currentTile;

        TileLayout.tiles[nextTile].GetComponent<Tile>().MoveRock(nextnextTile, TimeToMove);

        TileLayout.tiles[nextTile].GetComponent<Tile>().isRock = false;
        TileLayout.tiles[nextnextTile].GetComponent<Tile>().isRock = true;

    }

    private void SetAnimationState(int state) => animationManager.State = state;

    private void GetStarAnimation(int tileAt)
    {
        TileLayout.tiles[tileAt].GetComponentInChildren<Star>().GetCollected(1);
        TileLayout.tiles[tileAt].GetComponent<Tile>().isChest = false;
        AudioManager.Play("Star");
        Debug.Log(AudioManager.IsPlaying("Star"));
    }

    public void ShakeCamera()
    {
        Tween.Shake(Cam.transform, Cam.transform.position, new Vector3(0.15f, 0.15f, 0.15f), 0.15f, 0);
    }

    private void RecoverFromInadequateMove()
    {
        AudioManager.Play("Error");
        ShakeCamera();
        if (PlayerPrefs.GetInt("Vibrations") == 1) Handheld.Vibrate();
        CanMoveAgain();
    }

    private void SetSteps(int tile, Directions dir)
    {
        if (!gameManager.HasCableStar) return;

        TileLayout.tiles[tile].GetComponent<Tile>().SpawnSteps(90 + (float)dir * 90);
    }
    private void Win()
    {
        Fireworks.transform.position = transform.position; Fireworks.Play(); 
        AudioManager.Play("Win"); 
        Look(Directions.down, 0.5f);
    }

}
