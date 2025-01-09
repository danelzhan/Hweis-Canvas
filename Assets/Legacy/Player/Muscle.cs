using UnityEngine;

[System.Serializable]

public class Muscle {

    public Rigidbody2D bone;
    public float restRotation;
    public float force;

    public void activateMuscle() {
        bone.MoveRotation(Mathf.LerpAngle(bone.rotation, restRotation, force * Time.deltaTime));
    }

}