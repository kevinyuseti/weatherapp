xtramiles

Requires .NET 8 SDK and an OpenWeather API key.
This work as a simple web page. with assumption user always select country before selecting cities.

Setup
Set your API key: by copy and rename appsettings.Development.sample.json into appsettings.Development.json. The api key is available with the email I sent.

Build
dotnet build

Run
dotnet run
Visit /home/countries in the browser.

Test
dotnet test xtramiles.Tests
