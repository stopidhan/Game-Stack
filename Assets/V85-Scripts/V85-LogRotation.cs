using System.Collections;
using UnityEngine;

public class LogRotation : MonoBehaviour
{
    [System.Serializable]
    private class RotationElement
    {
        public float Speed;
        public float Duration;
    }

    [SerializeField]
    private RotationElement[] rotationPattern;

    private WheelJoint2D wheelJoint;
    private JointMotor2D motor;

    private void Awake()
    {
        wheelJoint = GetComponent<WheelJoint2D>();
        motor = new JointMotor2D();
        StartCoroutine(PlayRotationPattern());
    }

    private IEnumerator PlayRotationPattern()
    {
        int rotationIndex = 0;
        while (true)
        {
            motor.motorSpeed = rotationPattern[rotationIndex].Speed;
            motor.maxMotorTorque = 10000;
            wheelJoint.motor = motor;

            yield return new WaitForSeconds(rotationPattern[rotationIndex].Duration);
            
            rotationIndex++;
            if (rotationIndex >= rotationPattern.Length)
            {
                rotationIndex = 0;
            }
        }
    }
}