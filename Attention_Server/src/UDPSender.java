import java.io.DataInputStream;
import java.io.IOException;
import java.net.DatagramPacket;
import java.net.DatagramSocket;
import java.net.InetAddress;
import java.net.Socket;
import java.net.SocketException;
import java.util.Random;

public class UDPSender extends Thread{
	
	int port;
	InetAddress ip;
	DatagramSocket socket;
	
	public UDPSender(InetAddress i,int p) {
		ip=i;
		port=p;
		try {
			socket=new DatagramSocket(port);
		} catch (SocketException e) {
			e.printStackTrace();
		}
	}
	public UDPSender(InetAddress i) {
		this(i,61600);
	}
	
	public void run() {
		try {
			int count=0;
			System.out.println("Sending data");
			while(true) {
	        	//byte[] b =String.valueOf(new Random().nextDouble()*50).getBytes();
		        byte[] b = String.valueOf(count++).getBytes();
	        	socket.send(new DatagramPacket(b , b.length,ip,socket.getLocalPort()));
	            System.out.print("'"+b+"',");
	        	Thread.currentThread().sleep(new Random().nextInt(1000));
	        }
		} catch (IOException | InterruptedException e) {
			e.printStackTrace();
		}
		System.out.println("Stopped sending data");
	}
	
	public void changePort(int p) {
		port=p;
		socket.close();
		try {
			socket=new DatagramSocket(port);
		} catch (SocketException e) {
			System.out.println("Broke trying to change port");
			e.printStackTrace();
		}
	}
	
	
}
