# Net6_Angular15_Oauth2_Secure_HttpOnly_Cookie
**Net 6 and Angular 15 Project with Jwt saved into a 'Secure' and 'HttpOnly' Cookie**

- **Program.cs** is where the Cookie is checked for Authorization.
- **LoginController** is where the Cookie is created after a succesful Login. I omitted the credentials validation.
- Inside **LoginController** there is a method '**IsLoggedIn**' used to check if the user is Authorized.
- Also **WeatherForecast** Controller has is **get** method protected.