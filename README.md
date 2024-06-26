﻿# Employees_Api


DB - Oracle

Tables:  LEVANIGMZ_EMPLOYEE (ID, FIRST_NAME, LAST_NAME, AGE, PERSON_ID) 
LEVANIGMZ_EMPLOYEE_PHONES (ID, EMPLOYEE_ID, PHONE_NUMBER)
LEVANIGMZ_EMPLOYEE_PROFESSIONS (ID, EMPLOYEE_ID, PROFESSION)

package spec:
CREATE OR REPLACE PACKAGE OLERNING.LEVANIGMZA_PKG_EMPLOYEE AS
  PROCEDURE Employee_Add(p_first_name IN VARCHAR2,
                         p_last_name  IN VARCHAR2,
                         p_age        IN NUMBER,
                         p_person_id  IN NUMBER,
                         p_profession IN VARCHAR2,
                         p_phone_number IN VARCHAR2);
  
  PROCEDURE Employee_Update(p_first_name    IN VARCHAR2,
                            p_last_name     IN VARCHAR2,
                            p_age           IN NUMBER,
                            p_person_id     IN NUMBER,
                            p_new_person_id IN NUMBER,
                            p_profession    IN VARCHAR2,
                            p_phone_number  IN VARCHAR2);
  
  PROCEDURE Employee_Delete(p_person_id IN NUMBER);
  
  PROCEDURE Get_Employee(p_person_id  IN NUMBER,
                         p_first_name OUT VARCHAR2,
                         p_last_name  OUT VARCHAR2,
                         p_age        OUT NUMBER,
                         p_profession OUT VARCHAR2,
                         p_phone_number OUT VARCHAR2);
  
  PROCEDURE Get_Employee_ALL(p_employees OUT SYS_REFCURSOR);
END LEVANIGMZA_PKG_EMPLOYEE;

/


Body:

