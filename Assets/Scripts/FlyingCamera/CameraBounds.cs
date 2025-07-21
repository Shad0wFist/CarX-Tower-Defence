using UnityEngine;

[ExecuteAlways]
public class CameraBounds : MonoBehaviour
{
    public Vector3 boundsCenter = Vector3.zero;
    public Vector3 boundsSize = new Vector3(50, 20, 50);

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(boundsCenter, boundsSize);
    }

    public Vector3 ClampToBounds(Vector3 position)
    {
        Vector3 half = boundsSize * 0.5f;
        return new Vector3(
            Mathf.Clamp(position.x, boundsCenter.x - half.x, boundsCenter.x + half.x),
            Mathf.Clamp(position.y, boundsCenter.y - half.y, boundsCenter.y + half.y),
            Mathf.Clamp(position.z, boundsCenter.z - half.z, boundsCenter.z + half.z)
        );
    }
}
