using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Funds : MonoBehaviour
{
    /// <summary>
    /// The price of the current selected furniture piece
    /// </summary>
    [HideInInspector]
    public int Price;

    /// <summary>
    /// The total starting money
    /// </summary>
    public int StartingFunds = 1500;

    /// <summary>
    /// Text component of the funds in the canvas menu
    /// </summary>
    public Text FundsMenu;    

    /// <summary>
    /// The color the text blink when insufficient funds 
    /// </summary>
    public Color InsufficientFundsColorBlink;

    /// <summary>
    /// The speed the text blink when insufficient funds 
    /// </summary>
    public float InsufficientFundsColorBlinkSpeed;

    /// <summary>
    /// The amount the text blink when insufficient funds 
    /// </summary>
    public float InsufficientFundsColorBlinkAmount;

    /// <summary>
    /// The speed at which the funds visually decrease after a purchase
    /// </summary>
    public int fundsDecreaseSpeed;

    /// <summary>
    /// Used to make sure no simultaneous blinking occurs
    /// </summary>
    [HideInInspector]
    public bool isBlinking = false;

    /// <summary>
    /// During start the menu is updated
    /// </summary>
    private void Start()
    {
        FundsMenu.text = StartingFunds.ToString();
    }

    /// <summary>
    /// Called from Canvas button to set the price of the selected furniture piece
    /// </summary>
    /// <param name="price">Value of the price furniture piece</param>
    public void SetPrice(int price)
    {
        Price = price;
    }

    /// <summary>
    /// Called from FurniturePlacement.cs to update the funds
    /// </summary>
    public IEnumerator UpdateFunds()
    {
        var temporaryFunds = StartingFunds;
        StartingFunds -= Price;
        while (temporaryFunds > StartingFunds)
        {
            temporaryFunds -= fundsDecreaseSpeed;
            FundsMenu.text = temporaryFunds.ToString();
            yield return new WaitForSeconds(Time.deltaTime);
        }

        FundsMenu.text = StartingFunds.ToString();
    }

    /// <summary>
    /// Blinks the funds red to indicate you do not have enough funds
    /// </summary>
    /// <returns></returns>
    public IEnumerator BlinkRed()
    {
        isBlinking = true;
        var normalColor = FundsMenu.color;
        var blinkAmount = InsufficientFundsColorBlinkAmount * 2;
        for (var i = 0; i < blinkAmount; i++)
        {
            yield return new WaitForSeconds(InsufficientFundsColorBlinkSpeed);
            if(FundsMenu.color == normalColor)
            {
                FundsMenu.color = InsufficientFundsColorBlink;
            }
            else
            {
                FundsMenu.color = normalColor;
            }    
        }
        FundsMenu.color = normalColor;
        isBlinking = false;
    }
}
