using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WalletUI : MonoBehaviour
{
    [SerializeField] Wallet wally;
    public TextMeshProUGUI c;
    public TextMeshProUGUI s;
    public TextMeshProUGUI e;
    public TextMeshProUGUI g;
    public TextMeshProUGUI p;
    // Start is called before the first frame update
    void Start()
    {
        UpdateText();
    }

    // Update is called once per frame
    public void UpdateText()
    {
        c.text = wally.copper.ToString();
        s.text = wally.silver.ToString();
        e.text = wally.electrum.ToString();
        g.text = wally.gold.ToString();
        p.text = wally.platinum.ToString();
    }
}
