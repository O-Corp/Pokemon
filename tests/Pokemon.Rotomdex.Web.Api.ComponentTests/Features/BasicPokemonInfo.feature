Feature: BasicPokemonInfo
Return basic standard information about a Pokemon

    Scenario: Successfully retreive basic information about a Pokemon
        Given a valid request to retrieve information about Mewtwo
        When the request is sent
        Then an OK response is returned
        And the response is
          | Name   | Description Standard           | Habitat | Is Legendary |
          | Mewtwo | It was created by a scientist. | Rare    | true         |