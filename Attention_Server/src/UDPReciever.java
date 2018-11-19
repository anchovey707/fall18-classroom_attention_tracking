import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.io.IOException;
import java.io.PrintWriter;
import java.net.DatagramPacket;
import java.net.DatagramSocket;
import java.net.InetAddress;
import java.net.SocketException;

public class UDPReciever extends Thread{
	
	int port;
	InetAddress ip;
	DatagramSocket socket;
	DatagramPacket packet;
	String data;
	byte[] streambuffer = new byte[Byte.MAX_VALUE-1];
	DataInputStream inputStream;
	DataOutputStream OutputStream;
	
	public UDPReciever(InetAddress i,int p) {
		ip=i;
		port=p;
		try {
			socket=new DatagramSocket(port);
			packet = new DatagramPacket(streambuffer, streambuffer.length);
		} catch (SocketException e) {
			e.printStackTrace();
		}
	}
	public UDPReciever(InetAddress i) {
		this(i,61600);
	}

	public void run() {
		System.out.println(port);
		try {
			PrintWriter writer = new PrintWriter(String.valueOf(socket.getLocalPort())+".txt");
			while(true) {
				try {
					socket.receive(packet);
					byte[] bytes = packet.getData();
					data=new String(bytes, 0, packet.getLength());
					System.out.println(packet.getAddress()+": "+data);
					writer.write(data);
					writer.flush();
				}catch(Exception e) {
					System.out.println("error");
				}
			}
			
		} catch (IOException e) {
				e.printStackTrace();
		}	
		
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
