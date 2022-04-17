using UnityEngine;

public class Cable : MonoBehaviour
{
    public Dir firstDir;
    public Dir lastDir;

    public enum Dir
    {
        right, left, up, down
    }


    private void SetCable()
    {
        switch (firstDir)
        {
            case Dir.right:
                break;

            case Dir.left:
                break;

            case Dir.up:
                break;

            case Dir.down:
                break;
        }
    }
}
