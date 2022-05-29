using UnityEngine;

public class TileEditor : MonoBehaviour
{
    //Gives Easy acces to editing tiles in the Editor (for creating levels)


    private Tile tile;
    void Start()
    {
        tile = transform.GetComponent<Tile>();
    }

    void Update()
    {
        if (TileEditorMaker.SelectedTile == transform.GetSiblingIndex())
            return;

        if (tile.isEnd)
            gameObject.GetComponent<MeshRenderer>().material.color = new Color32(0, 75, 255, 150);

        else if (tile.isChest)
            gameObject.GetComponent<MeshRenderer>().material.color = new Color32(255, 175, 0, 150);

        else if (tile.isRock)
            gameObject.GetComponent<MeshRenderer>().material.color = new Color32(0, 0, 0, 150);

        else if (tile.isImmutable)
            gameObject.GetComponent<MeshRenderer>().material.color = new Color32(255, 0, 0, 150);
        else if (tile.type == Tile.Type.none)
        {
            gameObject.GetComponent<MeshRenderer>().sharedMaterial.color = new Color(1, 1, 1, 0.25f);
        }
    }
}
