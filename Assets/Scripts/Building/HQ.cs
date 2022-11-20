using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HQ : Building
{
    protected override void OnDied(object sender, EventArgs e)
    {       
        base.OnDied(sender, e);
        GameOverUI.Instance.Show();
    }
}
