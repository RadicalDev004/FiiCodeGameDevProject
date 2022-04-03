using System.Collections;
using UnityEngine;
using Pixelplacement;
public class Player : MonoBehaviour
{
    GameManager gameManager;

    public delegate void EndGame();
    public static event EndGame End;

    public int posX = 0, posY = 1;

    public float TimeToRotate = 0.25f;
    public float TimeToMove = 0.5f;

    public bool CanMove = true;

    public int CurrentTile { get { return (posX % 7) + posY * 7; } }

    public enum Directions { none = -1, up = 1, down = 3, right = 0, left = 2 }
    public Directions dir;


    private void Awake()
    {
        CanMove = true;
        gameManager = FindObjectOfType<GameManager>();
        dir = Directions.right;
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
                if (posY == 0) { Handheld.Vibrate(); yield break; }
                nextTile = CurrentTile - 7;
                break;

            case Directions.down:
                if (posY == 3) { Handheld.Vibrate(); yield break; }
                nextTile = CurrentTile + 7;
                break;

            case Directions.right:
                if (posX == 6) { Handheld.Vibrate(); yield break; }
                nextTile = CurrentTile + 1;
                break;

            case Directions.left:
                if (posX == 0) { Handheld.Vibrate(); yield break; }
                nextTile = CurrentTile - 1;
                break;

            default:
                break;
        }

        if (IsNextTileRock(nextTile))
        {
            if (CanMoveRock(CurrentTile, nextTile, _dir))
                MoveRock(CurrentTile, nextTile);
            else { CanMoveAgain(); Handheld.Vibrate(); yield break; }
        }

        if (IsNextTileImmutable(nextTile))
        {
            CanMoveAgain(); Handheld.Vibrate();
            yield break;
        }

        Move(nextTile, TimeToMove);

        yield return new WaitForSeconds(TimeToMove);

        if (IsNextTileEnd(nextTile)) { End(); yield break; }

        if (IsNextTileChest(nextTile))
        {
            Debug.LogWarning("TOUCHED Chest");
            gameManager.HasChestStar = true;

            //Chest Opening Code Coming Soon
            OpenChestAnimation();
        }

        SetTilesAttributes(prevTile);
    }

    private void Look(Directions directions, float time)
    {
        float to = Mathf.Abs(360 - 90 * Mathf.Abs((float)directions));

        LeanTween.rotate(gameObject, new Vector3(0, to, 0), time);
    }
    private void Move(int tileTo, float time)
    {

        if (IsMoveOutOfBounds(tileTo)) { Handheld.Vibrate(); return; }

        CheckIfNextTileIsCable(tileTo);

        Tween.Position(transform, new Vector3(TileLayout.tiles[tileTo].position.x, transform.position.y, TileLayout.tiles[tileTo].position.z), time, 0, Tween.EaseInOutStrong);
        //LeanTween.move(gameObject, new Vector3(TileLayout.tiles[tileTo].position.x, transform.position.y, TileLayout.tiles[tileTo].position.z), time);

        posX = tileTo % 7;
        posY = tileTo / 7;

        Invoke(nameof(CanMoveAgain), time);
    }

    private bool IsMoveOutOfBounds(int tileTo)
    {
        if (tileTo < 0 || tileTo > TileLayout.tiles.Length)
            return true;

        return false;
    }
    private void CanMoveAgain() => CanMove = true;

    private void SetTilesAttributes(int prevTile)
    {
        TileLayout.tiles[prevTile].GetComponent<Tile>().type = Tile.Type.cable;
        TileLayout.tiles[CurrentTile].GetComponent<Tile>().type = Tile.Type.astronaut;
    }

    private void CheckIfNextTileIsCable(int nextTile)
    {
        if (TileLayout.tiles[nextTile].GetComponent<Tile>().type == Tile.Type.cable)
        {
            Debug.LogWarning("TOUCHED Cable");
            gameManager.HasCableStar = false;
        }

    }

    private bool IsNextTileEnd(int tileTo)
    {
        return TileLayout.tiles[tileTo].GetComponent<Tile>().isEnd;
    }
    private bool IsNextTileChest(int tileTo)
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

    private void OpenChestAnimation()
    {

    }
}
