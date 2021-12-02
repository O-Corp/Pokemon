Feature: Translation
Translating Pokemon Descriptions

    Scenario: Translate Pokemon to Yoda description
        Given the pokemon <Name> exists
        And its habitat is <ExpectedHabitat>
        And its legendary status is <ExpectedLegendary>
        When the POST request is sent
        Then an OK response is returned
        And the Yoda translation API is called
        And the POST response is
          | Name   | Description Standard  | Habitat           | Is Legendary        |
          | <Name> | <ExpectedDescription> | <ExpectedHabitat> | <ExpectedLegendary> |

    Examples:
      | Name      | ExpectedDescription               | ExpectedHabitat | ExpectedLegendary |
      | mewtwo    | Fear is the path to the dark side | cave            | true              |
      | dragonite | Fear is the path to the dark side | rare            | true              |

    Scenario: Translate Pokemon to Shakespeare description
        Given the pokemon <Name> exists
        And its habitat is <ExpectedHabitat>
        And its legendary status is <ExpectedLegendary>
        When the POST request is sent
        Then an OK response is returned
        And the Shakespeare translation API is called
        And the POST response is
          | Name   | Description Standard  | Habitat           | Is Legendary        |
          | <Name> | <ExpectedDescription> | <ExpectedHabitat> | <ExpectedLegendary> |

    Examples:
      | Name    | ExpectedDescription                       | ExpectedHabitat | ExpectedLegendary |
      | pikachu | Give every man thy ear, but few thy voice | common          | false             |