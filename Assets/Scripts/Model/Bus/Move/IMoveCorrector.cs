using UnityEngine;

public interface IMoveCorrector
{
    bool IsFilled { get; }

    void EnableMovement();
    void EnableForwardMovement();
    void DisableForwardMovement();
    void SetTarget(Vector3 target, bool canLookAtTarget = true);
    void DisableMovement();
    void ResetTarget();
}
