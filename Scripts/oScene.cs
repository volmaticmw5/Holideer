using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;

public class oScene : Node
{
    public override void _Ready()
    {
        base._Ready();
#if DEBUG
        GameManager.Instance.sceneInitialized = true;
#endif
        if (!GameManager.Instance.sceneInitialized)
            GameManager.InitScene(this);
    }
}