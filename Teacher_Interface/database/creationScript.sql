CREATE DATABASE attentionTracking;

use attentionTracking;

CREATE TABLE student (
	id INT PRIMARY KEY AUTO_INCREMENT,
	login_id varchar(32),
	first_name varchar(32),
	last_name varchar(32)
);

CREATE TABLE teacher (
	id INT PRIMARY KEY AUTO_INCREMENT,
    login_id varchar(32),
	administrator BOOLEAN NOT NULL DEFAULT 0,
	pass varchar(255),
	first_name varchar(32),
	last_name varchar(32)
);

CREATE TABLE course (
	id INT PRIMARY KEY AUTO_INCREMENT,
	teacher_id INT,
	crn int(7),
	name VARCHAR(255),
    startTime time,
    endTime time
);

CREATE TABLE course_occurrence (
	id INT PRIMARY KEY AUTO_INCREMENT,
    course_id INT,
    course_day VARCHAR(32)
);


CREATE TABLE trackingData (
	course_id int,
	student_id int,
	x float,
	y float,
	openApplication varchar(255),
	dataTimestamp dateTime
);