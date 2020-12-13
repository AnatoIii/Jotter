Feature: User login
	User visit website and wants to log in.

@login2Test
Scenario: User was registered before, incorrect data
	Given User is presented in system, but entered incorrect login and password.
	When User entered incorrect login, password "test@gmail.com", "12345673"
	Then He gets an error message
	And He is not logged in
