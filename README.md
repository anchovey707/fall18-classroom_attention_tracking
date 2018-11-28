# fall18-classroom_attention_tracking
Software Engineering Fall 2018, Georgia Southern

Anthony Shedlowski (Project lead)
Philip Fernandez
Bronson Lane
Austin Lambeth
Tony Andrews
Adam Abdellaoui

This project is designed to stream attention data from multiple students(a full class) and multiple concurrent classes. It wil track eye movement, the current window that the student is looking at, username, and timestamp. The server determines what student's should be in which class by looking at the end time for their next class, via database queries.

This project/system is split into three parts, the Server(Java), the Teacher's Interface(C#), and the Student's Data Tracker. It was designed for use with the 'Tobii Eye Tracker 4C', and for a local network.

It uses multiple ports and is heavily multi-threaded. The server has a thread for each client(Teacher or student) and one for each currently streaming course. Each client has threads for the TCP and UDP connections and the teacher has more for updating the UI in realtime.


