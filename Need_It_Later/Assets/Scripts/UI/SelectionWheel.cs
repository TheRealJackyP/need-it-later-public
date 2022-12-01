using UnityEngine;

namespace UI
{
    public class SelectionWheel : MonoBehaviour
    {


        // Update is called once per frame
        /*void Update()
        {
            
            /*var position = transform.position;
            var ang = Mathf.Atan2(Input.mousePosition.y - position.y, Input.mousePosition.x - position.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(ang, Vector3.forward);
            Debug.Log(ang);#1#
        
            /*if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Mouse is down");
                //Ray2D ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up);#1#
                //if (Physics2D.Raycast(ray, out var hit))
                /*{
                    Debug.Log(hit);
                    //Debug.Log(hit.collider.gameObject.GetComponent<MeshRenderer>());
                    Debug.Log(hit.collider.gameObject.name);
                    hit.collider.gameObject.GetComponent<MeshRenderer>().materials[0].color = Color.red;
                    if (hit.collider.gameObject.GetComponent<MeshRenderer>().materials[0]) //.mesh.triangles[0].ToString() == "Wheel")
                    {
                        Debug.Log("Hit");
                    }
                }#1#
                /*else
                {
                    Debug.Log(ray);
                }#1#
            }
        }*/
    }
}
