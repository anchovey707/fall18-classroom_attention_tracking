import java.io.BufferedReader;
import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.io.IOException;
import java.io.InputStreamReader;
import java.net.*;
import java.util.Random;

public class TestTeacher {
	static DatagramSocket sock;
	static String input="";
	static int port=61601;
	static UDPReciever UDP;
	public static void main(String[] args) {
			
		if(args.length>0)
			port= Integer.parseInt(args[0]);
		InetAddress ip=InetAddress.getLoopbackAddress();
		Socket socket = new Socket();
		try {
			ip = InetAddress.getByName("10.40.43.43");
			ip = InetAddress.getByName("127.0.0.1");
			socket = new Socket(ip,61600);
		
			DataInputStream inputStream = new DataInputStream(socket.getInputStream());
			DataOutputStream outputStream = new DataOutputStream(socket.getOutputStream());
			char c=' ';
			while(c!=';') {
				byte[] b=new byte[1];
				b[0]=inputStream.readByte();
				c=(new String(b,"UTF-8")).charAt(0);
			}
			
			String temp="",user="#";
			Random rand = new Random();
			int r=0;
			while(r++<10)
				user += (char)(rand.nextInt(26) + 'a');
			user+=";";
			user="#aallen#1234;";
			System.out.println("I am "+user);
			//Sending the server My info
			outputStream.writeBytes(user);
			//UDP = new UDPReciever(ip,port);
			//UDP.start();
			//waiting to start
			
			System.out.println("Trying read from server");
			while(true) {
				Thread.sleep(1000);
				System.out.println("Trying read from server");
				while(inputStream.available()>0){
					byte[] b=new byte[1];
					b[0]=inputStream.readByte();
					temp+=(new String(b,"UTF-8"));
					System.out.println("temp="+temp);
				}
				if(temp.equals("")) {
					//do nothing
				}else if(temp.contains("#port:")&&temp.contains(";")){
					String newPort=temp.substring(6, temp.indexOf(";"));
					System.out.println("New port:'"+newPort+"'");
					//UDP.changePort(Integer.parseInt(newPort));
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
