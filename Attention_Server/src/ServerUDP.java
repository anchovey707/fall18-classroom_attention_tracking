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
	byte[] streambuffer = new byte[65536];
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
			PrintWriter writer = new PrintWriter(String.valueOf(streamSocket.getLocalPort())+".txt");
			byte[] recievedBytes;
			String recievedString;
			Random rand = new Random();
			int packets=0;
			while(++packets>0) {
				streamSocket.receive(streamPacket);
				//recievedBytes=new byte[streamPacket.getLength()];
				//System.arraycopy(streamPacket.getData(), streamPacket.getOffset(),recievedBytes,0,streamPacket.getLength());;
				
				recievedBytes = streamPacket.getData();
				
				recievedString=new String(recievedBytes,0,streamPacket.getLength());
				System.out.println(recievedString);
				//System.out.println("\tfrom:"+streamPacket.getAddress().getHostAddress());
				//+": "+recievedString
					//				+"\n\tto "+ip);

				
				streamSocket.send(new DatagramPacket(recievedBytes , recievedBytes.length,ip,streamSocket.getLocalPort()));
				writer.write("\n"+recievedString);
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

}