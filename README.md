# Seges.Samples.OAuth
Requires [.NET Core SDK 2.0](https://www.microsoft.com/net/download/windows) or newer

Run with

`dotnet run`

The authorize url will be printed, e.g.

```
Paste this url in browser, authenticate, and paste the resulting browser url back here:
####################################################################################
https://si-authzserver.vfltest.dk/SEGESOAuthSampleApi_DEBUG/oauth/authorize?client_id=SEGESOAuthSampleClient_DEBUG&response_type=code&scope=Default&redirect_uri=https%3A%2F%2Fwww.example.com%2Fsegesoauthsample_debug%2F&state=818a55c60f1ffda123552fe17211cd940bf6372d1f15b7a550ce115cc1546c08&nonce=2aed3227960134662e03bb4a98790f9542ea3ff772af924784f030332aef6f96&response_mode=fragment
####################################################################################
The browser will redirect to the return url, containing the authorization code in the fragment.
Enter the return url with fragment:
```
Follow the instructions, and paste the url into a browser window - step A of the [Authorization Code Flow](https://tools.ietf.org/html/rfc6749#section-4.1). 
If not previously authenticated, you will be prompted to authenticate with [DLBR Common Login](https://confluence.seges.dk/display/PUB/DLBR+Common+Login).
Do so using one of the [demo logins](https://confluence.seges.dk/display/PUB/Demo+Logins) - step B. 


When a user has been authenticated, the authorization server will redirect the browser to the redirect uri with an authorization code - step C.

As the response_mode is fragment, the authorization code is found in the url fragment of the return url. Note that typically fragment will not be used in a real application, as it is less secure than the alternatives.

Copy the url, which will be similar to
```
https://www.example.com/segesoauthsample_debug/?code=f27d67f532194420acb58b0be85751a8&state=818a55c60f1ffda123552fe17211cd940bf6372d1f15b7a550ce115cc1546c08
```
into the console and press enter.
The sample will then exchange the authorization code from the redirect uri for an access token / refresh token pair - step D+E. 

Note that this call (and the subsequent calls using the refresh token) is expected to be performed by a confidential client, i.e. using backend code. 

The tokens will be written to the console alongside the decoded access token contents:
```
Exchanging authorization code f27d67f532194420acb58b0be85751a8 for refresh/access tokens
####################################################################################
Token claims
client_id : SEGESOAuthSampleClient_DEBUG
scope : Default
sub : cvruser1@PROD.DLI
iss : https://si-authzserver.vfltest.dk/
aud : urn:SEGESOAuthSampleApi_DEBUG
exp : 1523311435
nbf : 1523307835
Expires: 3600
Access token: eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6Ijg3SE1Ta1Y4ZVA2ajItVWZPeFFiZzlGS29ldyJ9.eyJjbGllbnRfaWQiOiJTRUdFU09BdXRoU2FtcGxlQ2xpZW50X0RFQlVHIiwic2NvcGUiOiJEZWZhdWx0Iiwic3ViIjoiY3ZydXNlcjFAUFJPRC5ETEkiLCJpc3MiOiJodHRwczovL3NpLWF1dGh6c2VydmVyLnZmbHRlc3QuZGsvIiwiYXVkIjoidXJuOlNFR0VTT0F1dGhTYW1wbGVBcGlfREVCVUciLCJleHAiOjE1MjMzMTE0MzUsIm5iZiI6MTUyMzMwNzgzNX0.Zu4W1asyCR_a2-pY9vtIvkvIW7vC3S228gQuU6dejssNDt5z72UpRnxKBFq1O4Y9Zpqp0-SZZjL9vWoGFXaSUCoH64-yUc7hwqlcmLx3ZM5V2Dg2vaAvRYUDWlL3-g-JXowPPo4kqT6PPmrxKXxuKEu_bXBEqpPoif5VeQ2ETqkQK0oeS7ZiyV0EARnSQZwndhJAW4rA9yPQbv0_Vu6sdTJXrWU40-S0AoXUvGI1qCkouQw-eNCV5G-4sOO0yRCw_n1Q9LmIfuE-7-retEyZaNoltnW3udDGUKYq8gK5gwJHen7wHl66gfJavqdvy81jeBNxPrCP4JAby47Gt1O0zA
Refresh token: b8c93f6d327848f49b75b83e16f6e75b
####################################################################################
```
The sample will then exchange the refresh token for an access token / refresh token pair - step G+H from the [Refresh Token](https://tools.ietf.org/html/rfc6749#section-1.5) usage illustration.

Note that this call can fail if the refresh token has been revoked or otherwise invalidated - in that case the flow should be restarted.
```
Exchanging refresh token b8c93f6d327848f49b75b83e16f6e75b for fresh refresh/access tokens
####################################################################################
Token claims
client_id : SEGESOAuthSampleClient_DEBUG
scope : Default
sub : cvruser1@PROD.DLI
iss : https://si-authzserver.vfltest.dk/
aud : urn:SEGESOAuthSampleApi_DEBUG
exp : 1523311435
nbf : 1523307835
Expires: 3600
Access token: eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6Ijg3SE1Ta1Y4ZVA2ajItVWZPeFFiZzlGS29ldyJ9.eyJjbGllbnRfaWQiOiJTRUdFU09BdXRoU2FtcGxlQ2xpZW50X0RFQlVHIiwic2NvcGUiOiJEZWZhdWx0Iiwic3ViIjoiY3ZydXNlcjFAUFJPRC5ETEkiLCJpc3MiOiJodHRwczovL3NpLWF1dGh6c2VydmVyLnZmbHRlc3QuZGsvIiwiYXVkIjoidXJuOlNFR0VTT0F1dGhTYW1wbGVBcGlfREVCVUciLCJleHAiOjE1MjMzMTE0MzUsIm5iZiI6MTUyMzMwNzgzNX0.Zu4W1asyCR_a2-pY9vtIvkvIW7vC3S228gQuU6dejssNDt5z72UpRnxKBFq1O4Y9Zpqp0-SZZjL9vWoGFXaSUCoH64-yUc7hwqlcmLx3ZM5V2Dg2vaAvRYUDWlL3-g-JXowPPo4kqT6PPmrxKXxuKEu_bXBEqpPoif5VeQ2ETqkQK0oeS7ZiyV0EARnSQZwndhJAW4rA9yPQbv0_Vu6sdTJXrWU40-S0AoXUvGI1qCkouQw-eNCV5G-4sOO0yRCw_n1Q9LmIfuE-7-retEyZaNoltnW3udDGUKYq8gK5gwJHen7wHl66gfJavqdvy81jeBNxPrCP4JAby47Gt1O0zA
Refresh token: b8c93f6d327848f49b75b83e16f6e75b
####################################################################################
```
Finally, the sample illustrates the client side of using the access token to perform authorized access to a fake API, by transmitting it as a Bearer token in the Authorization header. The sample does not include an actual API, nor does it demonstrate token validation inside the API.
```
Making fake API call with access token eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6Ijg3SE1Ta1Y4ZVA2ajItVWZPeFFiZzlGS29ldyJ9.eyJjbGllbnRfaWQiOiJTRUdFU09BdXRoU2FtcGxlQ2xpZW50X0RFQlVHIiwic2NvcGUiOiJEZWZhdWx0Iiwic3ViIjoiY3ZydXNlcjFAUFJPRC5ETEkiLCJpc3MiOiJodHRwczovL3NpLWF1dGh6c2VydmVyLnZmbHRlc3QuZGsvIiwiYXVkIjoidXJuOlNFR0VTT0F1dGhTYW1wbGVBcGlfREVCVUciLCJleHAiOjE1MjMzMTE0MzUsIm5iZiI6MTUyMzMwNzgzNX0.Zu4W1asyCR_a2-pY9vtIvkvIW7vC3S228gQuU6dejssNDt5z72UpRnxKBFq1O4Y9Zpqp0-SZZjL9vWoGFXaSUCoH64-yUc7hwqlcmLx3ZM5V2Dg2vaAvRYUDWlL3-g-JXowPPo4kqT6PPmrxKXxuKEu_bXBEqpPoif5VeQ2ETqkQK0oeS7ZiyV0EARnSQZwndhJAW4rA9yPQbv0_Vu6sdTJXrWU40-S0AoXUvGI1qCkouQw-eNCV5G-4sOO0yRCw_n1Q9LmIfuE-7-retEyZaNoltnW3udDGUKYq8gK5gwJHen7wHl66gfJavqdvy81jeBNxPrCP4JAby47Gt1O0zA as Bearer token in Authorization header
```
Furthermore, to enable token validation, set PerformTokenValidation to true in ApplicationConfiguration.json and specify a valid path to the public signing certificate in Program.cs - ValidateToken. The public certificate can be found here: [AuthzServer Environments](https://confluence.seges.dk/display/PUB/AuthzServer+Environments)