using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GUIHandler : MonoBehaviour {

	public static GUIHandler Instance = null;

	public GameObject MenuProperty;
	public GameObject SelectionProperty;
	public TransferMenu MTransferMenu;

	[TextArea(1,10)]
	public string textMessage = "";

	public Slider P1HealthBar;
	public GameObject CurrentTarget;
	public TextMeshProUGUI ExpText;

	private List<GameObject> PropertyLists;

    private Ability abilityToUpdate;

	void Awake () {
		if (Instance == null)
			Instance = this;
		else if (Instance != this) {
			Destroy (gameObject);
		}
		PropertyLists = new List<GameObject> ();
	}
	void Update() {
		if (CurrentTarget != null) {
			var P1Controller = CurrentTarget.GetComponent<Attackable> ();

			P1HealthBar.value = P1Controller.Health;
			var exp = CurrentTarget.GetComponent<ExperienceHolder> ();
			ExpText.text = "Data: " + exp.VisualExperience;
		}
	}
		
	public static void CreateTransferMenu(PropertyHolder ph1, PropertyHolder ph2) {
		Instance.InternalTransferMenu (ph1 , ph2);
	}

	void InternalTransferMenu(PropertyHolder ph1, PropertyHolder ph2) {
		MTransferMenu.gameObject.SetActive (true);
		MTransferMenu.Clear ();

		MTransferMenu.AddPropertyHolder (ph1,0);
		MTransferMenu.AddPropertyHolder (ph2,1);
		MTransferMenu.init (ph1.NumTransfers);
	}

	public static void ClosePropertyLists() {
		foreach (GameObject go in Instance.PropertyLists) {
			Destroy (go);
		}
		Instance.PropertyLists.Clear ();
	}

    public static void SetAbility(Ability a)
    {
        Instance.abilityToUpdate = a;
    }

    public static void UpdateAbility(List<Property> p1, List<Ability> a1, List<Property> p2)
    {
		if (Instance.abilityToUpdate != null) {
			Instance.abilityToUpdate.SetTransferLists(p1,a1,p2);
		}
    }
    
}