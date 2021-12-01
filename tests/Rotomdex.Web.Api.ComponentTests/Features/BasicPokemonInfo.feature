Feature: BasicPokemonInfo
Return basic standard information about a Pokemon

    Scenario: Successfully retrieve basic information about a Pokemon
        Given a valid request to retrieve information about mewtwo
        And with language of <Language>
        When the request is sent
        Then an OK response is returned
        And a request is made to the Pokemon API
        And the response is
          | Name   | Description Standard | Habitat | Is Legendary |
          | mewtwo | <Description>        | rare    | true         |

    Examples:
      | Language | Description                        |
      | en       | It was created by a scientist.     |
      | fr       | Il a été créé par un scientifique. |

    Scenario: Attemping to retrieve information about a non-existent pokemon
        Given an invalid pokemon
        When the request is sent
        Then an NotFound response is returned

    Scenario: Third party is unavailable
        Given a valid request to retrieve information about arcanine
        And the third party is unavailable
        When the request is sent
        Then an ServiceUnavailable response is returned