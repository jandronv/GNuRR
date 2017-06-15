using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trgr_end_enemy : MonoBehaviour {

    public GameObject jangah;
    private SpriteRenderer jangah_SR;
    public GameObject helpme;
    public Sprite jangahidle;
    public GameObject trigger_jangah;

    void Start()
    {
        jangah_SR = jangah.GetComponent<SpriteRenderer>();
        helpme.SetActive(true);
        trigger_jangah.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet") {
            jangah_SR.sprite = jangahidle;
            helpme.SetActive(false);
            trigger_jangah.SetActive(true);
            Destroy(gameObject);
        }
    }



}
