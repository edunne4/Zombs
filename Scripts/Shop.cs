using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour {

	public GameObject good;
	//public Image image;
	public SpriteRenderer itemPreview;
	public Text price;
	public int cost;

	// Use this for initialization
	void Start () {

		updateSale ();
	}

	public void buyGood(){ //drop gun in front, then update sale
		PickupScript soldGood = Instantiate (good, transform.position,
			Quaternion.identity).GetComponent<PickupScript> ();
		soldGood.GetComponent<Rigidbody2D> ().AddForce (Vector2.down * 10f, ForceMode2D.Impulse);


	}

	public void updateSale(){ //get new gun and put it up for sale
		//print (bestWeapon);
		itemPreview.sprite = good.GetComponentInChildren<SpriteRenderer>().sprite;
		itemPreview.enabled = true;
		price.text = "$" + cost;
	}


}
