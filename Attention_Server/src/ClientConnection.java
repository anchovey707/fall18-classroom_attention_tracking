import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.io.IOException;
import java.net.InetAddress;
import java.net.Socket;

public class ClientConnection extends Thread{
	String username;
	int course=1234,port=61600;
	boolean ready=false,isTeacher=false;
	//timing and heartbeat
	int TCPTiming=500,heartbeatMax=20,heartbeat=heartbeatMax;
	
	//TCP connection
	Socket socket;
	InetAddress ip;
	DataInputStream inputStream;
	DataOutputStream OutputStream;
	String output="";
	
	public ClientConnection(Socket s) {
		this(s,1);
	}
	public ClientConnection(Socket s, int triesToConnect){
		//Setting up the socket
		socket=s;
		ip=s.getInetAddress();
		try {
			inputStream=new DataInputStream(s.getInputStream());
			int tries=0;
			//trying to get info
			while(!getInfo()) {
				tries++;
				if(tries>=triesToConnect)
					break;
			}
			if(tries<triesToConnect) {
				System.out.println("\tGot the connection");
				ready=true;
			}
		} catch (IOException e) {
			e.printStackTrace();
		}
	}
	
	//Handshaking with client service to get info
	public boolean getInfo() {
		try {
			
			//Example input:	Student: '#username;'
			//					Teacher: '#username#password;'
			
			//Send ready byte to client
			OutputStream=new DataOutputStream(socket.getOutputStream());
			OutputStream.writeBytes(";");
			System.out.print("\tWrote the starting char, ");
			
			//Get intial input information
			String input="";
			char temp='#';
			System.out.println("\tTrying to get info");
			while(temp!=';') {
				byte[] b=new byte[1];
				b[0]=inputStream.readByte();
				temp=(new String(b,"UTF-8")).charAt(0);
				input+=temp;
			}
			System.out.println("\t'"+input+"'");
			
			//Example input:	Student: '#username;'
			//					Teacher: '#username#course;'
			
			//Parse thy input
			if(input.contains("#")) {
					//Teacher
				if(input.substring(1).contains("#")){
					username=input.substring(1
							,input.indexOf("#",1));
					course=Integer.parseInt(input.substring(input.indexOf("#",1)+1,
							input.indexOf(";")));
					isTeacher=true;
				}else {
					//Student
					username=input.substring(1, input.indexOf(";"));
				}
			}
		} catch (IOException e) {
			e.printStackTrace();
			return false;
		}
		System.out.println("\tuser="+username+" course="+course+" isTeacher="+isTeacher);
		return true;
	}
	
	
	public void run() {
		
		int readyTime=0;
		while(!ready&readyTime++<TCPTiming) {
			//Just giving it time before it tries to write or anything
		}
		
		while(ready) {
			try {
				Thread.sleep(TCPTiming);
				//if there is something to write, then write it
				if(!output.equals("")) {
					System.out.println("\tWriting '"+output+"' to "+username+" '"+socket.getInetAddress()+"'");
					OutputStream.writeBytes(output);
					output="";
				}
				
				//checking if the client is still there, if not then stop running this thread
				if(inputStream.available()>0) {
					heartbeat=heartbeatMax;
					inputStream.readByte();
				}
				heartbeat=heartbeatMax;
				if(heartbeat--<=0)
					ready=false;
				
				
			} catch (IOException | InterruptedException e) {
				output="";
				e.printStackTrace();
			}
		}
		
		//Close the connection
		try {
			System.out.println("\t"+username+" dropped");
			socket.close();
		} catch (IOException e) {}
	}
	
	
	public void Stop() {
		ready=false;}
	
	//getters and setters
	public boolean isTeacher() {
		return isTeacher;}
	public String getUser() {
		return username;}
	public int getCourse() {
		return course;}
	public void setCourse(int c) {
		course=c;}
	
	//Connection getters and setters
	public InetAddress getIP() {
		return ip;}
	public int getPort() {
		return port;}
	public void updatePort(int newPort) {
		port=newPort;
		output="#port:"+newPort+";";}
	public boolean isRunning() {
		return ready;}
	
	
	public void teacherInitialize(){
		//validate user and pass via database query
		
		//if good then also query for their classes
		
		//if not the getInfo() again
		
	}
	
	
	
}