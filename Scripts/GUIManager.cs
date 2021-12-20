using System;
using Godot;

public class GUIManager : Control
{
    [Export] public NodePath pDialogContainer;
    public Dialog oDialog;
    [Export] public NodePath pPopupContainer;
    public static PopupContainer PopupContainer;

    public override void _Ready()
    {
        base._Ready();
        PopupContainer = GetNode<PopupContainer>(pPopupContainer);
        PopupContainer.Visible = false;
        oDialog = GetNode<Dialog>(pDialogContainer);
        oDialog.Visible = false;
    }
}
