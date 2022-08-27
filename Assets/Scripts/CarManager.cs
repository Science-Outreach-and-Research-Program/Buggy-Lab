using UnityEngine;
using UnityEngine.UI;

public class CarManager : MonoBehaviour
{
    [SerializeField] private GameObject chart;
    public int nextPosition;
    private static float[] realPositions = {-6.5f, -3.25f, 0, 3.25f, 6.5f};
    private static float maxSpeed = 5f;
    private float _currentSpeed;
    private bool _startMove;
    private float _timeSinceStart;

    public void Start()
    {
        _startMove = false;
        nextPosition = 1;
        _timeSinceStart = 0;
        _currentSpeed = 0;
        GameObject.Find("NextPositionText").GetComponent<Text>().text =
            string.Format("Next Position to Measure: {0:N1} m", nextPosition * 0.5f);
        GameObject.Find("ResultText").GetComponent<Text>().text = "";
    }

    void Update()
    {
        if (_startMove)
        {
            _timeSinceStart += Time.deltaTime;

            if (_currentSpeed < maxSpeed)
                _currentSpeed += 0.05f;
            float step = _currentSpeed * Time.deltaTime;
            Vector2 target = new Vector2(realPositions[nextPosition], gameObject.transform.position.y);
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, target, step);

            if (gameObject.transform.position.x == realPositions[nextPosition])
            {
                _startMove = false;
                _currentSpeed = 0;
                chart.GetComponent<ChartManager>()
                    .AddData(nextPosition * 0.5f, _timeSinceStart)
                    .RefreshAllComponent();
                GameObject.Find("ResultText").GetComponent<Text>().text =
                    string.Format("Time to reach {0:N1} m: {1:N3} s", nextPosition * 0.5f, _timeSinceStart);

                nextPosition++;
                _timeSinceStart = 0;
                GameObject.Find("NextPositionText").GetComponent<Text>().text =
                    string.Format("Next Position to Measure: {0:N1} m", nextPosition * 0.5f);
            }
        }
    }

    public void ReleaseCar()
    {
        gameObject.transform.position = new Vector2(realPositions[0], gameObject.transform.position.y);
        _startMove = true;

        if (nextPosition == 5)
        {
            nextPosition = 1;
        }
    }
}