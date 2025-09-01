using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

public class FeelRotation : MonoBehaviour
{

    public MMF_Player rotationPlayer;

    public void FeedbackTriggered()
    {
        rotationPlayer?.PlayFeedbacks();
    }
}
