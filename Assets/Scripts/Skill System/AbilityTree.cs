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
        root.tree = this;
    }

    public AbilityTree(Ability a, AbilityTreeNode treeNode)
    {
        root = new AbilityTreeNode(a, treeNode);
        root.tree = this;
    }

    public void ChooseAbility()
    {
        if (!root.CheckRequisites())
            return;

        if (root.Ultimate)
            UltimateChosen = true;
        if (!root.unlocked || (root.Tiered && root.unlocked && !root.Maxed))
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

    public void Add(Ability a, Branch b, AbilityType subtree)
    {
        AbilityTreeNode node = root;
        AbilityTreeNode temp = null;

        switch (subtree)
        {
            case AbilityType.COMBAT:
                node = LeftBranch.root;
                break;
            case AbilityType.SPECIAL:
                node = MiddleBranch.root;
                break;
            case AbilityType.ENVRIONMENTAL:
                node = RightBranch.root;
                break;
        }

        while(node != null)
        {

            switch (b)
            {
                case Branch.LEFT:
                    temp = LeftBranch.root;
                    break;
                case Branch.MIDDLE:
                    temp = MiddleBranch.root;
                    break;
                case Branch.RIGHT:
                    temp = RightBranch.root;
                    break;
            }

            if (temp != null)
                node = temp;
            else
                break;
        }
        if (node != null)
            node.tree.Add(a, b);

    }

    public Ability GetAbility(Branch b, int depth, AbilityType subtree)
    {
        int d = root.TreeDepth;
        AbilityTreeNode node = root;

        switch (subtree)
        {
            case AbilityType.COMBAT:
                node = LeftBranch.root;
                break;
            case AbilityType.SPECIAL:
                node = MiddleBranch.root;
                break;
            case AbilityType.ENVRIONMENTAL:
                node = RightBranch.root;
                break;
        }
        if(node != null)
            d = node.TreeDepth;

        while (d != depth && node != null)
        {
            switch (b)
            {
                case Branch.LEFT:
                    node = LeftBranch.root;
                    break;
                case Branch.MIDDLE:
                    node = MiddleBranch.root;
                    break;
                case Branch.RIGHT:
                    node = RightBranch.root;
                    break;
            }

            d = node.TreeDepth;
        }
        return node.ability;
    }

    public void PrintTree()
    {
        AbilityTreeNode node = root;

        if(node != null)
        {
            Debug.Log(node.ability);
            if (LeftBranch != null) LeftBranch.PrintTree();
            if (MiddleBranch != null) MiddleBranch.PrintTree();
            if (RightBranch != null) RightBranch.PrintTree();
        }
    }

    public void PassUltimate()
    {
        AbilityTreeNode node = root;
        while(node != null && node.TreeDepth != 1)
        {
            node.tree.UltimateChosen = true;
            node = node.parent;
        }

        if (node != null)
            node.tree.UltimateChosen = true;
    }
	
}

public class AbilityTreeNode
{
    public Ability ability;
    public AbilityTree tree;
    public AbilityTreeNode parent;
    public bool unlocked = false;
    public bool Ultimate = false;
    public bool Tiered = false;
    public bool Maxed = true;
    public int TreeDepth;

    void Awake()
    {
        if (parent != null)
            TreeDepth = parent.TreeDepth + 1;
        else
            TreeDepth = 0;

        if (ability)
        {
            Ultimate = ability.Ultimate;
            Tiered = ability.Tiered;
        }

        Maxed = ability.Maxed;
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
        if (ability.Passive)
        {
            ability.UseAbility();
        }
        if (TreeDepth == 0)
            ability.Upgrade();

        Maxed = ability.Maxed;
        Select();
    }

    public void Select()
    {
        //Modify combat control of player by replacing designated ability
        if (ability.RequiresReplacement)
        {
            CombatControl cc = Ability.Player.GetComponent<CombatControl>();

            switch (ability.AbilityClassification)
            {
                case AbilityType.COMBAT:
                    break;
                case AbilityType.SPECIAL:
                    break;
                case AbilityType.ENVRIONMENTAL:
                    break;
            }
        }

        ability.Select();
        //Modify ultimate selection if applicable
        if (Ultimate)
            tree.PassUltimate();
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
