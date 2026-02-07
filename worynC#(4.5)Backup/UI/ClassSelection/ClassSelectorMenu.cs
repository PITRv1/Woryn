using Godot;
using System;
using System.Collections.Generic;

public partial class ClassSelectorMenu : Control
{
	[Export] RichTextLabel class1Description;
	[Export] RichTextLabel class2Description;
	[Export] RichTextLabel class3Description;
	[Export] RichTextLabel class4Description;

	[Export] TextureRect class1Icon;
	[Export] TextureRect class2Icon;
	[Export] TextureRect class3Icon;
	[Export] TextureRect class4Icon;

    [Export] Button class1;
    [Export] Button class2;
    [Export] Button class3;
    [Export] Button class4;


    [Export] HBoxContainer container;
    [Export] Label title;

    [Export] Button nextButton;
    [Export] Control deckselectNode;
    [Export] Control classselectNode;
    

    List<Button> classes = new List<Button>();

    Button selectedClass;
    bool selected = false;
	public override void _Ready()
	{
        // classselectNode = GetNode<Control>("Class");
        // deckselectNode = GetNode<Control>("Deck");

        classes.Add(class1);
        classes.Add(class2);
        classes.Add(class3);
        classes.Add(class4);


        class1Description.Visible = false;
        class2Description.Visible = false;
        class3Description.Visible = false;
        class4Description.Visible = false;
	}

	public override void _Process(double delta)
	{
        if (selected)
        {
            ClassSelected(selectedClass);
            selected = false;
        }

	}
    private void ClassSelected(Button selectedClass)
    {
        foreach(Button _class in classes)
        {
            if (selectedClass != _class)
            {
                _class.QueueFree();
                nextButton.Visible = true;
            }
        }
        selectedClass.Disabled = true;
        title.Text = "CLASS SELECTED";

        
        //container.AddChild(selectedClass);
    }

    private void OnNextButtonPressed()
    {
        // GetTree().ChangeSceneToPacked(deckselectScene);
        classselectNode.Visible = false;
        deckselectNode.Visible = true;
    }

    //Class selection
    private void OnClass1Pressed()
    {
        selectedClass = class1;
        selected = true;
    }
    private void OnClass2Pressed()
    {
        selectedClass = class2;
        selected = true;
    }
    private void OnClass3Pressed()
    {
        selectedClass = class3;
        selected = true;
    }
    private void OnClass4Pressed()
    {
        selectedClass = class4;
        selected = true;
    }

    //Show description
    
    //Class1
    private void Class1MouseEntered()
    {
        class1Description.Visible = true;
        class1Icon.Visible = false;
    }

    private void Class1MouseExited()
    {
        class1Description.Visible = false;
        class1Icon.Visible = true;
    }

    //Class2
    private void Class2MouseEntered()
    {
        class2Description.Visible = true;
        class2Icon.Visible = false;
    }

    private void Class2MouseExited()
    {
        class2Description.Visible = false;
        class2Icon.Visible = true;
    }

    //Class3
    private void Class3MouseEntered()
    {
        class3Description.Visible = true;
        class3Icon.Visible = false;
    }

    private void Class3MouseExited()
    {
        class3Description.Visible = false;
        class3Icon.Visible = true;
    }

    //Class4
    private void Class4MouseEntered()
    {
        class4Description.Visible = true;
        class4Icon.Visible = false;
    }

    private void Class4MouseExited()
    {
        class4Description.Visible = false;
        class4Icon.Visible = true;
    }
}
