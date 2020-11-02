using System;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(RectTransform))]
public class FixedJoystick : 
	MonoBehaviour,
	IPointerDownHandler,
	IDragHandler,
	IPointerUpHandler
{
	public RectTransform stick;                 
	public float returnRate = 15.0F;               
	public float dragRadius = 65.0f;                
	public AlphaControll colorAlpha;

	private bool _returnHandle, pressed, isEnabled = true;
	private RectTransform _canvas;
	private Vector3 globalStickPos;
	private Vector2 stickOffset;
	private CanvasGroup canvasGroup;

	Vector2 Coordinates
	{
		get
		{
			if (stick.anchoredPosition.magnitude < dragRadius)
				return stick.anchoredPosition / dragRadius;
			return stick.anchoredPosition.normalized;
		}
	}

	void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
	{
		pressed = true;
		_returnHandle = false;
		stickOffset = GetJoystickOffset(eventData);
		stick.anchoredPosition = stickOffset;
	}

	void IDragHandler.OnDrag(PointerEventData eventData)
	{
		stickOffset = GetJoystickOffset(eventData);
		stick.anchoredPosition = stickOffset;
	}

	void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
	{
		pressed = false;
		_returnHandle = true;
	}

	private Vector2 GetJoystickOffset(PointerEventData eventData)
	{
		if (RectTransformUtility.ScreenPointToWorldPointInRectangle(_canvas, eventData.position, eventData.pressEventCamera, out globalStickPos))
			stick.position = globalStickPos;
		var handleOffset = stick.anchoredPosition;
		if (handleOffset.magnitude > dragRadius)
		{
			handleOffset = handleOffset.normalized * dragRadius;
			stick.anchoredPosition = handleOffset;
		}
		return handleOffset;
	}

	private void Start()
	{
		canvasGroup = GetComponent<CanvasGroup>();
		_returnHandle = true;
		var touchZone = GetComponent<RectTransform>();
		touchZone.pivot = Vector2.one * 0.5F;
		stick.transform.SetParent(transform);
		var curTransform = transform;
		do
		{
			if (curTransform.GetComponent<Canvas>() != null)
			{
				_canvas = curTransform.GetComponent<RectTransform>();
				break;
			}
			curTransform = transform.parent;
		}
		while (curTransform != null);
	}

	private void FixedUpdate()
	{
		if (_returnHandle)
			if (stick.anchoredPosition.magnitude > Mathf.Epsilon)
				stick.anchoredPosition -= new Vector2(stick.anchoredPosition.x * returnRate,
													  stick.anchoredPosition.y * returnRate) * Time.deltaTime;
			else
				_returnHandle = false;

		switch (isEnabled)
		{
			case true:
				canvasGroup.alpha = pressed ? colorAlpha.pressedAlpha : colorAlpha.idleAlpha;
				canvasGroup.interactable = canvasGroup.blocksRaycasts = true;
				break;
			case false:
				canvasGroup.alpha = 0;
				canvasGroup.interactable = canvasGroup.blocksRaycasts = false;
				break;
		}
	}

	public Vector3 MoveInput()
	{
		return new Vector3(Coordinates.x, 0, Coordinates.y);
	}

	public void Rotate(Transform transformToRotate, float speed)
	{
		if (Coordinates != Vector2.zero)
			transformToRotate.rotation = Quaternion.Slerp(transformToRotate.rotation,
														  Quaternion.LookRotation(new Vector3(Coordinates.x, 0, Coordinates.y)),
														  speed * Time.deltaTime);
	}

	public bool IsPressed()
	{
		return pressed;
	}

	public void Enable(bool enable)
	{
		isEnabled = enable;
	}
}

[Serializable]
public class AlphaControll
{
	public float idleAlpha = 0.5F, pressedAlpha = 1.0F;
}