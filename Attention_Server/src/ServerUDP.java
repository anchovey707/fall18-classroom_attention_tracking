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
	byte[] streambuffer = new byte[5000];
    DatagramSocket streamSocket;
    DatagramPacket streamPacket;
	DataInputStream inputStream;
	DataOutputStream OutputStream;
	InetAddress ip;
	
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
			//PrintWriter writer = new PrintWriter(String.valueOf(streamSocket.getLocalPort())+".txt");
			byte[] recievedBytes;
			String recievedString;
			Random rand = new Random();
			int packets=0;
			while(++packets>0) {
				try{
					streamSocket.receive(streamPacket);
					//recievedBytes = streamPacket.getData();
				
					//recievedString=new String(recievedBytes,0,streamPacket.getLength());
					recievedString=new String(streamPacket.getData(),streamPacket.getOffset(),streamPacket.getLength());				
					//System.out.println(recievedString);
					recievedBytes=recievedString.getBytes();
					//System.out.println("sending "+recievedBytes.length+" bytes to "+ip);

				
					streamSocket.send(new DatagramPacket(recievedBytes , recievedBytes.length,ip,streamSocket.getLocalPort()));
					//writer.write("\n"+recievedString);
					//writer.flush();
				
				}catch(Exception e){
					System.out.println("ServerUDP ERROR");
					e.printStackTrace();
				}
				
			}
			
		} catch (IOException e) {
				e.printStackTrace();
		}	
		
	}

}
