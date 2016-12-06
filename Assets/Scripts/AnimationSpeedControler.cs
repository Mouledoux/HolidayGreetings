using UnityEngine;
using System.Collections;

public class AnimationSpeedControler : MonoBehaviour
{
    private Animation anim;
    void Start()
    {
        anim = GetComponent<Animation>();
    }

    public void SetAnimationSpeed(float speed)
    {
        anim["Sled3.0"].speed = speed;
    }
}
