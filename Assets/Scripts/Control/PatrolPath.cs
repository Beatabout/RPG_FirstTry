using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        private void OnDrawGizmos() {
            Gizmos.color = Color.blue;
            for(int i = 0; i < transform.childCount; i++)
            {
                Gizmos.DrawSphere(GetWaypointPosition(i), 0.2f);
                int j = GetNextIndex(i);
                
                Gizmos.DrawLine(GetWaypointPosition(i), GetWaypointPosition(j));
            }
        }

        public Vector3 GetWaypointPosition(int i)
        {
            return transform.GetChild(i).position;
        }

        public int GetNextIndex(int i){
            if(i+1 == transform.childCount)
            {
                return 0;
            }
            return i+1;
        }
    }
}

