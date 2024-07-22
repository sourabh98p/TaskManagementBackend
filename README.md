I have develop Task Management API in .net 6 

![image](https://github.com/user-attachments/assets/4ef1cf3f-552e-4e1d-b232-c07ce79f9bc0)

postgresql schema 

CREATE TABLE TeamMembers (
    id SERIAL PRIMARY KEY,
    teamid INT NOT NULL,
    userid INT NOT NULL
);

CREATE TABLE Teams (
    id SERIAL PRIMARY KEY,
    teamname VARCHAR(255) NOT NULL
);

CREATE TABLE Notes (
    id SERIAL PRIMARY KEY,
    taskid INT NOT NULL,
    content TEXT,
    createdby INT NOT NULL,
    createdat TIMESTAMP 
);

CREATE TABLE Tasks (
    id SERIAL PRIMARY KEY,
    title VARCHAR(255) NOT NULL,
    description TEXT,
    status VARCHAR(50) NOT NULL,
    duedate DATE,
    createdby INT NOT NULL,
    assignedto INT NOT NULL,
    completedat TIMESTAMP,
    createdat TIMESTAMP ,
    updatedat TIMESTAMP 
);

CREATE TABLE Users (
    userid SERIAL PRIMARY KEY,
    email VARCHAR(255) NOT NULL,
    password VARCHAR(255) NOT NULL,
    fullname VARCHAR(255) NOT NULL,
    role VARCHAR(50) NOT NULL 
);

CREATE TABLE Attachments (
    id SERIAL PRIMARY KEY,
    taskid INT NOT NULL,
    filename VARCHAR(255) NOT NULL,
    filepath VARCHAR(255) NOT NULL,
    uploadedby INT NOT NULL,
    uploadedat TIMESTAMP 
);
