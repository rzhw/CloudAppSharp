CloudAppSharp 1.5.0
===================

Copyright (c) Richard Wang 2010

http://github.com/a2h/CloudAppSharp

Introduction
------------

It's a wrapper around the CloudApp API, written in C# (not C Flat). This bad pun brought to you courtesy of Richard!

Anyway, this library was created by me primarily to split out the CloudApp code from [FluffyApp](http://fluffyapp.com), but it's been open sourced so you can use the CloudApp awesomeness in your own apps as well... or help fix stuff in FluffyApp :P

Anyway, I have a wiki set up on the GitHub page linked above, if you'd like to know how to use CloudAppSharp. It's a wiki, so if you'd like to help out with docs, go ahead!

Changelog
---------

1.4.1 to 1.5.0:

* Split the async upload code to a new class CloudAppAsyncUploader (multiple asynchronous uploads theoretically are now implementable, but that hasn't been tested)
* Added support for upload cancellation

1.4.0 to 1.4.1:

* Made the booleans for the CloudAppUser class nullable, so that an exception isn't thrown when a value isn't received

1.3.1 to 1.4.0:

* Added support for retrieving account details
* Added support for grabbing the "standard URL" of an item, i.e. with cl.ly instead of a custom domain

1.3.0 to 1.3.1:

* Fixed crashing when the max upload size and/or uploads remaining params aren't received, which is the case for CloudApp Pro users

1.2.1 to 1.3.0:

* Added support for detecting whether the user is over any of the daily upload limits

1.2.0 to 1.2.1:

* Catch and pass on exceptions in CloudAppUploadCompletedEventArgs

1.1.1 to 1.2.0:

* Added custom exceptions CloudAppInvalidResponseException and CloudAppInvalidProtocolException

1.1.0 to 1.1.1:

* Pass blank strings instead of null on if a retrieved item's name is null - this fixes a crash

1.0.3 to 1.1.0:

* Added support for file privacy
* Some major rewriting of how requests are made

1.0.2 to 1.0.3:

* Allow setting a proxy, and use the system proxy by default

1.0.1 to 1.0.2:

* Allow disposal of the asynchronous uploader

1.0.0 to 1.0.1:

* Don't catch errors when uploading
* Don't bother with detecting the content type of files when uploading
* Fixed uploads timing out at the .NET default of 100 seconds

0.9.2 to 1.0.0:

* Added support for creating new bookmarks

0.9.1 to 0.9.2:

* Lowercase emails before making a request; apparently CloudApp stores
  emails lowercased in their database...
* Added support for logging into CloudApp via HA1 (thanks ShaRose!)

0.9.0 to 0.9.1:

* Made CloudApp.UploadAsync() actually fully async

0.8.1 to 0.9.0:

* Added support for deleting files

0.8 to 0.8.1:

* Fixed concurrent I/O exceptions