using System;
using Godot;

public class PopupContainer : Container
{
    [Export] public NodePath pText;
    private Label text;

    public override void _Ready()
    {
        base._Ready();
        text = GetNode<Label>(pText);
    }

    public void Open(string _text)
    {
        Visible = true;
        text.Text = _text;
    }

    public void Close()
    {
        Visible = false;
        text.Text = "";
    }
}
