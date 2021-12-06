Feature: BasicPokemonInfo
Return basic standard information about a Pokemon

    Scenario: Successfully retrieve basic information about a Pokemon
        Given that the pokemon mewtwo exists 
        When a request is made to get information about mewtwo in language of <Language>
        Then an OK response is returned
        And a request is made to the Pokemon API
        And the response is
          | Name   | Description Standard | Habitat | Is Legendary |
          | Mewtwo | <Description>        | Rare    | true         |

    Examples:
      | Language | Description                        |
      | en       | It was created by a scientist.     |
      | fr       | Il a été créé par un scientifique. |
      |          | It was created by a scientist.     |

    Scenario: Attemping to retrieve information about a Pokemon that does not exist
        Given the pokemon foobar does not exist
        When the request is sent to get information about foobar
        Then an NotFound response is returned

    Scenario: Third party is unavailable
        Given the third party is unavailable
        When the request is sent to get information about arcanine
        Then an ServiceUnavailable response is returned