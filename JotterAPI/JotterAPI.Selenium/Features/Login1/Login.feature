Feature: User login
	User visit website and wants to log in.

@login1Test
Scenario: User was registered before
	Given User presented in system and entered correct login and password.
	When User entered correct login password "test@gmail.com", "12345678"
	Then He successfully logged in
	And He can get a list of his categories