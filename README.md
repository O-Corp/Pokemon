# Pokemon API
This is my implementation of the Pokemon API and includes the following:
-  Retrieving Pokemon based on name
-  Translation based on the rules defined in spec

<br />

## Pre-Requisites
- Install dotnet 5 as minimum which can be found [here](https://dotnet.microsoft.com/download/dotnet)
- Install specflow extension in IDE to view and run tests, instructions can be found [here](https://docs.specflow.org/projects/getting-started/en/latest/index.html)

<br />

> If using Visual Studio 2019 please ensure you have the latest version as it won't detect the .NET 5 `Target Framework`. To do this launch Visual Studio Installer and update your installation to `16.11.6`.

<br />

## How to Launch the API
Once you have pulled the source, launch your choice of IDE (this was developed using JetBrains Rider). Build the solution or directly hit F5 which should launch the API locally.

<br />

> If the launch URI doesn't show the *Swagger UI* then append this to the URL `/swagger`

<br />

![](https://raw.githubusercontent.com/O-Corp/PaymentGateway/main/rider.gif?token=ABNY5DCSOGJ6XPZMTGMFF6TBSPQL4)

<br />


## What I Would Do Differently for Production
- Use a distributed cache such as Redis
- Multi regional deploys (two regions as a minimum)
- Alerting based on custom metrics / exceptions
- Traffic Manager weighted 50/50
- Green Blue Deployment
- Implement `acceptance tests` and `integration tests` that run after deploy

<br />

## Bonus
- I have implemented a continuous integration pipeline using Github actions
- Added Swagger documentation which is accessible via `https://localhost:5001/swagger/` after launching API
- You can access the deployed instance via