CREATE OR REPLACE PACKAGE BODY OLERNING.LEVANIGMZA_PKG_EMPLOYEE AS

  PROCEDURE Employee_Add(p_first_name IN VARCHAR2,
                         p_last_name  IN VARCHAR2,
                         p_age        IN NUMBER,
                         p_person_id  IN NUMBER,
                         p_profession IN VARCHAR2,
                         p_phone_number IN VARCHAR2) IS
    person_count NUMBER;
    new_id NUMBER;
  BEGIN
    SELECT COUNT(*)
      INTO person_count
      FROM LEVANIGMZ_EMPLOYEE
     WHERE PERSON_ID = p_person_id;
  
    IF person_count > 0 THEN
      RAISE_APPLICATION_ERROR(-20006, 'Person with ID ' || p_person_id || ' already exists.');
    END IF;

    IF LENGTH(p_first_name) > 50 OR LENGTH(p_first_name) < 2 THEN
      RAISE_APPLICATION_ERROR(-20001, 'First name must be between 2 and 50 characters long');
    END IF;
  
    IF LENGTH(p_last_name) > 50 OR LENGTH(p_last_name) < 2 THEN
      RAISE_APPLICATION_ERROR(-20002, 'Last name must be between 2 and 50 characters long');
    END IF;

    IF LENGTH(p_profession) > 100 OR LENGTH(p_profession) < 2 THEN
      RAISE_APPLICATION_ERROR(-20003, 'Profession must be between 2 and 100 characters long');
    END IF;

    IF p_age < 18 OR p_age > 99 THEN
      RAISE_APPLICATION_ERROR(-20004, 'Age must be between 18 and 99');
    END IF;
  
    INSERT INTO LEVANIGMZ_EMPLOYEE (ID, FIRST_NAME, LAST_NAME, AGE, PERSON_ID)
    VALUES (LEVANIGMZA_EMPLOYEES_SEQ.NEXTVAL, p_first_name, p_last_name, p_age, p_person_id)
    RETURNING ID INTO new_id;
    
    INSERT INTO LEVANIGMZ_EMPLOYEE_PHONES (ID, EMPLOYEE_ID, PHONE_NUMBER)
    VALUES (LEVANIGMZA_EMPLOYEES_SEQ.NEXTVAL, new_id, p_phone_number);

    INSERT INTO LEVANIGMZ_EMPLOYEE_PROFESSIONS (ID, EMPLOYEE_ID, PROFESSION)
    VALUES (LEVANIGMZA_EMPLOYEES_SEQ.NEXTVAL, new_id, p_profession);
  END Employee_Add;

  PROCEDURE Employee_Update(p_first_name    IN VARCHAR2,
                            p_last_name     IN VARCHAR2,
                            p_age           IN NUMBER,
                            p_person_id     IN NUMBER,
                            p_new_person_id IN NUMBER,
                            p_profession    IN VARCHAR2,
                            p_phone_number  IN VARCHAR2) IS
    emp_id NUMBER;
  BEGIN
    IF LENGTH(p_first_name) > 50 OR LENGTH(p_first_name) < 2 THEN
      RAISE_APPLICATION_ERROR(-20001, 'First name must be between 2 and 50 characters long');
    END IF;

    IF LENGTH(p_last_name) > 50 OR LENGTH(p_last_name) < 2 THEN
      RAISE_APPLICATION_ERROR(-20002, 'Last name must be between 2 and 50 characters long');
    END IF;

    IF LENGTH(p_profession) > 100 OR LENGTH(p_profession) < 2 THEN
      RAISE_APPLICATION_ERROR(-20003, 'Profession must be between 2 and 100 characters long');
    END IF;

    IF p_age < 18 OR p_age > 99 THEN
      RAISE_APPLICATION_ERROR(-20004, 'Age must be between 18 and 99');
    END IF;

    SELECT ID INTO emp_id FROM LEVANIGMZ_EMPLOYEE WHERE PERSON_ID = p_person_id;
  
    UPDATE LEVANIGMZ_EMPLOYEE
       SET FIRST_NAME = p_first_name,
           LAST_NAME  = p_last_name,
           AGE        = p_age,
           PERSON_ID  = p_new_person_id
     WHERE ID = emp_id;

    UPDATE LEVANIGMZ_EMPLOYEE_PROFESSIONS
       SET PROFESSION = p_profession
     WHERE EMPLOYEE_ID = emp_id;

    UPDATE LEVANIGMZ_EMPLOYEE_PHONES
       SET PHONE_NUMBER = p_phone_number
     WHERE EMPLOYEE_ID = emp_id;
  END Employee_Update;
  
  

  PROCEDURE Employee_Delete(p_person_id IN NUMBER) IS
    emp_id NUMBER;
  BEGIN
    SELECT ID INTO emp_id FROM LEVANIGMZ_EMPLOYEE WHERE PERSON_ID = p_person_id;

    DELETE FROM LEVANIGMZ_EMPLOYEE_PHONES WHERE EMPLOYEE_ID = emp_id;
    DELETE FROM LEVANIGMZ_EMPLOYEE_PROFESSIONS WHERE EMPLOYEE_ID = emp_id;
    DELETE FROM LEVANIGMZ_EMPLOYEE WHERE ID = emp_id;
  END Employee_Delete;

  PROCEDURE Get_Employee(p_person_id  IN NUMBER,
                         p_first_name OUT VARCHAR2,
                         p_last_name  OUT VARCHAR2,
                         p_age        OUT NUMBER,
                         p_profession OUT VARCHAR2,
                         p_phone_number OUT VARCHAR2) IS
    emp_id NUMBER;
  BEGIN
    SELECT ID, FIRST_NAME, LAST_NAME, AGE
      INTO emp_id, p_first_name, p_last_name, p_age
      FROM LEVANIGMZ_EMPLOYEE
     WHERE PERSON_ID = p_person_id;
  
    SELECT PROFESSION
      INTO p_profession
      FROM LEVANIGMZ_EMPLOYEE_PROFESSIONS
     WHERE EMPLOYEE_ID = emp_id;


    SELECT PHONE_NUMBER
      INTO p_phone_number
      FROM LEVANIGMZ_EMPLOYEE_PHONES
     WHERE EMPLOYEE_ID = emp_id;

  EXCEPTION
    WHEN NO_DATA_FOUND THEN
      RAISE_APPLICATION_ERROR(-20005, 'Person ID does not exist');
  END Get_Employee;

  PROCEDURE Get_Employee_ALL(p_employees OUT SYS_REFCURSOR) IS
  BEGIN
    OPEN p_employees FOR
      SELECT e.FIRST_NAME, e.LAST_NAME, e.AGE, e.PERSON_ID, p.PROFESSION, ph.PHONE_NUMBER
        FROM LEVANIGMZ_EMPLOYEE e
        JOIN LEVANIGMZ_EMPLOYEE_PROFESSIONS p ON e.ID = p.EMPLOYEE_ID
        JOIN LEVANIGMZ_EMPLOYEE_PHONES ph ON e.ID = ph.EMPLOYEE_ID;
  END Get_Employee_ALL;

END LEVANIGMZA_PKG_EMPLOYEE;

/

