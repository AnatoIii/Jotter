Feature: User login
	User visit website and wants to log in.
	
@login3Test
Scenario: User visited website for the first time, incorrect data
Given User is not presented in system and such email is registered.
	When User registered "test@gmail.com", "12345678", "My name" 
	Then User gets an error mesasge
