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

public class Server : MonoBehaviour
{

	public string mytext;
	
	private TcpListener tcpListenerSend; 
	
	private Thread sendThread;  	
	
	private TcpClient sendClient;
	
	private TcpListener tcpRecieveListener; 
	
	private Thread recieveThread;  	
	
	private TcpClient recieveClient; 
	
	public bool isDown = false;

	public void Press()
	{
		isDown = true;
	}
	
	public void Release()
	{
		isDown = false;
		Send("\r\n");
	}
		
	
	void Start () { 
		
				
		sendThread = new Thread (new ThreadStart(CreateSendListener)); 		
		sendThread.IsBackground = true; 		
		sendThread.Start(); 	
		
		
		recieveThread = new Thread (new ThreadStart(CreateRecieveListener)); 		
		recieveThread.IsBackground = true; 		
		recieveThread.Start(); 	
		
	}  	
	
	
	void Update () {

		if (isDown)
		{
			Debug.Log((Input.mousePosition.x - 30) + " ; " + (-1*Input.mousePosition.y+2230)+" ; ");
			Send((Input.mousePosition.x - 30) + " ; " + (-1*Input.mousePosition.y+2230)+" ;");
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
	
}