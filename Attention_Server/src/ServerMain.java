import java.io.IOException;
import java.net.DatagramSocket;
import java.net.InetAddress;
import java.net.InetSocketAddress;
import java.net.ServerSocket;
import java.net.Socket;
import java.net.SocketException;
import java.net.SocketTimeoutException;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;

public class ServerMain {
	//Contains <Course number,port number>
	static Map<Integer,Integer> currentCourses = new HashMap<Integer,Integer>();
	static List<ClientConnection> studentList = new ArrayList<ClientConnection>(),
			teacherList = new ArrayList<ClientConnection>();
	
	static List<ServerUDP> udpSockets = new ArrayList<ServerUDP>();
	//port 61601 is default UDP port, nothing listens on that port
	static int basePort=61600;
	static int[] usedPorts = new int[] {0,0,0};
	
	static boolean database=false;
	
	//Database vars
	static Connection conn;
	
	public static void main(String[] args) {
		try {
			//ServerSocket to accept new TCP socket connections for clients(teachers and students)
			ServerSocket server = new ServerSocket(basePort);
			//Setting a timeout so that the serve has a chance to remove unused ports and clientConnections
			server.setSoTimeout(2000);
			
			//Getting database connection
			try{
				DriverManager.registerDriver(new com.mysql.jdbc.Driver());
				conn=DriverManager.getConnection("jdbc:mysql://localhost:3306/attentiontracking","teacher","course");
				database=true;
			}catch(SQLException e) {
				System.out.println("!!!!!Couldn't connect to database!!!!!");
				e.printStackTrace();
			}
			
			//Server running
			while(true) {
				
				try{
					System.out.println("Waiting...");
					ClientConnection client = new ClientConnection(server.accept());
					client.start();
					//if teacher, then add course to map, create new datagram socket for it, and update students
					if(client.isTeacher()) {
						int UDPport = basePort+getAvaliablePort();
						ServerUDP UDPSocket;
						try {
							if(database)
								UDPSocket = new ServerUDP(client.getCourse(),client.getIP(),UDPport,conn);
							else
								UDPSocket = new ServerUDP(client.getCourse(),client.getIP(),UDPport);
							udpSockets.add(UDPSocket);
							new Thread(UDPSocket).start();
							client.updatePort(UDPport);
							teacherList.add(client);
							currentCourses.put(client.getCourse(),UDPport);
						}catch(SocketException e) {
							System.out.println("Teacher tried to connect, but failed");
							removePort(UDPport);
							client.Stop();
						}
					}else{
						//else is a student and add to the list and update their port if it can
						studentList.add(client);
						updateStudent(client);
					}
				
				}catch(SocketTimeoutException e){
					for(int i=0;i<studentList.size();i++) {
						//If the student isn't alive then remove them from the list
						if(!studentList.get(i).isRunning()) {
							System.out.println(studentList.get(i).getUser()+" was removed from the list of Students");
							studentList.remove(i--);
						}
					}
					
					for(int i=0;i<teacherList.size();i++) {
						//If the teacher isn't alive then remove them from the list, remove the class from the map
						//stop and unbind the UDPSocket and open that port number for use
						//updateStudents()
						if(!teacherList.get(i).isRunning()) {
							System.out.println(teacherList.get(i).getUser()+" was removed from the list of Teachers");
							udpSockets.get(i).stop();
							udpSockets.remove(i);
							removePort(teacherList.get(i).getPort()-basePort);
							currentCourses.remove(teacherList.get(i).getCourse());
							teacherList.remove(i--);
							updateStudents();
						}
					}
					
					
					
					
				}
			}
		} catch (IOException e) {
			e.printStackTrace();
		}
		//Stopping all clients
		for(int i=0;i<studentList.size();i++)
			studentList.get(i).Stop();
		for(int i=0;i<teacherList.size();i++)
			teacherList.get(i).Stop();
		for(int i=0;i<udpSockets.size();i++)
			udpSockets.get(i).stop();
		//If you're here, then something went wrong
		System.out.println("!!!!!!!!!!!!!!!!!!!!!!!!!!END OF SERVERMAIN!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
		
	}
	
	
	
	
	
	//This returns an available port, not necessarily available on the system, but not being used by this server
	private static int getAvaliablePort() {
		int port=1,emptyIndex=-1;
		boolean found=true;
		//finding what port can be used. If found is true then it found that port in the array,
		//so increment and try again
		while(found) {
			port++;
			found=false;
			//Checks for the port and for an empty slot to use after it finds a unused port
			for(int i=0;i<usedPorts.length;i++) {
				if(emptyIndex==-1){
					if(usedPorts[i]==0)
						emptyIndex=i;
				}if(port==usedPorts[i]) {
					found=true;
				}
			}
		}
		//If there isn't an empty slot, enlarge the array
		if(emptyIndex==-1) {
			int[] array = new int[usedPorts.length];
			for(int i=0;i<usedPorts.length;i++)
				array[i]=usedPorts[i];
			usedPorts=array;
			emptyIndex=usedPorts.length-1;
		}
		//setting the port in the array
		usedPorts[emptyIndex]=port;
		return port;
	}
	
	//Removes the port from the array of currently used ports
	private static void removePort(int port) {
		for(int i=0;i<usedPorts.length;i++) {
			if(usedPorts[i]==port){
				usedPorts[i]=0;
				break;
			}
		}
	}

	//New class is created, so update students to direct to class
	public static int updateStudents(){
		int changed=0;
		for(int i=0;i<studentList.size();i++) {
			if(currentCourses.containsKey(studentList.get(i).getCourse())&&studentList.get(i).getPort()==basePort+1) {				
				studentList.get(i).updatePort(currentCourses.get(studentList.get(i).getCourse()).intValue());
				System.out.println("found a class");
			
			//else set the student to default port (basePort+1)
			}else if(studentList.get(i).getPort()!=basePort+1)
				studentList.get(i).updatePort(basePort+1);
			changed++;
		}
		return changed;
	}
	
	//update one student's port
	public static void updateStudent(ClientConnection student){
		//if the course exist then get the port and tell the client to change to it
		//Update the student's course first
		updateCourse(student);
		//then check if it exist
		if(currentCourses.containsKey(student.getCourse())) {
			student.updatePort(currentCourses.get(student.getCourse()));
			System.out.println("found a class");
		//else set the student to default port (basePort+1)
		}else
			student.updatePort(basePort+1);
	}
	
	
	//query the database for their current course
	public static void updateCourse(ClientConnection student) {
		if(database) {
			System.out.println("Getting "+student.getUser()+"'s classes");
			try {
				ResultSet courses = retrieve("SELECT course.crn, course.endTime - CURRENT_TIME() AS timeDiff " + 
												"FROM course " + 
												"JOIN student_courses " +
												"ON course.crn=student_courses.crn " +
												"WHERE student_courses.student_id='"+student.getUser()+"' AND course.endTime - CURRENT_TIME() > 0 " + 
												"ORDER BY timeDiff DESC;");
				while(courses.next()) {
					System.out.println(courses.getInt(1));
					student.setCourse(courses.getInt(1));	
				}
			} catch (SQLException e) {
				e.printStackTrace();
			}
		}
	}
	
	//Baisc sql statment method
	public static ResultSet retrieve (String q) throws SQLException{
		Statement state;
		state = conn.createStatement();
		System.out.println("Retrieve="+q);
		state.execute(q);
		ResultSet result = state.getResultSet();
		return result;
	}
	
	
}
