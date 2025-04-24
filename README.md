# Star Wars API Project

## Description
This project provides a command-line interface (CLI) for retrieving and displaying Star Wars character information from the SWAPI (Star Wars API) through a custom API middleware. Users can view basic or detailed information about Star Wars characters including their appearances in films.

## Project Structure

### ConsoleUI
Terminal-based interface that allows users to interact with the Star Wars API.  

**Features:**
* Get basic information about a Star Wars character by ID
* Get detailed information about a character including films they appeared in
* Handles and displays API response errors in a user-friendly way

### Custom API
API middleware that serves as an intermediary between the ConsoleUI and the custom API.


**Components:**
* **Base Controllers** - Provides standardized HTTP response handling
* **PeopleController** - Manages requests for Star Wars character data
* **FilmController** - Manages requests for Star Wars film data

### Models
Data models for mapping API responses to C# objects:
* **PersonModel** - Basic character information (Name, Height, Mass)
* **FullPersonModel** - Complete character information including related resources
* **FilmModel** - Film details including title, director, and release date

## Flow of Data
1. User selects an option in the ConsoleUI
2. ConsoleUI calls the API service with the character ID
3. API service communicates with the custom API 
4. The custom API calls the SWAPI external API (swapi.info)
5. SWAPI returns data to the custom API, which processes it
6. Custom API returns data to the API service
7. API service returns processed data to the ConsoleUI
8. ConsoleUI displays the information to the user

## Unit Testing

### ConsoleUI Tests
Tests for the APIService class that handles HTTP communication:
* SuccessfulResponse - Tests proper deserialization of successful API responses
* NotFoundResponse - Tests proper handling of 404 responses

### API Tests

#### BaseStatusCodeControllerTests
Tests the HTTP response handler for various status codes:
* Successful responses (200)
* Not Found responses (404)
* Bad Request responses (400)
* Unauthorized responses (401)
* Forbidden responses (403)
* Internal Server Error responses (500)

#### PeopleControllerTests
Tests the PeopleController endpoints:
* Successful retrieval of person data
* Proper error handling for unauthorized requests
