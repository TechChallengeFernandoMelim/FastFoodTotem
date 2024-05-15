Feature: CreateOrder
    As a user
    I want to be able to create an order
    So that I can purchase items from a restaurant

Scenario: Successfully create an order
    Given the user provides a payment access token "fakeAccessToken"
    And the following items are added to the order:
        | ProductId | Amount |
        | 1         | 2      |
        | 2         | 1      |
    And the order details are mapped to an order entity
    And the product details are retrieved from the product repository
    And the order is successfully added to the order repository
    When the user attempts to create the order
    Then the system should generate a QR code for payment of the order
    And the system should return a response with order details