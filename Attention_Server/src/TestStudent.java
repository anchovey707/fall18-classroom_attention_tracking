import java.io.BufferedReader;
import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.io.IOException;
import java.io.InputStreamReader;
import java.net.*;
import java.util.Random;

public class TestStudent {
	static DatagramSocket sock;
	static String input="";
	static int port=61601;
	static UDPSender UDP;
	public static void main(String[] args) {
			
		if(args.length>0)
			port= Integer.parseInt(args[0]);
		InetAddress ip=InetAddress.getLoopbackAddress();
		Socket socket = new Socket();
		try {
			ip = InetAddress.getByName("192.168.0.196");
			//ip = InetAddress.getByName("127.0.0.1");
			socket = new Socket(ip,61600);
		
			DataInputStream inputStream = new DataInputStream(socket.getInputStream());
			DataOutputStream outputStream = new DataOutputStream(socket.getOutputStream());
			while(inputStream.readChar()!=';') {
				//Waiting for the confirmation to send info
			}
			
			String temp="",user="#";
			Random rand = new Random();
			int r=0;
			while(r++<10)
				user += (char)(rand.nextInt(26) + 'a');
			user+=";";
			user="#ws01864;";
			System.out.println("I am "+user);
			//Sending the server My info
			outputStream.writeChars(user);
			UDPSender UDP = new UDPSender(ip,port);
			UDP.start();
			//waiting to start
			
			System.out.println("Trying read from server");
			while(true) {
				Thread.sleep(1000);
				System.out.println("Trying read from server");
				while(inputStream.available()>0){
					temp+=inputStream.readChar();
					System.out.println("temp="+temp);
				}
				if(temp.equals("")) {
					//do nothing
				}else if(temp.contains("#port:")&&temp.contains(";")){
					String newPort=temp.substring(6, temp.indexOf(";"));
					System.out.println("New port:'"+newPort+"'");
					UDP.changePort(Integer.parseInt(newPort));
					temp="";
				}
				//send heartbeat char
				outputStream.writeChar(';');
				//System.out.println(temp);
			}
			
			
		}catch(Exception e) {
			e.printStackTrace();
		}		
		
	}
	
	
	
	
}
