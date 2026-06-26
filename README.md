# Cybersecurity Awareness Chatbot - Part 3 (POE)

## Student Information

* Student Name: YOUR NAME
* Student Number: YOUR STUDENT NUMBER
* Module: Programming
* Assessment: POE Part 3


# Project Overview

This project is a Cybersecurity Awareness Chatbot developed using C# and WPF.

The chatbot assists users with cybersecurity education through an interactive graphical interface while integrating task management, a cybersecurity quiz, natural language processing simulation and activity logging.

# Features

## Part 1

* Welcome screen
* Cybersecurity chatbot
* Dynamic responses
* Keyword recognition
* Sentiment detection

## Part 2

* GUI using WPF
* Improved chatbot responses
* Better user interaction

## Part 3

### Task Assistant

* Add cybersecurity tasks
* Save tasks into MySQL
* Reminder dates
* Mark tasks complete
* Delete tasks

### Cybersecurity Quiz

* 12+ Questions
* Multiple Choice
* True / False
* Instant feedback
* Final Score
* Progress Bar

### NLP Simulation

Recognises commands such as

* Add task
* Start quiz
* Remind me
* Password
* Phishing
* Show activity log

using keyword detection and Regex.

### Activity Log

Records:

* Quiz started
* Quiz completed
* Task added
* Task deleted
* Reminder created
* NLP commands


# Technologies Used

* C#
* WPF
* XAML
* MySQL
* Regex
* Visual Studio 2022


# Database

Create database

CREATE DATABASE chatbotdb;

Create table

CREATE TABLE tasks(
id INT AUTO_INCREMENT PRIMARY KEY,
title VARCHAR(255),
description TEXT,
reminderDate DATETIME,
isCompleted BOOLEAN
);


# Running the Project

1. Open the solution in Visual Studio.

2. Restore NuGet packages.

3. Install MySql.Data if required.

4. Configure the database connection.

5. Press F5.


# GitHub Releases

Version 1.0
Base Chatbot

Version 2.0
Task Assistant + Database

Version 3.0
Final Integrated Chatbot


# Future Improvements

* AI integration
* Voice recognition
* Cloud database
* Email reminders
* User login system

# Author

YOUR NAME
