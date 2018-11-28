import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.io.IOException;
import java.io.PrintWriter;
import java.net.DatagramPacket;
import java.net.DatagramSocket;
import java.net.InetAddress;
import java.net.SocketException;
import java.sql.Connection;
import java.sql.SQLException;
import java.sql.Statement;
import java.util.Random;

public class ServerUDP implements Runnable{
	private byte[] streambuffer = new byte[5000];
	private DatagramSocket streamSocket;
	private DatagramPacket streamPacket;
	private DataInputStream inputStream;
	private DataOutputStream OutputStream;
	private Connection DBconn;
	private boolean run=true,database=true;
	private int crn;
	private InetAddress ip;
	
	//Get the crn,the target IP(teacher),a datagramSocket, and a DB Connection
	public ServerUDP(int course,InetAddress i,DatagramSocket d,Connection conn) {
		ip=i;
		streamSocket=d;
		streamPacket = new DatagramPacket(streambuffer, streambuffer.length);
		crn=course;
		if(conn==null)
			database=false;
		else
			DBconn=conn;
		System.out.println("\t\tNew Datagram socket on "+d.getLocalPort());
	}
	//Or just give a port number and it will create a socket
	public ServerUDP(int crn,InetAddress i, int port, Connection conn) throws SocketException {
		this(crn,i,new DatagramSocket(port),conn);
	}
	//or don't give it a DB connection and it will not do anything with a DB
	public ServerUDP(int crn,InetAddress i,int port) throws SocketException {
		this(crn,i,new DatagramSocket(port),null);
	}

	
	public void run() {
		byte[] recievedBytes;
		String recievedString;
		//Setting a timeout will allow for the thread to get a chance to stop if it needs to
		try {streamSocket.setSoTimeout(1000);
		} catch (SocketException e1) {}
		
		while(run) {
			try{
				//get a packet in and send it off to the teacher
				streamSocket.receive(streamPacket);
				recievedString=new String(streamPacket.getData(),streamPacket.getOffset(),streamPacket.getLength());	
				recievedBytes=recievedString.getBytes();
				streamSocket.send(new DatagramPacket(recievedBytes , recievedBytes.length,ip,streamSocket.getLocalPort()));
				//try to write the data to the database
				if(database) {
					String user,posX,posY,time,app;
					int index=1;
                    user = recievedString.substring(index, recievedString.indexOf('#', index));
                    index += user.length() + 1;
					posX = recievedString.substring(index, recievedString.indexOf('#', index));
                    index += posX.length() + 1;
					posY = recievedString.substring(index, recievedString.indexOf('#', index));
                    index += posY.length() + 1;
                    time = recievedString.substring(index, recievedString.indexOf('#', index));
                    index += time.length() + 1;
					//Removing milliseconds
					//time=time.substring(0,time.indexOf('.'));
                    try {
                        app = recievedString.substring(index,recievedString.indexOf(';'));
                    }catch(Exception e){
                        app = "";
                    }
					try {
						Statement state = DBconn.createStatement();
						state.execute("Insert into trackingdata values("+crn+",'"+user+"',"+posX+","+posY+",'"+app+"','"+time+"');");
					} catch (SQLException e) {
						System.out.println("database insert failed Failed");
						e.printStackTrace();
						
					}
				
				}
				
			}catch(IOException e){
				//
				//System.out.println("ServerUDP ERROR");
			}
			
		}
		System.out.println("\t\tclosed the socket/port on"+streamSocket.getLocalPort());
		streamSocket.close();
		
	}

	
	public void stop() {
		run=false;
	}
}
