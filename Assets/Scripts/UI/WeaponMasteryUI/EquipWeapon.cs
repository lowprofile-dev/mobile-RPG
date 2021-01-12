using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EquipWeapon : MonoBehaviour
{
    [SerializeField] string name;
    Button button;
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
    }

    // Update is called once per frame

    public void onbuttonClick()
    {
      //  Debug.Log(name + "착용 !");
        WeaponManager.Instance.SetWeapon(name);
    }
}
