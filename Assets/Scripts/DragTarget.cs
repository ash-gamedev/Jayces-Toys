using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class DragTarget : MonoBehaviour
    {
        [SerializeField] public float targetRadius;

        void OnDrawGizmosSelected()
        {
            // Draw a yellow sphere at the transform's position
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, targetRadius);
        }

    }
}