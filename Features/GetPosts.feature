Feature: GetPosts
	Test GET posts operation with Restsharp.net
	
Background: 
	Given I get JWT authentication of User with following details
		| Email             | Password |
		| karthik@email.com | haha123  |

Scenario: Verify author of the posts 1 
	Given I perform GET operation for "posts/{postid}"
	And I perform operation for post "1"
	Then I should see the "author" name as "Karthik KK"

Scenario: Verify author of the posts 2
	Given I perform GET operation for "posts/{postid}"
	And I perform operation for post "2"
	Then I should see the "author" name as "Karthik KK"

Scenario: Verify author of the posts 13
	Given I perform GET operation for "posts/{postid}"
	And I perform operation for post "13"
	Then I should see the "author" name as "Execute Automation"