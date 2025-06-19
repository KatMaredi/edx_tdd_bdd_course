Feature: Search for pets by category

As a pet shop customer
I need to be able to search for a pet by category
So that I only see the category of pet I'm interested in buying

    Background:
        Given the following pets
          | name  | category | available |
          | Fido  | Dog      | True      |
          | Kitty | Cat      | True      |
          | Leo   | Lion     | False     |

    Scenario: Search for dogs
        Given I am the "Home Page"
        When I set the "Category" to "dog"
        And I click the "Search" button
        Then I should see the message "Success"
        And I should see "Fido" in the results
        But I should not see "Kitty" in the results
        And I should not see "Leo" in the results