import java.io.IOException;
import java.net.DatagramSocket;
import java.net.InetAddress;
import java.net.InetSocketAddress;
import java.net.ServerSocket;
import java.net.Socket;
import java.net.SocketTimeoutException;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.Map;

import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;

public class ServerMain {
	//Contains <Course number,port number>
	static Map<Integer,Integer> currentCourses = new HashMap<Integer,Integer>();
	static ArrayList<ClientConnection> studentList = new ArrayList<ClientConnection>(),
			teacherList = new ArrayList<ClientConnection>();
	static  ArrayList<ServerUDP> updSockets = new ArrayList<ServerUDP>();
	static int basePort=61600,UDPPortCount=2;
	//port 61601 is default UDP port
	static boolean database=false;
	
	//Database vars
	static Connection conn;
	
	public static void main(String[] args) {
		try {
			ServerSocket service = new ServerSocket(basePort);
			service.setSoTimeout(2000);
			//Getting database connection
			
			try{
				conn=DriverManager.getConnection("jdbc:mysql://localhost:3306/attentiontracking","root","aannt707");
				database=true;
			}catch(Exception e) {
				System.out.println("!!!!!Couldn't connect to database!!!!!");
			}
			
			//Server running
			while(true) {
				
				try{
					
					System.out.println("Waiting for new connection");
					ClientConnection client = new ClientConnection(service.accept());
					client.start();
					System.out.println("New Conection!");
					//if teacher, then add course to map, create new datagram socket for it, and update students
					if(client.isTeacher) {
						teacherList.add(client);
						currentCourses.put(client.getCourse(), basePort+UDPPortCount);
						ServerUDP UDPSocket = new ServerUDP(client.getIP(),basePort+UDPPortCount);
						updSockets.add(UDPSocket);
						new Thread(UDPSocket).start();
						client.updatePort(basePort+UDPPortCount);
						//still need to tell teacher what port
		
						UDPPortCount++;
						System.out.println("Students changed="+updateStudents());
					}else{
						//else is a student and add to list and update their port if it can
						studentList.add(client);
						updateStudent(client);
					}
				
				}catch(SocketTimeoutException e){
					/*
					for(int i=0;i<studentList.size();i++) {
						//If the student isn't alive then remove them from the list
						if(!studentList.get(i).isRunning()) {
							System.out.println(studentList.get(i).getUser()+" was removed from the list");
							studentList.remove(i--);
						}
					}
					
					for(int i=0;i<teacherList.size();i++) {
						//If the student isn't alive then remove them from the list
						if(!teacherList.get(i).isRunning()) {
							System.out.println(teacherList.get(i).getUser()+" was removed from the list");
							updSockets.remove(i);
							studentList.remove(i--);
						}
					}*/
					
					
					
					
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
		
		System.out.println("!!!!!!!!!!!!!!!!!!!!!!!!!!END OF SERVERMAIN!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
		
	}
	
	
	
	
	
	
	//New class is created, so update students to direct to class
	public static int updateStudents(){
		int changed=0;
		for(int i=0;i<studentList.size();i++) {
			if(currentCourses.containsKey(studentList.get(i).getCourse())) {
				studentList.get(i).updatePort(currentCourses.get(studentList.get(i).getCourse()).intValue());
				System.out.println("found a class");
			
			//else set the student to base port
			}else
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
		//else set the student to base port
		}else
			student.updatePort(basePort+1);
	}
	
	
	//query the database for their current course
	public static void updateCourse(ClientConnection student) {
		if(database) {
			System.out.println("Getting "+student.getUser()+"'s classes");
			try {
				ResultSet courses = retrieve("Select course_crn from courses join users_courses" + 
					" where course_id=courses.id and users_courses.user_id='"+student.getUser()+"';");
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
