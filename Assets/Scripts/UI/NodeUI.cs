using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;
using UnityEngine;

public class NodeUI : MonoBehaviour
{
    public Image image;
    public bool greyed;
    public AbilityTreeNode treeNode;

    public Button button;

    void Start()
    {
        if (treeNode == null) return;
        GrayOut();
        button = gameObject.GetComponent<Button>();
    }

    void GrayOut()
    {
        if (!treeNode.unlocked)
            image.color = Color.gray;
        else
            image.color = Color.white;
    }

    void FillData()
    {
        button.image = image;
        //Display the description of the ability
    }
    
    /// <summary>
    /// Use for OnClick method of button, selects the ability within the tree
    /// </summary>
    public void Click()
    {
        treeNode.tree.ChooseAbility();
    }
}
