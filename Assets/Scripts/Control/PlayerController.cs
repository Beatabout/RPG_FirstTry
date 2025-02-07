using RPG.Combat;
using RPG.Movement;
using UnityEngine;
using RPG.Attributes;
using System;
using UnityEngine.EventSystems;
using UnityEngine.AI;

namespace RPG.Control 
{
    public class PlayerController : MonoBehaviour
    {
        Mover mover;
        Health health;
        [SerializeField] CursorMapping[] cursorMappings;
        [SerializeField] float maxNavMeshProjectionDistance = 1f;
        [SerializeField] float maxNavPathLenght = 40f;

        [System.Serializable]
        struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
        }

        private void Awake() {
            mover = GetComponent<Mover>();
            health = GetComponent<Health>();
        }

        private void Update()
        {
            if(InteractWithUI()) return;
            if(health.IsDead())
            {
                SetCursor(CursorType.None);
                return;
            } 
            if(InteractWithComponent()) return;
            if(InteractWithMovement()) return;

            SetCursor(CursorType.None);
        }

        private bool InteractWithUI()
        {
            if(EventSystem.current.IsPointerOverGameObject())
            {
                SetCursor(CursorType.UI);
                return true;
            }
            return false;
        }

        private bool InteractWithComponent()
        {
            RaycastHit[] hits = ReycastAllSorted();

            foreach (RaycastHit hit in hits)
            {
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();

                foreach (IRaycastable raycastable in raycastables)
                {
                    if(raycastable.HandleRaycast(this))
                    {
                        SetCursor(raycastable.GetCursorType());
                        return true;
                    }
                }
            }
            return false;
        }

        RaycastHit[] ReycastAllSorted(){
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            float[] distances = new float[hits.Length];
            for (int i = 0; i < hits.Length; i++)
            {
                distances[i] = hits[i].distance;
            }
            Array.Sort(distances, hits);
            return hits;
        }

        private bool InteractWithMovement()
        {
            Vector3 target;
            bool hashit = RaycastNavMesh(out target);
            if (hashit)
            {
                if (Input.GetMouseButton(0))
                {
                    mover.StartMoveAction(target, 1f);
                }
                SetCursor(CursorType.Movement);
                return true;
            }
            return false;
            // Debug.DrawRay(ray.origin, ray.direction * 100);
        }

        private bool RaycastNavMesh(out Vector3 target)
        {
            target = new Vector3();

            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if(!hasHit) return false;
            
            NavMeshHit navMeshHit;
            bool hasCastToNavmesh = NavMesh.SamplePosition(hit.point, out navMeshHit, maxNavMeshProjectionDistance, NavMesh.AllAreas);
            if(!hasCastToNavmesh) return false;

            target = navMeshHit.position;

            NavMeshPath path = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(transform.position, target, NavMesh.AllAreas, path);
            if(!hasPath) return false;
            if(path.status != NavMeshPathStatus.PathComplete) return false;
            if(GetPathLength(path) > maxNavPathLenght) return false;

            return true;
        }

        private float GetPathLength(NavMeshPath path)
        {
            float distance = 0;
            if(path.corners.Length < 2) return distance;

            for(int i = 0; i < path.corners.Length - 1; i++){
                distance += Vector3.Distance(path.corners[i], path.corners[i+1]);
            }
            return distance;
        }

        private void SetCursor(CursorType type)
        {
            CursorMapping mapping = GetCursorMapping(type);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(CursorType type)
        {
            foreach (CursorMapping mapping in cursorMappings)
            {
                if(mapping.type == type){
                    return mapping;
                }
            }
            return cursorMappings[0];
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}
