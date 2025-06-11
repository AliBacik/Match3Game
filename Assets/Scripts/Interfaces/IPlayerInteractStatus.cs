using System.Collections;
using UnityEngine;

public interface IPlayerInteractStatus
{
    void PlayerStatusSwitch(bool active);
    void TriggerScore(int count);
    void TriggerPostBehaviours();
}
