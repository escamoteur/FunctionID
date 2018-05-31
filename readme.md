# FunctionId: an OSS identity provider for mobile Apps

## Why another identity provider?
Contrary to the impression of a lot of tech affine users many users refrain from using any of the available OAuth social identity providers like Google, Facebook or Twitter. They still prefer to use email/password combination.

So when writing any mobile App that needs user authentication you have to either write your own solution or use one of the available commercial identity providers.

### Avoiding browser based logins 
I know many will cringe at this point but face the truth, leaving your app for sign-up and login is a bad user experience and can be an onboarding obstacle for an app.

If done right this is not less secure than a browser based login. (faking a social login site isn't that difficult after all)

So the goal of this API is to be used from within your App.

### Why not Azure B2C? 
Although a current preview now offers a pure API login you still cannot register new users or do any email activation/password reset through it over the REST API also the user will always get an Microsoft or b2clogin.com displayed in their browser.

## Features

Feature accessible over an REST API:

* Register new users with username/email and password. Additional information on the user can be saves as JSON string. As long as an user Account isn't activated the user cannot login. Only salted hashes of passwords are stored.
* Automatic sending of an activation email via an email service provider
* Login with username/email and password which returns you an long living refresh token and a short living JWT authentication token.
* Accessing custom user data. 
* Login via refresh token
* Sending password reset emails


## Client SDKs
Currently I want to provide an .net and an Dart SDK for *FunctionId* 

## Technology
* The whole service will be implemented using Azure Functions. 
* Storing of user information initially in Azure Tables
* Emails send via an configurable email service provider, initially using [SendGrid](https://sendgrid.com/) which offers 100 free emails/day which should be enough for most Apps.

# Contributing
I will be more than happy if you like the project and decide to contribute to it.

I created an [issue for general discussion on the project](https://github.com/escamoteur/FunctionID/issues/1) please share your wishes and ideas for this project.

It would be awesome if I could get help especially in this areas

* HTML/JS for the password reset and activation confirmation pages (I'm not a web guy)
* Devops for the whole project
* Publishing scripts for Azure so that anyone can setup the system easily
* Documentation
* Client SDKs possibly also for other languages

If you want to get in touch with be bet to ping me on Twitter [@thomasburkhartb](https://twitter.com/ThomasBurkhartB) 