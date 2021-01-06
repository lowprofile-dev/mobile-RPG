using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManualScripts : MonoBehaviour {

	public GameObject[] PageObj;
	public int currentPage;
	public int minPage = 1;
	public int maxPage = 5;
	public Text txtPage;

	void Start () {
		for(int i = 0; i <= maxPage; i++) {
			PageObj [i].SetActive (false);
		}
		PageObj [minPage].SetActive (true);
		currentPage = minPage;
		txtPage.text = "PAGE " + currentPage + " / " + maxPage;
	}

	public void ChangedPage (int i) {
		currentPage = Mathf.Clamp (currentPage + i, minPage, maxPage);
		for(int j = 0; j <= maxPage; j++) {
			PageObj [j].SetActive (false);
		}
		PageObj [currentPage].SetActive (true);
		txtPage.text = "PAGE " + currentPage + " / " + maxPage;
	}
}