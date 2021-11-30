﻿Feature: BasicPokemonInfo
Return basic standard information about a Pokemon

    Scenario: Successfully retreive basic information about a Pokemon
        Given a valid request to retrieve information about mewtwo
        When the request is sent
        Then an OK response is returned
        And a request is made to the Pokemon API
        And the response is
          | Name   | Description Standard           | Habitat | Is Legendary |
          | mewtwo | It was created by a scientist. | rare    | true         |