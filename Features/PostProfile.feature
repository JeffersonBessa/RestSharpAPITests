Feature: PostProfile
	Test POST operation using RESTSharp Library

Background: 
	Given I get JWT authentication of User with following details
		| Email             | Password |
		| karthik@email.com | haha123  |

@smoke
Scenario: Verify Post operation for Profile
	Given I perform POST operation for "/posts/{profileNo}/profile" with body
	| name | profile |
	| Sams | 2       |
	Then I should see the "name" name as "Sams"
