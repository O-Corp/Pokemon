Feature: Translation
Translating Pokemon Descriptions

    Scenario: Translate Pokemon to Yoda description
        Given a request to translate mewtwo
        When the request is sent
        Then an OK response is returned
        And the response is translated with
          | Name   | Description Standard | Habitat           | Is Legendary        |
          | mewtwo | <ExpectedDecription> | <ExpectedHabitat> | <ExpectedLegendary> |

    Examples:
      | Name   | ExpectedDecription       | ExpectedHabitat | ExpectedLegendary |
      | mewtwo | Ye Olde Shakespeare Talk | cave            | true              |