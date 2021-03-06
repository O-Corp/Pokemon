Feature: Translation
Translating Pokemon Descriptions

    Scenario: Translate Pokemon to Yoda description
        Given the pokemon exists
          | Name   | Habitat   | Is Legendary | Description   |
          | <Name> | <Habitat> | <Legendary>  | <Description> |
        When the POST request is sent
        Then an OK response is returned
        And the <Translation> translation API is called
        And the POST response is
          | Name   | Description Standard     | Habitat   | Is Legendary |
          | <Name> | <Translated Description> | <Habitat> | <Legendary>  |

    Examples:
      | Name      | Description            | Translated Description                     | Habitat | Legendary | Translation |
      | Mewtwo    | Created by scientists. | Fear is the path to the dark side.         | Cave    | true      | Yoda        |
      | Dragonite | Favourite Dragon type. | Fear is the path to the dark side.         | Rare    | true      | Yoda        |
      | Pikachu   | Pika Pika!             | Give every man thy ear, but few thy voice. | Common  | false     | Shakespeare |

#    Scenario: Invalid payload return a bad request
#        Given the pokemon exists
#          | Name      | Habitat | Is Legendary | Description          |
#          | Dragonite | rare    | true         | Dragon type Pokemon. |
#        When an invalid POST request is sent
#        Then an BadRequest response is returned
#        And the validation error is
#        | Error Code  | Error Message          |
#        | InvalidText | Text must not be empty. |

    Scenario: Fallback to standard description as translation API is unavailable
        Given the translation API is unavailable
        And the pokemon exists
          | Name   | Habitat   | Is Legendary | Description   |
          | <Name> | <Habitat> | <Legendary>  | <Description> |
        When the POST request is sent
        Then an OK response is returned
        And the POST response is
          | Name   | Description Standard | Habitat   | Is Legendary |
          | <Name> | <Description>        | <Habitat> | <Legendary>  |

    Examples:
      | Name   | Habitat    | Legendary | Description                                                            |
      | Zekrom |            | true      | Concealing itself in lightning clouds, flying through the Unova region |
      | Entei  | Grasslands | true      | Volcanoes erupt when it barks.                                         |