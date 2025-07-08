using UnityEngine;

public class NearestMonsterSelector : MonoBehaviour, ITargetSelector
{
    public Transform SelectTarget(Vector3 origin, float range)
    {
        Monster nearest = null;
        float minDist = float.MaxValue;
        foreach (var monster in FindObjectsOfType<Monster>())
        {
            if (!monster.gameObject.activeInHierarchy) continue;
            float dist = Vector3.Distance(origin, monster.transform.position);
            if (dist <= range && dist < minDist)
            {
                minDist = dist;
                nearest = monster;
            }
        }
        return nearest?.transform;
    }
}
