using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityTreeUI : MonoBehaviour {

    public static AbilityTree tree;
    public static GameObject NodePrefab;

    public GameObject CurrentSelection;
    public bool created = false;

	// Use this for initialization
	void Start () {
        if (tree == null)
            return;
        CreateTreeNodes();
	}
	
	// Update is called once per frame
	void Update () {
        if (tree == null) return;
        if (!created)
            CreateTreeNodes();

		//Controls within tree/UI

            //Update currentSelection

	}

    void CreateTreeNodes()
    {
        GameObject g;
        AbilityTreeNode node = tree.GetRoot();
        Queue<AbilityTreeNode> queue = new Queue<AbilityTreeNode>();
        queue.Enqueue(node);
        node = queue.Dequeue();

        //Foreach node, create a button
        while (node != null)
        {
            //Create UI Things
            g = Instantiate(NodePrefab);
            g.GetComponent<NodeUI>().treeNode = node;

            //Attach new UI Thing to this script

            foreach (AbilityTreeNode n in node.tree.GetChildren())
                queue.Enqueue(n);
            node = queue.Dequeue();
        }

        //Attach visually to the parent

        created = true;

    }
    
}
