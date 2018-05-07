using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AbilityTree{
    public static GameObject Player;
    public AbilityTreeNode root;
    public AbilityTree LeftBranch;
    public AbilityTree MiddleBranch;
    public AbilityTree RightBranch;
    public bool UltimateChosen = false;

    public AbilityTree()
    {
        root = null;
    }

    public AbilityTree(Ability a)
    {
        root = new AbilityTreeNode(a);
    }

    public AbilityTree(Ability a, AbilityTreeNode treeNode)
    {
        root = new AbilityTreeNode(a, treeNode);
    }

    public void ChooseAbility()
    {
        if (!root.CheckRequisites())
            return;

        if (root.Ultimate)
            UltimateChosen = true;
        if (!root.unlocked)
            root.Unlock();
        else
            root.Select();
    }

    public void AddRoot(Ability a)
    {
        root = new AbilityTreeNode(a);
    }

    private void AddLeft(Ability a)
    {
        LeftBranch = new AbilityTree(a, root);
    }

    private void AddRight(Ability a)
    {
        RightBranch = new AbilityTree(a, root);
    }

    private void AddMiddle(Ability a)
    {
        MiddleBranch = new AbilityTree(a, root);
    }

    public void Add(Ability a, Branch b)
    {
        switch (b)
        {
            case Branch.LEFT:
                AddLeft(a);
                break;
            case Branch.MIDDLE:
                AddMiddle(a);
                break;
            case Branch.RIGHT:
                AddRight(a);
                break;
        }
    }
	
}

public class AbilityTreeNode
{
    public Ability ability;
    public AbilityTreeNode parent;
    public bool unlocked = false;
    public bool Ultimate = false;
    public int TreeDepth;

    void Awake()
    {
        if (parent != null)
            TreeDepth = parent.TreeDepth + 1;
        else
            TreeDepth = 0;

        if (ability)
            Ultimate = ability.Ultimate;
    }

    public AbilityTreeNode(Ability a)
    {
        ability = a;
        parent = null;
        Awake();
    }

    public AbilityTreeNode(Ability a, AbilityTreeNode treeNode)
    {
        ability = a;
        parent = treeNode;
        Awake();
    }

    public void Unlock()
    {
        unlocked = true;
        //Apply ability upgrades
        Select();
    }

    public void Select()
    {
        //Modify combat control of player by replacing designated ability
        //Modify ultimate selection if applicable
    }
    public bool CheckRequisites()
    {
        return parent.unlocked;
    }
}

public enum Branch
{
    LEFT, MIDDLE, RIGHT
}
