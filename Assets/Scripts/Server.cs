using System;
using System.Collections; 
using System.Collections.Generic; 
using System.Net; 
using System.Net.Sockets;
using System.Reflection;
using System.Text; 
using System.Threading; 
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Server : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerUpHandler, IPointerDownHandler
{

	public string mytext;
	
	private TcpListener tcpListenerSend; 
	
	private Thread sendThread;  	
	
	private TcpClient sendClient;
	
	private TcpListener tcpRecieveListener; 
	
	private Thread recieveThread;  	
	
	private TcpClient recieveClient;

	private PointerEventData data;

	private static float x;
	private static float y;
	
	public bool isDown = false;

	private GameObject go;

	private Shift shift;

	private Image im;

	void Start () {

		//Debug.Log("ноль");
		sendThread = new Thread (new ThreadStart(CreateSendListener)); 		
		sendThread.IsBackground = true; 		
		sendThread.Start();

		//Debug.Log("раз");
		recieveThread = new Thread (new ThreadStart(CreateRecieveListener)); 		
		recieveThread.IsBackground = true; 		
		recieveThread.Start();

		//Debug.Log("два");

		go = GameObject.Find("keyboard");
		shift = go.GetComponent<Shift>();
		im = GetComponent<Image>();

	}  	
	
	
	void Update () {
		if (isDown)
		{
			x= data.pointerCurrentRaycast.worldPosition.x;
			y= data.pointerCurrentRaycast.worldPosition.y;
			if(x<1080 && y<2300)
			Debug.Log((x + 540) + " ; " + (-y+1820)+" ; ");
			Send((x + 540) + " ; " + (-y+1820)+" ;");
		}
	}  	
	
	
	private void CreateRecieveListener () { 		
		try {
			tcpRecieveListener = new TcpListener(IPAddress.Parse("0.0.0.0"), 8080); 			
			tcpRecieveListener.Start();              
			Debug.Log("Server is listening");              
			Byte[] bytes = new Byte[1024];  			
			while (true) { 				
				using (recieveClient = tcpRecieveListener.AcceptTcpClient()) {
					using (NetworkStream stream = recieveClient.GetStream()) { 						
						int length;
						while ((length = stream.Read(bytes, 0, bytes.Length)) != 0) { 							
							var incommingData = new byte[length]; 							
							Array.Copy(bytes, 0, incommingData, 0, length);
							string clientMessage = Encoding.UTF8.GetString(incommingData); 							
							Debug.Log("Recieved text: " + clientMessage);
							mytext = clientMessage;
						} 					
					} 				
				} 			
			} 		
		} 		
		catch (SocketException socketException) { 			
			Debug.Log("SocketException " + socketException.ToString()); 		
		}     
	}  	
	
	private void CreateSendListener () { 		
		try { 			
			// Create listener on localhost port 8080. 			
			tcpListenerSend = new TcpListener(IPAddress.Parse("0.0.0.0"), 8000); 			
			tcpListenerSend.Start();
			sendClient = tcpListenerSend.AcceptTcpClient();
			NetworkStream stream = sendClient.GetStream();
			Debug.Log("Server is listening");              
			
		} 		
		catch (SocketException socketException) { 			
			Debug.Log("SocketException " + socketException.ToString()); 		
		}     
	}  	
	
	
	public void Send(string message) { 		
		if (sendClient == null) {             
			return;
		}  		
		Vector3 mousePos = Input.mousePosition;
		try {
			NetworkStream stream = sendClient.GetStream(); 			
			if (stream.CanWrite) {
				
				byte[] byteArrayMessage = Encoding.ASCII.GetBytes(message);
				stream.Write(byteArrayMessage, 0, byteArrayMessage.Length);
			}       
		} 		
		catch (SocketException socketException) {             
			Debug.Log("Socket exception: " + socketException);         
		} 	
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		data = eventData;
		x = eventData.pointerCurrentRaycast.worldPosition.x;
		y = eventData.pointerCurrentRaycast.worldPosition.y;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		
		
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		Debug.Log("Up");
		isDown = false;
		Send("\r\n");
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		Debug.Log("Down");
		isDown = true;

		if (!(Server.x > 10 && Server.y < -275 && Server.x < 100 && Server.y > -400))
		{
			if (shift.i == 1)
			{
				im.sprite = shift.small;
				shift.i = 0;
			}
		}
	}

}