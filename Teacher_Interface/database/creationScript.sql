-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema attentiontracking
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `attentiontracking` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci ;
USE `attentiontracking` ;

-- -----------------------------------------------------
-- Table `attentiontracking`.`teacher`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `attentiontracking`.`teacher` ;

CREATE TABLE IF NOT EXISTS `attentiontracking`.`teacher` (
  `login_id` VARCHAR(32) NOT NULL,
  `pass` VARCHAR(255) NOT NULL,
  `first_name` VARCHAR(32) NULL DEFAULT NULL,
  `last_name` VARCHAR(32) NULL DEFAULT NULL,
  `administrator` TINYINT(1) NOT NULL DEFAULT '0',
  PRIMARY KEY (`login_id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_general_ci;

-- -----------------------------------------------------
-- Table `attentiontracking`.`course`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `attentiontracking`.`course` ;

CREATE TABLE IF NOT EXISTS `attentiontracking`.`course` (
  `crn` INT(7) NOT NULL,
  `teacher_id` VARCHAR(32) NOT NULL,
  `name` VARCHAR(255) NULL DEFAULT NULL,
  `startTime` TIME NULL DEFAULT NULL,
  `endTime` TIME NOT NULL,
  PRIMARY KEY (`crn`),
  INDEX `fk_teid` (`teacher_id` ASC),
  CONSTRAINT `fk_teid`
    FOREIGN KEY (`teacher_id`)
    REFERENCES `attentiontracking`.`teacher` (`login_id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_general_ci;

-- -----------------------------------------------------
-- Table `attentiontracking`.`student`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `attentiontracking`.`student` ;

CREATE TABLE IF NOT EXISTS `attentiontracking`.`student` (
  `login_id` VARCHAR(32) NOT NULL,
  `first_name` VARCHAR(32) NULL DEFAULT NULL,
  `last_name` VARCHAR(32) NULL DEFAULT NULL,
  PRIMARY KEY (`login_id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_general_ci;

-- -----------------------------------------------------
-- Table `attentiontracking`.`student_courses`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `attentiontracking`.`student_courses` ;

CREATE TABLE IF NOT EXISTS `attentiontracking`.`student_courses` (
  `crn` INT(7) NOT NULL,
  `student_id` VARCHAR(32) NOT NULL,
  INDEX `fk_stcrn` (`crn` ASC),
  INDEX `fk_ststid` (`student_id` ASC),
  CONSTRAINT `fk_stcrn`
    FOREIGN KEY (`crn`)
    REFERENCES `attentiontracking`.`course` (`crn`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `fk_ststid`
    FOREIGN KEY (`student_id`)
    REFERENCES `attentiontracking`.`student` (`login_id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_general_ci;

-- -----------------------------------------------------
-- Table `attentiontracking`.`trackingdata`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `attentiontracking`.`trackingdata` ;

CREATE TABLE IF NOT EXISTS `attentiontracking`.`trackingdata` (
  `crn` INT(7) NOT NULL,
  `student_id` VARCHAR(32) NOT NULL,
  `x` FLOAT NULL DEFAULT NULL,
  `y` FLOAT NULL DEFAULT NULL,
  `openApplication` VARCHAR(255) NULL DEFAULT NULL,
  `dataTimestamp` DATETIME(3) NOT NULL,
  INDEX `fk_trcrn` (`crn` ASC) ,
  INDEX `fk_trid` (`student_id` ASC) ,
  CONSTRAINT `fk_trcrn`
    FOREIGN KEY (`crn`)
    REFERENCES `attentiontracking`.`course` (`crn`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `fk_trid`
    FOREIGN KEY (`student_id`)
    REFERENCES `attentiontracking`.`student` (`login_id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_general_ci;

SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;

#Create user to
drop user teacher@'%';
Flush privileges;
Create user 'teacher'@'%' identified by 'course';
Grant All ON attentiontracking.* to 'teacher'@'%';

#Test data
insert into student values('ws01864','Anthony','Shedlowski'),('pf00684','Phillip','Fernandez'),('Anthony','Anthony','Shedlowski'),('Austin','Austin','Lambeth');

insert into teacher values('aallen','W3IrMH/ObJRJBdEyaR1eSiIUt/6StziSDrP846kEIKGVEcMBCg53ErBU2u9bV7rVnsvZOzKA8hBXj1R/Su1NJQ==','Andrew','Allen',0),
		('pde','W3IrMH/ObJRJBdEyaR1eSiIUt/6StziSDrP846kEIKGVEcMBCg53ErBU2u9bV7rVnsvZOzKA8hBXj1R/Su1NJQ==','Pradipta','De',0),
		('admin','W3IrMH/ObJRJBdEyaR1eSiIUt/6StziSDrP846kEIKGVEcMBCg53ErBU2u9bV7rVnsvZOzKA8hBXj1R/Su1NJQ==','Admin','Istrator',1);

insert into course values(82325,'aallen','Software Engineering','15:30:00','16:45:00'),
						(87055,'aallen','Software Testing and QA','09:05:00','10:20:00'),
						(85620,'pde','Computer Architecture','08:00:00','08:50:00');

insert into student_courses values(82325,'Anthony'),(82325,'Austin'),(85620,'Anthony'),(82325,'ws01864'),(85620,'ws01864');