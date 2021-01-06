using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class fxController : MonoBehaviour {

	public int idType;
	public int idColor;
	public int idAnim;
	public int idPoint;
	public int idCircle;
	public int idBox;
	public int idArea;
	public Text prefabNameTxt;

	public GameObject[] pointObjs;
	public GameObject[] circleObjs;
	public GameObject[] boxObjs;
	public GameObject[] areaObjs;
	public GameObject[] DesFxObjs;

	void Start () {
		SetupFx ();
	}

	public void TypesChanged(int i) {
		idType = i;
		SetupFx ();
	}

	public void AnimsChanged(int i) {
		idAnim = i;
		SetupFx ();
	}

	public void ColorsChanged(int i) {
		idColor = i;
		SetupFx ();
	}

	public void SetupFx() {
		DesFxObjs = GameObject.FindGameObjectsWithTag ("Effects");

		foreach (GameObject DesFxObj in DesFxObjs)
			Destroy (DesFxObj.gameObject);

		switch (idType) {
		case 0: //Point
			PointPrefabs();
			break;
		case 1: //Circle
			CirclePrefabs();
			break;
		case 2: //Box
			BoxPrefabs();
			break;
		case 3: //Area
			AreaPrefabs();
			break;
		}
	}

	public void PointPrefabs () {
		switch (idAnim) {
		case 0: //Start
			switch (idColor) {
			case 0: idPoint = 0; break;
			case 1: idPoint = 1; break;
			case 2: idPoint = 2; break;
			case 3: idPoint = 3; break;
			case 4: idPoint = 4; break;
			}
			break;
		case 1: //Idle
			switch (idColor) {
			case 0: idPoint = 5; break;
			case 1: idPoint = 6; break;
			case 2: idPoint = 7; break;
			case 3: idPoint = 8; break;
			case 4: idPoint = 9; break;
			}
			break;
		case 2: //Alert
			switch (idColor) {
			case 0: idPoint = 10; break;
			case 1: idPoint = 11; break;
			case 2: idPoint = 12; break;
			case 3: idPoint = 13; break;
			case 4: idPoint = 14; break;
			}
			break;
		case 3: //End
			switch (idColor) {
			case 0: idPoint = 15; break;
			case 1: idPoint = 16; break;
			case 2: idPoint = 17; break;
			case 3: idPoint = 18; break;
			case 4: idPoint = 19; break;
			}
			break;
		}
		Instantiate (pointObjs [idPoint]);
		prefabNameTxt.text = "File name : " + pointObjs [idPoint].gameObject.name;
	}

	public void CirclePrefabs () {
		switch (idAnim) {
		case 0: //Start
			switch (idColor) {
			case 0: idCircle = 0; break;
			case 1: idCircle = 1; break;
			case 2: idCircle = 2; break;
			case 3: idCircle = 3; break;
			case 4: idCircle = 4; break;
			}
			break;
		case 1: //Idle
			switch (idColor) {
			case 0: idCircle = 5; break;
			case 1: idCircle = 6; break;
			case 2: idCircle = 7; break;
			case 3: idCircle = 8; break;
			case 4: idCircle = 9; break;
			}
			break;
		case 2: //Alert
			switch (idColor) {
			case 0: idCircle = 10; break;
			case 1: idCircle = 11; break;
			case 2: idCircle = 12; break;
			case 3: idCircle = 13; break;
			case 4: idCircle = 14; break;
			}
			break;
		case 3: //End
			switch (idColor) {
			case 0: idCircle = 15; break;
			case 1: idCircle = 16; break;
			case 2: idCircle = 17; break;
			case 3: idCircle = 18; break;
			case 4: idCircle = 19; break;
			}
			break;
		}
		Instantiate (circleObjs [idCircle]);
		prefabNameTxt.text = "File name : " + circleObjs [idCircle].gameObject.name;
	}

	public void BoxPrefabs () {
		switch (idAnim) {
		case 0: //Start
			switch (idColor) {
			case 0: idBox = 0; break;
			case 1: idBox = 1; break;
			case 2: idBox = 2; break;
			case 3: idBox = 3; break;
			case 4: idBox = 4; break;
			}
			break;
		case 1: //Idle
			switch (idColor) {
			case 0: idBox = 5; break;
			case 1: idBox = 6; break;
			case 2: idBox = 7; break;
			case 3: idBox = 8; break;
			case 4: idBox = 9; break;
			}
			break;
		case 2: //Alert
			switch (idColor) {
			case 0: idBox = 10; break;
			case 1: idBox = 11; break;
			case 2: idBox = 12; break;
			case 3: idBox = 13; break;
			case 4: idBox = 14; break;
			}
			break;
		case 3: //End
			switch (idColor) {
			case 0: idBox = 15; break;
			case 1: idBox = 16; break;
			case 2: idBox = 17; break;
			case 3: idBox = 18; break;
			case 4: idBox = 19; break;
			}
			break;
		}
		Instantiate (boxObjs [idBox]);
		prefabNameTxt.text = "File name : " + boxObjs [idBox].gameObject.name;
	}

	public void AreaPrefabs () {
		switch (idAnim) {
		case 0: //Start
			switch (idColor) {
			case 0: idArea = 0; break;
			case 1: idArea = 1; break;
			case 2: idArea = 2; break;
			case 3: idArea = 3; break;
			case 4: idArea = 4; break;
			}
			break;
		case 1: //Idle
			switch (idColor) {
			case 0: idArea = 5; break;
			case 1: idArea = 6; break;
			case 2: idArea = 7; break;
			case 3: idArea = 8; break;
			case 4: idArea = 9; break;
			}
			break;
		case 2: //Alert
			switch (idColor) {
			case 0: idArea = 10; break;
			case 1: idArea = 11; break;
			case 2: idArea = 12; break;
			case 3: idArea = 13; break;
			case 4: idArea = 14; break;
			}
			break;
		case 3: //End
			switch (idColor) {
			case 0: idArea = 15; break;
			case 1: idArea = 16; break;
			case 2: idArea = 17; break;
			case 3: idArea = 18; break;
			case 4: idArea = 19; break;
			}
			break;
		}
		Instantiate (areaObjs [idArea]);
		prefabNameTxt.text = "File name : " + areaObjs [idArea].gameObject.name;
	}
}