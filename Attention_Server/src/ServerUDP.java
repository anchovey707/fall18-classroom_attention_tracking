import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.io.IOException;
import java.io.PrintWriter;
import java.net.DatagramPacket;
import java.net.DatagramSocket;
import java.net.InetAddress;
import java.net.SocketException;
import java.util.Random;

public class ServerUDP implements Runnable{
	byte[] streambuffer = new byte[Byte.MAX_VALUE-1];
    DatagramSocket streamSocket;
    DatagramPacket streamPacket;
	DataInputStream inputStream;
	DataOutputStream OutputStream;
	InetAddress ip;
	String data;
	
	public ServerUDP(InetAddress i,DatagramSocket d) {
		ip=i;
		streamSocket=d;
		streamPacket = new DatagramPacket(streambuffer, streambuffer.length);
		System.out.println("\t\tNew Datagram socket on "+d.getLocalPort());
	}
	public ServerUDP(InetAddress i,int port) throws SocketException {
		this(i,new DatagramSocket(port));
	}

	public void run() {
		try {
			//Create a print writer to print to a file
			PrintWriter writer = new PrintWriter(String.valueOf(streamSocket.getLocalPort())+".txt");
			Random rand = new Random();
			int packets=0;
			while(++packets>0) {
				//streamSocket.receive(streamPacket);
				byte[] recieved =("#user424#"+packets+"#"+rand.nextInt(50)+"#anApp;").getBytes();//streamPacket.getData();
				//data=new String(recieved, 0, streamPacket.getLength());
				data=new String(recieved, 0, recieved.length);
				System.out.println("\t\t"+streamPacket.getAddress()+": "+data
									+"\n\t\tto "+ip);
				
				data=("#user424#"+rand.nextDouble()*50+"#anApp");
				streamSocket.send(new DatagramPacket(recieved , recieved.length,ip,streamSocket.getLocalPort()));
				writer.write(data);
				writer.flush();
				try{
				//Thread.sleep(350);
				}catch(Exception e){
				}
			}
			
			
		} catch (IOException e) {
				e.printStackTrace();
		}	
		
	}

	

	
	//Recieve data from student
	public String getStudentData() {
		return data;}

}
