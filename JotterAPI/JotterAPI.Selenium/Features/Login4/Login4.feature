Feature: User login
	User visit website and wants to log in.
	
@login4Test
Scenario: User visited website for the first time
	Given User is not presented in system and such email is not registered.
	When User registered with credentials "test1@gmail.com", "12345678", "My name"
	Then He successfully created and logged in
