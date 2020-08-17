using UnityEngine;

public class RigGeneralFunction : MonoBehaviour
{
    [TooltipAttribute("Assign using inspector from Hierarchy")]
    public GameObject Parent;
    public bool relative;
    public bool PositionRetain;
    public float RelativePosition_x;
    public float RelativePosition_y;
    public float RelativePosition_z;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        if (PositionRetain)
        {
            PositionReset();
        }
    }

    //this method is used to reposition itself*/
    public void PositionReset()
    {
        if (!relative)
        {
            transform.position =
            Parent.transform.position +
            new Vector3(RelativePosition_x, RelativePosition_y, RelativePosition_z);
        }
        else
        {
            var parenTransform = Parent.transform;
            transform.position = parenTransform.position +
                                 parenTransform.forward * RelativePosition_z +
                                 parenTransform.up * RelativePosition_y +
                                 parenTransform.right * RelativePosition_x;
        }
    }
    
    public void DirectionReset()
    {
        transform.forward = Parent.transform.forward;
    }
}
