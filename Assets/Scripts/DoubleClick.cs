using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class DoubleClick : MonoBehaviour, IPointerDownHandler
{
    //private Image im;
    public Sprite pic;
    public Sprite picpr;
    public GameObject panel;
    private Image im;
    void Start()
    {
        im = panel.GetComponent<Image>();
        im.sprite = picpr;
    }
    public void OnPointerDown (PointerEventData eventData) 
    {
        if(eventData.clickCount == 2){
            im.sprite = pic;
            Debug.Log ("Double Click");
            eventData.clickCount = 0;
            
        }
    }
    
    
}
/*Byte[] bytes = new Byte[1024];  			
			while (true) { 				
				using (client8000 = tcpListener8000.AcceptTcpClient()) { 					
					// Get a stream object for reading 					
					using (NetworkStream stream = client8000.GetStream()) { 						
						int length; 						
						// Read incomming stream into byte arrary. 						
						while ((length = stream.Read(bytes, 0, bytes.Length)) != 0) { 							
							var incommingData = new byte[length]; 							
							Array.Copy(bytes, 0, incommingData, 0, length);  							
							// Convert byte array to string message. 							
							string clientMessage = Encoding.UTF8.GetString(incommingData); 							
							Debug.Log("client message received as: " + clientMessage);
							
							mytext = clientMessage;
						} 					
					} 				
				} 			
			} */		