﻿Feature: Translation
Translating Pokemon Descriptions

    Scenario: Translate Pokemon to Yoda description
        Given the pokemon <Name> exists
        And its habitat is <ExpectedHabitat>
        And its legendary status is <ExpectedLegendary>
        When the POST request is sent
        Then an OK response is returned
        And the response is translated with
          | Name   | Description Standard | Habitat           | Is Legendary        |
          | <Name> | <ExpectedDecription> | <ExpectedHabitat> | <ExpectedLegendary> |

    Examples:
      | Name      | ExpectedDecription       | ExpectedHabitat | ExpectedLegendary |
      | mewtwo    | Ye Olde Shakespeare Talk | cave            | true              |
      | dragonite | Ye Olde Shakespeare Talk | rare            | true              |