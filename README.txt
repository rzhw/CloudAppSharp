CloudAppSharp 1.0.2
===================

http://github.com/a2h/CloudAppSharp

Introduction
------------

It's a wrapper around the CloudApp API, in C# (not C Flat). This bad pun brought to you courtesy of a2h.

Instructions
------------

**TODO**

Changelog
---------

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