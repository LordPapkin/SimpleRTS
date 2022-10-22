using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HQ : Building
{
    protected override void HealthSystem_OnDied(object sender, EventArgs e)
    {
        GameOverUI.Instance.Show();
        base.HealthSystem_OnDied(sender, e);
    }
}
