CREATE DATABASE attentionTracking;

use attentionTracking;

CREATE TABLE student (
	login_id varchar(32) PRIMARY KEY,
	first_name varchar(32),
	last_name varchar(32)
);

CREATE TABLE teacher (
    login_id varchar(32) PRIMARY KEY,
	administrator BOOLEAN NOT NULL DEFAULT 0,
	pass varchar(255) NOT NULL,
	first_name varchar(32),
	last_name varchar(32)
);

CREATE TABLE course (
	crn int(7) PRIMARY KEY,
	teacher_id varchar(32) NOT NULL,
	name VARCHAR(255),
    startTime time,
    endTime time NOT NULL
);

CREATE TABLE student_courses (
	course_id int(7),
    student_id varchar(32)
);

CREATE TABLE trackingData (
	course_id int NOT NULL,
	student_id int NOT NULL,
	x float,
	y float,
	openApplication varchar(255),
	dataTimestamp dateTime NOT NULL
);