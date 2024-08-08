# CurrencyExchangeRate
Simple CRUD API to manage the exchange rate of a pair of currencies (eg. USD to EUR) using https://www.alphavantage.co/ to get the exchange rate if it's not available on the Database.

When a new exchange rate is created either directly by POST or when retrieved by the alpha vantage service a message with the item details is sent to an Azure Service Bus queue.

### Important note
Since this is a personal project setup with my personal Azure account with Azure Web Apps and Service Bus it won't be available locally.
The published API is available for public consumption however.

## Usage

There are 4 endpoints publicly available:

* GET
  ```sh
  https://currencyexchangerate-webapp.azurewebsites.net/api/CurrencyExchangeRate/{currencyFrom}/{currencyTo}
  ```
* POST
  ```sh
  https://currencyexchangerate-webapp.azurewebsites.net/api/CurrencyExchangeRate
  ```
  Body request example
  ```sh
  {
    "fromCurrencyCode": "EUR",
    "toCurrencyCode": "GBP",
    "exchangeRate": 0.860000,
    "lastRefreshed": "2024-08-06T20:08:05",
    "bidPrice": 0.860000,
    "askPrice": 0.860000
  }
  ```  
* PUT
  ```sh
  https://currencyexchangerate-webapp.azurewebsites.net/api/CurrencyExchangeRate/{currencyFrom}/{currencyTo}
  ```
  Body request example
  ```sh
  {
    "exchangeRate": 0.860000,
    "lastRefreshed": "2024-08-06T20:08:05",
    "bidPrice": 0.860000,
    "askPrice": 0.860000
  }
  ```  
* DELETE
  ```sh
  https://currencyexchangerate-webapp.azurewebsites.net/api/CurrencyExchangeRate/{currencyFrom}/{currencyTo}
  ```

## Known Issues / Limitations / Possible improvements

1. The key used by alpha vantage is a free one which is limited to 25 requests per day, a premium key would allow at least 75 requests per minute.
2. This is running on a personal Azure free subscription which has limited capacity with few resources impacting both performance and ability for others to run the solution locally.
3. It has no authentication, so anyone can read/write and delete records.
4. No rate limit is being applied so it's vulnerable to DDoS.
5. Ideally the event/message sent to Azure Service Bus should be on a topic rather than a queue to use a publisher/subscription model allowing for multiple readers of the creation event, unfortunately the free Service Bus tier doesn't allow topics.
6. No integration tests with Alpha Vantage as these would only make sense with a premium key due to the requests limits of the free API key.
7. Only one environment (PROD) is set up, in a real-world scenario it should have at least 3 enviroments (DEV, QA/UAT and PROD) for proper development and testing before reaching production.
8. Only one branch (main) is set up, ideally one should follow a git branch strategy depending on the team size and requirements.
9. Using cache, either memory or distributed like Redis could have an improvement on performance.
10. CQRS with MediatR could improve on responsibility separation in case the API grows in scope.
11. Integration with Sonarqube or similar for code quality and security.
12. Consider using containers with Docker + Azure container apps for easier deployment and scalability.
