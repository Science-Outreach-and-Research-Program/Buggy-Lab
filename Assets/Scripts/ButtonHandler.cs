using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    [SerializeField] private GameObject car;
    [SerializeField] private GameObject ruler;
    [SerializeField] private GameObject chart;
    [SerializeField] private GameObject releaseButton;
    [SerializeField] private GameObject showChartButton;
    [SerializeField] private GameObject resetButton;
    [SerializeField] private GameObject goBackButton;
    [SerializeField] private GameObject drawLineButton;
    [SerializeField] private GameObject slopeText;


    public void ReleaseCar()
    {
        car.GetComponent<CarManager>().ReleaseCar();
    }

    public void LoadChartScene()
    {
        car.SetActive(false);
        ruler.SetActive(false);
        releaseButton.SetActive(false);
        showChartButton.SetActive(false);
        resetButton.SetActive(false);
        
        chart.SetActive(true);
        goBackButton.SetActive(true);
        drawLineButton.SetActive(true);
        slopeText.SetActive(true);
    }
    
    public void LoadMainScene()
    {
        car.SetActive(true);
        ruler.SetActive(true);
        releaseButton.SetActive(true);
        showChartButton.SetActive(true);
        resetButton.SetActive(true);
        
        chart.SetActive(false);
        goBackButton.SetActive(false);
        drawLineButton.SetActive(false);
        slopeText.SetActive(false);
    }

    public void DrawBestFitLine()
    {
        chart.GetComponent<ChartManager>().DrawBestFitLine();
    }

    public void ResetGame()
    {
        chart.GetComponent<ChartManager>().Init();
        car.GetComponent<CarManager>().Start();
        car.transform.position = new Vector2(-6.5f, car.transform.position.y);
        slopeText.GetComponent<Text>().text = "";
    }

}
