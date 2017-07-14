using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FpsCounter : MonoBehaviour
{
	
	public TextAnchor alignment = TextAnchor.UpperLeft;

	[Space(10)]
	public int textSize = 30;
	public Color textColor = Color.black;

	[Space(10)]
	public float fpsUpdateTime = 1f;
	private float fps;
	private bool updateFps = true;

	private GameObject newCanvas;
	private GameObject text;

	void Start()
	{
		//CREATE NEW UI CANVAS
		//create a new ui canvas
		newCanvas = new GameObject();
		newCanvas.name = "Fps Counter Canvas";
		//add components to newCanvas
		newCanvas.AddComponent<Canvas>();
		newCanvas.AddComponent<CanvasScaler>();
		//configure components
		newCanvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
		newCanvas.GetComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;

		//CREATE NEW TEXT
		text = new GameObject();
		text.name = "Fps Counter Text";
		text.transform.parent = newCanvas.transform;
		//add components to text
		text.AddComponent<CanvasRenderer>();
		text.AddComponent<Text>();
		//configure components
		text.GetComponent<Text>().horizontalOverflow = HorizontalWrapMode.Overflow;
		text.GetComponent<Text>().verticalOverflow = VerticalWrapMode.Overflow;
		text.GetComponent<Text>().font = Resources.GetBuiltinResource (typeof(Font), "Arial.ttf") as Font;
		text.GetComponent<Text>().fontSize = textSize;
		text.GetComponent<Text>().fontStyle = FontStyle.Bold;
		text.GetComponent<Text>().color = textColor;
		text.GetComponent<Text>().text = "FPS: ";
		//position object
		//the canvas takes time to update, so the functions will be called after the canvas is already updated
		StartCoroutine(Delay(0.2f));
	}
	
	IEnumerator Delay(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
		//position object
		text.GetComponent<RectTransform>().anchorMin = new Vector2(0,1);
		text.GetComponent<RectTransform>().anchorMax = new Vector2(0,1);
		text.GetComponent<RectTransform>().pivot = new Vector2(0,1);
		text.GetComponent<RectTransform>().sizeDelta = new Vector2(newCanvas.GetComponent<RectTransform>().sizeDelta.x, newCanvas.GetComponent<RectTransform>().sizeDelta.y);
		text.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0,0,0);
		text.GetComponent<Text>().alignment = alignment;
	}
		
	void Update()
	{
		fps++;
		if(updateFps == true)
		{
			StartCoroutine(ShowFps());
			updateFps = false;
		}
	}

	private IEnumerator ShowFps()
	{
		yield return new WaitForSeconds(fpsUpdateTime);
		text.GetComponent<Text>().text ="TRUE FPS: "+fps*2 + "-FPS: " + System.Math.Round(Time.timeScale / Time.deltaTime, 2).ToString();
		fps = 0;
		updateFps = true;
	}
}
