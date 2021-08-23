# yenepay.sdk.core.client (Coming Soon)
YenePay Client dotnet core SDK for merchants to send money automatically from their balance.
## Installation

Step 1: Install YenePayClient from Nugget Package Manager

```
Install-Package YenePay.Client
``` 

Step 2: Add ```services.AddYenePayClient()``` to your Startup.cs file.		
```
public void ConfigureServices(IServiceCollection services)
        {
            ...
            services.AddYenePayClient();
            ...
        }
```
## Pre-requisite

### Creating client token and signing key
If you don't have it already to create access token and signing key for your client app go to your yenepay account setting page https://www.yenepay.com/account/#/settings

Then create a new client by going to merchants apps menu. After you create a new client copy and save both the access token and the generated private key for that client.

NOTE: Becareful where you put your token and key since it is a highly sensitive information.

### Sending money
To send money you can use the following aspnet core code
```php

     var recipients = new MoneyRecipient[]
    {
        new MoneyRecipient
        {
            CustomerCode = "<ADD CUSTOMER CODE>",
            Amount = 10
        },
        new MoneyRecipient
        {
            CustomerCode = "<ADD ANOTHER CUSTOMER CODE>",
            Amount = 18
        }
    };
            
    var request = _yenePayService.CreateSignedRequest("Send Money Test", recipients);
    var result = await _yenePayService.SendMoney(request);

    //Check if payment completed
    if(result.IsPaid){
        //The payment is completed 
    }
```

The response object is an instance of a class [SendMoneyResult.cs](https://github.com/YenePay/yenepay.sdk.core.client/tree/master/YenePay.SDK.Core.Client/Models/SendMoneyResult.cs)

### Example
You can find a working example here https://github.com/YenePay/yenepay.sdk.core.client/tree/master/YenePay.SDK.Core.Client.Example. Please look at [HomeController.cs](https://github.com/YenePay/yenepay.sdk.core.client/tree/master/YenePay.SDK.Core.Client.Example/Controllers/HomeController.cs) to see how to send the request.

![YenePay Send Money Client](https://github.com/YenePay/yenepay.sdk.php.client/raw/master/example.png)

### More
For more information please visit
- https://yenepay.com
- https://yenepay.com/developer
- https://community.yenepay.com

