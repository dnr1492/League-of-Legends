using UnityEngine.UI;

public class GameManager
{
    private static GameManager instance;

    private GameManager()
    {
    }

    public static GameManager GetInstance()
    {
        if (instance == null) instance = new GameManager();
        return instance;
    }

    #region Gold
    private int totalGold;
    private Text txtTotalGold;

    public void IncreaseGold(int gold)
    {
        totalGold += gold;
        if (txtTotalGold != null) txtTotalGold.text = "소지한 골드 : " + totalGold.ToString();
    }

    public void DecreaseGold(int gold)
    {
        totalGold -= gold;
        if (txtTotalGold != null) txtTotalGold.text = "소지한 골드 : " + totalGold.ToString();
    }

    public int GetGold()
    {
        return totalGold;
    }

    public void DisplayGold(Text txtOwnGold)
    {
        txtTotalGold = txtOwnGold;
        txtTotalGold.text = "소지한 골드 : " + totalGold.ToString();
    }
    #endregion
}
