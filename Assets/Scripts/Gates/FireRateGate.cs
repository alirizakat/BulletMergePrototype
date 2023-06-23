using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRateGate : BaseGate
{
    public override void IncreaseHealthPoints(int damageAmount)
    {
        // Call the base implementation to increase health points
        base.IncreaseHealthPoints(damageAmount);
    }

    protected override void Start()
    {
        // Call the base implementation of Start method
        base.Start();

        // Add any additional logic specific to the FireRateGate here
    }
}
